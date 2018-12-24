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
            Editing, ClassBox, InterfaceBox, AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow

        }

        public MainWindow() {
            InitializeComponent();
            drawCanvas.PreviewMouseMove += DrawCanvasOnPreviewMouseMove;
            
        }

        private void DrawCanvasOnPreviewMouseMove(object sender, MouseEventArgs e) {
            //ss
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {

        }
    }
}
