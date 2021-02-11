namespace Aiche_Bois
{
    partial class FormAvance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAvance));
            this.txtPrixCLient = new System.Windows.Forms.TextBox();
            this.lblIDClient = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDateClient = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotalPrixClient = new System.Windows.Forms.Label();
            this.lblPPr = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.lblav = new System.Windows.Forms.Label();
            this.lblAvancePrixClient = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRestPrixClient = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNomClient = new System.Windows.Forms.Label();
            this.txtMines = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ckMines = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtPrixCLient
            // 
            this.txtPrixCLient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.txtPrixCLient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPrixCLient.Font = new System.Drawing.Font("Ubuntu", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrixCLient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.txtPrixCLient.Location = new System.Drawing.Point(12, 406);
            this.txtPrixCLient.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPrixCLient.Name = "txtPrixCLient";
            this.txtPrixCLient.Size = new System.Drawing.Size(506, 35);
            this.txtPrixCLient.TabIndex = 0;
            this.txtPrixCLient.TextChanged += new System.EventHandler(this.txtPrix_TextChanged);
            this.txtPrixCLient.Enter += new System.EventHandler(this.txtMines_Enter);
            this.txtPrixCLient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrix_KeyPress);
            this.txtPrixCLient.Leave += new System.EventHandler(this.txtMines_Leave);
            // 
            // lblIDClient
            // 
            this.lblIDClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblIDClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIDClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblIDClient.Location = new System.Drawing.Point(11, 32);
            this.lblIDClient.Name = "lblIDClient";
            this.lblIDClient.Size = new System.Drawing.Size(216, 37);
            this.lblIDClient.TabIndex = 50;
            this.lblIDClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 21);
            this.label2.TabIndex = 51;
            this.label2.Text = "Numéro du Facture";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label3.Location = new System.Drawing.Point(282, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 21);
            this.label3.TabIndex = 53;
            this.label3.Text = "La Date du Facture";
            // 
            // lblDateClient
            // 
            this.lblDateClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblDateClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblDateClient.Location = new System.Drawing.Point(286, 32);
            this.lblDateClient.Name = "lblDateClient";
            this.lblDateClient.Size = new System.Drawing.Size(232, 37);
            this.lblDateClient.TabIndex = 52;
            this.lblDateClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label5.Location = new System.Drawing.Point(282, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 21);
            this.label5.TabIndex = 55;
            this.label5.Text = "Prix Total du Facture";
            // 
            // lblTotalPrixClient
            // 
            this.lblTotalPrixClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblTotalPrixClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalPrixClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblTotalPrixClient.Location = new System.Drawing.Point(286, 126);
            this.lblTotalPrixClient.Name = "lblTotalPrixClient";
            this.lblTotalPrixClient.Size = new System.Drawing.Size(232, 37);
            this.lblTotalPrixClient.TabIndex = 54;
            this.lblTotalPrixClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPPr
            // 
            this.lblPPr.AutoSize = true;
            this.lblPPr.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPPr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblPPr.Location = new System.Drawing.Point(7, 381);
            this.lblPPr.Name = "lblPPr";
            this.lblPPr.Size = new System.Drawing.Size(98, 21);
            this.lblPPr.TabIndex = 56;
            this.lblPPr.Text = "Prix ​​à payer";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.btnConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnConfirm.Location = new System.Drawing.Point(10, 498);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(232, 44);
            this.btnConfirm.TabIndex = 1;
            this.btnConfirm.Text = "Confimer";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.btnAnnuler.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnnuler.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnuler.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnnuler.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnnuler.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnAnnuler.Location = new System.Drawing.Point(286, 498);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(232, 44);
            this.btnAnnuler.TabIndex = 2;
            this.btnAnnuler.Text = "OK";
            this.btnAnnuler.UseVisualStyleBackColor = false;
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // lblav
            // 
            this.lblav.AutoSize = true;
            this.lblav.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblav.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblav.Location = new System.Drawing.Point(282, 199);
            this.lblav.Name = "lblav";
            this.lblav.Size = new System.Drawing.Size(89, 21);
            this.lblav.TabIndex = 60;
            this.lblav.Text = "Prix Payée";
            // 
            // lblAvancePrixClient
            // 
            this.lblAvancePrixClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblAvancePrixClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvancePrixClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblAvancePrixClient.Location = new System.Drawing.Point(286, 220);
            this.lblAvancePrixClient.Name = "lblAvancePrixClient";
            this.lblAvancePrixClient.Size = new System.Drawing.Size(232, 37);
            this.lblAvancePrixClient.TabIndex = 59;
            this.lblAvancePrixClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label10.Location = new System.Drawing.Point(7, 199);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 21);
            this.label10.TabIndex = 62;
            this.label10.Text = "Prix non payé";
            // 
            // lblRestPrixClient
            // 
            this.lblRestPrixClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblRestPrixClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestPrixClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblRestPrixClient.Location = new System.Drawing.Point(11, 220);
            this.lblRestPrixClient.Name = "lblRestPrixClient";
            this.lblRestPrixClient.Size = new System.Drawing.Size(232, 37);
            this.lblRestPrixClient.TabIndex = 61;
            this.lblRestPrixClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label1.Location = new System.Drawing.Point(7, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 21);
            this.label1.TabIndex = 64;
            this.label1.Text = "Nom du Client";
            // 
            // lblNomClient
            // 
            this.lblNomClient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lblNomClient.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomClient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblNomClient.Location = new System.Drawing.Point(11, 126);
            this.lblNomClient.Name = "lblNomClient";
            this.lblNomClient.Size = new System.Drawing.Size(216, 37);
            this.lblNomClient.TabIndex = 63;
            this.lblNomClient.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMines
            // 
            this.txtMines.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.txtMines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMines.Enabled = false;
            this.txtMines.Font = new System.Drawing.Font("Ubuntu", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMines.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.txtMines.Location = new System.Drawing.Point(33, 314);
            this.txtMines.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMines.Name = "txtMines";
            this.txtMines.Size = new System.Drawing.Size(485, 35);
            this.txtMines.TabIndex = 65;
            this.txtMines.TextChanged += new System.EventHandler(this.txtMines_TextChanged);
            this.txtMines.Enter += new System.EventHandler(this.txtMines_Enter);
            this.txtMines.Leave += new System.EventHandler(this.txtMines_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label4.Location = new System.Drawing.Point(7, 285);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 21);
            this.label4.TabIndex = 66;
            this.label4.Text = "Moins le montant payé";
            // 
            // ckMines
            // 
            this.ckMines.AutoSize = true;
            this.ckMines.Location = new System.Drawing.Point(11, 325);
            this.ckMines.Name = "ckMines";
            this.ckMines.Size = new System.Drawing.Size(15, 14);
            this.ckMines.TabIndex = 67;
            this.ckMines.UseVisualStyleBackColor = true;
            this.ckMines.CheckedChanged += new System.EventHandler(this.ckMines_CheckedChanged);
            // 
            // FormAvance
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.CancelButton = this.btnAnnuler;
            this.ClientSize = new System.Drawing.Size(530, 555);
            this.Controls.Add(this.ckMines);
            this.Controls.Add(this.txtMines);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNomClient);
            this.Controls.Add(this.lblRestPrixClient);
            this.Controls.Add(this.lblAvancePrixClient);
            this.Controls.Add(this.btnAnnuler);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblTotalPrixClient);
            this.Controls.Add(this.lblDateClient);
            this.Controls.Add(this.lblIDClient);
            this.Controls.Add(this.txtPrixCLient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblav);
            this.Controls.Add(this.lblPPr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormAvance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avance";
            this.Load += new System.EventHandler(this.Avance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtPrixCLient;
        private System.Windows.Forms.Label lblIDClient;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDateClient;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblTotalPrixClient;
        private System.Windows.Forms.Label lblPPr;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnAnnuler;
        private System.Windows.Forms.Label lblav;
        private System.Windows.Forms.Label lblAvancePrixClient;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblRestPrixClient;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNomClient;
        private System.Windows.Forms.TextBox txtMines;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckMines;
    }
}