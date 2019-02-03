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
    public class MyPolyline : Shape
    {
        public List<Point> polyPoint;

        protected int xMin, yMin, xMax, yMax;

        public MyPolyline() : base() { }
        public MyPolyline(List<Point> lsPoint, MyPaintSettings settings, int _name)
            : base(_name, settings)
        {
            polyPoint = lsPoint;
        }

        public MyPolyline(MyPolyline s, int numeric)
            : base(s, numeric)
        {
            this.polyPoint = new List<Point>(s.polyPoint);
        }

        public override List<Point> getControlPoint()
        {
            List<Point> controlpoint = new List<Point>();

            xMin = polyPoint.Min(point => point.X);
            yMin = polyPoint.Min(point => point.Y);

            xMax = polyPoint.Max(point => point.X);
            yMax = polyPoint.Max(point => point.Y);

            controlpoint.Add(new Point(xMin, yMin));
            controlpoint.Add(new Point(xMax, yMax));

            controlpoint.Add(new Point(xMin, yMax));
            controlpoint.Add(new Point(xMax, yMin));


            if (angleIn != 0)
            {
                Point mid = new Point(0, 0);
                mid.X = (xMin + xMax) / 2;
                mid.Y = (yMin + yMax) / 2;

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
            xMin = polyPoint.Min(point => point.X);
            yMin = polyPoint.Min(point => point.Y);

            xMax = polyPoint.Max(point => point.X);
            yMax = polyPoint.Max(point => point.Y);

            mid.X = (xMin + xMax) / 2;
            mid.Y = (yMin + yMax) / 2;

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

            g.DrawLines(pen, polyPoint.ToArray());

            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), p);
            g.Transform = matrix;
        }

        public override List<Point> getControlPoint(Point p)
        {
            return null;
        }

        public override void moveShape(List<Point> listPointOld, Point pointStartMoving, Point pointCurrent)
        {

            for (int i = 0; i < polyPoint.Count; i++)
            {
                polyPoint[i] = new Point(listPointOld[i].X + pointCurrent.X - pointStartMoving.X,
                    listPointOld[i].Y + pointCurrent.Y - pointStartMoving.Y);

            }
        }

        public override void resize(Point start, Point pointCurrent)
        {

        }

        public override void resize(List<Point> points, Point start, Point endOld, Point pointCurrent)
        {
            double Sx = (double)(pointCurrent.X - start.X) / (endOld.X - start.X);
            double Sy = (double)(pointCurrent.Y - start.Y) / (endOld.Y - start.Y);

            for (int i = 0; i < polyPoint.Count; i++)
            {
                polyPoint[i] = new Point((int)(points[i].X * Sx + start.X * (1 - Sx)), (int)(points[i].Y * Sy + start.Y * (1 - Sy)));

            }
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

        public override ShapeType getType()
        {
            return ShapeType.polyline;
        }

        public override List<Point> getExactlyPoints()
        {
            List<Point> p = new List<Point>();
            foreach (var item in polyPoint)
            {
                p.Add(item);
            }
            return p;
        }


    }
}
