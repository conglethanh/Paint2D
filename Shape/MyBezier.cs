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
    public class MyBezier : MyPolyline
    {


        public MyBezier() : base() { }
        public MyBezier(List<Point> lsPoint, MyPaintSettings settings, int _name)
            : base(lsPoint, settings, _name)
        {

        }

        public MyBezier(MyBezier s, int numeric)
            : base(s, numeric)
        {
            this.polyPoint = new List<Point>(s.polyPoint);
        }

        public override void drawWithAngle(ToolSettings toolsettings, Point p)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), p);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            Pen pen = new Pen(new SolidBrush(outlineColor), width);
            pen.DashStyle = lineStyle;

            //draw the new line
            if (polyPoint.Count == 4)
                g.DrawBezier(pen, polyPoint[0], polyPoint[1], polyPoint[2], polyPoint[3]);

            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), p);
            g.Transform = matrix;
        }


        public override ShapeType getType()
        {
            return ShapeType.bezier;
        }
    }
}