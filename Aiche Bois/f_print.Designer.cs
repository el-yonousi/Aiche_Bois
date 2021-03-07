
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
            this.lt_facture = new System.Windows.Forms.ListBox();
            this.b_print = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lt_mesure = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            // lt_facture
            // 
            this.lt_facture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lt_facture.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lt_facture.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lt_facture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lt_facture.FormattingEnabled = true;
            this.lt_facture.ItemHeight = 27;
            this.lt_facture.Items.AddRange(new object[] {
            "aucune"});
            this.lt_facture.Location = new System.Drawing.Point(244, 85);
            this.lt_facture.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.lt_facture.Name = "lt_facture";
            this.lt_facture.Size = new System.Drawing.Size(231, 351);
            this.lt_facture.TabIndex = 17;
            this.toolTip1.SetToolTip(this.lt_facture, "la liste des facture client");
            // 
            // b_print
            // 
            this.b_print.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_print.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_print.FlatAppearance.BorderSize = 0;
            this.b_print.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_print.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_print.Location = new System.Drawing.Point(8, 445);
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
            // lt_mesure
            // 
            this.lt_mesure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lt_mesure.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lt_mesure.Font = new System.Drawing.Font("Ubuntu", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lt_mesure.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lt_mesure.FormattingEnabled = true;
            this.lt_mesure.ItemHeight = 27;
            this.lt_mesure.Items.AddRange(new object[] {
            "aucune"});
            this.lt_mesure.Location = new System.Drawing.Point(9, 85);
            this.lt_mesure.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.lt_mesure.Name = "lt_mesure";
            this.lt_mesure.Size = new System.Drawing.Size(231, 351);
            this.lt_mesure.TabIndex = 16;
            this.toolTip1.SetToolTip(this.lt_mesure, "la liste des mesures");
            // 
            // f_print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(484, 501);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lt_mesure);
            this.Controls.Add(this.lt_facture);
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
        private System.Windows.Forms.ListBox lt_facture;
        private System.Windows.Forms.Button b_print;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lt_mesure;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}