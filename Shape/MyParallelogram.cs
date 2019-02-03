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
    public class MyParallelogram : MyRectangle
    {
        public int alpha;
        public MyParallelogram() : base() { }

        public MyParallelogram(Point _startPoint, Point _endPoint, MyPaintSettings settings, int _name)
            : base(_startPoint, _endPoint, settings, _name)
        {
            alpha = -50;
        }

        public MyParallelogram(MyParallelogram s, int numeric)
            : base(s, numeric)
        {
            alpha = s.alpha;
        }

        public override ShapeType getType()
        {
            return ShapeType.parallelogram;
        }

        public override void drawAllPointToShape(ToolSettings toolSettings, int sizeControlPoint)
        {
            base.drawAllPointToShape(toolSettings, sizeControlPoint);
            List<Point> pointRect = ParallelogramTool.get4PointRectangleFrom2Point(startPoint, endPoint);
            pointRect = ParallelogramTool.get4PointParallelogramFromRectangle(pointRect, alpha);
            Point point = new Point(pointRect[1].X, pointRect[1].Y);

            point = rotatePoint(point, angleIn, getMid());

            Pen pen = new Pen(Color.Yellow, sizeControlPoint);
            Graphics.FromImage(toolSettings.bitmap).DrawRectangle(pen, point.X, point.Y, sizeControlPoint, sizeControlPoint);
            toolSettings.pictureBox.Invalidate();

        }

        public override void changeAlphaParallelogram(Point start, Point end){
            alpha = -50 + (end.X - start.X);
        }

        private Point getMid()
        {
            Point mid = new Point(0, 0);
            mid.X = (startPoint.X + endPoint.X) / 2;
            mid.Y = (startPoint.Y + endPoint.Y) / 2;
            return mid;
        }

        public override bool checkNearPoints(SelectToolType selectype, Point p)
        {
            if (selectype == SelectToolType.moving)
            {
                return checkNearMidPoint(p, getControlPoint(), 10);
            }
            else if (selectype == SelectToolType.resize)
            {
                return checkNearControlPoint(p, getControlPoint());
            }
            else if (selectype == SelectToolType.parallelPoint)
            {
                return checkNearParallelPoint(p);
            }
            else
            {
                return checkNearRotatePoint(p, getControlPoint());
            }
        }

        public bool checkNearParallelPoint(Point p)
        {
            List<Point> pointRect = ParallelogramTool.get4PointRectangleFrom2Point(startPoint, endPoint);
            pointRect = ParallelogramTool.get4PointParallelogramFromRectangle(pointRect, alpha);
            Point point = pointRect[1];
            if (Math.Abs(p.X - point.X) < 5 && Math.Abs(p.Y - point.Y) < 5)
            {
                return true;
            }
            return false;
        }

        public override void drawWithAngle(ToolSettings toolsettings, Point mid)
        {

            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), mid);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            List<Point> pointRect;
            pointRect = ParallelogramTool.get4PointRectangleFrom2Point(startPoint, endPoint);
            pointRect = ParallelogramTool.get4PointParallelogramFromRectangle(pointRect, alpha);


            Brush fill = getBrush();
            g.FillPolygon(fill, pointRect.ToArray());

            Pen pen = new Pen(new SolidBrush(outlineColor), width);
            pen.DashStyle = lineStyle;
            //draw the new rect
            g.DrawPolygon(pen, pointRect.ToArray());
            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), mid);
            g.Transform = matrix;
        }


    }
}