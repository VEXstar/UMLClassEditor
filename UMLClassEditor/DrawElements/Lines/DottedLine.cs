using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor {
    class DottedLine {
        Polyline polyline = new Polyline();
        PointCollection pointCollection = new PointCollection();

        public DottedLine(Point startPoint, Point endPoint) {
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            polyline.StrokeThickness = 1;
            DoubleCollection coll = new DoubleCollection();
            coll.Add(2);
            polyline.StrokeDashArray = coll;
        }

        public Polyline GetPolyline() {
            return polyline;
        }
    }
}
