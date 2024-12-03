using System.Drawing;

namespace GraphicEditor
{
    abstract class Shape
    {
        public Point FirstPoint { get; set; }

        public Point LastPoint { get; set; }

        public Shape(Point first, Point last)
        {
            FirstPoint = first;
            LastPoint = last;
        }
    }
}