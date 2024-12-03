using System;
using System.Drawing;

namespace GraphicEditor
{
    class PFRectangle : Shape, IDrawable
    {
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public PFRectangle(Point first, Point last) : base(first, last) { }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics", "PFRectangle can't draw. Graphics is null");
            }
            if (Pen == null || Brush == null)
            {
                throw new ShapeException("Can't draw PFRectangle. Pen or Brush is null");
            }

            Rectangle rect = new Rectangle(FirstPoint.X, FirstPoint.Y, LastPoint.X - FirstPoint.X, LastPoint.Y - FirstPoint.Y);
            graphics.FillRectangle(Brush, rect);
            graphics.DrawRectangle(Pen, rect);
        }
    }
}