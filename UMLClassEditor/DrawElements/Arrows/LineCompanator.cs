﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.DrawElements.Lines;
using UMLClassEditor.DrawElements.Tips;

namespace UMLClassEditor.DrawElements.Arrows
{
    public class LineCompanator:UMLElement
    {
        public enum Tips
        {
            AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
        }

        private Arrow arrow;

        public LineCompanator(UMLElement firstBlock, UMLElement secondBlock, Tips type)
        {
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
            Tip t = null;
            Lines.Lines r = null; 
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

            arrow = new Arrow(r.GetPolyline(),t.GetPolyline());
            s.addObserver(t);
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
    }
}