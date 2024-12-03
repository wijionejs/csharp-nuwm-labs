using System;

namespace GraphicEditor
{
    [Serializable]
    class MyCanvasException : Exception
    {
        public MyCanvasException(string message) : base(message) { }
    }
}
