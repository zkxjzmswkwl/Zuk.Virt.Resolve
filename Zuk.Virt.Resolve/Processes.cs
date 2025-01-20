using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

public static class Processes
{
   private const uint PROCESS_ALL_ACCESS = 0x001F0FFF;

    // Somehow more verbose than C++ typedefs.
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
    [DllImport("psapi.dll", SetLastError = true)]
    static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] char[] lpFilename, uint nSize);

    [DllImport("psapi.dll", SetLastError = true)]
    private static extern bool EnumProcessModulesEx(
        IntPtr hProcess,
        [Out] IntPtr[] lphModule,
        uint cb,
        out uint lpcbNeeded,
        uint dwFilterFlag);

    [DllImport("psapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern uint GetModuleBaseName(
        IntPtr hProcess,
        IntPtr hModule,
        [Out] char[] lpBaseName,
        uint nSize);

    [DllImport("psapi.dll", SetLastError = true)]
    private static extern bool GetModuleInformation(
        IntPtr hProcess,
        IntPtr hModule,
        out MODULEINFO lpmodinfo,
        uint cb);

    [StructLayout(LayoutKind.Sequential)]
    public struct MODULEINFO
    {
        public IntPtr lpBaseOfDll;
        public uint SizeOfImage;
        public IntPtr EntryPoint;
    }

    /// <summary>
    /// Enumerates running processes until a process with a matching ProcessName is found.
    /// </summary>
    /// <param name="executableName"></param>
    /// <returns>PROCESS_ALL_ACCESS HANDLE</returns>
    public static IntPtr GetProcessHandle(string executableName)
    {
        // For future me, when I inevitably come back to this project and forget about this quirk.
        Contract.Requires(!executableName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase), "Don't include file extension, just binary name.");

        foreach (var process in Process.GetProcesses())
        {
            try
            {
                if (string.Equals(process.ProcessName, executableName, StringComparison.OrdinalIgnoreCase))
                {
                    IntPtr handle = OpenProcess(PROCESS_ALL_ACCESS, false, process.Id);
                    if (handle != IntPtr.Zero)
                    {
                        return handle;
                    }
                }
            }
            catch
            {
                return IntPtr.Zero;
            }
        }

        return IntPtr.Zero;
    }

    public static string GetProcessPath(IntPtr processHandle)
    {
        char[] buffer = new char[1024];
        if (GetModuleFileNameEx(processHandle, IntPtr.Zero, buffer, (uint)buffer.Length) > 0)
        {
            return new string(buffer).TrimEnd('\0');
        }
        return null;
    }

    public static IntPtr GetModuleBaseAddress(IntPtr processHandle, string moduleName)
    {
        IntPtr[] moduleHandles = new IntPtr[1024];
        uint cb = (uint)(IntPtr.Size * moduleHandles.Length);
        uint cbNeeded;

        if (!EnumProcessModulesEx(processHandle, moduleHandles, cb, out cbNeeded, 0x03))
            throw new InvalidOperationException($"Failed to enumerate modules. Error: {Marshal.GetLastWin32Error()}");

        for (int i = 0; i < (cbNeeded / IntPtr.Size); i++)
        {
            char[] moduleBaseName = new char[256];
            if (GetModuleBaseName(processHandle, moduleHandles[i], moduleBaseName, (uint)moduleBaseName.Length) > 0)
            {
                string name = new string(moduleBaseName).TrimEnd('\0');
                if (string.Equals(name, moduleName, StringComparison.OrdinalIgnoreCase))
                {
                    if (GetModuleInformation(processHandle, moduleHandles[i], out MODULEINFO moduleInfo, (uint)Marshal.SizeOf<MODULEINFO>()))
                    {
                        return moduleInfo.lpBaseOfDll;
                    }
                }
            }
        }

        return IntPtr.Zero;
    }
}
