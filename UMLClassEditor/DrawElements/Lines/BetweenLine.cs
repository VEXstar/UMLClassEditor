using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Lines {
    [Serializable()]
    class BetweenLine: IDrawable,ISerializable
    {
        Polyline polyline = new Polyline();
        PointCollection pointCollection = new PointCollection();

        public enum  Type
        {
            Solid,Dotted
        }
        public BetweenLine(Point startPoint, Point endPoint, Type t)
        {
            pointCollection.Add(startPoint);
            pointCollection.Add(endPoint);
            polyline.Points = pointCollection;
            polyline.Stroke = Brushes.Black;
            if (t == Type.Dotted) {
                polyline.StrokeThickness = 1;
                DoubleCollection coll = new DoubleCollection();
                coll.Add(2);
                polyline.StrokeDashArray = coll;
            }
            polyline.StrokeThickness = 3;
        }
        public Polyline GetPolyline() {
            return polyline;
        }

        public void removeGraphicFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(polyline);
        }

        public void updateGraphicPoints(Point[] points)
        {
            polyline.Points.Clear();
            foreach (var point in points)
            {
                polyline.Points.Add(point);
            }
            polyline.RenderTransform = Transform.Identity;
            
        }

        public void draw(Canvas canvas)
        {
            canvas.Children.Add(polyline);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("pointCount", polyline.Points.Count, typeof(int));
            int i = 0;
            foreach (var el in polyline.Points)
            {
                info.AddValue("point" + (i++), el , typeof(Point));
            }
        }

        public BetweenLine(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < (int)info.GetValue("pointCount", typeof(int)); i++)
            {
                Point s = (Point)info.GetValue("point" + i, typeof(Point));
                polyline.Points.Add(s);
            }
        }


    }
}
