using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UMLClassEditor.DrawElements.Arrows;
using UMLClassEditor.DrawElements.Blocks;
using UMLClassEditor.DrawElements.Lines;
using UMLClassEditor.DrawElements.Tips;

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public enum State {
            Editing, ClassBox, InterfaceBox, AssociationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
                
        }

        Point point1;
        Point point2;

        public MainWindow() {
            InitializeComponent();
            drawCanvas.PreviewMouseMove += DrawCanvasOnPreviewMouseMove;
            
        }

        private void DrawCanvasOnPreviewMouseMove(object sender, MouseEventArgs e) {
            
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
            point1 = e.GetPosition(drawCanvas);
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
            point2 = e.GetPosition(drawCanvas);
            AssociationTip tip = new AssociationTip(point2, "TipToRigh");
            Lines line = new Lines(point1, tip.GetEndPointForLine(), "Dotted");
            Arrow arrow = new Arrow(line.GetPolyline(), tip.GetPolyline());
            arrow.draw(drawCanvas);
        }
    }
}
