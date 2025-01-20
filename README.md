# Zuk.Virt.Resolve

### Aside
I have less experience with C# than any other language. It won't be idiomatic.

### Usage
- Input binary file name
- "Open Handle"
    - If the target process has elevated perms, so must Zuk.
- Module name, e.g `rs2client.exe`, `client.dll`, etc.
- Function RVA **(Not absolute address)**.
- "Disassemble"
    - All indirect calls should now be marked in the table.
    - You can check/uncheck.
- "Debug"
    - Attaches debugger to process.
    - Makes zero attempt to hide this fact.
    - Places breakpoint on each indirect call.
    - Once hit, single-steps into the call
    - Logs instruction pointer to tree view.
    - Job done


### Screenshot
![image](https://i.imgur.com/YRQNvwu.png)


### Video
[Here](https://www.youtube.com/watch?v=hbpS-vIv5C0)