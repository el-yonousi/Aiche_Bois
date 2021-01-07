
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPdf));
            this.crystalRepClient = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.cmbShoosePrint = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCorrespondFacture = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalRepClient
            // 
            this.crystalRepClient.ActiveViewIndex = -1;
            this.crystalRepClient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalRepClient.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalRepClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalRepClient.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
            this.crystalRepClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.crystalRepClient.Location = new System.Drawing.Point(0, 91);
            this.crystalRepClient.Name = "crystalRepClient";
            this.crystalRepClient.Size = new System.Drawing.Size(931, 570);
            this.crystalRepClient.TabIndex = 0;
            this.crystalRepClient.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // cmbShoosePrint
            // 
            this.cmbShoosePrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbShoosePrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.cmbShoosePrint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShoosePrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbShoosePrint.Font = new System.Drawing.Font("Nirmala UI", 16F, System.Drawing.FontStyle.Bold);
            this.cmbShoosePrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.cmbShoosePrint.FormattingEnabled = true;
            this.cmbShoosePrint.Location = new System.Drawing.Point(262, 41);
            this.cmbShoosePrint.Name = "cmbShoosePrint";
            this.cmbShoosePrint.Size = new System.Drawing.Size(666, 38);
            this.cmbShoosePrint.TabIndex = 1;
            this.cmbShoosePrint.SelectedIndexChanged += new System.EventHandler(this.cmbShoosePrint_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 13F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.label1.Location = new System.Drawing.Point(262, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(666, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choisissez la facture à imprimer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.92696F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.07304F));
            this.tableLayoutPanel1.Controls.Add(this.btnCorrespondFacture, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbShoosePrint, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(931, 91);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // btnCorrespondFacture
            // 
            this.btnCorrespondFacture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCorrespondFacture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCorrespondFacture.Font = new System.Drawing.Font("Nirmala UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnCorrespondFacture.Location = new System.Drawing.Point(3, 32);
            this.btnCorrespondFacture.Name = "btnCorrespondFacture";
            this.btnCorrespondFacture.Size = new System.Drawing.Size(253, 56);
            this.btnCorrespondFacture.TabIndex = 5;
            this.btnCorrespondFacture.Text = "Mesures correspondant au numéro de facture";
            this.btnCorrespondFacture.UseVisualStyleBackColor = true;
            this.btnCorrespondFacture.Click += new System.EventHandler(this.btnCorrespondFacture_Click);
            // 
            // PrintPdf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ClientSize = new System.Drawing.Size(931, 661);
            this.Controls.Add(this.crystalRepClient);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Nirmala UI", 8.25F);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrintPdf";
            this.Text = "PrintPdf";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PrintPdf_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalRepClient;
        private System.Windows.Forms.ComboBox cmbShoosePrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCorrespondFacture;
    }
}