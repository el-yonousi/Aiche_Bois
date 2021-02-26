namespace Aiche_Bois
{
    partial class f_avance
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_avance));
            this.t_price_add_cLient = new System.Windows.Forms.TextBox();
            this.l_id_client = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.l_date_client = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.l_price_total_client = new System.Windows.Forms.Label();
            this.lblPPr = new System.Windows.Forms.Label();
            this.b_save = new System.Windows.Forms.Button();
            this.b_cancel = new System.Windows.Forms.Button();
            this.lblav = new System.Windows.Forms.Label();
            this.l_price_avance_client = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.l_price_rest_client = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.l_nom_client = new System.Windows.Forms.Label();
            this.t_mines_price = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ckMines = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // t_price_add_cLient
            // 
            this.t_price_add_cLient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.t_price_add_cLient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t_price_add_cLient.Font = new System.Drawing.Font("Ubuntu", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_price_add_cLient.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.t_price_add_cLient.Location = new System.Drawing.Point(12, 406);
            this.t_price_add_cLient.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.t_price_add_cLient.Name = "t_price_add_cLient";
            this.t_price_add_cLient.Size = new System.Drawing.Size(506, 35);
            this.t_price_add_cLient.TabIndex = 0;
            this.toolTip1.SetToolTip(this.t_price_add_cLient, "le champ qui veut saisir le prix d\'avance a client");
            this.t_price_add_cLient.TextChanged += new System.EventHandler(this.txtPrix_TextChanged);
            this.t_price_add_cLient.Enter += new System.EventHandler(this.txtMines_Enter);
            this.t_price_add_cLient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrix_KeyPress);
            this.t_price_add_cLient.Leave += new System.EventHandler(this.txtMines_Leave);
            // 
            // l_id_client
            // 
            this.l_id_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_id_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_id_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_id_client.Location = new System.Drawing.Point(11, 32);
            this.l_id_client.Name = "l_id_client";
            this.l_id_client.Size = new System.Drawing.Size(216, 37);
            this.l_id_client.TabIndex = 50;
            this.l_id_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_id_client, "le Numéro  de client");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label2.Location = new System.Drawing.Point(7, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 21);
            this.label2.TabIndex = 51;
            this.label2.Text = "Numéro du Client";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.label3.Location = new System.Drawing.Point(282, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 21);
            this.label3.TabIndex = 53;
            this.label3.Text = "La Date du Client";
            // 
            // l_date_client
            // 
            this.l_date_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_date_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_date_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_date_client.Location = new System.Drawing.Point(286, 32);
            this.l_date_client.Name = "l_date_client";
            this.l_date_client.Size = new System.Drawing.Size(232, 37);
            this.l_date_client.TabIndex = 52;
            this.l_date_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_date_client, "la date de client");
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
            // l_price_total_client
            // 
            this.l_price_total_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_price_total_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_price_total_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_price_total_client.Location = new System.Drawing.Point(286, 126);
            this.l_price_total_client.Name = "l_price_total_client";
            this.l_price_total_client.Size = new System.Drawing.Size(232, 37);
            this.l_price_total_client.TabIndex = 54;
            this.l_price_total_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_price_total_client, "prixe total du client");
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
            // b_save
            // 
            this.b_save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_save.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_save.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_save.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_save.Location = new System.Drawing.Point(10, 498);
            this.b_save.Name = "b_save";
            this.b_save.Size = new System.Drawing.Size(232, 44);
            this.b_save.TabIndex = 1;
            this.b_save.Text = "SAUVEGARDER";
            this.toolTip1.SetToolTip(this.b_save, "sauvegarder le prix à la base de donnée");
            this.b_save.UseVisualStyleBackColor = false;
            this.b_save.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // b_cancel
            // 
            this.b_cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_cancel.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_cancel.Location = new System.Drawing.Point(286, 498);
            this.b_cancel.Name = "b_cancel";
            this.b_cancel.Size = new System.Drawing.Size(232, 44);
            this.b_cancel.TabIndex = 2;
            this.b_cancel.Text = "RETOUR";
            this.toolTip1.SetToolTip(this.b_cancel, "retournez à la page précédente");
            this.b_cancel.UseVisualStyleBackColor = false;
            this.b_cancel.Click += new System.EventHandler(this.btnAnnuler_Click);
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
            // l_price_avance_client
            // 
            this.l_price_avance_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_price_avance_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_price_avance_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_price_avance_client.Location = new System.Drawing.Point(286, 220);
            this.l_price_avance_client.Name = "l_price_avance_client";
            this.l_price_avance_client.Size = new System.Drawing.Size(232, 37);
            this.l_price_avance_client.TabIndex = 59;
            this.l_price_avance_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_price_avance_client, "prix paye de client");
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
            // l_price_rest_client
            // 
            this.l_price_rest_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_price_rest_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_price_rest_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_price_rest_client.Location = new System.Drawing.Point(11, 220);
            this.l_price_rest_client.Name = "l_price_rest_client";
            this.l_price_rest_client.Size = new System.Drawing.Size(232, 37);
            this.l_price_rest_client.TabIndex = 61;
            this.l_price_rest_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_price_rest_client, "prix non paye de client");
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
            // l_nom_client
            // 
            this.l_nom_client.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_nom_client.Font = new System.Drawing.Font("Ubuntu", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_nom_client.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.l_nom_client.Location = new System.Drawing.Point(11, 126);
            this.l_nom_client.Name = "l_nom_client";
            this.l_nom_client.Size = new System.Drawing.Size(216, 37);
            this.l_nom_client.TabIndex = 63;
            this.l_nom_client.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.l_nom_client, "le nom du client");
            // 
            // t_mines_price
            // 
            this.t_mines_price.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.t_mines_price.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t_mines_price.Enabled = false;
            this.t_mines_price.Font = new System.Drawing.Font("Ubuntu", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_mines_price.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.t_mines_price.Location = new System.Drawing.Point(33, 314);
            this.t_mines_price.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.t_mines_price.Name = "t_mines_price";
            this.t_mines_price.Size = new System.Drawing.Size(485, 35);
            this.t_mines_price.TabIndex = 65;
            this.toolTip1.SetToolTip(this.t_mines_price, "le champ qui veut saisir le prix de réduire à client");
            this.t_mines_price.TextChanged += new System.EventHandler(this.txtMines_TextChanged);
            this.t_mines_price.Enter += new System.EventHandler(this.txtMines_Enter);
            this.t_mines_price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPrix_KeyPress);
            this.t_mines_price.Leave += new System.EventHandler(this.txtMines_Leave);
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
            this.toolTip1.SetToolTip(this.ckMines, "bouton pour activer le champs");
            this.ckMines.UseVisualStyleBackColor = true;
            this.ckMines.CheckedChanged += new System.EventHandler(this.ckMines_CheckedChanged);
            // 
            // f_avance
            // 
            this.AcceptButton = this.b_save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.CancelButton = this.b_cancel;
            this.ClientSize = new System.Drawing.Size(530, 555);
            this.Controls.Add(this.ckMines);
            this.Controls.Add(this.t_mines_price);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.l_nom_client);
            this.Controls.Add(this.l_price_rest_client);
            this.Controls.Add(this.l_price_avance_client);
            this.Controls.Add(this.b_cancel);
            this.Controls.Add(this.b_save);
            this.Controls.Add(this.l_price_total_client);
            this.Controls.Add(this.l_date_client);
            this.Controls.Add(this.l_id_client);
            this.Controls.Add(this.t_price_add_cLient);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblav);
            this.Controls.Add(this.lblPPr);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Ubuntu", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "f_avance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Avance";
            this.Load += new System.EventHandler(this.Avance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox t_price_add_cLient;
        private System.Windows.Forms.Label l_id_client;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label l_date_client;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label l_price_total_client;
        private System.Windows.Forms.Label lblPPr;
        private System.Windows.Forms.Button b_save;
        private System.Windows.Forms.Button b_cancel;
        private System.Windows.Forms.Label lblav;
        private System.Windows.Forms.Label l_price_avance_client;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label l_price_rest_client;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label l_nom_client;
        private System.Windows.Forms.TextBox t_mines_price;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckMines;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}