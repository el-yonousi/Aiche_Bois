
namespace Aiche_Bois
{
    partial class f_print
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
        [System.Obsolete]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_print));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.b_print = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ck_lt_facture_client = new System.Windows.Forms.CheckedListBox();
            this.ck_lt_mesure = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(484, 39);
            this.label1.TabIndex = 18;
            this.label1.Text = "Choisissez la facture à imprimer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.label2.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(235, 38);
            this.label2.TabIndex = 20;
            this.label2.Text = "Mesure/Pvc";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // b_print
            // 
            this.b_print.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_print.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_print.FlatAppearance.BorderSize = 0;
            this.b_print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_print.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_print.Location = new System.Drawing.Point(8, 458);
            this.b_print.Margin = new System.Windows.Forms.Padding(0);
            this.b_print.Name = "b_print";
            this.b_print.Size = new System.Drawing.Size(467, 50);
            this.b_print.TabIndex = 19;
            this.b_print.Text = "Imprimez";
            this.toolTip1.SetToolTip(this.b_print, "le bouton pour imprimer la selection du facture ou de mesure");
            this.b_print.UseVisualStyleBackColor = false;
            this.b_print.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.label3.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.label3.Location = new System.Drawing.Point(244, 42);
            this.label3.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 38);
            this.label3.TabIndex = 21;
            this.label3.Text = "Facture";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ck_lt_facture_client
            // 
            this.ck_lt_facture_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.ck_lt_facture_client.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ck_lt_facture_client.CheckOnClick = true;
            this.ck_lt_facture_client.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ck_lt_facture_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ck_lt_facture_client.FormattingEnabled = true;
            this.ck_lt_facture_client.Location = new System.Drawing.Point(244, 85);
            this.ck_lt_facture_client.Name = "ck_lt_facture_client";
            this.ck_lt_facture_client.Size = new System.Drawing.Size(231, 362);
            this.ck_lt_facture_client.TabIndex = 22;
            this.toolTip1.SetToolTip(this.ck_lt_facture_client, "la liste des factures");
            // 
            // ck_lt_mesure
            // 
            this.ck_lt_mesure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.ck_lt_mesure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ck_lt_mesure.CheckOnClick = true;
            this.ck_lt_mesure.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ck_lt_mesure.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(31)))), ((int)(((byte)(31)))));
            this.ck_lt_mesure.FormattingEnabled = true;
            this.ck_lt_mesure.Location = new System.Drawing.Point(8, 85);
            this.ck_lt_mesure.Name = "ck_lt_mesure";
            this.ck_lt_mesure.Size = new System.Drawing.Size(231, 362);
            this.ck_lt_mesure.TabIndex = 23;
            this.toolTip1.SetToolTip(this.ck_lt_mesure, "la liste des mesures");
            // 
            // f_print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(484, 517);
            this.Controls.Add(this.ck_lt_mesure);
            this.Controls.Add(this.ck_lt_facture_client);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.b_print);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Ubuntu", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "f_print";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imprimez Document";
            this.Load += new System.EventHandler(this.PrintPdf_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button b_print;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckedListBox ck_lt_facture_client;
        private System.Windows.Forms.CheckedListBox ck_lt_mesure;
    }
}