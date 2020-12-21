namespace Aiche_Bois
{
    partial class FormAjout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAjout));
            this.lstCmb = new System.Windows.Forms.ListBox();
            this.txtCmb = new System.Windows.Forms.TextBox();
            this.btnAddLstCmb = new FontAwesome.Sharp.IconButton();
            this.btnDeleteMesure = new FontAwesome.Sharp.IconButton();
            this.btnBack = new FontAwesome.Sharp.IconButton();
            this.btnSave = new FontAwesome.Sharp.IconButton();
            this.lblEcris = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstCmb
            // 
            this.lstCmb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstCmb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.lstCmb.FormattingEnabled = true;
            this.lstCmb.ItemHeight = 20;
            this.lstCmb.Location = new System.Drawing.Point(9, 83);
            this.lstCmb.Name = "lstCmb";
            this.lstCmb.Size = new System.Drawing.Size(636, 480);
            this.lstCmb.TabIndex = 1;
            this.lstCmb.TabStop = false;
            // 
            // txtCmb
            // 
            this.txtCmb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCmb.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCmb.Font = new System.Drawing.Font("Nirmala UI", 19F, System.Drawing.FontStyle.Bold);
            this.txtCmb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.txtCmb.Location = new System.Drawing.Point(9, 33);
            this.txtCmb.Name = "txtCmb";
            this.txtCmb.Size = new System.Drawing.Size(590, 34);
            this.txtCmb.TabIndex = 0;
            this.txtCmb.TextChanged += new System.EventHandler(this.txtCmb_TextChanged);
            // 
            // btnAddLstCmb
            // 
            this.btnAddLstCmb.BackColor = System.Drawing.Color.White;
            this.btnAddLstCmb.FlatAppearance.BorderSize = 0;
            this.btnAddLstCmb.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLstCmb.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnAddLstCmb.ForeColor = System.Drawing.Color.White;
            this.btnAddLstCmb.IconChar = FontAwesome.Sharp.IconChar.PlusCircle;
            this.btnAddLstCmb.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.btnAddLstCmb.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAddLstCmb.IconSize = 35;
            this.btnAddLstCmb.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddLstCmb.Location = new System.Drawing.Point(605, 32);
            this.btnAddLstCmb.Name = "btnAddLstCmb";
            this.btnAddLstCmb.Rotation = 0D;
            this.btnAddLstCmb.Size = new System.Drawing.Size(40, 36);
            this.btnAddLstCmb.TabIndex = 1;
            this.btnAddLstCmb.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnAddLstCmb.UseVisualStyleBackColor = false;
            this.btnAddLstCmb.Click += new System.EventHandler(this.btnAddLstCmb_Click);
            // 
            // btnDeleteMesure
            // 
            this.btnDeleteMesure.BackColor = System.Drawing.Color.White;
            this.btnDeleteMesure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteMesure.FlatAppearance.BorderSize = 0;
            this.btnDeleteMesure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteMesure.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnDeleteMesure.ForeColor = System.Drawing.Color.White;
            this.btnDeleteMesure.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnDeleteMesure.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.btnDeleteMesure.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnDeleteMesure.IconSize = 30;
            this.btnDeleteMesure.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeleteMesure.Location = new System.Drawing.Point(230, 578);
            this.btnDeleteMesure.Name = "btnDeleteMesure";
            this.btnDeleteMesure.Rotation = 0D;
            this.btnDeleteMesure.Size = new System.Drawing.Size(193, 36);
            this.btnDeleteMesure.TabIndex = 3;
            this.btnDeleteMesure.UseVisualStyleBackColor = false;
            this.btnDeleteMesure.Click += new System.EventHandler(this.btnDeleteMesure_Click);
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.White;
            this.btnBack.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.IconChar = FontAwesome.Sharp.IconChar.ArrowAltCircleRight;
            this.btnBack.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.btnBack.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnBack.IconSize = 30;
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBack.Location = new System.Drawing.Point(451, 578);
            this.btnBack.Name = "btnBack";
            this.btnBack.Rotation = 0D;
            this.btnBack.Size = new System.Drawing.Size(193, 36);
            this.btnBack.TabIndex = 4;
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.btnSave.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.btnSave.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnSave.IconSize = 30;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(9, 578);
            this.btnSave.Name = "btnSave";
            this.btnSave.Rotation = 0D;
            this.btnSave.Size = new System.Drawing.Size(193, 36);
            this.btnSave.TabIndex = 2;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblEcris
            // 
            this.lblEcris.AutoSize = true;
            this.lblEcris.Location = new System.Drawing.Point(5, 10);
            this.lblEcris.Name = "lblEcris";
            this.lblEcris.Size = new System.Drawing.Size(66, 20);
            this.lblEcris.TabIndex = 44;
            this.lblEcris.Text = "Saiser le";
            // 
            // FormAjout
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(170)))), ((int)(((byte)(0)))));
            this.CancelButton = this.btnBack;
            this.ClientSize = new System.Drawing.Size(656, 626);
            this.Controls.Add(this.lblEcris);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnDeleteMesure);
            this.Controls.Add(this.btnAddLstCmb);
            this.Controls.Add(this.txtCmb);
            this.Controls.Add(this.lstCmb);
            this.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAjout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajout";
            this.Load += new System.EventHandler(this.Ajout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox lstCmb;
        private System.Windows.Forms.TextBox txtCmb;
        private FontAwesome.Sharp.IconButton btnAddLstCmb;
        private FontAwesome.Sharp.IconButton btnDeleteMesure;
        private FontAwesome.Sharp.IconButton btnBack;
        private FontAwesome.Sharp.IconButton btnSave;
        private System.Windows.Forms.Label lblEcris;
    }
}