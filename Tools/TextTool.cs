using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace _1612829_1612842
{
    public class TextTool : Tool
    {
        public TextTool(ToolSettings toolSettings)
            : base(toolSettings)
        {
            toolSetting.pictureBox.Cursor = Cursors.Cross;
        }

        public override void OnMouseUp(object sender, MouseEventArgs e)
        {
            InputTextForm textForm = new InputTextForm();
            
            if (textForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Graphics g = Graphics.FromImage(toolSetting.bitmap);
                
                g.DrawString(textForm.getText(), textForm.getFont(), new SolidBrush(toolSetting.settings.OutlineColor), e.Location);
                toolSetting.pictureBox.Invalidate();
            }
        }

        public override void OnMouseMove(object sender, MouseEventArgs e)
        {

        }

        public override void OnMouseDown(object sender, MouseEventArgs e)
        {
            
        }

        public override void UnloadTool()
        {
            toolSetting.pictureBox.Cursor = Cursors.Default;

        }

        public override ToolType getToolType()
        {
            return ToolType.TextTool;
        }
    }
}
