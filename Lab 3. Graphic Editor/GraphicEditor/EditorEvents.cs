using System;
using System.Drawing;

namespace GraphicEditor
{
    public delegate void ToolChanged(object sender, ToolChangeEventArgs e);
    public delegate void ColorChanged(object sender, ColorChangeEventArgs e);

    public class ToolChangeEventArgs : EventArgs
    {
        public DrawingTools Tool { get; private set; }
        public ToolChangeEventArgs(DrawingTools tool)
        {
            Tool = tool;
        }
    }

    public class ColorChangeEventArgs : EventArgs
    {
        public Color Color { get; private set; }
        public ColorChangeEventArgs(Color foreColor)
        {
            Color = foreColor;
        }
    }

    public class InverseProgressChangedEventArgs : EventArgs
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Progress { get; set; }
    }
}