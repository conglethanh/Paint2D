using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class ToolSettings
    {
        public Bitmap bitmap;
        public PictureBox pictureBox;
        public MyPaintSettings settings;

        public ToolSettings(Bitmap bmp, PictureBox picBox, MyPaintSettings settings)
        {
            bitmap = bmp;
            pictureBox = picBox;
            this.settings = settings;
        }
    }
}
