using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GraphicEditor
{
    /// <summary>
    /// Диалоговое окно создания ниового рисунка.
    /// Задаёт размеры рисунки
    /// </summary>
    public partial class frmNewDrawing : Form
    {
        #region CONSTS

        private readonly string MIN_SIZE_ERROR_MESSAGE = String.Format("Minimum size {0} px", Program.MIN_DRAWING_SIZE);

        #endregion

        #region PROPERTIES

        public int DrawingWidth { get; private set; }
        public int DrawingHeight { get; private set; }

        #endregion

        #region CTOR
        
        public frmNewDrawing()
        {
            InitializeComponent();
        }

        #endregion

        #region OVERRIDED METHOD
        
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DialogResult != DialogResult.OK)
            {
                return;
            }
            try
            {
                DrawingWidth = Int32.Parse(mtbWidth.Text.Replace(" ", ""));
                DrawingHeight = Int32.Parse(mtbHeight.Text.Replace(" ", ""));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error! Wrong picture size \n{0}", ex.Message);
                MessageBox.Show("Can't create drawing. Wrong picture size", Program.APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
                return;
            }

            //Максимальный размер орграничен полями ввода

            bool isValidValues = DrawingWidth >= Program.MIN_DRAWING_SIZE && DrawingHeight >= Program.MIN_DRAWING_SIZE;
            if (!isValidValues)
            {
                if (DrawingWidth < Program.MIN_DRAWING_SIZE)
                {
                    errSize.SetError(mtbWidth, MIN_SIZE_ERROR_MESSAGE);
                }
                if (DrawingHeight < Program.MIN_DRAWING_SIZE)
                {
                    errSize.SetError(mtbHeight, MIN_SIZE_ERROR_MESSAGE);
                }
                e.Cancel = true;
            }
            else
            {
                errSize.SetError(mtbWidth, "");
                errSize.SetError(mtbHeight, "");
            }
        }

        #endregion
    }
}