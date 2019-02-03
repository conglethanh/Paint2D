using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1612829_1612842
{
    class BezierTool : LineTool
    {
        List<Point> polyPoint;
        Point temp;

        public BezierTool(ToolSettings toolSettings)
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
                if (polyPoint.Count == 3)
                    return;
                drawing = true;
                sPoint = e.Location;
                polyPoint.Add(e.Location);


                g = Graphics.FromImage(toolSetting.bitmap);



                pen = new Pen(new SolidBrush(toolSetting.settings.OutlineColor), toolSetting.settings.Width);
                pen.DashStyle = toolSetting.settings.LineStyle;

                g.DrawRectangle(new Pen(new SolidBrush(Color.Red)), e.X, e.Y, 2, 2);

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

                Pen delPen = new Pen(delBrush);
                if (polyPoint.Count == 1)
                {
                    g.DrawLine(delPen, polyPoint[0], temp);
                }
                else if (polyPoint.Count == 2)
                {

                    g.DrawBezier(delPen, polyPoint[0], polyPoint[1], temp, temp);
                }
                else if (polyPoint.Count == 3)
                {
                    g.DrawBezier(delPen, polyPoint[0], polyPoint[1], polyPoint[2], temp);
                    g.DrawBezier(pen, polyPoint[0], polyPoint[1], polyPoint[2], e.Location);
                }

                toolSetting.pictureBox.Invalidate();
                delRect = GetRectangleFromPoints(polyPoint[0], e.Location);
                temp = e.Location;

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
                    if (polyPoint.Count == 4)
                    {
                        g.DrawBezier(pen, polyPoint[0], polyPoint[1], polyPoint[2], e.Location);
                    }


                    toolSetting.pictureBox.Invalidate();

                    // free resources

                    delBrush.Dispose();
                    g.Dispose();

                }
            }
        }



        public override ToolType getToolType()
        {
            return ToolType.BezierTool;
        }
    }
}
