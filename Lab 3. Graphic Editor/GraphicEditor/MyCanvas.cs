using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Threading;

namespace GraphicEditor
{
    class MyCanvas : Panel
    {
        private BufferedGraphics _drawingBufferedGraphics;
        private BufferedGraphics _imageBufferedGraphics;
        private bool _isDrawing;
        private Point _clickPoint;
        private Pen _currentPen;
        private Brush _currentBrush;
        private IDrawable _currentShape;
        private DrawingTools _currentTool;
        private List<Point> _movementPoints;

        public Bitmap Drawing
        {
            get
            {
                try
                { 
                    Bitmap currentDrawing = null;
                    using (Graphics canvasGraphics = CreateGraphics())
                    {
                        currentDrawing = new Bitmap(Width, Height, canvasGraphics);
                    }
                    using (Graphics bitmapGraphics = Graphics.FromImage(currentDrawing))
                    {
                        _imageBufferedGraphics.Render(bitmapGraphics);
                    }
                    return currentDrawing;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error creating Bitmap\n{0}", ex.Message);
                    throw new MyCanvasException("Can't create Bitmap");
                }
            }
        }

        private Graphics DrawingGraphics
        {
            get
            {
                Graphics graphicsFromBufferedGraphics = _drawingBufferedGraphics.Graphics;
                graphicsFromBufferedGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsFromBufferedGraphics.Clear(Color.White);
                _imageBufferedGraphics.Render(graphicsFromBufferedGraphics);
                return graphicsFromBufferedGraphics;
            }
        }

        public event EventHandler DrawingChanged;
        public event EventHandler MousePositionCoordChanged;
        public event EventHandler InverseProressChanged;
        public event EventHandler BeginInverse;
        public event EventHandler EndInverse;

        private MyCanvas()
        {
            _movementPoints = new List<Point>();
            DoubleBuffered = true;
            BackColor = Color.White;
            Location = new Point(5, 5);
            Cursor = Cursors.Cross;
        }

        public MyCanvas(int width, int height) : this()
        {
            if (Program.MIN_DRAWING_SIZE > width || width >= Program.MAX_DRAWING_SIZE 
                || Program.MIN_DRAWING_SIZE > height || height >= Program.MAX_DRAWING_SIZE)
            {
                string message = String.Format("Incorrect canvas size. Size range: {0} ... {1}",
                    Program.MIN_DRAWING_SIZE, Program.MAX_DRAWING_SIZE);
                throw new MyCanvasException(message);
            }
            Width = width;
            Height = height;

            InitBuffering();
        }

        public MyCanvas(Image image) : this(image.Width, image.Height)
        {
            if (image == null)
            {
                throw new MyCanvasException("Can't create canvas. Image is null");
            }

            _imageBufferedGraphics.Graphics.DrawImage(image, new Point(0, 0));
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            _isDrawing = true;
            _clickPoint = e.Location;

            switch (_currentTool)
            {
                case DrawingTools.PEN:
                    _movementPoints.Add(e.Location);
                    _currentShape = new PFPen(_movementPoints) { Pen = _currentPen };
                    break;
                case DrawingTools.ELASTIC:
                    _movementPoints.Add(e.Location);
                    _currentShape = new ElasticPen(_movementPoints);
                    break;
                case DrawingTools.LINE:
                    _currentShape = new PFLine(Point.Empty, Point.Empty) { Pen = _currentPen };
                    break;
                case DrawingTools.RECTANGLE:
                    _currentShape = new PFRectangle(Point.Empty, Point.Empty) { Pen = _currentPen, Brush = _currentBrush };
                    break;
                case DrawingTools.ELLIPSE:
                    _currentShape = new PFEllipse(Point.Empty, Point.Empty) { Pen = _currentPen, Brush = _currentBrush };
                    break;
                default:
                    throw new MyCanvasException("Unknown drawing tool");
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (MousePositionCoordChanged != null)
            {
                MousePositionCoordChanged(this, e);
            }

            if (_isDrawing == false)
            {
                return;
            }

            Graphics drawingGraphics = DrawingGraphics;

            switch (_currentTool)
            {
                case DrawingTools.PEN:
                    _movementPoints.Add(e.Location);
                    break;
                case DrawingTools.ELASTIC:
                    _movementPoints.Add(e.Location);
                    break;
                case DrawingTools.LINE:
                    ((Shape)_currentShape).FirstPoint = _clickPoint;
                    ((Shape)_currentShape).LastPoint = e.Location;
                    break;
                case DrawingTools.RECTANGLE:
                case DrawingTools.ELLIPSE:

                    Point startPoint = new Point(_clickPoint.X < e.X ? _clickPoint.X : e.X, _clickPoint.Y < e.Y ? _clickPoint.Y : e.Y);
                    Point endPoint = new Point(_clickPoint.X >= e.X ? _clickPoint.X : e.X, _clickPoint.Y >= e.Y ? _clickPoint.Y : e.Y);
                    ((Shape)_currentShape).FirstPoint = startPoint;
                    ((Shape)_currentShape).LastPoint = endPoint;
                    break;
                default:
                    throw new MyCanvasException("Unknown drawing tool");
            }
            _currentShape.Draw(drawingGraphics);

            using (Graphics panelGraphics = CreateGraphics())
            { 
                _drawingBufferedGraphics.Render(panelGraphics);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isDrawing == false)
            {
                return;
            }

            _currentShape.Draw(_imageBufferedGraphics.Graphics);
            _currentShape = null;
            _isDrawing = false;

            if(_currentTool == DrawingTools.PEN)
            { 
                _movementPoints.Clear();
            }

            if (DrawingChanged != null)
            {
                DrawingChanged(this, new EventArgs());
            }

            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _imageBufferedGraphics.Render(e.Graphics);
            base.OnPaint(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (_drawingBufferedGraphics != null)
            { 
                _drawingBufferedGraphics.Dispose();
                _drawingBufferedGraphics = null;
            }
            if (_imageBufferedGraphics != null)
            {
                _imageBufferedGraphics.Dispose();
                _imageBufferedGraphics = null;
            }
            if (_currentBrush != null)
            { 
                _currentBrush.Dispose();
                _currentBrush = null;
            }
            if (_currentPen != null)
            { 
                _currentPen.Dispose();
                _currentPen = null;
            }
            _currentShape = null;
            _movementPoints.Clear();
            _movementPoints = null;

            base.Dispose(disposing);
        }

        public void OnToolChanged(object sender, ToolChangeEventArgs e)
        {
            _currentTool = e.Tool;
        }

        public void OnForeColorChanged(object sender, ColorChangeEventArgs e)
        {
            if (_currentPen != null)
            { 
                _currentPen.Dispose();
            }
            _currentPen = new Pen(e.Color, Program.DEFAULT_PEN_WIDTH);
        }

        public void OnBackColorChanged(object sender, ColorChangeEventArgs e)
        {
            if (_currentBrush != null)
            { 
               _currentBrush.Dispose();
            }
            _currentBrush = new SolidBrush(e.Color);
        }

        public void InverseDrawing()
        {
            new Thread(Inverse) { IsBackground = true }.Start();
        }

        private void Inverse()
        {
            if (BeginInverse != null)
            {
                BeginInverse(this, new EventArgs());
            }

            try
            {

                Invoke(new Action(() => Enabled = false));

                using (Bitmap bitmap = Drawing)
                {
                    InverseBitmap(bitmap);
                    _imageBufferedGraphics.Graphics.DrawImage(bitmap, new Point(0, 0));
                }

                Invalidate();

                if (DrawingChanged != null)
                {
                    DrawingChanged(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error during inversing image\n{0}", ex.Message);
            }
            finally
            {
                Invoke(new Action(() => Enabled = true));

                if (EndInverse != null)
                {
                    EndInverse(this, new EventArgs());
                }
            }
        }

        private void InverseBitmap(Bitmap bitmap)
        {
            for (int row = 0; row < bitmap.Height; row++)
            {
                for (int col = 0; col < bitmap.Width; col++)
                {
                    Color currentClr = bitmap.GetPixel(col, row);
                    Color inverseClr = Color.FromArgb(255, 255 - currentClr.R, 255 - currentClr.G, 255 - currentClr.B);
                    bitmap.SetPixel(col, row, inverseClr);

                    if (InverseProressChanged != null)
                    {
                        InverseProgressChangedEventArgs e = new InverseProgressChangedEventArgs()
                        {
                            Min = 0,
                            Max = bitmap.Width * bitmap.Height,
                            Progress = row * bitmap.Width + col
                        };
                        InverseProressChanged(this, e);
                    }
                }
            }
        }

        private void InitBuffering()
        {
            using (Graphics canvasGraphics = CreateGraphics())
            {
                Rectangle rect = new Rectangle(0, 0, Width, Height);
                _drawingBufferedGraphics = BufferedGraphicsManager.Current.Allocate(canvasGraphics, rect);
                _imageBufferedGraphics = BufferedGraphicsManager.Current.Allocate(canvasGraphics, rect);
                _imageBufferedGraphics.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                _imageBufferedGraphics.Graphics.Clear(Color.White);
            }
        }
    }
}