using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

public class Memory
{
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out IntPtr lpNumberOfBytesRead);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);

    private const uint PROCESS_VM_READ = 0x0010;
    private const uint PROCESS_QUERY_INFORMATION = 0x0400;

    public static byte[] ReadMemory(IntPtr processHandle, IntPtr address, int size)
    {
        byte[] buffer = new byte[size];
        if (!ReadProcessMemory(processHandle, address, buffer, (uint)size, out IntPtr bytesRead))
        {
            throw new InvalidOperationException($"Failed to read memory at address {address.ToInt64():X}. Error: {Marshal.GetLastWin32Error()}");
        }

        if (bytesRead.ToInt32() != size)
        {
            Array.Resize(ref buffer, bytesRead.ToInt32());
        }

        return buffer;
    }
}
