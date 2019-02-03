using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612829_1612842
{
    class HyperbolTool : LineTool
    {

        int a;
        int b;

        List<Point> left;
        List<Point> right;
        Point temp;
        Pen delPen;
        Point endPoint;
        public HyperbolTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            a = 20;
            b = 15;
            left = new List<Point>();
            right = new List<Point>();
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

                drawing = true; 
                sPoint = e.Location;



                g = Graphics.FromImage(toolSetting.bitmap);

                pen = new Pen(new SolidBrush(toolSetting.settings.OutlineColor), toolSetting.settings.Width);
                pen.DashStyle = toolSetting.settings.LineStyle;

                // delete brush
                delBrush = new TextureBrush(toolSetting.bitmap);
                delPen = new Pen(delBrush);

            }
        }

        public List<Point> getPolyPoint()
        {
            List<Point> left = getPointsToDrawLeftHyperbol(sPoint, endPoint);
            List<Point> right = getPointsToDrawRightHyperbol(sPoint, endPoint);
            foreach (var item in right)
            {
                left.Add(item);
            }
            return left;
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                //if (left.Count >= 2)
                //    g.DrawLines(delPen, left.ToArray());

                //if (right.Count >= 2)
                //    g.DrawLines(delPen, right.ToArray());
               Rectangle r= GetRectangleFromPoints(sPoint, temp);
               r.Inflate(toolSetting.settings.Width,toolSetting.settings.Width);
               g.FillRectangle(delBrush, r);


                left = getPointsToDrawLeftHyperbol(sPoint, e.Location);
                right = getPointsToDrawRightHyperbol(sPoint, e.Location);
                if (left.Count >= 2)
                    g.DrawLines(pen, left.ToArray());

                if (right.Count >= 2)
                    g.DrawLines(pen, right.ToArray());


                toolSetting.pictureBox.Invalidate();
                temp = e.Location;

            }
        }

        private List<Point> getPointsToDrawRightHyperbol(Point start, Point end)
        {
            List<Point> res = new List<Point>();

            if (a == 0 || b == 0)
                return res;

            Point mid = new Point(0, 0);
            mid.X = (start.X + end.X) / 2;
            mid.Y = (start.Y + end.Y) / 2;

            int yStart = start.Y;

            for (int y = yStart; y <= end.Y; y++)
            {
                double tmp = Math.Sqrt(a * a + a * a * (y - mid.Y) * (y - mid.Y) / (b * b)) + mid.X;
                int x = (int)tmp;
                if (x >= start.X && x <= end.X)
                    res.Add(new Point(x, y));
            }

            return res;
        }

        private List<Point> getPointsToDrawLeftHyperbol(Point start, Point end)
        {
            List<Point> res = new List<Point>();

            if (a == 0 || b == 0)
                return res;

            Point mid = new Point(0, 0);
            mid.X = (start.X + end.X) / 2;
            mid.Y = (start.Y + end.Y) / 2;

            int yStart = start.Y;

            for (int y = yStart; y <= end.Y; y++)
            {
                double tmp = -Math.Sqrt(a * a + a * a * (y - mid.Y) * (y - mid.Y) / (b * b)) + mid.X;
                int x = (int)tmp;
                if (x >= start.X && x <= end.X)
                    res.Add(new Point(x, y));
            }

            return res;
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;


                endPoint = e.Location;
                toolSetting.pictureBox.Invalidate();

                // free resources

                delBrush.Dispose();
                g.Dispose();

            }

        }



        public override ToolType getToolType()
        {
            return ToolType.HyperbolTool;
        }
    }
}
