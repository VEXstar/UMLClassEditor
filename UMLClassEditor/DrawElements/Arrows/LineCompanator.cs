using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.DrawElements.Lines;
using UMLClassEditor.DrawElements.Tips;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Arrows
{
    public class LineCompanator : UMLElement, IObserver
    {
        public enum Tips
        {
            AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
        }

        private Arrow arrow;
        private Lines.Lines a;
        Tip t = null;
        Lines.Lines r = null;
        Canvas canvas;

        public LineCompanator(UMLElement firstBlock, UMLElement secondBlock, Tips type, Canvas canvas)
        {
            this.canvas = canvas;
            if(!(firstBlock is UMLClassBox)||!(secondBlock is UMLClassBox))
                return;
            UMLClassBox f = firstBlock as UMLClassBox;
            UMLClassBox s = secondBlock as UMLClassBox;
            Point[] fp = f.getStartPoints();
            Point[] sp = s.getStartPoints();
            double ox = s.GetCenterPoint().X - f.GetCenterPoint().X;
            string turn = (ox <0) ? "r" : "l";
            int indS = (ox < 0) ? 1 : 0;
            int indF = (ox < 0) ? 0 : 1;
            //Tip t = null;
            //Lines.Lines r = null; 
            if (type == Tips.AssotiationArrow)
            {
                t = new AssociationTip(s.getStartPoints()[indS],turn);
                r = new Lines.Lines(f.getStartPoints()[indF],t.GetEndPointForLine(),"solid");

            }
            else if (type == Tips.DerivArrow)
            {
                t = new DerivTip(s.getStartPoints()[indS], turn);
                r = new Lines.Lines(f.getStartPoints()[indF], t.GetEndPointForLine(), "solid");
            }
            else if(type == Tips.AggregationArrow)
            {
                t = new AggregateTip(s.getStartPoints()[indS],turn,Brushes.White);
                r = new Lines.Lines(f.getStartPoints()[indF], t.GetEndPointForLine(), "solid");
            }
            else if (type == Tips.CompositionArrow)
            {
                t = new AggregateTip(s.getStartPoints()[indS], turn, Brushes.Black);
                r = new Lines.Lines(f.getStartPoints()[indF], t.GetEndPointForLine(), "solid");
            }
            else if(type == Tips.DependenceArrow)
            {
                t = new AssociationTip(s.getStartPoints()[indS], turn);
                r = new Lines.Lines(f.getStartPoints()[indF], t.GetEndPointForLine(), "Dotted");
            }
            else if (type == Tips.ImplementationArrow)
            {
                t = new DerivTip(s.getStartPoints()[indS], turn);
                r = new Lines.Lines(f.getStartPoints()[indF], t.GetEndPointForLine(), "Dotted");
            }

            a = r;
            r.setConnectElems(f,t);
            arrow = new Arrow(r.GetPolyline(),t.GetPolyline());
            s.addObserver(t);
            f.addObserver(this);
            s.addObserver(this);
        }

        public override void setPicked(bool set)
        {

        }

        public override void draw(Canvas canvas)
        {
           arrow.draw(canvas);
        }

        public override void deleteFrom(Canvas canvas)
        {
            arrow.deleteFrom(canvas);
        }

        public override void move(Point point)
        {
        }

        public override bool canPick(Point point)
        {
            return false;
        }

        public override void update(Canvas canvas)
        {
            a.reDraw(canvas);
        }

        public void onEvent(object e) {
           // ноль реакции
        }

        public void UpdateForDelete() {
            canvas.Children.Remove(r.GetPolyline());
            canvas.Children.Remove(t.GetPolyline());
        }
    }
}