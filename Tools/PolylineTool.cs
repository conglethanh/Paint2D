using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612829_1612842
{
    class PolylineTool : LineTool
    {
        List<Point> polyPoint;
        public PolylineTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            polyPoint = new List<Point>();
        }

        public List<Point> getPolyPoint(){
            return polyPoint;
        }

        public override void OnMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (drawing == false)
                {
                    polyPoint.Clear();
                }
                drawing = true;
                sPoint = e.Location;
                polyPoint.Add(e.Location);

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


                g.DrawLine(pen, sPoint, e.Location);
                toolSetting.pictureBox.Invalidate();
                delRect = GetRectangleFromPoints(sPoint, e.Location);

            }
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                if (e.Button == MouseButtons.Right)
                {
                    drawing = false;

                    polyPoint.Add(e.Location);
                    g.DrawLines(pen, polyPoint.ToArray());


                    toolSetting.pictureBox.Invalidate();

                    // free resources
                  
                    delBrush.Dispose();
                    g.Dispose();
                   
                }
            }
        }
        


        public override ToolType getToolType()
        {
            return ToolType.PolylineTool;
        }
    }
}
