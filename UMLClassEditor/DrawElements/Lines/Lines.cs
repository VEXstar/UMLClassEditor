using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Lines {
    class Lines {
        Polyline polyline = new Polyline();
        PointCollection pointCollection = new PointCollection();

        public Lines(Point startPoint, Point endPoint, string str) {
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            if (str == "Dotted") {
                polyline.StrokeThickness = 1;
                DoubleCollection coll = new DoubleCollection();
                coll.Add(2);
                polyline.StrokeDashArray = coll;
            }
        }

        public Polyline GetPolyline() {
            return polyline;
        }
    }
}
