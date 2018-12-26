using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Tips
{
    [Serializable()]
    class DerivTip: Tip
    {

        public DerivTip(Point point2, Turns t)
        {
            Point Tip1;
            Point Tip2;
            Point TipEnd;
            Point TipBegin;
            PointCollection pointCollection = new PointCollection();
            TipEnd = point2;
            if (t == Turns.Left) {
                Tip1 = new Point(point2.X - 10, point2.Y - 10);
                Tip2 = new Point(point2.X - 10, point2.Y + 10);
                TipBegin = new Point(point2.X - 10, point2.Y);
                TipTale = new Point(TipEnd.X - 20, TipEnd.Y);
            }
            else {
                Tip1 = new Point(point2.X + 10, point2.Y - 10);
                Tip2 = new Point(point2.X + 10, point2.Y + 10);
                TipBegin = new Point(point2.X + 10, point2.Y);
                TipTale = new Point(TipEnd.X + 20, TipEnd.Y);
            }
            pointCollection.Add(Tip1);
            pointCollection.Add(TipEnd);
            pointCollection.Add(Tip2);
            pointCollection.Add(TipBegin);
            pointCollection.Add(TipTale);
            pointCollection.Add(TipBegin);
            pointCollection.Add(Tip1);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            polyline.Fill = Brushes.White;
        }

        public override Polyline GetPolyline()
        {
            return polyline;
        }

        public override  Point GetEndPointForLine()
        {
            return  TipTale;
        }

        public override void removeGraphicFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(polyline);
        }

        public override void updateGraphicPoints(Point[] points)
        {
            throw new NotImplementedException();
        }

        public override void draw(Canvas canvas)
        {
            canvas.Children.Add(polyline);
        }

        private Point? last;
        public override void move(Point offset,Canvas canvas)
        {
            if (!last.HasValue)
            {
                last = offset;
                return;
            }
           last = new Point(last.Value.X + offset.X, last.Value.Y + offset.Y);
           Matrix m = new Matrix();
           m.Translate(last.Value.X,last.Value.Y);
           polyline.RenderTransform = new MatrixTransform(m);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("pointCount", polyline.Points.Count, typeof(int));
            info.AddValue("Tale", TipTale, typeof(Point));
            int i = 0;
            foreach (var el in polyline.Points)
            {
                info.AddValue("point" + (i++), el, typeof(Point));
            }
        }

        public DerivTip(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < (int)info.GetValue("pointCount", typeof(int)); i++)
            {
                Point s = (Point)info.GetValue("point" + i, typeof(Point));
                polyline.Points.Add(s);
            }

            TipTale = (Point) info.GetValue("Tale", typeof(Point));
        }
    }
}