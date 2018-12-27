using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace UMLClassEditor.DrawElements.Tips
{
    [Serializable()]
    class AggregateTip:Tip
    {

        public AggregateTip(Point point2, Turns t, Brush color)
        {
            Point Tip1;
            Point Tip2;
            Point TipEnd;
            Point TipBegin;
            PointCollection pointCollection = new PointCollection();
            if (t == Turns.Right) { // хвост выходит из енда, смотрит вправо
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
            polyline.StrokeThickness = 3;
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            polyline.Fill = color;
        }

        public override Polyline GetPolyline() {
            return polyline;
        }

        public override Point GetEndPointForLine() {
            return TipTale;
        }

        public override void removeGraphicFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(polyline);
        }

        public override void updateGraphicPoints(Point[] points)
        {
            throw new System.NotImplementedException();
        }

        private Point? last;
        public override void move(Point offset, Canvas canvas)
        {
            if (!last.HasValue)
            {
                last = offset;
                return;
            }
            last = new Point(last.Value.X + offset.X, last.Value.Y + offset.Y);
            Matrix m = new Matrix();
            m.Translate(last.Value.X, last.Value.Y);
            polyline.RenderTransform = new MatrixTransform(m);
        }
        public override void draw(Canvas canvas)
        {
            canvas.Children.Add(polyline);
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

        public AggregateTip(SerializationInfo info, StreamingContext context)
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
