using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor {
    class AssociationArrow : Arrows {
        Polyline polyline; // отрисовываемый элемент управления
        PointCollection pointCollection; // точки по которым будем строить стрелку

        public AssociationArrow(Point startPoint, Point endPoint) {
            polyline = new Polyline();
            pointCollection = new PointCollection();
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            polyline.Points = pointCollection; // пока что стрелка это просто прямая линия
            polyline.Stroke = Brushes.Black;
        }

        public override void draw() {
            
        }

        public override void move(int dx, int dy) {
            
        }
    }
}
