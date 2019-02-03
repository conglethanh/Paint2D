using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class PolygonTool:RectangleTool
    {
        List<Point> polyPoint;
        public PolygonTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            polyPoint = new List<Point>();
        }

        public List<Point> getPolyPoint()
        {
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

                outlinePen = new Pen(new SolidBrush(toolSetting.settings.OutlineColor), toolSetting.settings.Width);
                outlinePen.DashStyle = toolSetting.settings.LineStyle;

                fillBrush = getFillStyle(toolSetting);

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
                rect.Inflate(w, w);
                g.FillRectangle(delBrush, rect);


                g.DrawLine(outlinePen, sPoint, e.Location);
                toolSetting.pictureBox.Invalidate();
                rect = GetRectangleFromPoints(sPoint, e.Location);

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
                    g.FillPolygon(fillBrush, polyPoint.ToArray());
                    g.DrawPolygon(outlinePen, polyPoint.ToArray());


                    toolSetting.pictureBox.Invalidate();

                    // free resources
                    outlinePen.Dispose();
                    delBrush.Dispose();
                    g.Dispose();
                    
                }
            }
        }

        public override ToolType getToolType()
        {
            return ToolType.PolygonTool;
        }
    }
}
