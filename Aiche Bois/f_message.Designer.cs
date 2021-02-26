
namespace Aiche_Bois
{
    partial class f_message
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f_message));
            this.b_ok = new System.Windows.Forms.Button();
            this.b_no = new System.Windows.Forms.Button();
            this.b_yes = new System.Windows.Forms.Button();
            this.i_message = new FontAwesome.Sharp.IconPictureBox();
            this.l_show_message = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.i_message)).BeginInit();
            this.SuspendLayout();
            // 
            // b_ok
            // 
            this.b_ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.b_ok.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.b_ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_ok.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold);
            this.b_ok.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_ok.Location = new System.Drawing.Point(365, 256);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(207, 43);
            this.b_ok.TabIndex = 262;
            this.b_ok.Text = "D\'ACCORD";
            this.toolTip1.SetToolTip(this.b_ok, "click d\'accord pour retour à la page");
            this.b_ok.UseVisualStyleBackColor = false;
            this.b_ok.Visible = false;
            // 
            // b_no
            // 
            this.b_no.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.b_no.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_no.DialogResult = System.Windows.Forms.DialogResult.No;
            this.b_no.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_no.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_no.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_no.Location = new System.Drawing.Point(365, 256);
            this.b_no.Name = "b_no";
            this.b_no.Size = new System.Drawing.Size(207, 43);
            this.b_no.TabIndex = 263;
            this.b_no.Text = "Non";
            this.toolTip1.SetToolTip(this.b_no, "Si vous n\'êtes pas d\'accord, cliquez sur Non.");
            this.b_no.UseVisualStyleBackColor = false;
            this.b_no.Visible = false;
            // 
            // b_yes
            // 
            this.b_yes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(63)))));
            this.b_yes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.b_yes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.b_yes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.b_yes.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.b_yes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.b_yes.Location = new System.Drawing.Point(149, 256);
            this.b_yes.Name = "b_yes";
            this.b_yes.Size = new System.Drawing.Size(202, 43);
            this.b_yes.TabIndex = 264;
            this.b_yes.Text = "Oui";
            this.toolTip1.SetToolTip(this.b_yes, "Si vous acceptez, cliquez sur Oui");
            this.b_yes.UseVisualStyleBackColor = false;
            this.b_yes.Visible = false;
            // 
            // i_message
            // 
            this.i_message.BackColor = System.Drawing.Color.Transparent;
            this.i_message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.i_message.IconChar = FontAwesome.Sharp.IconChar.ThumbsUp;
            this.i_message.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.i_message.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.i_message.IconSize = 62;
            this.i_message.Location = new System.Drawing.Point(23, 120);
            this.i_message.Name = "i_message";
            this.i_message.Size = new System.Drawing.Size(62, 70);
            this.i_message.TabIndex = 265;
            this.i_message.TabStop = false;
            this.i_message.UseGdi = true;
            // 
            // l_show_message
            // 
            this.l_show_message.BackColor = System.Drawing.Color.Transparent;
            this.l_show_message.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.l_show_message.Font = new System.Drawing.Font("Ubuntu", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l_show_message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(244)))), ((int)(((byte)(228)))));
            this.l_show_message.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.l_show_message.Location = new System.Drawing.Point(150, 25);
            this.l_show_message.Name = "l_show_message";
            this.l_show_message.Size = new System.Drawing.Size(428, 202);
            this.l_show_message.TabIndex = 217;
            this.l_show_message.Text = "Nom du client";
            this.l_show_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // f_message
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.ClientSize = new System.Drawing.Size(587, 311);
            this.Controls.Add(this.i_message);
            this.Controls.Add(this.b_yes);
            this.Controls.Add(this.b_no);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.l_show_message);
            this.Font = new System.Drawing.Font("Ubuntu", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "f_message";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Detail du facture";
            this.Load += new System.EventHandler(this.FormShowFacture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.i_message)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button b_ok;
        private System.Windows.Forms.Button b_no;
        private System.Windows.Forms.Button b_yes;
        private FontAwesome.Sharp.IconPictureBox i_message;
        private System.Windows.Forms.Label l_show_message;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}