namespace Zuk.Virt.Resolve
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            acrylicButton1 = new AcrylicUI.Controls.AcrylicButton();
            fnLocTextBox = new AcrylicUI.Controls.AcrylicTextBox();
            moduleNameTextBox = new AcrylicUI.Controls.AcrylicTextBox();
            procNameTextBox = new AcrylicUI.Controls.AcrylicTextBox();
            acrylicButton2 = new AcrylicUI.Controls.AcrylicButton();
            acrylicStatusStrip1 = new AcrylicUI.Controls.AcrylicStatusStrip();
            statusLabel = new ToolStripStatusLabel();
            asmGrid = new AcrylicUI.Controls.AcrylicDataGridView();
            pictureBox1 = new PictureBox();
            acrylicButton3 = new AcrylicUI.Controls.AcrylicButton();
            acrylicSectionPanel1 = new AcrylicUI.Controls.AcrylicSectionPanel();
            acrylicTreeView1 = new AcrylicUI.Controls.AcrylicTreeView();
            acrylicStatusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)asmGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            acrylicSectionPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // acrylicButton1
            // 
            acrylicButton1.Default = false;
            acrylicButton1.Image = null;
            acrylicButton1.ImagePadding = 5;
            acrylicButton1.Location = new Point(384, 41);
            acrylicButton1.Name = "acrylicButton1";
            acrylicButton1.Padding = new Padding(5);
            acrylicButton1.Size = new Size(90, 23);
            acrylicButton1.TabIndex = 4;
            acrylicButton1.Text = "Disassemble";
            acrylicButton1.UseVisualStyleBackColor = false;
            acrylicButton1.Click += acrylicButton1_Click;
            // 
            // fnLocTextBox
            // 
            fnLocTextBox.BackColor = Color.FromArgb(31, 31, 31);
            fnLocTextBox.BorderStyle = BorderStyle.FixedSingle;
            fnLocTextBox.ForeColor = Color.FromArgb(245, 245, 245);
            fnLocTextBox.Location = new Point(198, 41);
            fnLocTextBox.Name = "fnLocTextBox";
            fnLocTextBox.PlaceholderText = "Function RVA";
            fnLocTextBox.Size = new Size(180, 23);
            fnLocTextBox.TabIndex = 3;
            // 
            // moduleNameTextBox
            // 
            moduleNameTextBox.BackColor = Color.FromArgb(31, 31, 31);
            moduleNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            moduleNameTextBox.ForeColor = Color.FromArgb(245, 245, 245);
            moduleNameTextBox.Location = new Point(12, 41);
            moduleNameTextBox.Name = "moduleNameTextBox";
            moduleNameTextBox.PlaceholderText = "Module";
            moduleNameTextBox.Size = new Size(180, 23);
            moduleNameTextBox.TabIndex = 2;
            // 
            // procNameTextBox
            // 
            procNameTextBox.BackColor = Color.FromArgb(31, 31, 31);
            procNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            procNameTextBox.ForeColor = Color.FromArgb(245, 245, 245);
            procNameTextBox.Location = new Point(12, 12);
            procNameTextBox.Name = "procNameTextBox";
            procNameTextBox.PlaceholderText = "Process name (e.g rs2client)";
            procNameTextBox.Size = new Size(180, 23);
            procNameTextBox.TabIndex = 0;
            // 
            // acrylicButton2
            // 
            acrylicButton2.Default = false;
            acrylicButton2.Image = null;
            acrylicButton2.ImagePadding = 5;
            acrylicButton2.Location = new Point(198, 12);
            acrylicButton2.Name = "acrylicButton2";
            acrylicButton2.Padding = new Padding(5);
            acrylicButton2.Size = new Size(90, 23);
            acrylicButton2.TabIndex = 1;
            acrylicButton2.Text = "Open Handle";
            acrylicButton2.UseVisualStyleBackColor = false;
            acrylicButton2.Click += acrylicButton2_Click;
            // 
            // acrylicStatusStrip1
            // 
            acrylicStatusStrip1.AutoSize = false;
            acrylicStatusStrip1.BackColor = Color.FromArgb(31, 31, 31);
            acrylicStatusStrip1.ForeColor = Color.FromArgb(192, 192, 192);
            acrylicStatusStrip1.IsAcrylicEnabled = false;
            acrylicStatusStrip1.Items.AddRange(new ToolStripItem[] { statusLabel });
            acrylicStatusStrip1.Location = new Point(0, 831);
            acrylicStatusStrip1.Name = "acrylicStatusStrip1";
            acrylicStatusStrip1.Padding = new Padding(0, 5, 0, 3);
            acrylicStatusStrip1.Size = new Size(1584, 30);
            acrylicStatusStrip1.SizingGrip = false;
            acrylicStatusStrip1.TabIndex = 5;
            acrylicStatusStrip1.Text = "acrylicStatusStrip1";
            // 
            // statusLabel
            // 
            statusLabel.Margin = new Padding(3, 3, 0, 2);
            statusLabel.Name = "statusLabel";
            statusLabel.Padding = new Padding(12, 0, 0, 0);
            statusLabel.Size = new Size(97, 17);
            statusLabel.Text = "Awaiting input";
            // 
            // asmGrid
            // 
            asmGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            asmGrid.ColumnHeadersHeight = 4;
            asmGrid.Location = new Point(12, 70);
            asmGrid.Name = "asmGrid";
            asmGrid.RowHeadersWidth = 200;
            asmGrid.RowTemplate.Resizable = DataGridViewTriState.True;
            asmGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            asmGrid.Size = new Size(780, 749);
            asmGrid.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.TzKal_Zuk__seated_;
            pictureBox1.Location = new Point(545, -23);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1055, 906);
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // acrylicButton3
            // 
            acrylicButton3.Default = false;
            acrylicButton3.Image = null;
            acrylicButton3.ImagePadding = 5;
            acrylicButton3.Location = new Point(480, 41);
            acrylicButton3.Name = "acrylicButton3";
            acrylicButton3.Padding = new Padding(5);
            acrylicButton3.Size = new Size(90, 23);
            acrylicButton3.TabIndex = 9;
            acrylicButton3.Text = "Debug";
            acrylicButton3.UseVisualStyleBackColor = false;
            acrylicButton3.Click += acrylicButton3_Click;
            // 
            // acrylicSectionPanel1
            // 
            acrylicSectionPanel1.Controls.Add(acrylicTreeView1);
            acrylicSectionPanel1.Location = new Point(798, 70);
            acrylicSectionPanel1.MinimumSize = new Size(774, 749);
            acrylicSectionPanel1.Name = "acrylicSectionPanel1";
            acrylicSectionPanel1.SectionHeader = null;
            acrylicSectionPanel1.Size = new Size(774, 749);
            acrylicSectionPanel1.TabIndex = 10;
            // 
            // acrylicTreeView1
            // 
            acrylicTreeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            acrylicTreeView1.BackColor = Color.FromArgb(31, 31, 31);
            acrylicTreeView1.DisableHorizontalScrollBar = false;
            acrylicTreeView1.Location = new Point(1, 25);
            acrylicTreeView1.MaxDragChange = 20;
            acrylicTreeView1.Name = "acrylicTreeView1";
            acrylicTreeView1.Size = new Size(772, 723);
            acrylicTreeView1.TabIndex = 0;
            acrylicTreeView1.Text = "acrylicTreeView1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1584, 861);
            Controls.Add(acrylicSectionPanel1);
            Controls.Add(acrylicButton3);
            Controls.Add(asmGrid);
            Controls.Add(acrylicStatusStrip1);
            Controls.Add(acrylicButton2);
            Controls.Add(procNameTextBox);
            Controls.Add(moduleNameTextBox);
            Controls.Add(fnLocTextBox);
            Controls.Add(acrylicButton1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Location = new Point(0, 0);
            Name = "Form1";
            Text = "Zuk.Virt.Resolve";
            Load += Form1_Load;
            acrylicStatusStrip1.ResumeLayout(false);
            acrylicStatusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)asmGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            acrylicSectionPanel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AcrylicUI.Controls.AcrylicButton acrylicButton1;
        private AcrylicUI.Controls.AcrylicTextBox fnLocTextBox;
        private AcrylicUI.Controls.AcrylicTextBox moduleNameTextBox;
        private AcrylicUI.Controls.AcrylicTextBox procNameTextBox;
        private AcrylicUI.Controls.AcrylicButton acrylicButton2;
        private AcrylicUI.Controls.AcrylicStatusStrip acrylicStatusStrip1;
        private ToolStripStatusLabel statusLabel;
        private AcrylicUI.Controls.AcrylicDataGridView asmGrid;
        private PictureBox pictureBox1;
        private AcrylicUI.Controls.AcrylicButton acrylicButton3;
        private AcrylicUI.Controls.AcrylicSectionPanel acrylicSectionPanel1;
        private AcrylicUI.Controls.AcrylicTreeView acrylicTreeView1;
    }
}
