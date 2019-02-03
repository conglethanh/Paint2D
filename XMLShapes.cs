using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace _1612829_1612842
{
    
    public class XMLShapes
    {
        public bool isBackGroundImage;
        public ImageFile imageFile;

        public int w;
        public int h;
        public IColor bgColor;
        

        [XmlElement(typeof(MyArcCircle))]
        [XmlElement(typeof(MyArcEllipse))]
        [XmlElement(typeof(MyBezier))]
        [XmlElement(typeof(MyCircle))]
        [XmlElement(typeof(MyEllipse))]
        [XmlElement(typeof(MyHyperbol))]
        [XmlElement(typeof(MyParabol))]
        [XmlElement(typeof(MyParallelogram))]
        [XmlElement(typeof(MyPolygon))]
        [XmlElement(typeof(MyPolyline))]
        [XmlElement(typeof(MyRectangle))]
        [XmlElement(typeof(MyText))]
        [XmlElement(typeof(StraightLine))]
        public List<Shape> listShape{get;set;}
        
        
        public XMLShapes()
        {
            
        }

        public XMLShapes(List<Shape> t, bool isBGImage,ImageFile im,int width, int height, Color backColor)
        {
            listShape = t;
            imageFile = im;
            isBackGroundImage = isBGImage;

            w = width;
            h = height;
            bgColor = new IColor(backColor);
        }

        

       

        public struct IColor
        {
            public int A;
            public int R;
            public int G;
            public int B;
            public IColor(Color color)
            {
                this.A = color.A;
                this.R = color.R;
                this.G = color.G;
                this.B = color.B;
            }

            public static IColor FromColor(Color color)
            {
                return new IColor(color);
            }

            public Color ToColor()
            {
                return Color.FromArgb(A, R, G, B);
            }
        }

    }
}
