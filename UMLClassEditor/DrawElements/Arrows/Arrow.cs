using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace UMLClassEditor.DrawElements.Arrows {
    class Arrow : UMLElement {
        Polyline line;
        Polyline tip;

        public Arrow(Polyline line, Polyline tip) {
            this.line = line;
            this.tip = tip;
        }

        public override void setPicked(bool set)
        {
            throw new NotImplementedException();
        }

        public override void draw(Canvas canvas) {
            canvas.Children.Add(line);
            canvas.Children.Add(tip);
        }

        public override void deleteFrom(Canvas canvas)
        {
            throw new NotImplementedException();
        }

        public override void move(Point point)
        {
            throw new NotImplementedException();
        }

        public override bool canPick(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
