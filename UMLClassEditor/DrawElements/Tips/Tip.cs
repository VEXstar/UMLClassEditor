using System.Windows;
using System.Windows.Shapes;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Tips
{
    public abstract class Tip: IObserver
    {
        public abstract Polyline GetPolyline();
        public abstract Point GetEndPointForLine();
        public abstract void onEvent(object e);
    }
}