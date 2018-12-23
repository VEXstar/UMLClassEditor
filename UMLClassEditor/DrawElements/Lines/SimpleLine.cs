using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor {
    class SimpleLine  {
        Polyline polyline = new Polyline(); 
        PointCollection pointCollection = new PointCollection(); 

        public SimpleLine(Point startPoint, Point endPoint) {
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            polyline.Points = pointCollection; 
            polyline.Stroke = Brushes.Black;
        }

        public Polyline GetPolyline() {
            return polyline;
        }
    }
}
