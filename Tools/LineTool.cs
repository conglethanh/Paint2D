using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class LineTool : Tool
    {
        protected bool drawing;
        protected Point sPoint;
        protected TextureBrush delBrush;
        protected Pen pen;
        protected Rectangle delRect;

        public LineTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            drawing = false;
            toolSettings.pictureBox.Cursor = Cursors.Cross;
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                drawing = false;

                toolSetting.pictureBox.Invalidate();

                // free resources
                pen.Dispose();
                delBrush.Dispose();
                g.Dispose();
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


                drawLine(sPoint, e.Location);

                delRect = GetRectangleFromPoints(sPoint, e.Location);
               
            }
        }

        public virtual void drawLine(Point start, Point end)
        {
            //draw the new line
            g.DrawLine(pen, start, end);
            toolSetting.pictureBox.Invalidate();
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

        public override void UnloadTool()
        {
            toolSetting.pictureBox.Cursor = Cursors.Default;
           
        }

        public override ToolType getToolType()
        {
            return ToolType.LineTool;
        }
    }
}
