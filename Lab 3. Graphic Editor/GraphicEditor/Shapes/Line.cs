using System;
using System.Drawing;

namespace GraphicEditor
{
    class PFLine : Shape, IDrawable
    {
        public Pen Pen { get; set; }

        public PFLine(Point first, Point last) : base(first, last) { }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics", "PFLine can't draw. Graphics is null");
            }
            if (Pen == null)
            {
                throw new ShapeException("Can't draw PFLine. Pen is null");
            }

            graphics.DrawLine(Pen, FirstPoint, LastPoint);
        }
    }
}