using System.Data;
using System.Runtime.InteropServices;
using AcrylicUI.Controls;
using AcrylicUI.Forms;
using AcrylicUI.Resources;

namespace Zuk.Virt.Resolve
{
    public partial class Form1 : AcrylicForm
    {
        private IntPtr procHandle;
        private DataTable dt;

        public Form1()
        {
            InitializeComponent();
            this.IsAcrylic = true;
            this.BackColor = Colors.GreyBackground;
            this.acrylicStatusStrip1.IsAcrylicEnabled = true;

            procNameTextBox.Text = "rs2client";
            moduleNameTextBox.Text = "rs2client.exe";
            fnLocTextBox.Text = "14C670";

            dt = new DataTable();
            var addrCol = dt.Columns.Add("Address", typeof(string));
            var disAsmCol = dt.Columns.Add("Disassembly", typeof(string));
            var icCol = dt.Columns.Add("Indirect Call", typeof(bool));
            asmGrid.DataSource = dt;
            asmGrid.ForeColor = Colors.GreyBackground;

            asmGrid.Columns[0].Width = asmGrid.Width / 3;
            asmGrid.Columns[1].Width = (asmGrid.Width / 3) + 100;
            asmGrid.Columns[2].Width = (asmGrid.Width / 3) - 100;

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //AllocConsole();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //FreeConsole();
            base.OnFormClosing(e);
        }


        private void acrylicButton1_Click(object sender, EventArgs e)
        {
            if (procHandle == IntPtr.Zero)
            {
                MessageBox.Show("Open handle first.");
                return;
            }

            // Get base address of module
            var moduleBase = Processes.GetModuleBaseAddress(procHandle, moduleNameTextBox.Text);
            if (moduleBase == IntPtr.Zero)
            {
                MessageBox.Show($"Could not find module by name '{moduleNameTextBox.Text}'");
                return;
            }

            // clear data
            dt.Rows.Clear();

            var fnLoc = new IntPtr(Convert.ToInt64(fnLocTextBox.Text, 16));
            var disassembledFn = Disasm.DisassembleFunction(procHandle, fnLoc + moduleBase, 2500);
            // Populate data grid
            foreach (var isn in disassembledFn)
            {
                var isnStr = isn.ToString();
                var spl = isnStr.Split(" ");
                var addedRow = dt.Rows.Add(spl[0], String.Join(" ", spl.Skip(1).ToArray()), false);

                // Identify indirect calls
                bool isIndirectCall = isn.Mnemonic == SharpDisasm.Udis86.ud_mnemonic_code.UD_Icall &&
                                      isn.Operands.Length > 0 &&
                                      isn.Operands[0].Type == SharpDisasm.Udis86.ud_type.UD_OP_MEM &&
                                      isn.Operands[0].Size == 64;
                if (isIndirectCall)
                {
                    addedRow.SetField("Indirect Call", true);
                }
            }
        }

        private void acrylicButton2_Click(object sender, EventArgs e)
        {
            string procName = procNameTextBox.Text;
            procHandle = Processes.GetProcessHandle(procName);
            if (procHandle == IntPtr.Zero)
            {
                statusLabel.Text = $"Could not obtain handle to {procName}";
                acrylicStatusStrip1.ForeColor = Colors.DocumentRedText;
                return;
            }
            statusLabel.Text = $"Have handle to {procName}";
            acrylicStatusStrip1.ForeColor = Colors.DocumentBlueText;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        private void acrylicButton3_Click(object sender, EventArgs e)
        {
            List<IntPtr> bpLocs = new List<IntPtr>(); 
            for (int i = asmGrid.Rows.Count - 1; i >= 0; i--)
            {
                var row = asmGrid.Rows[i];
                if (row.IsNewRow) continue;

                var cellValue = row.Cells[2].Value;
                if ((cellValue is bool isIndirect && isIndirect))
                {
                    var addrString = row.Cells[0].Value as string;
                    bpLocs.Add(new IntPtr(Convert.ToInt64(addrString, 16)));
                }
            }

            Task.Factory.StartNew(() => Debugger.AttachToProcess(procHandle, bpLocs.ToArray(), Processes.GetModuleBaseAddress(procHandle, moduleNameTextBox.Text)));
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var bpDict = ResolvedCallManager.Instance.GetAllResolvedCalls();

                    acrylicTreeView1.Invoke((Action)(() =>
                    {
                        foreach (var bpLoc in bpDict.Keys)
                        {
                            var nowNode = acrylicTreeView1.Nodes
                                .OfType<AcrylicTreeNode>()
                                .FirstOrDefault(node => node.Tag?.ToString() == $"0x{bpLoc:X}");

                            if (nowNode == null)
                            {
                                nowNode = new AcrylicTreeNode($"0x{bpLoc:X}")
                                {
                                    Tag = $"0x{bpLoc:X}"
                                };

                                foreach (var resolvedCall in bpDict[bpLoc])
                                {
                                    AcrylicTreeNode child = new AcrylicTreeNode($"0x{resolvedCall:X}");
                                    nowNode.Nodes.Add(child);
                                }
                                acrylicTreeView1.Nodes.Add(nowNode);
                            }
                            else
                            {
                                var existingChildTags = new HashSet<string>(
                                    nowNode.Nodes.OfType<AcrylicTreeNode>().Select(child => child.Text)
                                );

                                foreach (var resolvedCall in bpDict[bpLoc])
                                {
                                    var modBase = Processes.GetModuleBaseAddress(procHandle, moduleNameTextBox.Text);
                                    if (!existingChildTags.Contains($"0x{resolvedCall:X}"))
                                    {
                                        AcrylicTreeNode child = new AcrylicTreeNode($"0x{resolvedCall:X}");
                                        nowNode.Nodes.Add(child);
                                    }
                                }
                            }
                        }
                        acrylicTreeView1.Update();
                    }));
                    Thread.Sleep(100);
                }
            });
        }
    }
}
