﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Tips {
    class DerivTip {
        Point Tip1;
        Point Tip2;
        Point TipEnd;
        Polyline polyline = new Polyline();
        PointCollection pointCollection = new PointCollection();

        public DerivTip(Point point2, int ForX1, int ForX2, int ForY1, int ForY2) {
            TipEnd = point2;
            Tip1 = new Point(point2.X + ForX1, point2.Y + ForY1);
            Tip2 = new Point(point2.X + ForX2, point2.Y + ForY2);
            pointCollection.Add(Tip1);
            pointCollection.Add(TipEnd);
            pointCollection.Add(Tip2);
            pointCollection.Add(Tip1);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            polyline.Fill = Brushes.White;
        }

        public Polyline GetPolyline() {
            return polyline;
        }
    }
}