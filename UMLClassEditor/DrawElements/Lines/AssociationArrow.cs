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
        Polyline polyline = new Polyline(); // отрисовываемый элемент управления
        PointCollection pointCollection = new PointCollection(); // точки по которым будем строить стрелку

        public AssociationArrow(Point startPoint, Point endPoint) {
            Point pointForTip1 = new Point(endPoint.X - 10, endPoint.Y - 10);
            Point pointForTip2 = new Point(endPoint.X + 10, endPoint.Y - 10);
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            pointCollection.Add(pointForTip1);
            pointCollection.Add(endPoint);
            pointCollection.Add(pointForTip2);
            polyline.Points = pointCollection; // пока что стрелка это просто прямая линия с наконечником
            polyline.Stroke = Brushes.Black;
        }

        public Polyline GetPolyline() {
            return polyline;
        }

        public override void draw() {
            
        }

        public override void move(int dx, int dy) {
            
        }
    }
}
