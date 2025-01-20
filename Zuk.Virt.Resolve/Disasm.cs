using SharpDisasm;


public class Disasm
{
    public static List<Instruction> DisassembleFunction(IntPtr processHandle, IntPtr functionAddress, int maxBytes)
    {
        byte[] buffer = Memory.ReadMemory(processHandle, functionAddress, maxBytes);
        if (buffer == null || buffer.Length == 0)
        {
            Console.WriteLine("Failed to read memory.");
            throw new Exception("shit's fucked.");
        }

        Disassembler.Translator.IncludeAddress = true;
        Disassembler.Translator.IncludeBinary = true;

        var disasm = new Disassembler(buffer, ArchitectureMode.x86_64, (ulong)functionAddress.ToInt64());
        return disasm.Disassemble()
                     .TakeWhile(instruction => instruction.Mnemonic != SharpDisasm.Udis86.ud_mnemonic_code.UD_Iint3)
                     .ToList();
    }
}
