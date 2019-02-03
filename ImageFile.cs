using System;
using System.Drawing;
using System.IO;

namespace _1612829_1612842
{
    public class ImageFile
    {
        public string fileName;
        private Bitmap bitmap;

        public StringImage myBackGroundImage;

        public ImageFile() { }

        public ImageFile(ImageFile s)
        {
            fileName = s.fileName;
            myBackGroundImage = s.myBackGroundImage;
            bitmap = (Bitmap)myBackGroundImage.ToImage();
        }

        public ImageFile(String s)
        {
            fileName = null;
            myBackGroundImage.s = s;
            bitmap = (Bitmap)myBackGroundImage.ToImage();
        }

        public ImageFile(Size size, Color backColor)
        {
            fileName = null;

            bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            myBackGroundImage = new StringImage(null);

            // fill by background color
            Graphics g = Graphics.FromImage(Bitmap);
            g.Clear(backColor);
            g.Dispose();
        }



        public bool Open(string file)
        {
            try
            {
                bitmap = new Bitmap(file);
                fileName = file;
                myBackGroundImage = new StringImage(bitmap);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save(string file)
        {
            try
            {
                bitmap.Save(file);
                fileName = file;
                myBackGroundImage = new StringImage(bitmap);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string FileName
        {
            get { return fileName; }
        }

        public Bitmap Bitmap
        {
            get { return bitmap; }

        }

        public Bitmap getMyBGImageXML()
        {
            return (Bitmap)myBackGroundImage.ToImage();
        }

        public struct StringImage
        {
            public string s;
            public StringImage(Image im)
            {
                if (im == null)
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


    }
}

