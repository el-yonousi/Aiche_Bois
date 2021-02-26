
namespace Aiche_Bois
{
    partial class f_user
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_user));
            this.b_connection = new FontAwesome.Sharp.IconButton();
            this.b_cancel = new FontAwesome.Sharp.IconButton();
            this.t_pass_word = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.b_show_pass_word = new FontAwesome.Sharp.IconButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.t_user_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // b_connection
            // 
            this.b_connection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_connection.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.b_connection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_connection.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_connection.ForeColor = System.Drawing.Color.Transparent;
            this.b_connection.IconChar = FontAwesome.Sharp.IconChar.None;
            this.b_connection.IconColor = System.Drawing.Color.Black;
            this.b_connection.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.b_connection.Location = new System.Drawing.Point(27, 431);
            this.b_connection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_connection.Name = "b_connection";
            this.b_connection.Size = new System.Drawing.Size(174, 37);
            this.b_connection.TabIndex = 1;
            this.b_connection.Text = "Connexion";
            this.toolTip1.SetToolTip(this.b_connection, "Un bouton pour se connecter à l\'application, si le mot de passe est correct");
            this.b_connection.UseVisualStyleBackColor = true;
            this.b_connection.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // b_cancel
            // 
            this.b_cancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.b_cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_cancel.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_cancel.ForeColor = System.Drawing.Color.White;
            this.b_cancel.IconChar = FontAwesome.Sharp.IconChar.None;
            this.b_cancel.IconColor = System.Drawing.Color.Black;
            this.b_cancel.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.b_cancel.Location = new System.Drawing.Point(244, 431);
            this.b_cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.b_cancel.Name = "b_cancel";
            this.b_cancel.Size = new System.Drawing.Size(174, 37);
            this.b_cancel.TabIndex = 2;
            this.b_cancel.Text = "Annuler";
            this.toolTip1.SetToolTip(this.b_cancel, "le bouton annuler");
            this.b_cancel.UseVisualStyleBackColor = true;
            this.b_cancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // t_pass_word
            // 
            this.t_pass_word.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t_pass_word.Font = new System.Drawing.Font("Ubuntu", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_pass_word.Location = new System.Drawing.Point(27, 354);
            this.t_pass_word.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.t_pass_word.Name = "t_pass_word";
            this.t_pass_word.Size = new System.Drawing.Size(391, 32);
            this.t_pass_word.TabIndex = 0;
            this.toolTip1.SetToolTip(this.t_pass_word, "Le champ qui veut entrer le mot de passe de la base de données");
            this.t_pass_word.UseSystemPasswordChar = true;
            this.t_pass_word.TextChanged += new System.EventHandler(this.txtPassWord_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(24, 315);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Entrer le Mot de Passe";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(118, 23);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(207, 126);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(168, 162);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Aiche Bois";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Font = new System.Drawing.Font("Nirmala UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(23, 394);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 7;
            // 
            // b_show_pass_word
            // 
            this.b_show_pass_word.BackColor = System.Drawing.Color.White;
            this.b_show_pass_word.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_show_pass_word.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_show_pass_word.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            this.b_show_pass_word.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.b_show_pass_word.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.b_show_pass_word.IconSize = 25;
            this.b_show_pass_word.Location = new System.Drawing.Point(371, 354);
            this.b_show_pass_word.Name = "b_show_pass_word";
            this.b_show_pass_word.Size = new System.Drawing.Size(47, 32);
            this.b_show_pass_word.TabIndex = 8;
            this.toolTip1.SetToolTip(this.b_show_pass_word, "si veux masquer/afficher le mote de passe");
            this.b_show_pass_word.UseVisualStyleBackColor = false;
            this.b_show_pass_word.Click += new System.EventHandler(this.showPassWord_Click);
            // 
            // t_user_name
            // 
            this.t_user_name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.t_user_name.Font = new System.Drawing.Font("Ubuntu", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.t_user_name.Location = new System.Drawing.Point(27, 254);
            this.t_user_name.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.t_user_name.Name = "t_user_name";
            this.t_user_name.ReadOnly = true;
            this.t_user_name.Size = new System.Drawing.Size(391, 32);
            this.t_user_name.TabIndex = 9;
            this.toolTip1.SetToolTip(this.t_user_name, "Le champ qui veut entrer le mot de passe de la base de données");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 215);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Nom d\'utilisateur";
            // 
            // f_user
            // 
            this.AcceptButton = this.b_connection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.ClientSize = new System.Drawing.Size(443, 501);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.t_user_name);
            this.Controls.Add(this.b_show_pass_word);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.b_cancel);
            this.Controls.Add(this.b_connection);
            this.Controls.Add(this.t_pass_word);
            this.Font = new System.Drawing.Font("Ubuntu", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "f_user";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aiche Bois";
            this.Load += new System.EventHandler(this.FormUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FontAwesome.Sharp.IconButton b_connection;
        private FontAwesome.Sharp.IconButton b_cancel;
        private System.Windows.Forms.TextBox t_pass_word;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblError;
        private FontAwesome.Sharp.IconButton b_show_pass_word;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox t_user_name;
    }
}