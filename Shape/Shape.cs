using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

namespace _1612829_1612842
{

    public abstract class Shape
    {

        public int stt;
        protected Color primaryColor;
        public MyColor myPrimaryColor;
        protected Color secondColor;
        public MyColor mySecondColor;
        protected Color outlineColor;
        public MyColor myOutlineColor;
        public int width;
        public DashStyle lineStyle;

        public int angleIn;

        public Shape()
        {

        }

        public struct MyColor
        {
            public int A;
            public int R;
            public int G;
            public int B;
            public MyColor(Color color)
            {
                this.A = color.A;
                this.R = color.R;
                this.G = color.G;
                this.B = color.B;
            }

            public static MyColor FromColor(Color color)
            {
                return new MyColor(color);
            }

            public Color ToColor()
            {
                return Color.FromArgb(A, R, G, B);
            }
        }

        public void changeStt(int n)
        {
            stt = n;
        }

        public Shape(Shape s, int numeric)
        {
            stt = numeric;
            myPrimaryColor = s.myPrimaryColor;
            mySecondColor = s.mySecondColor;
            myOutlineColor = s.myOutlineColor;

            primaryColor = myPrimaryColor.ToColor();
            secondColor = mySecondColor.ToColor();
            outlineColor = myOutlineColor.ToColor();
            width = s.width;
            lineStyle = s.lineStyle;
            angleIn = 0;
        }

        public Shape(int _name, MyPaintSettings settings)
        {
            myPrimaryColor = new MyColor(settings.PrimaryColor);
            mySecondColor = new MyColor(settings.SecondaryColor);
            myOutlineColor = new MyColor(settings.OutlineColor);
            stt = _name;
            primaryColor = myPrimaryColor.ToColor();
            secondColor = mySecondColor.ToColor();
            outlineColor = myOutlineColor.ToColor();
            width = settings.Width;
            lineStyle = settings.LineStyle;
            angleIn = 0;
        }

        public String getName()
        {
            return stt.ToString();
        }

        public virtual void updateInfoAfterLoadFileXML()
        {
            primaryColor = myPrimaryColor.ToColor();
            secondColor = mySecondColor.ToColor();
            outlineColor = myOutlineColor.ToColor();
        }

        public abstract ShapeType getType();
        public abstract List<Point> getControlPoint();

        //get control point with center is p
        public abstract List<Point> getControlPoint(Point p);

        public abstract void moveShape(List<Point> controlPoint, Point pointStartMoving, Point pointCurrent);

        public abstract void resize(Point start, Point pointCurrent);
        public abstract void rotateAndDraw(List<Point> controlPoint, Point startMouse, Point pointCurrent, ToolSettings toolsettings);

        public abstract void drawWithAngle(ToolSettings toolsettings);
        public abstract void drawWithAngle(ToolSettings toolsettings, Point p);

        public static Point rotatePoint(Point coord, int angleIn, Point mid)
        {
            int cosaPx = (int)((coord.X - mid.X) * Math.Cos(Math.PI * angleIn / 180));
            int sinaPy = (int)((coord.Y - mid.Y) * Math.Sin(Math.PI * angleIn / 180));
            int sinaPx = (int)((coord.X - mid.X) * Math.Sin(Math.PI * angleIn / 180));
            int cosaPy = (int)((coord.Y - mid.Y) * Math.Cos(Math.PI * angleIn / 180));

            coord.X = cosaPx - sinaPy + mid.X;
            coord.Y = sinaPx + cosaPy + mid.Y;


            return coord;
        }

        public int getAngle()
        {
            return angleIn;
        }

        public void setAngle(int a)
        {
            angleIn = a;
        }
        public void changeWidth(int w)
        {
            width = w;
        }

        public void changeLineStyle(DashStyle d)
        {
            this.lineStyle = d;
        }

        public void changePrimaryColor(Color c)
        {
            this.primaryColor = c;
            myPrimaryColor = new MyColor(c);
        }

        public void changeSecondColor(Color c)
        {
            this.secondColor = c;
            mySecondColor = new MyColor(c);
        }

        public void changeOutlineColor(Color c)
        {
            this.outlineColor = c;
            myOutlineColor = new MyColor(c);
        }

        public abstract void changeFillStyle(FillStyle fs);
        public abstract void changeHatchStyle(HatchStyle hs);

        public virtual void changeImageTexture(Image im)
        {

        }

        public virtual void drawAllPointToShape(ToolSettings toolSettings, int sizeControlPoint)
        {
            drawControlPointToSelectShape(toolSettings, sizeControlPoint);
            drawRotatePointToSelectShape(toolSettings, sizeControlPoint, Color.Green);
        }


        public virtual void drawControlPointToSelectShape(ToolSettings toolSettings, int sizeControlPoint)
        {
            List<Point> control = getControlPoint();
            Pen pen = new Pen(Color.Red, sizeControlPoint);
            foreach (Point point in control)
                Graphics.FromImage(toolSettings.bitmap).DrawRectangle(pen, point.X, point.Y, sizeControlPoint, sizeControlPoint);
        }

        public virtual void drawRotatePointToSelectShape(ToolSettings toolSettings, int sizeControlPoint, Color color)
        {
            List<Point> control = getControlPoint();
            Point point = new Point(0, 0);
            foreach (Point p in control)
            {
                point.X += p.X;
                point.Y += p.Y;
            }
            point.X /= control.Count;
            point.Y /= control.Count;
            int ymin = control[0].Y;
            foreach (Point p in control)
            {
                if (p.Y < ymin)
                {
                    ymin = p.Y;
                }
            }
            point.Y = point.Y - (point.Y - ymin) - 30;
            if (point.Y < 0)
                point.Y = 0;
            Pen pen = new Pen(color, sizeControlPoint);
            Graphics.FromImage(toolSettings.bitmap).DrawEllipse(pen, point.X, point.Y, sizeControlPoint, sizeControlPoint);

        }

        public virtual void changeAlphaParallelogram(Point start, Point end)
        {

        }

        public virtual bool checkNearPoints(SelectToolType selectype, Point p)
        {
            if (selectype == SelectToolType.moving)
            {
                return checkNearMidPoint(p, getControlPoint(), 30);
            }
            else if (selectype == SelectToolType.resize)
            {
                return checkNearControlPoint(p, getControlPoint());
            }
            else
            {
                return checkNearRotatePoint(p, getControlPoint());
            }
        }
        public virtual bool checkNearControlPoint(Point s, List<Point> hotpoint)
        {
            foreach (Point point in hotpoint)
            {
                if (Math.Abs(s.X - point.X) <= 3 && Math.Abs(s.Y - point.Y) <= 3)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool checkNearMidPoint(Point s, List<Point> hotpoint)
        {
            Point avg = new Point(0, 0);
            int k = 0;
            foreach (Point point in hotpoint)
            {
                k++;
                avg.X += point.X;
                avg.Y += point.Y;
            }
            avg.X /= k;
            avg.Y /= k;


            if (Math.Abs(s.X - avg.X) <= 5 && Math.Abs(s.Y - avg.Y) <= 5)
            {
                return true;
            }

            return false;
        }

        public virtual bool checkNearMidPoint(Point s, List<Point> hotpoint, int delta)
        {
            Point avg = new Point(0, 0);
            int k = 0;
            foreach (Point point in hotpoint)
            {
                k++;
                avg.X += point.X;
                avg.Y += point.Y;
            }
            avg.X /= k;
            avg.Y /= k;


            if (Math.Abs(s.X - avg.X) <= delta && Math.Abs(s.Y - avg.Y) <= delta)
            {
                return true;
            }

            return false;
        }



        public virtual bool checkNearRotatePoint(Point e, List<Point> control)
        {
            Point point = new Point(0, 0);
            foreach (Point p in control)
            {
                point.X += p.X;
                point.Y += p.Y;
            }
            point.X /= control.Count;

            point.Y /= control.Count;
            int ymin = control[0].Y;
            foreach (Point p in control)
            {
                if (p.Y < ymin)
                {
                    ymin = p.Y;
                }
            }
            point.Y = point.Y - (point.Y - ymin) - 30;
            if (point.Y < 0)
                point.Y = 0;

            if (Math.Abs(e.X - point.X) <= 5 && Math.Abs(e.Y - point.Y) <= 5)
                return true;
            return false;
        }

        public virtual List<Point> getExactlyPoints()
        {
            return null;
        }

        public virtual void resize(List<Point> points, Point start, Point endOld, Point tmp)
        {

        }

       
    }
}
