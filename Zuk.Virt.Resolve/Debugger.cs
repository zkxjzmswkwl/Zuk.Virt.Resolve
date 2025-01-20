using System.Runtime.InteropServices;

class Debugger
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool DebugActiveProcess(int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WaitForDebugEvent(out DEBUG_EVENT lpDebugEvent, uint dwMilliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ContinueDebugEvent(int dwProcessId, int dwThreadId, uint dwContinueStatus);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool SetThreadContext(IntPtr hThread, ref CONTEXT lpContext);

    [DllImport("kernel32.dll")]
    private static extern int GetProcessId(IntPtr hProcess);

    private const uint CONTEXT_FULL = 0x10007;
    private const uint EXCEPTION_DEBUG_EVENT = 1;
    private const uint DBG_CONTINUE = 0x00010002;

    [StructLayout(LayoutKind.Sequential)]
    public struct DEBUG_EVENT
    {
        public uint dwDebugEventCode;
        public int dwProcessId;
        public int dwThreadId;
        public DEBUG_EVENT_UNION u;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DEBUG_EVENT_UNION
    {
        [FieldOffset(0)] public EXCEPTION_DEBUG_INFO Exception;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_DEBUG_INFO
    {
        public EXCEPTION_RECORD ExceptionRecord;
        public uint dwFirstChance;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_RECORD
    {
        public uint ExceptionCode;
        public uint ExceptionFlags;
        public IntPtr ExceptionRecord;
        public IntPtr ExceptionAddress;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
        public IntPtr[] ExceptionInformation;
    }

    [StructLayout(LayoutKind.Explicit, Size = 1232)]
    internal struct CONTEXT
    {
        [FieldOffset(0x0)] internal ulong P1Home;
        [FieldOffset(0x8)] internal ulong P2Home;
        [FieldOffset(0x10)] internal ulong P3Home;
        [FieldOffset(0x18)] internal ulong P4Home;
        [FieldOffset(0x20)] internal ulong P5Home;
        [FieldOffset(0x28)] internal ulong P6Home;
        [FieldOffset(0x30)] internal uint ContextFlags;
        [FieldOffset(0x34)] internal uint MxCsr;
        [FieldOffset(0x38)] internal ushort SegCs;
        [FieldOffset(0x3a)] internal ushort SegDs;
        [FieldOffset(0x3c)] internal ushort SegEs;
        [FieldOffset(0x3e)] internal ushort SegFs;
        [FieldOffset(0x40)] internal ushort SegGs;
        [FieldOffset(0x42)] internal ushort SegSs;
        [FieldOffset(0x44)] internal uint EFlags;
        [FieldOffset(0x48)] internal ulong Dr0;
        [FieldOffset(0x50)] internal ulong Dr1;
        [FieldOffset(0x58)] internal ulong Dr2;
        [FieldOffset(0x60)] internal ulong Dr3;
        [FieldOffset(0x68)] internal ulong Dr6;
        [FieldOffset(0x70)] internal ulong Dr7;
        [FieldOffset(0x78)] internal ulong Rax;
        [FieldOffset(0x80)] internal ulong Rcx;
        [FieldOffset(0x88)] internal ulong Rdx;
        [FieldOffset(0x90)] internal ulong Rbx;
        [FieldOffset(0x98)] internal ulong Rsp;
        [FieldOffset(0xa0)] internal ulong Rbp;
        [FieldOffset(0xa8)] internal ulong Rsi;
        [FieldOffset(0xb0)] internal ulong Rdi;
        [FieldOffset(0xb8)] internal ulong R8;
        [FieldOffset(0xc0)] internal ulong R9;
        [FieldOffset(0xc8)] internal ulong R10;
        [FieldOffset(0xd0)] internal ulong R11;
        [FieldOffset(0xd8)] internal ulong R12;
        [FieldOffset(0xe0)] internal ulong R13;
        [FieldOffset(0xe8)] internal ulong R14;
        [FieldOffset(0xf0)] internal ulong R15;
        [FieldOffset(0xf8)] internal ulong Rip;
    }

    public static void AttachToProcess(IntPtr processHandle, IntPtr[] breakpoints, IntPtr modBase)
    {
        var processId = GetProcessId(processHandle);
        if (!DebugActiveProcess(processId)) throw new Exception("Failed to attach to process.");

        var originalBytes = new Dictionary<IntPtr, byte>();
        foreach (var breakpoint in breakpoints)
        {
            var int3 = new byte[] { 0xCC };
            var originalByte = new byte[1];
            ReadProcessMemory(processHandle, breakpoint, originalByte, 1, out _);
            originalBytes[breakpoint] = originalByte[0];
            WriteProcessMemory(processHandle, breakpoint, int3, (uint)int3.Length, out _);
        }

        var debugEvent = new DEBUG_EVENT();
        IntPtr bpAddress = IntPtr.Zero;

        while (true)
        {
            if (!WaitForDebugEvent(out debugEvent, 1000)) continue;

            if (debugEvent.dwDebugEventCode == EXCEPTION_DEBUG_EVENT)
            {
                var exceptionInfo = debugEvent.u.Exception;

                if (exceptionInfo.ExceptionRecord.ExceptionCode == 0x80000003)
                {
                    bpAddress = exceptionInfo.ExceptionRecord.ExceptionAddress;

                    if (!originalBytes.ContainsKey(bpAddress))
                    {
                        ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, DBG_CONTINUE);
                        continue;
                    }

                    var threadHandle = OpenThread(0x001F03FF, false, (uint)debugEvent.dwThreadId);
                    var context = new CONTEXT { ContextFlags = CONTEXT_FULL };

                    if (GetThreadContext(threadHandle, ref context))
                    {
                        context.Rip -= 1;
                        WriteProcessMemory(processHandle, bpAddress, new[] { originalBytes[bpAddress] }, 1, out _);
                        context.EFlags |= 0x100;
                        SetThreadContext(threadHandle, ref context);
                    }

                    CloseHandle(threadHandle);
                    ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, DBG_CONTINUE);
                }
                else if (exceptionInfo.ExceptionRecord.ExceptionCode == 0x80000004)
                {
                    var threadHandle = OpenThread(0x001F03FF, false, (uint)debugEvent.dwThreadId);
                    var context = new CONTEXT { ContextFlags = CONTEXT_FULL };

                    if (GetThreadContext(threadHandle, ref context))
                    {
                        var ripRva = context.Rip - (ulong)modBase;
                        var bpRva = bpAddress - modBase;
                        Console.WriteLine($"insn ptr after indirect call: 0x{ripRva:X}");
                        ResolvedCallManager.Instance.AddResolvedCall(bpRva, ripRva);
                    }

                    CloseHandle(threadHandle);
                    WriteProcessMemory(processHandle, bpAddress, new[] { (byte)0xCC }, 1, out _);
                    ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, DBG_CONTINUE);
                }
                else
                {
                    ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, 0x80010001);
                }
            }
            else
            {
                ContinueDebugEvent(debugEvent.dwProcessId, debugEvent.dwThreadId, DBG_CONTINUE);
            }
        }
    }
}