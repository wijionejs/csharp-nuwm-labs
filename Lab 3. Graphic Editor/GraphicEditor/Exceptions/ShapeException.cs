using System;

namespace GraphicEditor
{
    [Serializable]
    class ShapeException : Exception
    {
        public ShapeException(string message) : base(message) { }
    }
}
