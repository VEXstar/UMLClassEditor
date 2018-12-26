using System;
using System.Windows;
using System.Windows.Controls;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements
{
    [Serializable()]
    public abstract class UMLElement: IMovable, IDrawable
    {
        protected bool isPicked = false;

        public bool getPicked()
        {
            return isPicked;
        }

        public abstract void setPicked(bool set);
        public abstract void removeGraphicFromCanvas(Canvas canvas);
        public abstract void updateGraphicPoints(Point[] points);
        public abstract void draw(Canvas canvas);
        public abstract void move(Point point, Canvas canvas);
        public abstract bool canPick(Point point);
        public abstract string getGuid();
        public abstract void updateGUI();
    }
}