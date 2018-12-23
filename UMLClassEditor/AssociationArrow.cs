using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace UMLClassEditor {
    class AssociationArrow : Arrows {
        Polyline path;

        public AssociationArrow(Point startPoint, Point endPoint) {
            path = new Polyline();
            
        }
    }
}
