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
        Point TipTale;
        PointCollection pointCollection = new PointCollection();

        public AssociationTip(Point point2, string str) {
            TipEnd = point2;
            if (str == "TipToRight") {
                Tip1 = new Point(point2.X + 10, point2.Y - 10);
                Tip2 = new Point(point2.X + 10, point2.Y + 10);
                TipTale = new Point(TipEnd.X + 20, TipEnd.Y);
            }
            else {
                Tip1 = new Point(point2.X - 10, point2.Y - 10);
                Tip2 = new Point(point2.X - 10, point2.Y + 10);
                TipTale = new Point(TipEnd.X - 20, TipEnd.Y);
            }
            pointCollection.Add(Tip1);
            pointCollection.Add(TipEnd);
            pointCollection.Add(TipTale);
            pointCollection.Add(TipEnd);
            pointCollection.Add(Tip2);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
        }

        public Polyline GetPolyline() {
            return polyline;
        }

        public Point GetEndPointForLine() {
            return TipTale;
        }
    }
}
