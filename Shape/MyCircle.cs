using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1612829_1612842
{
    public class MyCircle : MyRectangle
    {
        public MyCircle() : base() { }

        public MyCircle(Point _startPoint, Point _endPoint, MyPaintSettings settings, int _name)
            : base(_startPoint, _endPoint, settings, _name)
        {
        }
        public MyCircle(MyCircle s, int numeric)
            : base(s, numeric)
        {

        }
       

        public override ShapeType getType()
        {
            return ShapeType.circle;
        }

       

        public override void drawWithAngle(ToolSettings toolsettings, Point mid)
        {

            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), mid);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            Brush fill = getBrush();
            Rectangle rect = CircleTool.getSquareFrom2Points(startPoint, endPoint);
            g.FillEllipse(fill, rect);

            Pen pen = new Pen(new SolidBrush(outlineColor), width);
            pen.DashStyle = lineStyle;
            //draw the new rect
            g.DrawEllipse(pen, rect);
            toolsettings.pictureBox.Invalidate();

            matrix.RotateAt(-(float)(angleIn), mid);
            g.Transform = matrix;
        }


    }
}