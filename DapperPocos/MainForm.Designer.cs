namespace DapperPocos
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.btnGenerate = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSavePocos = new System.Windows.Forms.ToolStripMenuItem();
            this.gbxConnectionString = new System.Windows.Forms.GroupBox();
            this.txbConnectionString = new System.Windows.Forms.TextBox();
            this.gbxSprocOrSelect = new System.Windows.Forms.GroupBox();
            this.txbSprocOrSelect = new System.Windows.Forms.TextBox();
            this.gbxInputPoco = new System.Windows.Forms.GroupBox();
            this.txbInputPoco = new System.Windows.Forms.TextBox();
            this.gbxOutputPoco = new System.Windows.Forms.GroupBox();
            this.txbOutputPoco = new System.Windows.Forms.TextBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.pnlOutput = new System.Windows.Forms.Panel();
            this.splitterOutput = new System.Windows.Forms.Splitter();
            this.splitterConnectionString = new System.Windows.Forms.Splitter();
            this.splitterSelectOrSproc = new System.Windows.Forms.Splitter();
            this.menuStrip.SuspendLayout();
            this.gbxConnectionString.SuspendLayout();
            this.gbxSprocOrSelect.SuspendLayout();
            this.gbxInputPoco.SuspendLayout();
            this.gbxOutputPoco.SuspendLayout();
            this.pnlOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnGenerate,
            this.btnSavePocos});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(101, 20);
            this.btnGenerate.Text = "&Generate Pocos";
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnSavePocos
            // 
            this.btnSavePocos.Name = "btnSavePocos";
            this.btnSavePocos.Size = new System.Drawing.Size(78, 20);
            this.btnSavePocos.Text = "&Save Pocos";
            this.btnSavePocos.Click += new System.EventHandler(this.btnSavePocos_Click);
            // 
            // gbxConnectionString
            // 
            this.gbxConnectionString.Controls.Add(this.txbConnectionString);
            this.gbxConnectionString.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxConnectionString.Location = new System.Drawing.Point(0, 24);
            this.gbxConnectionString.Name = "gbxConnectionString";
            this.gbxConnectionString.Size = new System.Drawing.Size(800, 80);
            this.gbxConnectionString.TabIndex = 1;
            this.gbxConnectionString.TabStop = false;
            this.gbxConnectionString.Text = "Connection String";
            // 
            // txbConnectionString
            // 
            this.txbConnectionString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbConnectionString.Location = new System.Drawing.Point(3, 16);
            this.txbConnectionString.Multiline = true;
            this.txbConnectionString.Name = "txbConnectionString";
            this.txbConnectionString.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbConnectionString.Size = new System.Drawing.Size(794, 61);
            this.txbConnectionString.TabIndex = 0;
            // 
            // gbxSprocOrSelect
            // 
            this.gbxSprocOrSelect.Controls.Add(this.txbSprocOrSelect);
            this.gbxSprocOrSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxSprocOrSelect.Location = new System.Drawing.Point(0, 107);
            this.gbxSprocOrSelect.Name = "gbxSprocOrSelect";
            this.gbxSprocOrSelect.Size = new System.Drawing.Size(800, 80);
            this.gbxSprocOrSelect.TabIndex = 3;
            this.gbxSprocOrSelect.TabStop = false;
            this.gbxSprocOrSelect.Text = "Stored Procedure or SQL Select";
            // 
            // txbSprocOrSelect
            // 
            this.txbSprocOrSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbSprocOrSelect.Location = new System.Drawing.Point(3, 16);
            this.txbSprocOrSelect.Multiline = true;
            this.txbSprocOrSelect.Name = "txbSprocOrSelect";
            this.txbSprocOrSelect.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbSprocOrSelect.Size = new System.Drawing.Size(794, 61);
            this.txbSprocOrSelect.TabIndex = 0;
            // 
            // gbxInputPoco
            // 
            this.gbxInputPoco.Controls.Add(this.txbInputPoco);
            this.gbxInputPoco.Dock = System.Windows.Forms.DockStyle.Left;
            this.gbxInputPoco.Location = new System.Drawing.Point(0, 0);
            this.gbxInputPoco.Name = "gbxInputPoco";
            this.gbxInputPoco.Size = new System.Drawing.Size(400, 260);
            this.gbxInputPoco.TabIndex = 0;
            this.gbxInputPoco.TabStop = false;
            this.gbxInputPoco.Text = "Input";
            // 
            // txbInputPoco
            // 
            this.txbInputPoco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbInputPoco.Location = new System.Drawing.Point(3, 16);
            this.txbInputPoco.Multiline = true;
            this.txbInputPoco.Name = "txbInputPoco";
            this.txbInputPoco.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbInputPoco.Size = new System.Drawing.Size(394, 241);
            this.txbInputPoco.TabIndex = 0;
            this.txbInputPoco.WordWrap = false;
            // 
            // gbxOutputPoco
            // 
            this.gbxOutputPoco.Controls.Add(this.txbOutputPoco);
            this.gbxOutputPoco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxOutputPoco.Location = new System.Drawing.Point(400, 0);
            this.gbxOutputPoco.Name = "gbxOutputPoco";
            this.gbxOutputPoco.Size = new System.Drawing.Size(400, 260);
            this.gbxOutputPoco.TabIndex = 2;
            this.gbxOutputPoco.TabStop = false;
            this.gbxOutputPoco.Text = "Output";
            // 
            // txbOutputPoco
            // 
            this.txbOutputPoco.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txbOutputPoco.Location = new System.Drawing.Point(3, 16);
            this.txbOutputPoco.Multiline = true;
            this.txbOutputPoco.Name = "txbOutputPoco";
            this.txbOutputPoco.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbOutputPoco.Size = new System.Drawing.Size(394, 241);
            this.txbOutputPoco.TabIndex = 0;
            this.txbOutputPoco.WordWrap = false;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "C# Files|*.cs|(All Files)|*.*";
            // 
            // pnlOutput
            // 
            this.pnlOutput.Controls.Add(this.splitterOutput);
            this.pnlOutput.Controls.Add(this.gbxOutputPoco);
            this.pnlOutput.Controls.Add(this.gbxInputPoco);
            this.pnlOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOutput.Location = new System.Drawing.Point(0, 190);
            this.pnlOutput.Name = "pnlOutput";
            this.pnlOutput.Size = new System.Drawing.Size(800, 260);
            this.pnlOutput.TabIndex = 5;
            // 
            // splitterOutput
            // 
            this.splitterOutput.Location = new System.Drawing.Point(400, 0);
            this.splitterOutput.Name = "splitterOutput";
            this.splitterOutput.Size = new System.Drawing.Size(3, 260);
            this.splitterOutput.TabIndex = 1;
            this.splitterOutput.TabStop = false;
            // 
            // splitterConnectionString
            // 
            this.splitterConnectionString.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterConnectionString.Location = new System.Drawing.Point(0, 104);
            this.splitterConnectionString.Name = "splitterConnectionString";
            this.splitterConnectionString.Size = new System.Drawing.Size(800, 3);
            this.splitterConnectionString.TabIndex = 2;
            this.splitterConnectionString.TabStop = false;
            // 
            // splitterSelectOrSproc
            // 
            this.splitterSelectOrSproc.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitterSelectOrSproc.Location = new System.Drawing.Point(0, 187);
            this.splitterSelectOrSproc.Name = "splitterSelectOrSproc";
            this.splitterSelectOrSproc.Size = new System.Drawing.Size(800, 3);
            this.splitterSelectOrSproc.TabIndex = 4;
            this.splitterSelectOrSproc.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlOutput);
            this.Controls.Add(this.splitterSelectOrSproc);
            this.Controls.Add(this.gbxSprocOrSelect);
            this.Controls.Add(this.splitterConnectionString);
            this.Controls.Add(this.gbxConnectionString);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "Dapper Pocos";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.gbxConnectionString.ResumeLayout(false);
            this.gbxConnectionString.PerformLayout();
            this.gbxSprocOrSelect.ResumeLayout(false);
            this.gbxSprocOrSelect.PerformLayout();
            this.gbxInputPoco.ResumeLayout(false);
            this.gbxInputPoco.PerformLayout();
            this.gbxOutputPoco.ResumeLayout(false);
            this.gbxOutputPoco.PerformLayout();
            this.pnlOutput.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem btnGenerate;
        private System.Windows.Forms.ToolStripMenuItem btnSavePocos;
        private System.Windows.Forms.GroupBox gbxConnectionString;
        private System.Windows.Forms.TextBox txbConnectionString;
        private System.Windows.Forms.GroupBox gbxInputPoco;
        private System.Windows.Forms.TextBox txbInputPoco;
        private System.Windows.Forms.GroupBox gbxOutputPoco;
        private System.Windows.Forms.TextBox txbOutputPoco;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.GroupBox gbxSprocOrSelect;
        private System.Windows.Forms.TextBox txbSprocOrSelect;
        private System.Windows.Forms.Panel pnlOutput;
        private System.Windows.Forms.Splitter splitterOutput;
        private System.Windows.Forms.Splitter splitterConnectionString;
        private System.Windows.Forms.Splitter splitterSelectOrSproc;
    }
}

