
namespace Aiche_Bois
{
    partial class PrintPdf
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
            this.crystalRepClient = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.cmbShoosePrint = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // crystalRepClient
            // 
            this.crystalRepClient.ActiveViewIndex = -1;
            this.crystalRepClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalRepClient.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalRepClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalRepClient.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
            this.crystalRepClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.crystalRepClient.Location = new System.Drawing.Point(0, 0);
            this.crystalRepClient.Name = "crystalRepClient";
            this.crystalRepClient.Size = new System.Drawing.Size(931, 661);
            this.crystalRepClient.TabIndex = 0;
            this.crystalRepClient.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // cmbShoosePrint
            // 
            this.cmbShoosePrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.cmbShoosePrint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShoosePrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbShoosePrint.Font = new System.Drawing.Font("Nirmala UI", 12F);
            this.cmbShoosePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.cmbShoosePrint.FormattingEnabled = true;
            this.cmbShoosePrint.Items.AddRange(new object[] {
            "CrystalReportMesure.rpt"});
            this.cmbShoosePrint.Location = new System.Drawing.Point(484, 0);
            this.cmbShoosePrint.Name = "cmbShoosePrint";
            this.cmbShoosePrint.Size = new System.Drawing.Size(288, 29);
            this.cmbShoosePrint.TabIndex = 1;
            // 
            // PrintPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(931, 661);
            this.Controls.Add(this.cmbShoosePrint);
            this.Controls.Add(this.crystalRepClient);
            this.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "PrintPdf";
            this.Text = "PrintPdf";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintPdf_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalRepClient;
        private System.Windows.Forms.ComboBox cmbShoosePrint;
    }
}