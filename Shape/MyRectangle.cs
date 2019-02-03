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
    public class MyRectangle : Shape
    {

        public Point startPoint;

        public Point endPoint;

        public FillStyle fillStyle;

        public HatchStyle hatchStyle;

        protected Image imageTexture;
        public MyImage imageTextureString;

        public struct MyImage
        {
            public string s;
            public MyImage(Image im)
            {
                if (im==null)
                {
                    s = "";
                    return;
                }
                MemoryStream ms = new MemoryStream();
                im.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] array = ms.ToArray();

                s = Convert.ToBase64String(array);

            }
            public Image ToImage()
            {
                if (s == "")
                    return null;
                byte[] array = Convert.FromBase64String(s);
                Image image = Image.FromStream(new MemoryStream(array));

                return image;
            }

        }

        public MyRectangle() : base() { }

        public MyRectangle(Point _startPoint, Point _endPoint, MyPaintSettings settings, int _name)
            : base(_name, settings)
        {
            startPoint = _startPoint;
            endPoint = _endPoint;
            fillStyle = settings.FillStyle;
            hatchStyle = settings.HatchStyle;
            imageTexture = settings.TextureBrushImage;
            imageTextureString = new MyImage(settings.TextureBrushImage);
        }

        public MyRectangle(MyPaintSettings settings, int _name)
            : base(_name, settings)
        {
            fillStyle = settings.FillStyle;
            hatchStyle = settings.HatchStyle;
            imageTexture = settings.TextureBrushImage;
            imageTextureString = new MyImage(settings.TextureBrushImage);
        }

        public MyRectangle(MyRectangle s,int numeric)
            : base(s,numeric)
        {
            startPoint = s.startPoint;
            endPoint =s.endPoint;
            fillStyle = s.fillStyle;
            hatchStyle = s.hatchStyle;
            imageTexture = s.imageTexture;
            imageTextureString = s.imageTextureString;
        }

        public override ShapeType getType()
        {
            return ShapeType.rectangle;
        }

        public override void updateInfoAfterLoadFileXML()
        {
            base.updateInfoAfterLoadFileXML();
            
            imageTexture = imageTextureString.ToImage();
        
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

        public override List<Point> getControlPoint(Point p)
        {
            List<Point> controlpoint = new List<Point>();
            controlpoint.Add(startPoint);
            controlpoint.Add(endPoint);
            controlpoint.Add(new Point(startPoint.X, endPoint.Y));
            controlpoint.Add(new Point(endPoint.X, startPoint.Y));
            if (angleIn != 0)
            {
                for (int i = 0; i < controlpoint.Count; i++)
                {
                    controlpoint[i] = rotatePoint(controlpoint[i], angleIn, p);
                }
            }
            return controlpoint;
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

        public override void drawWithAngle(ToolSettings toolsettings)
        {
            Point mid = new Point(0, 0);
            List<Point> controlPoint = getControlPoint();
            foreach (var p in controlPoint)
            {
                mid.X += p.X;
                mid.Y += p.Y;
            }
            mid.X /= controlPoint.Count;
            mid.Y /= controlPoint.Count;

            drawWithAngle(toolsettings, mid);
        }

        public override void drawWithAngle(ToolSettings toolsettings, Point mid)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), mid);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            Rectangle rect = Tool.GetRectangleFromPoints(startPoint, endPoint);


            Brush fill = getBrush();

            g.FillRectangle(fill, rect);

            Pen pen = new Pen(new SolidBrush(outlineColor), width);
            pen.DashStyle = lineStyle;
            //draw the new rect
            g.DrawRectangle(pen, rect);
            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), mid);
            g.Transform = matrix;
        }

       
        
        public Brush getBrush()
        {
            Brush fill;
            if (fillStyle == FillStyle.SolidBrush)
            {
                fill = new SolidBrush(this.primaryColor);
            }
            else if (this.fillStyle == FillStyle.Transparent)
            {
                fill = new SolidBrush(Color.Transparent);
            }
            else if (fillStyle == FillStyle.TextureBrush)
            {
                if (imageTexture == null)
                {
                    fill = new SolidBrush(Color.Transparent);
                    return fill;
                }
                fill = new TextureBrush(imageTexture);
            }
            else
            {
                fill = new HatchBrush(hatchStyle, primaryColor, secondColor);
            }
            return fill;
        }


        public override void changeFillStyle(FillStyle fs)
        {
            fillStyle = fs;
        }

        public override void changeHatchStyle(HatchStyle hs)
        {
            hatchStyle = hs;
        }

        public override void changeImageTexture(Image im)
        {
            imageTexture = im;
            imageTextureString = new MyImage(imageTexture);
        }
    }
}
