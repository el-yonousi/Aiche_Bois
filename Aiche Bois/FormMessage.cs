using System;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class FormMessage : Form
    {
        public String Message = "";
        String Title = "";
        bool OK;
        bool Yes;
        bool No;
        FontAwesome.Sharp.IconChar icon;
        public FormMessage()
        {
            InitializeComponent();
        }

        public FormMessage(String Message, bool OK)
        {
            this.Message = Message;
            this.OK = OK;
            InitializeComponent();
        }

        public FormMessage(String Message, String Title, bool OK)
        {
            this.Title = Title;
            this.Message = Message;
            this.OK = OK;
            InitializeComponent();
        }

        public FormMessage(String Message, String Title, bool OK, FontAwesome.Sharp.IconChar icon)
        {
            this.Title = Title;
            this.Message = Message;
            this.OK = OK;
            this.icon = icon;
            InitializeComponent();
        }

        public FormMessage(String Message, String Title, bool Yes, bool No, FontAwesome.Sharp.IconChar icon)
        {
            this.Title = Title;
            this.Message = Message;
            this.No = No;
            this.Yes = Yes;
            this.icon = icon;
            InitializeComponent();
        }

        private void FormShowFacture_Load(object sender, EventArgs e)
        {
            if (icon == FontAwesome.Sharp.IconChar.Ban)
            {
                iconCharMessage.IconColor = System.Drawing.Color.FromArgb(217, 15, 70);
            }
            else if (icon == FontAwesome.Sharp.IconChar.ExclamationTriangle)
            {
                iconCharMessage.IconColor = System.Drawing.Color.FromArgb(240, 205, 01);
            }
            else if (icon == FontAwesome.Sharp.IconChar.CheckCircle)
            {
                iconCharMessage.IconColor = System.Drawing.Color.FromArgb(11, 244, 132);
            }
            else
            {
                iconCharMessage.IconColor = System.Drawing.Color.White;
            }

            lblShowMessage.Text = Message;
            this.Text = Title;
            btnOK.Visible = OK;
            btnYes.Visible = No;
            btnNo.Visible = Yes;

            iconCharMessage.IconChar = icon;
        }
    }
}
