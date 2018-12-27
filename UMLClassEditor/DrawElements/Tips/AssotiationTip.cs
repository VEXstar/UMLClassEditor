using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLClassEditor.DrawElements.Tips;

namespace UMLClassEditor.DrawElements.Lines
{
    [Serializable()]
    class AssociationTip: Tip
    {

       
        public AssociationTip(Point point2, Turns t)
        {
            Point Tip1;
            Point Tip2;
            Point TipEnd;
            PointCollection pointCollection = new PointCollection();
            TipEnd = point2;
            if (t == Turns.Left) {
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
            polyline.StrokeThickness = 3;
        }

        public override Polyline GetPolyline() {
            return polyline;
        }

        public override  Point GetEndPointForLine() {
            return  TipTale;
        }

        public override void removeGraphicFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(polyline);
        }

        public override void updateGraphicPoints(Point[] points)
        {
          
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

        public AssociationTip(SerializationInfo info, StreamingContext context)
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
