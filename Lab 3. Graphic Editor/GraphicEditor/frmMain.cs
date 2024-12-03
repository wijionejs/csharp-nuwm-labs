using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicEditor
{
    public partial class frmMain : Form
    {
        private ToolStripButton _activeToolButton;
        private MyCanvas _myCanvas;
        private Color _foreColor = Color.Black;
        private Color _backColor = Color.White;
        private bool _isDitry;
        private DrawingHelper _drawingHelper;

        private event ToolChanged DrawingToolChanged;
        private event ColorChanged DrawingForeColorChanged;
        private event ColorChanged DrawingBackColorChanged;

        public frmMain()
        {
            InitializeComponent();

            _myCanvas = new MyCanvas(Program.DEFAULT_DRAWING_WIDTH, Program.DEFAULT_DRAWING_HEIGHT);
            InitCanvas();

            _drawingHelper = new DrawingHelper();

            tsbtnPen.Tag = DrawingTools.PEN;
            tsbtnElastic.Tag = DrawingTools.ELASTIC;
            tsbtnLine.Tag = DrawingTools.LINE;
            tsbtnRectange.Tag = DrawingTools.RECTANGLE;
            tsbtnEllipse.Tag = DrawingTools.ELLIPSE;

            tsbtnForeColor.BackColor = _foreColor;
            tsbtnBackColor.BackColor = _backColor;
            tsbtnPen.Checked = true;
            _activeToolButton = tsbtnPen;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = AskUserOfLosingChanges();
            base.OnClosing(e);
        }

        private void mnAbout_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }
        

        private void drawingTools_Click(object sender, EventArgs e)
        {
            ToolStripButton currentButton = sender as ToolStripButton;
            if (currentButton != null)
            {
                currentButton.Checked = true;
                _activeToolButton.Checked = false;
                _activeToolButton = currentButton;
                Console.WriteLine("CHECKED TO ACTIVE TARGET");
            }
        }

        private void drawingTools_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripButton currentButton = sender as ToolStripButton;
            if (currentButton == null)
            {
                return;
            }
            if (currentButton.Checked == true)
            {
                if (DrawingToolChanged != null)
                {
                    ToolChangeEventArgs te = new ToolChangeEventArgs((DrawingTools)currentButton.Tag);
                    DrawingToolChanged(currentButton, te);
                }

                tslblTool.Text = String.Format("Tool: {0}", (DrawingTools)currentButton.Tag);
            }
        }

        private void tsbtnColorButtons_Click(object sender, EventArgs e)
        {
            ToolStripButton currentButton = sender as ToolStripButton;
            if (currentButton == null)
            {
                return;
            }

            using (ColorDialog colorDialog = new ColorDialog())
            { 
                colorDialog.Color = (currentButton == tsbtnForeColor) ? _foreColor : _backColor;
                if (colorDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                currentButton.BackColor = colorDialog.Color;
                _foreColor = (currentButton == tsbtnForeColor) ? colorDialog.Color : _foreColor;
                _backColor = (currentButton == tsbtnBackColor) ? colorDialog.Color : _backColor;

                ColorChangeEventArgs ce = new ColorChangeEventArgs(colorDialog.Color);

                if (currentButton == tsbtnForeColor && DrawingForeColorChanged != null)
                {
                    DrawingForeColorChanged(currentButton, ce);
                }
                if (currentButton == tsbtnBackColor && DrawingBackColorChanged != null)
                {
                    DrawingBackColorChanged(currentButton, ce);
                }
            }
        }

        private void createControl_Click(object sender, EventArgs e)
        {
            if (AskUserOfLosingChanges() == true)
            {
                return;
            }
            try
            { 
                MyCanvas canvas = _drawingHelper.CreateDrawing();
                if (canvas == null)
                {
                    return;
                }
                ReplaceCanvas(canvas);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating drawing\n{0}", ex.Message);
                MessageBox.Show("Can't create drawing", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void saveControl_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem currentMenuItem = sender as ToolStripMenuItem;
            try
            { 
                bool isSaved = _drawingHelper.SaveDrawing(_myCanvas, currentMenuItem != null && currentMenuItem == mnSaveAs);
                _isDitry = !isSaved;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error saving file\n{0}", ex.Message);
                MessageBox.Show("Can't save drawing", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void openControl_Click(object sender, EventArgs e)
        {
            if (AskUserOfLosingChanges() == true)
            {
                return;
            }
            try
            {
                MyCanvas canvas = _drawingHelper.OpenDrawing();
                if (canvas == null)
                {
                    return;
                }
                ReplaceCanvas(canvas);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error opening file\n{0}", ex.Message);
                MessageBox.Show("Can't open file", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void mnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OnDrawingChanged(object sender, EventArgs e)
        {
            _isDitry = true;
        }

        private void OnMousePositionChanged(object sender, EventArgs e)
        {
            if (e is MouseEventArgs)
            { 
                tslblMouseCoord.Text = String.Format("Position: {0}", ((MouseEventArgs)e).Location);
            }
        }

        private void OnInverseProgressChanged(object sender, EventArgs e)
        {
            InverseProgressChangedEventArgs eArgs = e as InverseProgressChangedEventArgs;
            if (eArgs != null)
            { 
                Invoke(new Action(() => sbpbProgress.Value = 100 * eArgs.Progress / (eArgs.Max - eArgs.Min)));
            }
        }

        private void OnBeginInverse(object sender, EventArgs e)
        {
            SetControlState(false);
        }

        private void OnEndInverse(object sender, EventArgs e)
        {
            SetControlState(true);
        }

        private void InitCanvas()
        {
            DrawingToolChanged += _myCanvas.OnToolChanged;
            DrawingForeColorChanged += _myCanvas.OnForeColorChanged;
            DrawingBackColorChanged += _myCanvas.OnBackColorChanged;

            _myCanvas.DrawingChanged += OnDrawingChanged;
            _myCanvas.MousePositionCoordChanged += OnMousePositionChanged;
            _myCanvas.InverseProressChanged += OnInverseProgressChanged;
            _myCanvas.BeginInverse += OnBeginInverse;
            _myCanvas.EndInverse += OnEndInverse;
            
            DrawingToolChanged(this, new ToolChangeEventArgs(DrawingTools.PEN));
            
            DrawingForeColorChanged(this, new ColorChangeEventArgs(_foreColor));
            
            DrawingBackColorChanged(this, new ColorChangeEventArgs(_backColor));

            pnBase.Controls.Add(_myCanvas);
        }

        private void RemoveCanvas()
        {
            DrawingToolChanged -= _myCanvas.OnToolChanged;
            DrawingForeColorChanged -= _myCanvas.OnForeColorChanged;
            DrawingBackColorChanged -= _myCanvas.OnBackColorChanged;

            _myCanvas.DrawingChanged -= OnDrawingChanged;
            _myCanvas.MousePositionCoordChanged -= OnMousePositionChanged;
            _myCanvas.InverseProressChanged -= OnInverseProgressChanged;
            _myCanvas.BeginInverse -= OnBeginInverse;
            _myCanvas.EndInverse -= OnEndInverse;

            pnBase.Controls.Clear();
            _myCanvas = null;
        }

        private bool AskUserOfLosingChanges()
        {
            if (_isDitry == true)
            {
                return MessageBox.Show("Do you want to save changes?", Program.APP_NAME, MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK;
            }
            return false;
        }

        private void ReplaceCanvas(MyCanvas canvas)
        {
            RemoveCanvas();
            _myCanvas = canvas;
            InitCanvas();
            _isDitry = false;
        }

        private void SetControlState(bool value)
        {
            Invoke(new Action(() => tsbtnNew.Enabled = value));
            Invoke(new Action(() => tsbtnOpen.Enabled = value));
            Invoke(new Action(() => tsbtnSave.Enabled = value));

            Invoke(new Action(() => mnNew.Enabled = value));
            Invoke(new Action(() => mnOpen.Enabled = value));
            Invoke(new Action(() => mnSave.Enabled = value));
            Invoke(new Action(() => mnSaveAs.Enabled = value));;

            Invoke(new Action(() => sbpbProgress.Visible = !value));
        }
    }
}