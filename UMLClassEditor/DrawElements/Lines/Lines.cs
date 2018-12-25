using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.DrawElements.Tips;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Lines {
    class Lines:IObserver {
        Polyline polyline = new Polyline();
        PointCollection pointCollection = new PointCollection();
        public Lines(Point startPoint, Point endPoint, string str)
        {

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

        private UMLElement el;
        private Tip tip;
        public void setConnectElems(UMLElement el, Tip tip)
        {
            this.tip = tip;
            this.el = el;
        }
        public void reDraw(Canvas canvas)
        {
            polyline.Points.Clear();
            polyline.Points.Add(tip.GetEndPointForLine());
            Point[] z =((UMLClassBox) el).getStartPoints();
            double ox = ((UMLClassBox) el).GetCenterPoint().X - tip.GetEndPointForLine().X;
            int ind = (ox < 0) ? 1 : 0;
            polyline.Points.Add(z[ind]);
        }
        public Polyline GetPolyline() {
            return polyline;
        }

        public void onEvent(object e)
        {
        }
    }
}
