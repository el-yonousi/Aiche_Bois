
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
            this.CrystalReport21 = new Aiche_Bois.CrystalReportClient();
            this.SuspendLayout();
            // 
            // crystalRepClient
            // 
            this.crystalRepClient.ActiveViewIndex = -1;
            this.crystalRepClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalRepClient.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalRepClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalRepClient.Location = new System.Drawing.Point(0, 0);
            this.crystalRepClient.Name = "crystalRepClient";
            this.crystalRepClient.Size = new System.Drawing.Size(931, 661);
            this.crystalRepClient.TabIndex = 0;
            this.crystalRepClient.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // PrintPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 661);
            this.Controls.Add(this.crystalRepClient);
            this.Name = "PrintPdf";
            this.Text = "PrintPdf";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintPdf_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalRepClient;
        private CrystalReportClient CrystalReport21;
    }
}