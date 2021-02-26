namespace Aiche_Bois
{
    partial class frm_ajout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ajout));
            this.lt_type_bois_pvc = new System.Windows.Forms.ListBox();
            this.t_type_bois_pvc = new System.Windows.Forms.TextBox();
            this.b_add_to_list = new FontAwesome.Sharp.IconButton();
            this.b_delete_type = new FontAwesome.Sharp.IconButton();
            this.b_back = new FontAwesome.Sharp.IconButton();
            this.b_save_list = new FontAwesome.Sharp.IconButton();
            this.lblEcris = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // lt_type_bois_pvc
            // 
            this.lt_type_bois_pvc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.lt_type_bois_pvc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lt_type_bois_pvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lt_type_bois_pvc.FormattingEnabled = true;
            this.lt_type_bois_pvc.ItemHeight = 20;
            this.lt_type_bois_pvc.Location = new System.Drawing.Point(10, 82);
            this.lt_type_bois_pvc.Name = "lt_type_bois_pvc";
            this.lt_type_bois_pvc.Size = new System.Drawing.Size(511, 422);
            this.lt_type_bois_pvc.TabIndex = 1;
            this.lt_type_bois_pvc.TabStop = false;
            this.toolTip1.SetToolTip(this.lt_type_bois_pvc, "la liste des types");
            // 
            // t_type_bois_pvc
            // 
            this.t_type_bois_pvc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.t_type_bois_pvc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t_type_bois_pvc.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.t_type_bois_pvc.Font = new System.Drawing.Font("Nirmala UI", 19F, System.Drawing.FontStyle.Bold);
            this.t_type_bois_pvc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.t_type_bois_pvc.Location = new System.Drawing.Point(10, 32);
            this.t_type_bois_pvc.Name = "t_type_bois_pvc";
            this.t_type_bois_pvc.Size = new System.Drawing.Size(465, 41);
            this.t_type_bois_pvc.TabIndex = 0;
            this.toolTip1.SetToolTip(this.t_type_bois_pvc, "Saisir le type");
            this.t_type_bois_pvc.TextChanged += new System.EventHandler(this.txtCmb_TextChanged);
            // 
            // b_add_to_list
            // 
            this.b_add_to_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_add_to_list.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_add_to_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_add_to_list.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_add_to_list.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.b_add_to_list.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_add_to_list.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.b_add_to_list.IconSize = 35;
            this.b_add_to_list.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.b_add_to_list.Location = new System.Drawing.Point(481, 32);
            this.b_add_to_list.Name = "b_add_to_list";
            this.b_add_to_list.Size = new System.Drawing.Size(40, 41);
            this.b_add_to_list.TabIndex = 1;
            this.b_add_to_list.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip1.SetToolTip(this.b_add_to_list, "Ajouter à la liste");
            this.b_add_to_list.UseVisualStyleBackColor = false;
            this.b_add_to_list.Click += new System.EventHandler(this.btnAddLstCmb_Click);
            // 
            // b_delete_type
            // 
            this.b_delete_type.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_delete_type.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.b_delete_type.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_delete_type.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_delete_type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_delete_type.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.b_delete_type.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_delete_type.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.b_delete_type.IconSize = 30;
            this.b_delete_type.Location = new System.Drawing.Point(196, 510);
            this.b_delete_type.Name = "b_delete_type";
            this.b_delete_type.Size = new System.Drawing.Size(139, 45);
            this.b_delete_type.TabIndex = 3;
            this.toolTip1.SetToolTip(this.b_delete_type, "Supprimer de la base de données");
            this.b_delete_type.UseVisualStyleBackColor = false;
            this.b_delete_type.Click += new System.EventHandler(this.btnDeleteMesure_Click);
            // 
            // b_back
            // 
            this.b_back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_back.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_back.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_back.IconChar = FontAwesome.Sharp.IconChar.ArrowAltCircleRight;
            this.b_back.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_back.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.b_back.IconSize = 30;
            this.b_back.Location = new System.Drawing.Point(382, 510);
            this.b_back.Name = "b_back";
            this.b_back.Size = new System.Drawing.Size(139, 45);
            this.b_back.TabIndex = 4;
            this.toolTip1.SetToolTip(this.b_back, "Annuler");
            this.b_back.UseVisualStyleBackColor = false;
            this.b_back.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // b_save_list
            // 
            this.b_save_list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_save_list.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_save_list.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_save_list.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_save_list.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.b_save_list.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_save_list.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.b_save_list.IconSize = 30;
            this.b_save_list.Location = new System.Drawing.Point(10, 510);
            this.b_save_list.Name = "b_save_list";
            this.b_save_list.Size = new System.Drawing.Size(139, 45);
            this.b_save_list.TabIndex = 2;
            this.b_save_list.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            this.toolTip1.SetToolTip(this.b_save_list, "Enregistrer vers la base de données");
            this.b_save_list.UseVisualStyleBackColor = false;
            this.b_save_list.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblEcris
            // 
            this.lblEcris.AutoSize = true;
            this.lblEcris.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.lblEcris.Location = new System.Drawing.Point(8, 9);
            this.lblEcris.Name = "lblEcris";
            this.lblEcris.Size = new System.Drawing.Size(69, 20);
            this.lblEcris.TabIndex = 44;
            this.lblEcris.Text = "Saiser le";
            // 
            // frm_ajout
            // 
            this.AcceptButton = this.b_save_list;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.CancelButton = this.b_back;
            this.ClientSize = new System.Drawing.Size(530, 565);
            this.Controls.Add(this.b_back);
            this.Controls.Add(this.b_save_list);
            this.Controls.Add(this.b_delete_type);
            this.Controls.Add(this.b_add_to_list);
            this.Controls.Add(this.t_type_bois_pvc);
            this.Controls.Add(this.lt_type_bois_pvc);
            this.Controls.Add(this.lblEcris);
            this.Font = new System.Drawing.Font("Ubuntu", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_ajout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajout";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAjout_FormClosing);
            this.Load += new System.EventHandler(this.Ajout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lt_type_bois_pvc;
        private System.Windows.Forms.TextBox t_type_bois_pvc;
        private FontAwesome.Sharp.IconButton b_add_to_list;
        private FontAwesome.Sharp.IconButton b_delete_type;
        private FontAwesome.Sharp.IconButton b_back;
        private FontAwesome.Sharp.IconButton b_save_list;
        private System.Windows.Forms.Label lblEcris;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}