using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class EraseTool : Tool
    {
        
        public EraseTool(ToolSettings args)
            : base(args)
        {
            args.pictureBox.Cursor = Cursors.Hand;

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
           
            toolSetting.pictureBox.Cursor = Cursors.Default;
        }

        public override ToolType getToolType()
        {
            return ToolType.EraseTool;
        }
    }
}
