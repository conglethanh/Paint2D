using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612829_1612842
{
    public class EllipseTool:RectangleTool
    {
        public EllipseTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
        }
        public override void DrawRectangle(System.Drawing.Pen outline, System.Drawing.Brush fill)
        {
            g.FillEllipse(fill, rect);
            g.DrawEllipse(outline, rect);
        }

        public override ToolType getToolType()
        {
            return ToolType.EllipseTool;
        }
    }
}
