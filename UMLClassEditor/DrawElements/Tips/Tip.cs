using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Tips
{
    [Serializable()]
    public abstract class Tip:IDrawable,IMovable,ISerializable
    {
        protected Polyline polyline = new Polyline();
        protected Point TipTale;
        public enum Turns
        {
            Left,Right
        }
        public abstract Polyline GetPolyline();
        public abstract Point GetEndPointForLine();
        public abstract void removeGraphicFromCanvas(Canvas canvas);
        public abstract void updateGraphicPoints(Point[] points);
        public abstract void draw(Canvas canvas);

        public abstract void move(Point offset,Canvas canvas);
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}
