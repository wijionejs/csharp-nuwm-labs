using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphicEditor
{
    class ElasticPen : IDrawable
    {
        public List<Point> Points { get; set; }

        public Pen Pen { get; } = new Pen(Color.White);

        public ElasticPen(List<Point> pointsList)
        {
            if (pointsList == null)
            {
                throw new ArgumentNullException("pointsList", "Can't create ElasticPen. pointsList is null");
            }
            Points = pointsList;
        }

        public void Draw(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics", "ElasticPen can't draw. Graphics is null");
            }
            if (Pen == null)
            {
                throw new ShapeException("Can't draw ElasticPen. Pen is null");
            }
            if (Points == null)
            {
                throw new ShapeException("Can't draw ElasticPen. Points is null");
            }

            for (int i = 0; i < Points.Count - 1; i++)
            {
                Point p1 = Points[i];
                Point p2 = Points[i + 1];
                graphics.DrawLine(Pen, p1, p2);
            }
        }
    }
}