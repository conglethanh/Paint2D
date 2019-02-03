using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612829_1612842
{
    public enum ShapeType
    {
        straightLine,
        rectangle,
        parallelogram,
        polygon,
        polyline,
        circle,
        ellipse,
        arcEllipse,
        parabol,
        hyperbol,
        text,
        bezier,
        arcCircle
    }

    public enum ToolType
    {
        SelectTool,
        LineTool,
        RectangleTool,
        TextTool,
        FillTool,
        EllipseTool,
        CircleTool,
        ParallelogramTool,
        ArcEllipseTool,
        ArcCircleTool,
        PolygonTool,
        PolylineTool,
        BezierTool,
        ParabolTool,
        HyperbolTool,
        EraseTool
    }

    public enum FillStyle
    {
        SolidBrush,
        Transparent,
        TextureBrush,
        HatchBrush
    }

    public enum SelectToolType
    {
        moving,
        resize,
        rotate,
        select,
        parallelPoint
    }
}
