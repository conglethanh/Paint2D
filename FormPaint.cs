using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.Xml;
namespace _1612829_1612842
{
    public partial class FormPaint : Form, MyPaintSettings
    {
        bool drawing;
        bool isErasing;
        public int WIDTH, HEIGHT;

        private MyPaintSettings settings;
        private ToolSettings toolSettings;
        private Tool curTool;

        private int prevShape;
        private Point start;
        private Point endOld;
        private int sizeControlPoint;

        private SelectToolType selectType;
        private List<Point> controlPointOld;
        private List<Point> exactlyPointOld;
        private int angle;
        private int alpha;
        Point mid;

        public XMLShapes saveShape;
        private static List<Shape> listShape;
        private Color backGroundColor;
        private ImageFile imageFile;
        private bool isBackgroundImage;



        private MyImage buffer;
        private Shape copyCutShape;
        private List<Shape> undoLShape;
        private List<Shape> redoLShape;

        public FormPaint()
        {
            InitializeComponent();
            settings = (MyPaintSettings)this;

            listShape = new List<Shape>();
            isBackgroundImage = false;
            backGroundColor = Color.White;
            selectType = SelectToolType.select;
            sizeControlPoint = 3;
            prevShape = -1;
            drawing = false;
            isErasing = false;
            WIDTH = 677;
            HEIGHT = 385;
            button_Redo.Enabled = false;
            button_Undo.Enabled = false;
        }

        private void toolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            if (isErasing)
                isErasing = false;
            curTool.UnloadTool();
            ToolBarButton curButton = e.Button;
            setToolbarBtnSelected(curButton);
            if (curButton == btn_select)
            {
                curTool = new SelectTool(toolSettings);
            }

            else if (curButton == btn_line)
            {
                curTool = new LineTool(toolSettings);
            }

            else if (curButton == btn_rectangle)
            {
                curTool = new RectangleTool(toolSettings);
            }
            else if (curButton == btn_text)
            {
                curTool = new TextTool(toolSettings);
            }
            else if (curButton == btn_fill)
            {
                curTool = new FillTool(toolSettings);
            }
            else if (curButton == btn_ellipse)
            {
                curTool = new EllipseTool(toolSettings);
            }
            else if (curButton == btn_circle)
            {
                curTool = new CircleTool(toolSettings);
            }
            else if (curButton == btn_parallelogram)
            {
                curTool = new ParallelogramTool(toolSettings);
            }
            else if (curButton == btn_arcEllipse)
            {
                curTool = new ArcEllipseTool(toolSettings);
            }
            else if (curButton == btn_arcCircle)
            {
                curTool = new ArcCircleTool(toolSettings);
            }
            else if (curButton == btn_polygon)
            {
                curTool = new PolygonTool(toolSettings);
            }
            else if (curButton == btn_polyline)
            {
                curTool = new PolylineTool(toolSettings);
            }
            else if (curButton == btn_bezier)
            {
                curTool = new BezierTool(toolSettings);
            }
            else if (curButton == btn_parabol)
            {
                curTool = new ParabolTool(toolSettings);
            }
            else if (curButton == btn_hyperbol)
            {
                curTool = new HyperbolTool(toolSettings);
            }
            else if (curButton == btn_eraser)
            {
                curTool = new EraseTool(toolSettings);
            }

        }

        void setToolbarBtnSelected(ToolBarButton curButton)
        {
            curButton.Pushed = true;
            foreach (ToolBarButton btn in toolBar.Buttons)
            {
                if (btn != curButton)
                    btn.Pushed = false;
            }
        }

        private void ShowImage()
        {
            if (imageFile.FileName == null)
            {
                Text = "Paint - Untitled";
            }
            else
            {
                Text = "Paint - " + new FileInfo(imageFile.FileName).Name;
            }
            pictureBox.ClientSize = imageFile.Bitmap.Size;
            pictureBox.Invalidate();
            toolSettings = new ToolSettings(imageFile.Bitmap, pictureBox, settings);

            if (curTool != null)
                curTool.UnloadTool();
            curTool = new SelectTool(toolSettings);
            setToolbarBtnSelected(btn_select);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            Rectangle clipRect = e.ClipRectangle;
            Bitmap b = toolSettings.bitmap.Clone(clipRect, toolSettings.bitmap.PixelFormat);
            e.Graphics.DrawImageUnscaledAndClipped(b, clipRect);
            b.Dispose();
        }


        int MyPaintSettings.Width
        {
            get
            {
                return Int32.Parse(cbx_lineWidth.Text);
            }
        }

        Color MyPaintSettings.PrimaryColor
        {
            get
            {
                return primaryColor.BackColor;
            }
        }

        Color MyPaintSettings.SecondaryColor
        {
            get
            {
                return secondaryColor.BackColor;
            }
        }

        Color MyPaintSettings.OutlineColor
        {
            get
            {
                return picBoxOutlineColor.BackColor;
            }
        }

        DashStyle MyPaintSettings.LineStyle
        {
            get
            {
                if (cbx_lineStyle.SelectedIndex == 6) {
                    return (DashStyle)(0);
                }
                return (DashStyle)cbx_lineStyle.SelectedIndex;
            }
        }

        HatchStyle MyPaintSettings.HatchStyle
        {
            get
            {
                int index = cbx_fillStyle.SelectedIndex;
                if (index <= 3)
                    index = 0;
                else
                    index -= 3;
                return (HatchStyle)index;
            }
        }

        Font MyPaintSettings.font
        {
            get
            {
                return InputTextForm.font;
            }
        }

        FillStyle MyPaintSettings.FillStyle
        {
            get
            {
                int index = cbx_fillStyle.SelectedIndex;
                if (index <= 3)
                    return (FillStyle)index;
                else
                    return FillStyle.HatchBrush;
            }
        }

        Image MyPaintSettings.TextureBrushImage
        {
            get
            {
                Image temp = imageTextureBox.Image;

                //if (temp == null)
                //{
                //    temp = new Bitmap(pictureBox.Width, pictureBox.Height);
                //}

                return temp;
            }
        }

        private void FormPaint_Load(object sender, EventArgs e)
        {
            // fill Width list
            for (int i = 1; i < 11; i++)
            {
                cbx_lineWidth.Items.Add(i);

            }
            for (int i = 15; i <= 60; i += 5)
            {
                cbx_lineWidth.Items.Add(i);

            }
            cbx_lineWidth.SelectedIndex = 0;


            for (int i = 0; i < 5; i++)
            {
                DashStyle ds = (DashStyle)i;

                cbx_lineStyle.Items.Add("PS_" + ds.ToString());
            }
            cbx_lineStyle.Items.Add("PS_null");
            cbx_lineStyle.SelectedIndex = 0;

            // fill (fill style) list
            for (int i = 0; i < 3; i++)
            {
                FillStyle temp = (FillStyle)i;
                cbx_fillStyle.Items.Add(temp);
            }
            for (int i = 0; i < 50; i++)
            {
                HatchStyle tmp = (HatchStyle)i;
                cbx_fillStyle.Items.Add(tmp);
            }
            cbx_fillStyle.SelectedIndex = 0;

            // default image
            imageFile = new ImageFile(new Size(pictureBox.Width, pictureBox.Height), Color.White);
            ShowImage();
        }

        private void listShape_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics.FromImage(imageFile.Bitmap).Clear(Color.White);
            backGroundColor = Color.White;
            picBoxBackGroundColor.BackColor = backGroundColor;
            pictureBox.Invalidate();
            imageFile = new ImageFile(new Size(WIDTH, HEIGHT), Color.White);

            ShowImage();
            listShape.Clear();
            checkListShape.Items.Clear();
            prevShape = -1;
            status_selectShape.Text = "";
            Text = "Paint - Untitled";
            isBackgroundImage = false;

            drawing = false;

        }

        private void primaryColor_Click(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;

            colorDlg.Color = picBox.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                picBox.BackColor = colorDlg.Color;
                if (prevShape != -1)
                {
                    listShape[prevShape].changePrimaryColor(picBox.BackColor);
                    redrawAllShapeToScreen();
                }
            }
        }

        private void secondaryColor_Click(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;

            colorDlg.Color = picBox.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                picBox.BackColor = colorDlg.Color;
                if (prevShape != -1)
                {
                    listShape[prevShape].changeSecondColor(picBox.BackColor);
                    redrawAllShapeToScreen();
                }
            }
        }
        private int getMaxNumericFromListShape()
        {

            if (listShape.Count == 0)
                return 0;
            int res = listShape[0].stt;
            foreach (var item in listShape)
            {
                if (res < item.stt)
                {
                    res = item.stt;
                }
            }
            return res + 1;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (drawing && curTool.getToolType() == ToolType.EraseTool)
            {
                drawing = false;
                imageFile = new ImageFile(new MyImage(toolSettings.bitmap).s);
                return;
            }

            //=================================//
            curTool.OnMouseUp(sender, e);
            if (drawing && e.Button == MouseButtons.Right)
            {
                drawing = false;
                int numeric = getMaxNumericFromListShape();
                if (curTool.getToolType() == ToolType.PolylineTool)
                {

                    List<Point> listP = ((PolylineTool)curTool).getPolyPoint();
                    curTool = new PolylineTool(toolSettings);
                    Shape poly = new MyPolyline(listP, toolSettings.settings, numeric);
                    listShape.Add(poly);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                }
                else if (curTool.getToolType() == ToolType.PolygonTool)
                {

                    List<Point> listP = ((PolygonTool)curTool).getPolyPoint();
                    curTool = new PolygonTool(toolSettings);
                    Shape poly = new MyPolygon(listP, toolSettings.settings, numeric);
                    listShape.Add(poly);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                }
                else if (curTool.getToolType() == ToolType.BezierTool)
                {

                    List<Point> listP = ((BezierTool)curTool).getPolyPoint();
                    curTool = new BezierTool(toolSettings);
                    if (listP.Count == 4)
                    {
                        Shape bezier = new MyBezier(listP, toolSettings.settings, numeric);
                        listShape.Add(bezier);
                        checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                    }
                    redrawAllShapeToScreen();
                }




            }
            else if (drawing && e.Button == MouseButtons.Left)
            {
                if (curTool.getToolType() != ToolType.PolylineTool && curTool.getToolType() != ToolType.PolygonTool
                    && curTool.getToolType() != ToolType.BezierTool)
                    drawing = false;
                else
                    drawing = true;
                int numeric = getMaxNumericFromListShape();
                if (curTool.getToolType() == ToolType.LineTool)
                {
                    //save line
                    Shape line = new StraightLine(start, e.Location, toolSettings.settings, numeric);
                    listShape.Add(line);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.SelectTool)
                {
                    selectType = SelectToolType.select;
                }
                else if (curTool.getToolType() == ToolType.RectangleTool)
                {
                    Shape rect = new MyRectangle(start, e.Location, toolSettings.settings, numeric);
                    listShape.Add(rect);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.TextTool)
                {
                    Shape tex = new MyText(InputTextForm.text, e.Location, toolSettings.settings, numeric);
                    listShape.Add(tex);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.ParallelogramTool)
                {
                    Shape parallel = new MyParallelogram(start, e.Location, toolSettings.settings, numeric);
                    listShape.Add(parallel);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.EllipseTool)
                {
                    Shape ellipse = new MyEllipse(start, e.Location, toolSettings.settings, numeric);
                    listShape.Add(ellipse);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.CircleTool)
                {
                    Shape circle = new MyCircle(start, e.Location, toolSettings.settings, numeric);
                    listShape.Add(circle);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.ArcEllipseTool)
                {
                    ArcEllipseTool arcE = (ArcEllipseTool)curTool;
                    alpha = arcE.getAlphaAngleArc();

                    Shape arcEllipse = new MyArcEllipse(start, e.Location, alpha, toolSettings.settings, numeric);
                    listShape.Add(arcEllipse);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());


                }
                else if (curTool.getToolType() == ToolType.ArcCircleTool)
                {
                    ArcCircleTool arcC = (ArcCircleTool)curTool;
                    alpha = arcC.getAlphaAngleArc();

                    Shape arcCircle = new MyArcCircle(start, e.Location, alpha, toolSettings.settings, numeric);
                    listShape.Add(arcCircle);
                    checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                }
                else if (curTool.getToolType() == ToolType.ParabolTool)
                {

                    List<Point> listP = ((ParabolTool)curTool).getPolyPoint();
                    curTool = new ParabolTool(toolSettings);
                    if (listP.Count >= 4)
                    {
                        Shape parabol = new MyParabol(listP, toolSettings.settings, numeric);
                        listShape.Add(parabol);
                        checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                    }
                }
                else if (curTool.getToolType() == ToolType.HyperbolTool)
                {

                    List<Point> listH = ((HyperbolTool)curTool).getPolyPoint();
                    curTool = new HyperbolTool(toolSettings);

                    if (listH.Count >= 4)
                    {
                        Shape hyper = new MyHyperbol(listH, toolSettings.settings, numeric);
                        listShape.Add(hyper);
                        checkListShape.Items.Add("Shape " + listShape[listShape.Count - 1].getName());
                    }
                }

            }
            else
            {
                selectType = SelectToolType.select;
            }

            if (listShape.Count > 0 && button_Redo.Enabled == false)
                button_Undo.Enabled = true;
        }
        public Point getPointFarest(List<Point> listPoint, Point point)
        {
            double distanceMax = Math.Pow(point.X - listPoint[0].X, 2) + Math.Pow(point.Y - listPoint[0].Y, 2);
            Point res = listPoint[0];
            foreach (Point p in listPoint)
            {
                if (Math.Pow(point.X - p.X, 2) + Math.Pow(point.Y - p.Y, 2) > distanceMax)
                {
                    distanceMax = Math.Pow(point.X - p.X, 2) + Math.Pow(point.Y - p.Y, 2);
                    res = p;
                }
            }
            return res;
        }

        public Point getMid(List<Point> points)
        {
            Point mid = new Point(0, 0);

            foreach (var p in points)
            {
                mid.X += p.X;
                mid.Y += p.Y;
            }
            mid.X /= points.Count;
            mid.Y /= points.Count;
            return mid;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            button_Undo.Enabled = true;
            button_Redo.Enabled = false;
            ExportToXML("undo.xml");

            start = e.Location;
            if (curTool.getToolType() == ToolType.SelectTool)
            {
                if (selectType == SelectToolType.select && prevShape == -1)
                {
                    chooseShapeWithMouse(e.Location);
                    if (e.Button == MouseButtons.Right)
                    {
                        if (prevShape != -1)
                            contextMenuStrip.Show(this, e.Location, ToolStripDropDownDirection.Right);
                        else
                            contextMenuStripNotSelectObj.Show(this, e.Location, ToolStripDropDownDirection.Right);
                    }
                }
                if (prevShape != -1)
                {
                    if (listShape[prevShape].getType() == ShapeType.parallelogram
                        && listShape[prevShape].checkNearPoints(SelectToolType.parallelPoint, e.Location))
                    {
                        if (selectType == SelectToolType.select)
                        {
                            selectType = SelectToolType.parallelPoint;
                            start = e.Location;
                            return;
                        }
                    }


                    if (listShape[prevShape].checkNearPoints(SelectToolType.resize, e.Location))
                    {
                        if (selectType == SelectToolType.select)
                        {
                            selectType = SelectToolType.resize;


                            start = getPointFarest(listShape[prevShape].getControlPoint(), e.Location);
                            endOld = e.Location;
                            exactlyPointOld = listShape[prevShape].getExactlyPoints();
                            mid = getMid(listShape[prevShape].getControlPoint());
                            angle = listShape[prevShape].getAngle();
                            if (angle != 0)
                            {
                                start = Shape.rotatePoint(start, 0, mid);
                                endOld = Shape.rotatePoint(endOld, 0, mid);
                                listShape[prevShape].setAngle(0);
                            }

                        }
                    }
                    else if (listShape[prevShape].checkNearPoints(SelectToolType.moving, e.Location))
                    {
                        if (selectType == SelectToolType.select)
                        {
                            selectType = SelectToolType.moving;

                            //lấy start là góc để xoay sau đó di chuyển và xoay trở lại
                            start = e.Location;
                            angle = listShape[prevShape].getAngle();
                            listShape[prevShape].setAngle(0);

                            exactlyPointOld = listShape[prevShape].getExactlyPoints();
                            controlPointOld = listShape[prevShape].getControlPoint();
                        }
                    }
                    else if (listShape[prevShape].checkNearPoints(SelectToolType.rotate, e.Location))
                    {
                        if (selectType == SelectToolType.select)
                        {
                            selectType = SelectToolType.rotate;
                            start = e.Location;
                            controlPointOld = listShape[prevShape].getControlPoint();
                        }
                    }
                    else
                    {
                        chooseShapeWithMouse(e.Location);
                        if (e.Button == MouseButtons.Right)
                        {
                            contextMenuStrip.Show(this, e.Location, ToolStripDropDownDirection.Right);
                        }
                    }
                }
            }
            else if (curTool.getToolType() == ToolType.FillTool)
            {
                prevShape = Tool.getShapeNearMouse(listShape, e.Location);
                if (prevShape != -1)
                {
                    status_selectShape.Text = "Selected: Shape " + prevShape.ToString();
                    checkListShape.SelectedIndex = prevShape;
                    listShape[prevShape].changePrimaryColor(settings.PrimaryColor);
                    listShape[prevShape].changeSecondColor(settings.SecondaryColor);
                    listShape[prevShape].changeFillStyle(settings.FillStyle);
                    listShape[prevShape].changeHatchStyle(settings.HatchStyle);
                    listShape[prevShape].changeImageTexture(settings.TextureBrushImage);
                    redrawAllShapeToScreen();
                }
            }
            else if (curTool.getToolType() == ToolType.EraseTool)
            {
                if (isErasing == false)
                {
                    isErasing = true;
                    buffer = new MyImage(toolSettings.bitmap);

                    listShape.Clear();
                    checkListShape.Items.Clear();
                    prevShape = -1;
                    status_selectShape.Text = "";

                    imageFile = new ImageFile(buffer.s);
                    isBackgroundImage = true;
                }
                else
                {
                    drawing = true;
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    drawing = true;
                    curTool.OnMouseDown(sender, e);
                }
            }
        }

        private void chooseShapeWithMouse(Point p)
        {
            prevShape = Tool.getShapeNearMouse(listShape, p);
            if (prevShape != -1)
            {
                status_selectShape.Text = "Selected: Shape " + listShape[prevShape].getName();
                checkListShape.SelectedItem = "Shape " + listShape[prevShape].getName();
                redrawAllShapeToScreen();
            }
        }



        private void redrawAllShapeToScreen()
        {

            if (isBackgroundImage)
            {
                toolSettings.bitmap = imageFile.getMyBGImageXML();
                pictureBox.Image = imageFile.getMyBGImageXML();
            }
            else
                Graphics.FromImage(toolSettings.bitmap).Clear(backGroundColor);
            pictureBox.Invalidate();
            if (listShape.Count > 0)
            {
                foreach (Shape item in listShape)
                {
                    item.drawWithAngle(toolSettings);
                }
                if (prevShape != -1)
                {
                    listShape[prevShape].drawAllPointToShape(toolSettings, sizeControlPoint);
                }
            }
        }



        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (curTool.getToolType() == ToolType.SelectTool)
            {
                if (prevShape != -1)



                    if (selectType == SelectToolType.resize)
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Cross;

                        if (listShape[prevShape].getType() == ShapeType.straightLine)
                        {
                            listShape[prevShape].resize(start, e.Location);
                            redrawAllShapeToScreen();
                        }
                        else
                        {
                            Point tmp = e.Location;
                            tmp = Shape.rotatePoint(tmp, 0, mid);
                            if (listShape[prevShape].getType() != ShapeType.polyline && listShape[prevShape].getType() != ShapeType.polygon
                                && listShape[prevShape].getType() != ShapeType.parabol
                                && listShape[prevShape].getType() != ShapeType.hyperbol
                                && listShape[prevShape].getType() != ShapeType.bezier
                                )
                                listShape[prevShape].resize(start, tmp);
                            else
                                listShape[prevShape].resize(exactlyPointOld, start, endOld, tmp);
                            listShape[prevShape].setAngle(angle);
                            redrawAllShapeToScreen();
                        }
                    }
                    else if (selectType == SelectToolType.moving)
                    {
                        toolSettings.pictureBox.Cursor = Cursors.SizeAll;
                        if (listShape[prevShape].getType() == ShapeType.polyline || listShape[prevShape].getType() == ShapeType.polygon
                            || listShape[prevShape].getType() == ShapeType.parabol
                            || listShape[prevShape].getType() == ShapeType.hyperbol
                            || listShape[prevShape].getType() == ShapeType.bezier
                            )
                        {
                            listShape[prevShape].moveShape(exactlyPointOld, start, e.Location);
                        }
                        else
                            listShape[prevShape].moveShape(controlPointOld, start, e.Location);
                        listShape[prevShape].setAngle(angle);
                        redrawAllShapeToScreen();

                    }
                    else if (selectType == SelectToolType.rotate)
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Hand;

                        Graphics.FromImage(toolSettings.bitmap).Clear(backGroundColor);
                        foreach (Shape item in listShape)
                        {
                            if (item == listShape[prevShape])
                                listShape[prevShape].rotateAndDraw(controlPointOld, start, e.Location, toolSettings);
                            else
                            {
                                item.drawWithAngle(toolSettings);
                            }
                        }
                        if (prevShape != -1)
                        {
                            listShape[prevShape].drawAllPointToShape(toolSettings, sizeControlPoint);
                        }
                    }
                    else if (selectType == SelectToolType.parallelPoint)
                    {

                        toolSettings.pictureBox.Cursor = Cursors.Hand;
                        listShape[prevShape].changeAlphaParallelogram(start, e.Location);
                        redrawAllShapeToScreen();

                    }
                    else if (listShape[prevShape].checkNearPoints(SelectToolType.resize, e.Location))
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Cross;
                    }
                    else if (listShape[prevShape].checkNearPoints(SelectToolType.moving, e.Location))
                    {
                        toolSettings.pictureBox.Cursor = Cursors.SizeAll;
                    }
                    else if (listShape[prevShape].checkNearPoints(SelectToolType.rotate, e.Location))
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Hand;
                    }
                    else if (listShape[prevShape].getType() == ShapeType.parallelogram &&
                        listShape[prevShape].checkNearPoints(SelectToolType.parallelPoint, e.Location))
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Hand;
                    }
                    else
                    {
                        toolSettings.pictureBox.Cursor = Cursors.Arrow;
                    }
            }
            else if (curTool.getToolType() == ToolType.EraseTool)
            {
                if (isErasing && drawing)
                {
                    Brush brush = new SolidBrush(backGroundColor);
                    int ww = 10 * toolSettings.settings.Width;
                    Rectangle rect = new Rectangle(e.X - ww / 2, e.Y - ww / 2, ww, ww);
                    Graphics.FromImage(toolSettings.bitmap).FillEllipse(brush, rect);
                    pictureBox.Invalidate();
                }
            }
            else
                curTool.OnMouseMove(sender, e);
        }

        private void checkListShape_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tmp = checkListShape.SelectedIndex;
            if (tmp < 0 || tmp > listShape.Count - 1)
                return;

            status_selectShape.Text = "Selected: " + checkListShape.Items[tmp].ToString();
            Shape shapeSelected = listShape[tmp];

            List<Point> control = shapeSelected.getControlPoint();

            redrawAllShapeToScreen();
            pictureBox.Invalidate();
            prevShape = tmp;
        }



        private void btn_deselect_Click(object sender, EventArgs e)
        {
            if (prevShape != -1)
            {
                prevShape = -1;
                redrawAllShapeToScreen();
                status_selectShape.Text = "";
            }
        }

        private void cbx_lineWidth_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (prevShape != -1)
            {
                listShape[prevShape].changeWidth(settings.Width);
                redrawAllShapeToScreen();
            }
        }

        private void cbx_lineStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (prevShape != -1)
            {
                if (cbx_lineStyle.SelectedItem.ToString() == "PS_null")
                {
                    
                    listShape[prevShape].changeOutlineColor(backGroundColor);
                    listShape[prevShape].changeLineStyle((DashStyle)0);
                }
                else
                {
                    listShape[prevShape].changeOutlineColor(picBoxOutlineColor.BackColor);
                    listShape[prevShape].changeLineStyle(settings.LineStyle);
                }

                redrawAllShapeToScreen();
            }
            else
            {
                if (cbx_lineStyle.SelectedItem.ToString() == "PS_null")
                {
                    picBoxOutlineColor.BackColor = picBoxBackGroundColor.BackColor;
                }
                else
                {
                    picBoxOutlineColor.BackColor = Color.Black;
                }
            }
        }


        private void imageTextureBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Image Files .BMP .JPG .GIF .Png|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                imageTextureBox.Image = Image.FromFile(openDlg.FileName);
                if (prevShape != -1)
                {
                    listShape[prevShape].changeImageTexture(imageTextureBox.Image);
                    redrawAllShapeToScreen();
                }
            }
        }

        private void cbx_fillStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (prevShape != -1)
            {
                listShape[prevShape].changeFillStyle(settings.FillStyle);
                listShape[prevShape].changeHatchStyle(settings.HatchStyle);
                listShape[prevShape].changeImageTexture(settings.TextureBrushImage);
                redrawAllShapeToScreen();
            }
        }

        private void reset()
        {
            drawing = false;
            this.prevShape = -1;
            listShape.Clear();
            checkListShape.Items.Clear();
            status_selectShape.Text = "";
            Text = "Untitled";
            isBackgroundImage = false;
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reset();
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "Image Files .BMP .JPG .GIF .Png|*.BMP;*.JPG;*.GIF;*.PNG";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                if (imageFile.Open(openDlg.FileName))
                {
                    isBackgroundImage = true;
                    ShowImage();
                }
                else
                {
                    MessageBox.Show("Error");
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "Image Files (*.BMP)|*.BMP";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                toolSettings.bitmap.Save(saveDlg.FileName);
                imageFile = new ImageFile(new MyImage(toolSettings.bitmap).s);
                ShowImage();
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonFontDialog_Click(object sender, EventArgs e)
        {
            if (prevShape != -1)
            {
                if (listShape[prevShape].getType() == ShapeType.text)
                {
                    MyText temp = (MyText)listShape[prevShape];
                    InputTextForm textForm = new InputTextForm(temp.getText(), temp.getFont());

                    if (textForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        temp.setFont(textForm.getFont());
                        temp.setText(textForm.getText());
                        listShape[prevShape] = temp;
                        redrawAllShapeToScreen();
                    }
                }
            }

        }

        private void resetPaintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearAllToolStripMenuItem_Click(sender, e);
        }

        private void deleteSelectedShapeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listShape.Count > 0)
            {
                listShape.Remove(listShape[prevShape]);
                prevShape = -1;

                checkListShape.Items.Clear();
                foreach (var item in listShape)
                {
                    checkListShape.Items.Add("Shape " + item.getName());
                }
                status_selectShape.Text = "";
                redrawAllShapeToScreen();
            }
        }

        private void saveFileXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "XML File (*.xml) |*.xml";
            DialogResult ret = saveDlg.ShowDialog();
            if (ret == DialogResult.OK)
            {
                string fileName = saveDlg.FileName;
                ExportToXML(fileName);
            }
        }

        private void openFileXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "XML File (*.xml) |*.xml";
            DialogResult ret = openDlg.ShowDialog();
            if (ret == DialogResult.OK)
            {
                string fileName = openDlg.FileName;
                DeSerializeXML(fileName);
                redrawAllShapeToScreen();
            }
        }

        public void ExportToXML(string fileName)
        {
            TextWriter w = new StreamWriter(fileName);
            saveShape = new XMLShapes(listShape, isBackgroundImage, imageFile, this.WIDTH, this.HEIGHT, backGroundColor);
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(XMLShapes));
                s.Serialize(w, saveShape);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Serialize XML " + e.Message);
                System.Console.WriteLine("Serialize XML " + e.InnerException.Message);
            }
            finally
            {
                w.Close();
            }
        }

        public void DeSerializeXML(string fileName)
        {
            if (File.Exists(fileName))
            {
                StreamReader sr = new StreamReader(fileName);
                XmlTextReader xr = new XmlTextReader(sr);
                XmlSerializer xs = new XmlSerializer(typeof(XMLShapes));
                if (xs.CanDeserialize(xr))
                {
                    try
                    {
                        saveShape = (XMLShapes)xs.Deserialize(xr);
                        listShape = saveShape.listShape;

                        drawing = false;
                        this.prevShape = -1;
                        checkListShape.Items.Clear();
                        status_selectShape.Text = "";
                        Text = "Paint - " + fileName;

                        isBackgroundImage = saveShape.isBackGroundImage;
                        if (isBackgroundImage)
                            imageFile = new ImageFile(saveShape.imageFile);
                        else
                        {
                            imageFile = new ImageFile(new Size(saveShape.w, saveShape.h), saveShape.bgColor.ToColor());

                            pictureBox.ClientSize = imageFile.Bitmap.Size;
                            pictureBox.Invalidate();
                            toolSettings = new ToolSettings(imageFile.Bitmap, pictureBox, settings);

                            if (curTool != null)
                                curTool.UnloadTool();
                            curTool = new SelectTool(toolSettings);
                            setToolbarBtnSelected(btn_select);

                            backGroundColor = saveShape.bgColor.ToColor();
                            picBoxBackGroundColor.BackColor = backGroundColor;
                        }




                        foreach (var item in listShape)
                        {
                            item.updateInfoAfterLoadFileXML();
                            checkListShape.Items.Add("Shape " + item.getName());
                        }

                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Open file " + e.InnerException.Message);
                    }
                }
                xr.Close();
                sr.Close();
            }
        }

        private void picBoxOutlineColor_Click(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;

            colorDlg.Color = picBox.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                picBox.BackColor = colorDlg.Color;
                if (prevShape != -1)
                {
                    listShape[prevShape].changeOutlineColor(picBox.BackColor);
                    redrawAllShapeToScreen();
                }
            }
        }

        private void picBoxBackGroundColor_Click(object sender, EventArgs e)
        {
            PictureBox picBox = (PictureBox)sender;
            ColorDialog colorDlg = new ColorDialog();
            colorDlg.FullOpen = true;

            colorDlg.Color = picBox.BackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                picBox.BackColor = colorDlg.Color;

                backGroundColor = picBox.BackColor;
                redrawAllShapeToScreen();

            }
        }

        private void cbx_lineWidth_TextChanged(object sender, EventArgs e)
        {
            if (cbx_lineWidth.Text == "")
                cbx_lineWidth.Text = "1";
        }

        private void cbx_lineStyle_TextChanged(object sender, EventArgs e)
        {
            if (cbx_lineStyle.Text == "")
                cbx_lineStyle.Text = "PS_Solid";
        }

        private void cbx_fillStyle_TextChanged(object sender, EventArgs e)
        {
            if (cbx_fillStyle.Text == "")
                cbx_fillStyle.Text = "SolidBrush";
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInputSizeWorkplace form = new FormInputSizeWorkplace();
            form.ShowDialog();
            form.ShowInTaskbar = false;
            if (form.DialogResult == DialogResult.OK)
            {
                clearAllToolStripMenuItem_Click(sender, e);
                WIDTH = form.getSizeWidth();
                HEIGHT = form.getSizeHeight();


                pictureBox.ClientSize = new Size(WIDTH, HEIGHT);
                imageFile = new ImageFile(new Size(WIDTH, HEIGHT), backGroundColor);
                toolSettings = new ToolSettings(imageFile.Bitmap, pictureBox, settings);

                pictureBox.Invalidate();
            }
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormInputSizeWorkplace form = new FormInputSizeWorkplace();
            form.ShowDialog();
            form.ShowInTaskbar = false;
            if (form.DialogResult == DialogResult.OK)
            {

                WIDTH = form.getSizeWidth();
                HEIGHT = form.getSizeHeight();


                pictureBox.ClientSize = new Size(WIDTH, HEIGHT);
                imageFile = new ImageFile(new Size(WIDTH, HEIGHT), backGroundColor);
                toolSettings = new ToolSettings(imageFile.Bitmap, pictureBox, settings);

                pictureBox.Invalidate();
                redrawAllShapeToScreen();
            }
        }

        private void bringToFontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Shape tmp = listShape[prevShape];
            listShape.RemoveAt(prevShape);
            listShape.Add(tmp);
            prevShape = listShape.Count - 1;
            checkListShape.Items.Clear();
            foreach (var item in listShape)
            {
                checkListShape.Items.Add("Shape " + item.getName());
            }
            redrawAllShapeToScreen();
        }

        private void bringForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (prevShape != listShape.Count - 1)
            {
                Shape tmp = listShape[prevShape];
                listShape.RemoveAt(prevShape);
                listShape.Insert(prevShape + 1, tmp);
                prevShape++;
                checkListShape.Items.Clear();
                foreach (var item in listShape)
                {
                    checkListShape.Items.Add("Shape " + item.getName());
                }
                redrawAllShapeToScreen();
            }
        }

        private void sendToBackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Shape tmp = listShape[prevShape];
            listShape.RemoveAt(prevShape);
            listShape.Insert(0, tmp);
            prevShape = 0;
            checkListShape.Items.Clear();
            foreach (var item in listShape)
            {
                checkListShape.Items.Add("Shape " + item.getName());
            }
            redrawAllShapeToScreen();
        }

        private void sendBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (prevShape > 0)
            {
                Shape tmp = listShape[prevShape];
                listShape.RemoveAt(prevShape);
                listShape.Insert(prevShape - 1, tmp);
                prevShape--;
                checkListShape.Items.Clear();
                foreach (var item in listShape)
                {
                    checkListShape.Items.Add("Shape " + item.getName());
                }
                redrawAllShapeToScreen();
            }
        }

        public struct MyImage
        {
            public string s;
            public MyImage(Image im)
            {
                if (im == null)
                {
                    s = "";
                    return;
                }
                MemoryStream ms = new MemoryStream();
                im.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] array = ms.ToArray();

                s = Convert.ToBase64String(array);

            }
            public Image ToImage()
            {
                if (s == "")
                    return null;
                byte[] array = Convert.FromBase64String(s);
                Image image = Image.FromStream(new MemoryStream(array));

                return image;
            }

        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyCutShape = listShape[prevShape];
            status_selectShape.Text = "Copied!";
        }

        void updateAllShape()
        {
            redrawAllShapeToScreen();
            checkListShape.Items.Clear();
            foreach (var item in listShape)
            {
                checkListShape.Items.Add("Shape " + item.getName());
            }
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int numeric = getMaxNumericFromListShape();
            cloneShape(numeric);
            listShape.Add(copyCutShape);
            status_selectShape.Text = "Pasted!";
            updateAllShape();
            prevShape = listShape.Count - 1;
        }

        private void cloneShape(int numeric)
        {
            switch (copyCutShape.getType())
            {
                case ShapeType.rectangle:
                    copyCutShape = new MyRectangle((MyRectangle)copyCutShape, numeric);
                    break;
                case ShapeType.straightLine:
                    copyCutShape = new StraightLine((StraightLine)copyCutShape, numeric);
                    break;
                case ShapeType.text:
                    copyCutShape = new MyText((MyText)copyCutShape, numeric);
                    break;
                case ShapeType.polyline:
                    copyCutShape = new MyPolyline((MyPolyline)copyCutShape, numeric);
                    break;
                case ShapeType.polygon:
                    copyCutShape = new MyPolygon((MyPolygon)copyCutShape, numeric);
                    break;
                case ShapeType.parallelogram:
                    copyCutShape = new MyParallelogram((MyParallelogram)copyCutShape, numeric);
                    break;
                case ShapeType.parabol:
                    copyCutShape = new MyParabol((MyParabol)copyCutShape, numeric);
                    break;
                case ShapeType.hyperbol:
                    copyCutShape = new MyHyperbol((MyHyperbol)copyCutShape, numeric);
                    break;
                case ShapeType.ellipse:
                    copyCutShape = new MyEllipse((MyEllipse)copyCutShape, numeric);
                    break;
                case ShapeType.circle:
                    copyCutShape = new MyCircle((MyCircle)copyCutShape, numeric);
                    break;
                case ShapeType.bezier:
                    copyCutShape = new MyBezier((MyBezier)copyCutShape, numeric);
                    break;
                case ShapeType.arcCircle:
                    copyCutShape = new MyArcCircle((MyArcCircle)copyCutShape, numeric);
                    break;
                case ShapeType.arcEllipse:
                    copyCutShape = new MyArcEllipse((MyArcEllipse)copyCutShape, numeric);
                    break;

            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyCutShape = listShape[prevShape];
            status_selectShape.Text = "Cut!";
            listShape.RemoveAt(prevShape);
            prevShape = -1;
            updateAllShape();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem1_Click(sender, e);
        }

        private void button_Undo_Click(object sender, EventArgs e)
        {
            button_Undo.Enabled = false;
            button_Redo.Enabled = true;
            ExportToXML("redo.xml");
            if (File.Exists("undo.xml"))
            {
                DeSerializeXML("undo.xml");
                redrawAllShapeToScreen();

            }

        }

        private void button_Redo_Click(object sender, EventArgs e)
        {
            button_Undo.Enabled = true;
            button_Redo.Enabled = false;
            if (File.Exists("redo.xml"))
            {
                ExportToXML("undo.xml");
                DeSerializeXML("redo.xml");
                redrawAllShapeToScreen();
                
            }

        }

        private void FormPaint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists("redo.xml"))
            {
                File.Delete("redo.xml");
            }
            if (File.Exists("undo.xml"))
            {
                File.Delete("undo.xml");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.ShowInTaskbar = false;
            a.ShowDialog();
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HDSD a = new HDSD();
            a.ShowInTaskbar = false;
            a.Show();
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (prevShape != -1)
            {
                Bitmap b= new Bitmap(WIDTH,HEIGHT,PixelFormat.Format32bppArgb);
                Graphics.FromImage(b).Clear(Color.White);
                PictureBox pic = new PictureBox();
                ToolSettings tmp = new ToolSettings(b, pic, settings);
                listShape[prevShape].drawWithAngle(tmp);
                Clipboard.SetImage(b);
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

        }

    }
}