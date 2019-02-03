using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1612829_1612842
{

    public class MyText : Shape
    {
        public String text;
        public Point coord;
        protected Font font;
        public MyFont myFont;
        public struct MyFont
        {
            //public bool bold;
            //public bool italic;
            //public string name;
            //public float size;
            //public bool strikeout;
            //public bool underline;
            public String stringFont;
            public MyFont(Font font)
            {
                //bold = font.Bold;
                //italic = font.Italic;
                //name = font.Name;
                //size = font.Size;
                //strikeout = font.Strikeout; 
                //underline = font.Underline;
                stringFont= TypeDescriptor.GetConverter(typeof(Font)).ConvertToInvariantString(font);
                //font = TypeDescriptor.GetConverter(typeof(Font)).ConvertFromInvariantString(s) as Font;
            }

            public Font toFont()
            {
                return TypeDescriptor.GetConverter(typeof(Font)).ConvertFromInvariantString(stringFont) as Font;
            }

        }
        public MyText() : base() { }

        public MyText(String _text, Point _coord, MyPaintSettings settings, int _name)
            : base(_name, settings)
        {
            text = _text;
            coord = _coord;
            myFont = new MyFont(settings.font);
            font = myFont.toFont();
        }

        public MyText(MyText s, int numeric)
            : base(s, numeric)
        {
            text = s.text;
            coord = s.coord;
            myFont = s.myFont;
            font = myFont.toFont();
        }

        public override ShapeType getType()
        {
            return ShapeType.text;
        }

        public override List<Point> getControlPoint()
        {
            List<Point> controlpoint = new List<Point>();
            controlpoint.Add(new Point(coord.X, coord.Y));

            return controlpoint;
        }

        public override void updateInfoAfterLoadFileXML()
        {
            base.updateInfoAfterLoadFileXML();
            font = myFont.toFont();   
        }

        public override void drawWithAngle(ToolSettings toolsettings)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt((float)(angleIn), coord);
            Graphics g = Graphics.FromImage(toolsettings.bitmap);
            g.Transform = matrix;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            g.DrawString(text, font, new SolidBrush(outlineColor), coord);
            toolsettings.pictureBox.Invalidate();


            matrix.RotateAt(-(float)(angleIn), coord);
            g.Transform = matrix;

        }
        public override void drawWithAngle(ToolSettings toolsettings, Point p)
        {

        }

        public override List<Point> getControlPoint(Point p)
        {
            return null;
        }



        public override void moveShape(List<Point> controlPoint, Point pointStartMoving, Point pointCurrent)
        {
            this.coord = pointCurrent;
        }

        public override void resize(Point start, Point pointCurrent)
        {

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

        public Font getFont()
        {
            return font;
        }
        public String getText()
        {
            return this.text;
        }

        public void setFont(Font t)
        {
            font = t;
        }
        public void setText(String t)
        {
            text = t; ;
        }

    }
}
