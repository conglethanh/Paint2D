using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class SelectTool : Tool
    {
        public SelectTool(ToolSettings args)
            : base(args)
        {
            args.pictureBox.Cursor = Cursors.Arrow;
           
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
            return ToolType.SelectTool;
        }
    }
}
