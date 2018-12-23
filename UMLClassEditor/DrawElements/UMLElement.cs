using System.Windows.Controls;

namespace UMLClassEditor.DrawElements
{
    public abstract class UMLElement
    {
        public abstract void draw(Canvas canvas);
        public abstract void move(int dx, int dy);
    }
}