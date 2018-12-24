using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Tips {
    class AggregateTip {
        Point Tip1;
        Point Tip2;
        Point TipEnd;
        Point TipBegin;
        Polyline polyline = new Polyline();
        Point TipTale;
        PointCollection pointCollection = new PointCollection();

        public AggregateTip(Point point2, string str, Brush color) {
            if (str == "TipToRight") { // хвост выходит из енда, смотрит вправо
                TipEnd = new Point(point2.X + 20, point2.Y);
                TipBegin = point2;
                Tip1 = new Point(point2.X + 10, point2.Y - 10);
                Tip2 = new Point(point2.X + 10, point2.Y + 10);
                TipTale = new Point(point2.X + 40, point2.Y);
                pointCollection.Add(Tip1);
                pointCollection.Add(TipEnd);
                pointCollection.Add(TipTale);
                pointCollection.Add(TipEnd);
                pointCollection.Add(Tip2);
                pointCollection.Add(TipBegin);
                pointCollection.Add(Tip1);
            }
            else { // хвост выходит из бегина, смотрит влево
                TipEnd = point2;
                Tip1 = new Point(point2.X - 10, point2.Y - 10);
                Tip2 = new Point(point2.X - 10, point2.Y + 10);
                TipBegin = new Point(point2.X - 20, point2.Y);
                TipTale = new Point(point2.X - 40, point2.Y);
                pointCollection.Add(Tip1);
                pointCollection.Add(TipEnd);
                pointCollection.Add(Tip2);
                pointCollection.Add(TipBegin);
                pointCollection.Add(TipTale);
                pointCollection.Add(TipBegin);
                pointCollection.Add(Tip1);
            }
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            polyline.Fill = color;
        }

        public Polyline GetPolyline() {
            return polyline;
        }

        public Point GetEndPointForLine() {
            return TipTale;
        }
    }
}
