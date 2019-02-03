using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace _1612829_1612842
{
    public class ParallelogramTool : RectangleTool
    {
        List<Point> pointRect;
        int alpha;
        int xMin, yMin, xMax, yMax;
        public ParallelogramTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            pointRect = new List<Point>();
            alpha = -50;
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (pointRect.Count >= 4)
                {
                    int w = toolSetting.settings.Width;
                    g.DrawRectangle(delPen, xMin, yMin, xMax - xMin + w, yMax - yMin + w);
                    g.FillRectangle(delBrush, xMin, yMin, xMax - xMin + w, yMax - yMin + w);
                }
                pointRect = get4PointRectangleFrom2Point(sPoint, e.Location);
                pointRect = get4PointParallelogramFromRectangle(pointRect, alpha);
                //draw the new rect
                if (pointRect.Count >= 4)
                    DrawRectangle(outlinePen, fillBrush);
                toolSetting.pictureBox.Invalidate();

                xMin = pointRect.Min(point => point.X);
                yMin = pointRect.Min(point => point.Y);

                xMax = pointRect.Max(point => point.X);
                yMax = pointRect.Max(point => point.Y);
            }
        }

        public static List<Point> get4PointRectangleFrom2Point(Point start, Point end)
        {
            List<Point> temp = new List<Point>();
            temp.Add(start);
            temp.Add(new Point(start.X, end.Y));
            temp.Add(end);
            temp.Add(new Point(end.X, start.Y));
            return temp;
        }

        public static List<Point> get4PointParallelogramFromRectangle(List<Point> list, int alpha)
        {
            List<Point> temp = new List<Point>();
            temp = list;
            if (list.Count >= 4)
            {
                temp[2] = new Point(temp[2].X + alpha, temp[2].Y);
                temp[1] = new Point(temp[1].X + alpha, temp[1].Y);
            }
            return temp;
        }


        public override void DrawRectangle(Pen outline, Brush fill)
        {
            if (pointRect.Count >= 4)
            {
                g.FillPolygon(fill, pointRect.ToArray());
                g.DrawPolygon(outline, pointRect.ToArray());
            }
        }

        public override ToolType getToolType()
        {
            return ToolType.ParallelogramTool;
        }
    }
}
