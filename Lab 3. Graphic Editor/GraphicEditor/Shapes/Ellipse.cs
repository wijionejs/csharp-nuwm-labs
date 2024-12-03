using System;
using System.Drawing;

namespace GraphicEditor
{
    class PFEllipse : Shape, IDrawable
    {
        public Pen Pen { get; set; }

        public Brush Brush { get; set; }

        public PFEllipse(Point first, Point last) : base(first, last) { }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics", "PFEllipse can't draw. Graphics is null");
            }
            if (Pen == null || Brush == null)
            {
                throw new ShapeException("Can't draw PFEllipse. Pen or Brush is null");
            }

            Rectangle rect = new Rectangle(FirstPoint.X, FirstPoint.Y, LastPoint.X - FirstPoint.X, LastPoint.Y - FirstPoint.Y);
            graphics.FillEllipse(Brush, rect);
            graphics.DrawEllipse(Pen, rect);
        }
    }
}