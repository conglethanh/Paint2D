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

    public class MyArcCircle : StraightLine
    {
        public int alpha;
        public MyArcCircle() : base() { }
        public MyArcCircle(Point _startPoint, Point _endPoint, int _alpha, MyPaintSettings settings, int _name)
            : base(_startPoint, _endPoint, settings, _name)
        {

            alpha = _alpha;
        }

         public MyArcCircle(MyArcCircle s, int numeric)
            : base(s, numeric)
        {
            alpha = s.alpha;
        }

        public override ShapeType getType()
        {
            return ShapeType.arcCircle;
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

            int w;
            w = (endPoint.X - startPoint.X) < (endPoint.Y - startPoint.Y) ? (endPoint.X - startPoint.X) : (endPoint.Y - startPoint.Y);
            //draw 
            if (w > 10 )
                g.DrawArc(pen, startPoint.X, startPoint.Y, w, w, 180, alpha);



            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), p);
            g.Transform = matrix;
        }

        public override List<Point> getControlPoint()
        {
            List<Point> controlpoint = new List<Point>();
            controlpoint.Add(startPoint);
            controlpoint.Add(endPoint);
            controlpoint.Add(new Point(startPoint.X, endPoint.Y));
            controlpoint.Add(new Point(endPoint.X, startPoint.Y));
            if (angleIn != 0)
            {
                Point mid = new Point(0, 0);
                mid.X = (startPoint.X + endPoint.X) / 2;
                mid.Y = (startPoint.Y + endPoint.Y) / 2;

                for (int i = 0; i < controlpoint.Count; i++)
                {
                    controlpoint[i] = rotatePoint(controlpoint[i], angleIn, mid);
                }
            }
            return controlpoint;
        }
    }
}
