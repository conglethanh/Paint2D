using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class FillTool : Tool
    {
        public FillTool(ToolSettings args)
            : base(args)
        {
            args.pictureBox.Cursor = Cursors.IBeam;

        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {

        }



        public override void OnMouseMove(object sender, MouseEventArgs e)
        {

        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {

        }
        public override void UnloadTool()
        {

        }

        public override ToolType getToolType()
        {
            return ToolType.FillTool;
        }
    }
}
