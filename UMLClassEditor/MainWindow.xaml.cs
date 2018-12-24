using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using UMLClassEditor.DrawElements;

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public enum Mode {
            Nothing, Box, EditBox, AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
            
        }

        List<UMLElement> elements = new List<UMLElement>();
        public MainWindow() {
            InitializeComponent();
            drawCanvas.PreviewMouseMove += DrawCanvasOnPreviewMouseMove;
            
        }

        private void DrawCanvasOnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            //ss
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {

        }

        private void ClassSelected_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
