using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class RectangleTool : Tool
    {
        protected bool drawing;

        protected TextureBrush delBrush;
        protected Pen delPen;

        protected Point sPoint;

        protected Rectangle rect;
        protected Pen outlinePen;
        protected Brush fillBrush;

        public RectangleTool(ToolSettings toolSettings) 
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

               
                if (outlinePen != null)
                    outlinePen.Dispose();
                if (fillBrush != null)
                    fillBrush.Dispose();
                delBrush.Dispose();
                g.Dispose();
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (drawing)
            {
                DrawRectangle(delPen, delBrush);

                rect = GetRectangleFromPoints(sPoint, e.Location);
                //draw the new rect
                DrawRectangle(outlinePen, fillBrush);
                toolSetting.pictureBox.Invalidate();

            }
        }

        public virtual void DrawRectangle(Pen outline, Brush fill)
        {
            g.FillRectangle(fill, rect);
            g.DrawRectangle(outline, rect);
            
        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drawing = true;
                sPoint = e.Location;

                g = Graphics.FromImage(toolSetting.bitmap);

                fillBrush = getFillStyle(toolSetting);
                
                outlinePen = new Pen(toolSetting.settings.OutlineColor, toolSetting.settings.Width);
                outlinePen.DashStyle = toolSetting.settings.LineStyle;


                // delete brush
                delBrush = new TextureBrush(toolSetting.bitmap);
                delPen = new Pen(delBrush, toolSetting.settings.Width);

            }
        }

        public override void UnloadTool()
        {
            toolSetting.pictureBox.Cursor = Cursors.Default;

        }

        public override ToolType getToolType()
        {
            return ToolType.RectangleTool;
        }
    }
}
