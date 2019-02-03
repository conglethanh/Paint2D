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

    public class StraightLine : Shape
    {
        public Point startPoint;
        public Point endPoint;

        public StraightLine() : base() { }
        public StraightLine(Point _startPoint, Point _endPoint, MyPaintSettings settings, int _name)
            : base(_name, settings)
        {
            startPoint = _startPoint;
            endPoint = _endPoint;

        }

        public StraightLine(StraightLine s, int numeric)
            : base(s, numeric)
        {
            startPoint = s.startPoint;
            endPoint = s.endPoint;

        }

        public StraightLine(MyPaintSettings settings, int _name)
            : base(_name, settings)
        {

        }

        public override ShapeType getType()
        {
            return ShapeType.straightLine;
        }

        public override List<Point> getControlPoint()
        {
            List<Point> controlpoint = new List<Point>();
            controlpoint.Add(startPoint);
            controlpoint.Add(endPoint);

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

        public override void drawWithAngle(ToolSettings toolsettings)
        {
            Point mid = new Point(0, 0);
            mid.X = (startPoint.X + endPoint.X) / 2;
            mid.Y = (startPoint.Y + endPoint.Y) / 2;

            drawWithAngle(toolsettings, mid);

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
            g.DrawLine(pen, this.startPoint, this.endPoint);

            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), p);
            g.Transform = matrix;
        }

        public override List<Point> getControlPoint(Point p)
        {
            return null;
        }

        public override void moveShape(List<Point> controlPoint, Point pointStartMoving, Point pointCurrent)
        {
            this.startPoint = new Point(controlPoint[0].X + pointCurrent.X - pointStartMoving.X,
                controlPoint[0].Y + pointCurrent.Y - pointStartMoving.Y);
            this.endPoint = new Point(controlPoint[1].X + pointCurrent.X - pointStartMoving.X,
                controlPoint[1].Y + pointCurrent.Y - pointStartMoving.Y);
        }

        public override void resize(Point start, Point pointCurrent)
        {
            this.startPoint = start;
            this.endPoint = pointCurrent;
        }

        public override void rotateAndDraw(List<Point> controlPoint, Point startMouse, Point pointCurrent, ToolSettings toolsettings)
        {
            angleIn = pointCurrent.X - startMouse.X;
            angleIn %= 360;
            drawWithAngle(toolsettings);
        }

        public override void changeFillStyle(FillStyle fs)
        {

        }

        public override void changeHatchStyle(HatchStyle hs)
        {

        }
    }
}
