using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public abstract class Tool
    {
        protected ToolSettings toolSetting;
        protected Graphics g;

        public Tool(ToolSettings args)
        {
            this.toolSetting = args;
           
        }

        public static int getShapeNearMouse(List<Shape> listShape, Point p)
        {
            if (listShape.Count == 0)
                return -1;
            int res = 0;
            int distance = distanceFromPointToCenterShape(listShape[res], p);
            for (int i = 1;i<listShape.Count;i++)
            {
                if (distanceFromPointToCenterShape(listShape[i], p) < distance)
                {
                    distance = distanceFromPointToCenterShape(listShape[i], p);
                    res = i;
                }
            }
            return res;
        }

        public static int distanceFromPointToCenterShape(Shape s, Point p){
            int res = 0;
            Point mid = getMidPointOfShape(s);
            res = (int)(Math.Pow(mid.X - p.X, 2) + Math.Pow(mid.Y - p.Y, 2));
            return res;
        }

        public static Point getMidPointOfShape(Shape s)
        {
            Point mid = new Point(0, 0);

            List<Point> controlPoint = s.getControlPoint();
            foreach (var p in controlPoint)
            {
                mid.X += p.X;
                mid.Y += p.Y;
            }
            mid.X /= controlPoint.Count;
            mid.Y /= controlPoint.Count;

            return mid;
        }

    
        public static Rectangle GetRectangleFromPoints(Point p1, Point p2)
        {
            Point oPoint;
            Rectangle rect;

            if ((p2.X > p1.X) && (p2.Y > p1.Y))
            {
                rect = new Rectangle(p1, new Size(p2.X - p1.X, p2.Y - p1.Y));
            }
            else if ((p2.X < p1.X) && (p2.Y < p1.Y))
            {
                rect = new Rectangle(p2, new Size(p1.X - p2.X, p1.Y - p2.Y));
            }
            else if ((p2.X > p1.X) && (p2.Y < p1.Y))
            {
                oPoint = new Point(p1.X, p2.Y);
                rect = new Rectangle(oPoint, new Size(p2.X - p1.X, p1.Y - oPoint.Y));
            }
            else
            {
                oPoint = new Point(p2.X, p1.Y);
                rect = new Rectangle(oPoint, new Size(p1.X - p2.X, p2.Y - p1.Y));
            }
            return rect;
        }

        public static Brush getFillStyle(ToolSettings toolSetting)
        {
            if (toolSetting.settings.FillStyle == FillStyle.SolidBrush)
            {
                return new SolidBrush(toolSetting.settings.PrimaryColor);
            }
            else if (toolSetting.settings.FillStyle == FillStyle.Transparent)
            {
                return new SolidBrush(Color.Transparent);
            }
            else if (toolSetting.settings.FillStyle == FillStyle.TextureBrush)
            {
                if (toolSetting.settings.TextureBrushImage == null)
                {
                    Brush t = new SolidBrush(Color.Transparent);
                    return t;
                }
                TextureBrush tb = new TextureBrush(toolSetting.settings.TextureBrushImage);
                
                return tb;
            }
            else
            {
                HatchBrush temp = new HatchBrush(toolSetting.settings.HatchStyle, toolSetting.settings.PrimaryColor, toolSetting.settings.SecondaryColor);
                return temp;
            }
        }


        public abstract void UnloadTool();
        public abstract void OnMouseUp(object sender, MouseEventArgs e);
        public abstract void OnMouseMove(object sender, MouseEventArgs e);
        public abstract void OnMouseDown(object sender, MouseEventArgs e);

        public abstract ToolType getToolType();
    }
}
