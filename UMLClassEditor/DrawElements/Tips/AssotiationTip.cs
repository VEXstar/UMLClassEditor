using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Lines {
    class AssociationTip {
        Point Tip1;
        Point Tip2;
        Point TipEnd;
        Polyline polyline = new Polyline(); 
        PointCollection pointCollection = new PointCollection(); 
        public AssociationTip(int pointForEndX, int pointForEndY, int ForX1, int ForX2, int ForY1, int ForY2) {
            TipEnd = new Point(pointForEndX, pointForEndY);
            Tip1 = new Point(pointForEndX + ForX1, pointForEndY + ForY1);
            Tip2 = new Point(pointForEndX + ForX2, pointForEndY + ForY2);
            pointCollection.Add(Tip1);
            pointCollection.Add(TipEnd);
            pointCollection.Add(Tip2);
        }
    }
}