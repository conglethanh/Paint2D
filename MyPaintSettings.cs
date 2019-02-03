using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public interface MyPaintSettings
    {
       
        int Width
        {
            get;
        }

       
        Color PrimaryColor
        {
            get;
        }

        Color SecondaryColor
        {
            get;
        }

        Color OutlineColor
        { get;
        }


        DashStyle LineStyle
        {
            get;
        }

        FillStyle FillStyle
        {
            get;
        }

        HatchStyle HatchStyle
        {
            get;
        }
        Image TextureBrushImage
        {
            get;
        }

        Font font
        {
            get;
        }
    }
}
