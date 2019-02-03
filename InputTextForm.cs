using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public partial class InputTextForm : Form
    {
        public InputTextForm()
        {
            InitializeComponent();
        }

        public InputTextForm(String t, Font f)
        {
            InitializeComponent();
            inputText.Font = f;
            inputText.Text = t;
        }

        public static String text = "";
        public static Font font=new Font("Arial",16);
        private void inputText_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button_font_Click(object sender, EventArgs e)
        {
            FontDialog fontDlg = new FontDialog();
            fontDlg.Font = new Font("Arial", 16);

            if (fontDlg.ShowDialog() == DialogResult.OK)
                inputText.Font = fontDlg.Font;
        }

        public Font getFont()
        {
            font = inputText.Font;
            return inputText.Font;
        }

        public String getText()
        {
            text = inputText.Text;
            return inputText.Text;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
