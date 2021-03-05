using System;
using System.Windows.Forms;

namespace Aiche_Bois
{
    public partial class f_message : Form
    {
        public String Message = "";
        String Title = "";
        bool OK;
        bool Yes;
        bool No;
        FontAwesome.Sharp.IconChar icon;

        /// <summary>
        /// COnstructor:: by default
        /// </summary>
        public f_message()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor:: String Message, bool OK
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="OK"></param>
        public f_message(String Message, bool OK)
        {
            this.Message = Message;
            this.OK = OK;
            InitializeComponent();
        }

        /// <summary>
        /// Constructor:: String Message, String Title, bool OK
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Title"></param>
        /// <param name="OK"></param>
        public f_message(String Message, String Title, bool OK)
        {
            this.Title = Title;
            this.Message = Message;
            this.OK = OK;
            InitializeComponent();
        }

        /// <summary>
        /// Constructor:: String Message, String Title, bool OK, FontAwesome.Sharp.IconChar icon
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Title"></param>
        /// <param name="OK"></param>
        /// <param name="icon"></param>
        public f_message(String Message, String Title, bool OK, FontAwesome.Sharp.IconChar icon)
        {
            this.Title = Title;
            this.Message = Message;
            this.OK = OK;
            this.icon = icon;
            InitializeComponent();
        }

        /// <summary>
        /// Constructor:: String Message, String Title, bool Yes, bool No, FontAwesome.Sharp.IconChar icon
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Title"></param>
        /// <param name="Yes"></param>
        /// <param name="No"></param>
        /// <param name="icon"></param>
        public f_message(String Message, String Title, bool Yes, bool No, FontAwesome.Sharp.IconChar icon)
        {
            this.Title = Title;
            this.Message = Message;
            this.No = No;
            this.Yes = Yes;
            this.icon = icon;
            InitializeComponent();
        }

        /// <summary>
        /// on the load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormShowFacture_Load(object sender, EventArgs e)
        {
            if (icon == FontAwesome.Sharp.IconChar.Ban)
            {
                i_message.IconColor = System.Drawing.Color.FromArgb(217, 15, 70);
            }
            else if (icon == FontAwesome.Sharp.IconChar.ExclamationTriangle)
            {
                i_message.IconColor = System.Drawing.Color.FromArgb(240, 205, 01);
            }
            else if (icon == FontAwesome.Sharp.IconChar.CheckCircle)
            {
                i_message.IconColor = System.Drawing.Color.FromArgb(11, 244, 132);
            }
            else
            {
                i_message.IconColor = System.Drawing.Color.White;
            }

            l_show_message.Text = Message;
            this.Text = Title;
            b_ok.Visible = OK;
            b_yes.Visible = No;
            b_no.Visible = Yes;

            i_message.IconChar = icon;
        }
    }
}
