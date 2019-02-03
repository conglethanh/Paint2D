using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1612829_1612842
{
    public class MyHyperbol : MyParabol
    {

        public MyHyperbol() : base() { }
        public MyHyperbol(List<Point> lsPoint, MyPaintSettings settings, int _name)
            : base(lsPoint, settings, _name)
        {

        }
        public MyHyperbol(MyHyperbol s, int numeric)
            : base(s, numeric)
        {
            
        }

        public override ShapeType getType()
        {
            return ShapeType.hyperbol;
        }

        public override void drawWithAngle(ToolSettings toolsettings, Point p)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), p);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            Pen pen = new Pen(new SolidBrush(outlineColor), width);
            pen.DashStyle = lineStyle;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            List<PointF> tempLeft = new List<PointF>();
            for (int i = 0; i < polyPoint.Count / 2 - 2; i++)
            {
                tempLeft.Add(polyPoint[i]);
            }
            //draw 
            if (polyPoint.Count > 2)
                g.DrawLines(pen, tempLeft.ToArray());
            List<PointF> tempRight = new List<PointF>();
            for (int i = polyPoint.Count / 2 + 2; i < polyPoint.Count; i++)
            {
                tempRight.Add(polyPoint[i]);
            }
            //draw 
            if (polyPoint.Count > 2)
                g.DrawLines(pen, tempRight.ToArray());

            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), p);
            g.Transform = matrix;
        }




    }
}
