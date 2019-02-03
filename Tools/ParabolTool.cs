using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612829_1612842
{
    class ParabolTool : LineTool
    {

        private Point endPoint;
        public ParabolTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
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
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                // delete old line
                int w = toolSetting.settings.Width;
                delRect.Inflate(w, w);
                g.FillRectangle(delBrush, delRect);

                //List<Point> temp = get4PointBezierForRightParabol(sPoint, e.Location);
                //g.DrawBezier(pen, temp[0], temp[1], temp[2], temp[3]);
                //temp = get4PointBezierForLetfParabol(sPoint, e.Location);
                //g.DrawBezier(pen, temp[0], temp[1], temp[2], temp[3]);
                List<Point> tmp = getPointsToDrawParabol(sPoint, e.Location);
                if (tmp.Count >= 2)
                    g.DrawLines(pen, tmp.ToArray());
                endPoint = e.Location;

                toolSetting.pictureBox.Invalidate();
                delRect = GetRectangleFromPoints(sPoint, e.Location);

            }
        }

        private List<Point> get4PointBezierForLetfParabol(Point start, Point end)
        {
            List<Point> res = new List<Point>();
            Point tmp = new Point(0, 0);
            tmp.X = (start.X + end.X) / 2;
            tmp.Y = end.Y;
            res.Add(tmp);
            res.Add(new Point(start.X, end.Y));
            res.Add(start);
            res.Add(start);

            return res;
        }

        private List<Point> get4PointBezierForRightParabol(Point start, Point end)
        {
            List<Point> res = new List<Point>();
            Point tmp = new Point(0, 0);
            tmp.X = (start.X + end.X) / 2;
            tmp.Y = end.Y;
            res.Add(tmp);
            res.Add(new Point(end.X, end.Y));
            res.Add(new Point(end.X, start.Y));
            res.Add(new Point(end.X, start.Y));

            return res;
        }

        public static List<Point> getPointsToDrawParabol(Point start, Point end)
        {
            List<Point> res = new List<Point>();
            Point vertex = new Point(0, 0);
            vertex.X = (start.X + end.X) / 2;
            vertex.Y = end.Y;
            int distanceX = end.X - vertex.X;
            for (int x = start.X; x < end.X; x++)
            {
                int y = (int)(-5.0 / distanceX * (x - vertex.X) * (x - vertex.X) + vertex.Y);
                if (y < start.Y)
                    continue;
                res.Add(new Point(x, y));
            }
            return res;
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;



                toolSetting.pictureBox.Invalidate();

                // free resources

                delBrush.Dispose();
                g.Dispose();

            }

        }

        public List<Point> getPolyPoint()
        {
            return getPointsToDrawParabol(sPoint,endPoint);
        }

        public override ToolType getToolType()
        {
            return ToolType.ParabolTool;
        }
    }
}
