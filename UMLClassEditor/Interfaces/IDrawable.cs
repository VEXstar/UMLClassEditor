using System.Windows;
using System.Windows.Controls;

namespace UMLClassEditor.Interfaces
{
    public interface IDrawable
    {
        void removeGraphicFromCanvas(Canvas canvas);
        void updateGraphicPoints(Point[] points);
        void draw(Canvas canvas);
    }
}