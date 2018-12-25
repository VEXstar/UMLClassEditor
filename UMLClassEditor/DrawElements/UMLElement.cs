using System.Windows;
using System.Windows.Controls;

namespace UMLClassEditor.DrawElements
{
    public abstract class UMLElement
    {
        protected bool isPicked = false;

        public bool getPicked()
        {
            return isPicked;
        }

        public abstract void setPicked(bool set);
        public abstract void draw(Canvas canvas);
        public abstract void deleteFrom(Canvas canvas);
        public abstract void move(Point point);
        public abstract bool canPick(Point point);
        public abstract void update(Canvas canvas);
    }
}