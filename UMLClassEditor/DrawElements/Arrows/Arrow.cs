using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void draw(Canvas canvas) {
            canvas.Children.Add(line);
            canvas.Children.Add(tip);
        }

        public override void move(int dx, int dy) {
            
        }
    }
}
