using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Lines
{
    class AssociationTip
    {
        Point Tip1;
        Point Tip2;
        Point TipEnd;
        Polyline polyline = new Polyline(); // отрисовываемый элемент управления
        PointCollection pointCollection = new PointCollection(); // точки по которым будем строить стрелку
        public AssociationTip(int pointForEndX, int pointForEndY, int ForX, int ForY) {
            TipEnd = new Point(pointForEndX, pointForEndY);
            Tip1 = new Point(pointForEndX, pointForEndY);
            Tip2 = new Point(pointForEndX, pointForEndY);
            pointCollection.Add(Tip1);
            pointCollection.Add(TipEnd);
            pointCollection.Add(Tip2);

        }
    }
}
