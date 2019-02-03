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
    public partial class FormInputSizeWorkplace : Form
    {
        public FormInputSizeWorkplace()
        {
            InitializeComponent();
        }

        private void FormInputSizeWorkplace_Load(object sender, EventArgs e)
        {

        }

        public int getSizeWidth()
        {
            return (int)numericUpDown_width.Value;
        }

        public int getSizeHeight()
        {
            return (int)numericUpDown_height.Value;
        }
    }
}
