using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace GraphicEditor
{
    class DrawingHelper
    {
        private readonly Dictionary<string, ImageFormat> _extentionFormat = new Dictionary<string, ImageFormat>()
        {
            { ".bmp", ImageFormat.Bmp },
            { ".jpg", ImageFormat.Jpeg },
            { ".jpeg", ImageFormat.Jpeg },
            { ".png", ImageFormat.Png },
            { ".gif", ImageFormat.Gif }
        };

        private readonly string _openFilter = "All|*.*|Image *.bmp|*.bmp|Image *.jpg|*.jpg|Image *.png|*.png|Image *.gif|*.gif";
        private readonly string _saveFilter = "Image*.bmp|*.bmp|Image*.jpg|*.jpg|Image*.png|*.png|Image*.gif|*.gif";

        public string FileName { get; set; }

        public MyCanvas CreateDrawing()
        {
            MyCanvas result = null;
            frmNewDrawing newDrawingDiaog = new frmNewDrawing();
            if (newDrawingDiaog.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            result = new MyCanvas(newDrawingDiaog.DrawingWidth, newDrawingDiaog.DrawingHeight);
            FileName = null;

            return result;
        }

        public MyCanvas OpenDrawing()
        {
            MyCanvas result;
            string tmpFileName;
            using (OpenFileDialog openDialog = new OpenFileDialog() { InitialDirectory = Application.StartupPath, Filter = _openFilter })
            {
                if (openDialog.ShowDialog() != DialogResult.OK)
                {
                    return null;
                }
                tmpFileName = openDialog.FileName;
            }
            using (Image openedImage = Image.FromFile(tmpFileName))
            {
                result = new MyCanvas(openedImage);
            }
            FileName = tmpFileName;
            return result;
        }
        
        public bool SaveDrawing(MyCanvas canvas, bool isSaveAs)
        {
            if (isSaveAs == true || FileName == null)
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog() { AddExtension = true, DefaultExt = ".bmp", InitialDirectory = Application.StartupPath, Filter = _saveFilter })
                {
                    if (saveDialog.ShowDialog() != DialogResult.OK)
                    {
                        return false;
                    }
                    FileName = saveDialog.FileName;
                }
            }
            using (Bitmap currentDrawing = canvas.Drawing)
            {
                string extention = Path.GetExtension(FileName);
                currentDrawing.Save(FileName, _extentionFormat[extention]);
                return true;
            }
        }
    }
}