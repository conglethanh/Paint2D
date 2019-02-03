using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class CircleTool : RectangleTool
    {
        public CircleTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
        }

        public override void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (drawing)
            {
                DrawRectangle(delPen, delBrush);

                rect = getSquareFrom2Points(sPoint, e.Location);

                //draw the new rect
                DrawRectangle(outlinePen, fillBrush);
                toolSetting.pictureBox.Invalidate();

            }
        }

        public override void DrawRectangle(System.Drawing.Pen outline, System.Drawing.Brush fill)
        {
            g.FillEllipse(fill, rect);
            g.DrawEllipse(outline, rect);
        }

        public static Rectangle getSquareFrom2Points(Point p1, Point p2)
        {
            Point oPoint;
            Rectangle rect;

            //đường chéo chính 1 < 2
            if ((p2.X > p1.X) && (p2.Y > p1.Y))
            {
                rect = new Rectangle(p1, new Size(p2.X - p1.X, p2.X - p1.X));
            }
            //đường chéo chính 1 > 2
            else if ((p2.X < p1.X) && (p2.Y < p1.Y))
            {
                rect = new Rectangle(p2, new Size(p1.X - p2.X, p1.X - p2.X));
            }
            //đường chéo phụ
            else if ((p2.X > p1.X) && (p2.Y < p1.Y))
            {
                oPoint = new Point(p1.X, p2.Y);
                rect = new Rectangle(oPoint, new Size(p2.X - p1.X, p2.X - p1.X));
            }
            else
            {
                oPoint = new Point(p2.X, p1.Y);
                rect = new Rectangle(oPoint, new Size(p1.X - p2.X, p1.X - p2.X));
            }
            return rect;
        }

        public override ToolType getToolType()
        {
            return ToolType.CircleTool;
        }
    }
}