using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612829_1612842
{
    class ArcCircleTool : LineTool
    {
        int alpha;
        public ArcCircleTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            alpha = 0;
        }


        public override void drawLine(Point start, Point end)
        {
            alpha = Math.Abs(end.X - start.X);
            int w = (end.X - start.X) < (end.Y - start.Y) ? (end.X - start.X) : (end.Y - start.Y);

           

            //draw the new line
            if (w > 10 )
                g.DrawArc(pen, start.X, start.Y, w, w, 180, alpha);

            toolSetting.pictureBox.Invalidate();
        }

        public int getAlphaAngleArc()
        {
            return alpha;
        }

        public override ToolType getToolType()
        {
            return ToolType.ArcCircleTool;
        }
    }
}
