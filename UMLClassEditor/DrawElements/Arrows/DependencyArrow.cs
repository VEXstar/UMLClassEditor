using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.DrawElements.Lines;
using UMLClassEditor.DrawElements.Tips;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Arrows
{
    [Serializable()]
    public class DependencyArrow : UMLElement, IObserver,ISerializable
    {
        public enum Tips
        {
            AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
        }

        private Tip t;
        private BetweenLine r;
        private UMLClassBox fb;
        private UMLClassBox sb;
        private string fguid;
        private string sguid;
        private Tips mode;
        public DependencyArrow(UMLElement firstBlock, UMLElement secondBlock, Tips type)
        {
            if(!(firstBlock is UMLClassBox)||!(secondBlock is UMLClassBox))
                throw  new Exception("Один из элементов пустой");
            fb = firstBlock as UMLClassBox;
            sb = secondBlock as UMLClassBox;
            fguid = fb.getGuid();
            sguid = sb.getGuid();
            mode = type;
            t = generateTip(type);
            switch (type)
            {
                case Tips.AssotiationArrow:
                case Tips.DerivArrow:
                case Tips.AggregationArrow:
                case Tips.CompositionArrow:
                    r = new BetweenLine(getPointForFirstBLock(),t.GetEndPointForLine(),BetweenLine.Type.Solid);
                    break;
                case Tips.DependenceArrow:
                case Tips.ImplementationArrow:
                    r = new BetweenLine(getPointForFirstBLock(), t.GetEndPointForLine(), BetweenLine.Type.Dotted);
                    break;
            }

            fieldsSync();
            fb.addObserver(this);
            sb.addObserver(this);
        }

        private Point getPointForFirstBLock()
        {
            double ox = (sb).GetCenterPoint().X - (fb).GetCenterPoint().X;
            int indF = (ox < 0) ? 0 : 1;
            return ((UMLClassBox) fb).getStartPoints()[indF];
        }
        private Tip generateTip(Tips type)
        {

            double ox = (sb).GetCenterPoint().X - (fb).GetCenterPoint().X;
            int indS = (ox < 0) ? 1 : 0;
            Tip.Turns turn = (ox < 0) ? Tip.Turns.Left : Tip.Turns.Right;
            Tip t = null;
            UMLClassBox s = sb;
            
            switch (type)
            {
                case Tips.AssotiationArrow:
                    t = new AssociationTip(s.getStartPoints()[indS], turn);
                    break;
                case Tips.DerivArrow:
                    t = new DerivTip(s.getStartPoints()[indS], turn);
                    break;
                case Tips.AggregationArrow:
                    t = new AggregateTip(s.getStartPoints()[indS], turn, Brushes.White);
                    break;
                case Tips.CompositionArrow:
                    t = new AggregateTip(s.getStartPoints()[indS], turn, Brushes.Black);
                    break;
                case Tips.DependenceArrow:
                    t = new AssociationTip(s.getStartPoints()[indS], turn);
                    break;
                case Tips.ImplementationArrow:
                    t = new DerivTip(s.getStartPoints()[indS], turn);
                    break;
            }

            return t;
        }

        public override void setPicked(bool set)
        {
            isPicked = set;
        }

        public override void removeGraphicFromCanvas(Canvas canvas)
        {
            t.removeGraphicFromCanvas(canvas);
            r.removeGraphicFromCanvas(canvas);
        }

        public override void updateGraphicPoints(Point[] points)
        {
           r.updateGraphicPoints(points);
        }

        public override void draw(Canvas canvas)
        {
          r.draw(canvas);
          t.draw(canvas);
        }

        public override void move(Point point, Canvas canvas)
        {
            t.removeGraphicFromCanvas(canvas);
            t = generateTip(mode);
            t.draw(canvas);
            r.updateGraphicPoints(new Point[]{t.GetEndPointForLine(),getPointForFirstBLock()});
        }

        public override bool canPick(Point point)
        {
            return false;
        }

        public override string getGuid()
        {
            return "s";
        }

        public override void updateGUI()
        {
            throw new NotImplementedException();
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Tip",mode,typeof(Tips));
            info.AddValue("fg",fguid,typeof(string));
            info.AddValue("sg",sguid,typeof(string));
        }

        public DependencyArrow(SerializationInfo info, StreamingContext context)
        {
            mode = (Tips) info.GetValue("Tip", typeof(Tips));
            fguid = (string) info.GetValue("fg", typeof(string));
            sguid = (string) info.GetValue("sg", typeof(string));

        }
        public string getFGUID()
        {
            return fguid;
        }
        public string getSGUID()
        {
            return sguid;
        }

        public void setDependencyBeforeLoad(UMLElement firstBlock, UMLElement secondBlock)
        {
            if (!(firstBlock is UMLClassBox) || !(secondBlock is UMLClassBox))
                throw new Exception("Один из элементов пустой");
            fb = firstBlock as UMLClassBox;
            sb = secondBlock as UMLClassBox;
            fguid = fb.getGuid();
            sguid = sb.getGuid();
            t = generateTip(mode);
            switch (mode)
            {
                case Tips.AssotiationArrow:
                case Tips.DerivArrow:
                case Tips.AggregationArrow:
                case Tips.CompositionArrow:
                    r = new BetweenLine(getPointForFirstBLock(), t.GetEndPointForLine(), BetweenLine.Type.Solid);
                    break;
                case Tips.DependenceArrow:
                case Tips.ImplementationArrow:
                    r = new BetweenLine(getPointForFirstBLock(), t.GetEndPointForLine(), BetweenLine.Type.Dotted);
                    break;
            }
            fieldsSync();
            fb.addObserver(this);
            sb.addObserver(this);
        }

        public void onEvent(object sender, object e, object type)
        {
            if (sender is UMLClassBox && e is moveStruct point&&((UMLClassBox.NotifyType)type) == UMLClassBox.NotifyType.Move)
            {
                move(point.offset, point.WorkCanvas);
            }
            else if (sender is UMLClassBox&&((UMLClassBox)sender) == sb && ((UMLClassBox.NotifyType) type) == UMLClassBox.NotifyType.Change)
            {
                TextBox h = e as TextBox;
                foreach (var textBox in fb.getFieldsList())
                {
                    if (textBox.Name != h.Name) continue;
                    textBox.Text = h.Text;
                  //  fb.updateGUI();
                    return;
                }
                foreach (var textBox in fb.getMethodsList())
                {
                    if (textBox.Name != h.Name) continue;
                    textBox.Text = h.Text;
                  //  fb.updateGUI();
                    return;
                }
            }
            else if (sender is UMLClassBox && ((UMLClassBox) sender) == sb &&
                     ((UMLClassBox.NotifyType) type) == UMLClassBox.NotifyType.AddM)
            {
                TextBox h = e as TextBox;
                TextBox n = new TextBox();
                n.Name = h.Name;
                n.Text = h.Text;
                fb.getMethodsList().Add(n);
                fb.imitateEvent(n,type);
                fb.updateGUI();
            }
            else if (sender is UMLClassBox && ((UMLClassBox)sender) == sb &&
                     ((UMLClassBox.NotifyType)type) == UMLClassBox.NotifyType.AddF)
            {
                TextBox h = e as TextBox;
                TextBox n = new TextBox();
                n.Name = h.Name;
                n.Text = h.Text;
                fb.getFieldsList().Add(n);
                fb.imitateEvent(n, type);
                fb.updateGUI();
            }
            else if (sender is UMLClassBox && ((UMLClassBox)sender) == sb &&
                     ((UMLClassBox.NotifyType)type) == UMLClassBox.NotifyType.DeleteL)
            {
                TextBox h = e as TextBox;
                TextBox todelet = null;
                foreach (var textBox in fb.getFieldsList())
                {
                    if (textBox.Name == h.Name)
                    {
                        todelet = textBox;
                        break;
                    }

                }

                if (todelet != null)
                {
                    fb.getFieldsList().Remove(todelet);
                    fb.imitateEvent(todelet, type);
                    fb.updateGUI();
                    return;
                }
                foreach (var textBox in fb.getMethodsList())
                {
                    if (textBox.Name == h.Name)
                    {
                        todelet = textBox;
                        break;
                    }

                }
                if (todelet != null)
                {
                    fb.getMethodsList().Remove(todelet);
                    fb.imitateEvent(todelet, type);
                    fb.updateGUI();
                }
            }
        }

        private void fieldsSync()
        {
            List<TextBox> fM = fb.getMethodsList();
            List<TextBox> fF = fb.getFieldsList();
            List<TextBox> sM = sb.getMethodsList();
            List<TextBox> sF = sb.getFieldsList();
            if (mode == Tips.ImplementationArrow || mode == Tips.DerivArrow)
            {
                foreach (var textBox in sM)
                {
                    if(textBox.Text.StartsWith("-"))
                        continue;
                    bool founded = false;
                    foreach (var box in fM)
                    {
                        if (box.Text.Contains(textBox.Text.Replace(textBox.Text.Substring(0, 1), "")))
                        {
                            box.Name = textBox.Name;
                            founded = true;
                            break;
                        }
                        
                    }

                    if (!founded)
                    {
                        TextBox h = new TextBox();
                        h.Text = textBox.Text;
                        h.Name = textBox.Name;
                        fM.Add(h);
                    }
                }
                foreach (var textBox in sF)
                {
                    if (textBox.Text.StartsWith("-"))
                        continue;
                    bool founded = false;
                    foreach (var box in fF)
                    {
                        if (box.Text.Contains(textBox.Text.Replace(textBox.Text.Substring(0, 1), "")))
                        {
                            box.Name = textBox.Name;
                            founded = true;
                            break;
                        }

                    }

                    if (!founded)
                    {
                        TextBox h = new TextBox();
                        h.Text = textBox.Text;
                        h.Name = textBox.Name;
                        fF.Add(h);
                    }
                }
                fb.updateGUI();
            }

        }

    }
}