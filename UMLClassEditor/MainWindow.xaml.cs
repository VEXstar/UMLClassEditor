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
using UMLClassEditor.DrawElements.Lines;
using UMLClassEditor.DrawElements.Tips;

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public enum Mode {
            Nothing, Box, EditBox, AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
        }

        Mode state = new Mode();
        Point point1 = new Point(0, 0);
        Point point2 = new Point(0, 0);

        public MainWindow() {
            InitializeComponent();
            state = Mode.ImplementationArrow;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
            point1.X = e.GetPosition((Canvas)sender).X;
            point1.Y = e.GetPosition((Canvas)sender).Y;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
            point2.X = e.GetPosition((Canvas)sender).X;
            point2.Y = e.GetPosition((Canvas)sender).Y;
            switch (state) {
                case Mode.AssotiationArrow:
                    SimpleLine line = new SimpleLine(point1, point2);
                    AssociationTip tip = new AssociationTip(point2, -10, 10, -10, -10); // в зависимости от нужного поворота принимаются разные параметры
                    Arrow arrow = new Arrow(line.GetPolyline(), tip.GetPolyline());
                    arrow.draw(MainCanvas);
                    break;
                case Mode.DerivArrow:
                    SimpleLine line1 = new SimpleLine(point1, point2);
                    DerivTip tip1 = new DerivTip(point2, -10, 10, -10, -10); // в зависимости от нужного поворота принимаются разные параметры
                    Arrow arrow1 = new Arrow(line1.GetPolyline(), tip1.GetPolyline());
                    arrow1.draw(MainCanvas);
                    break;
                case Mode.AggregationArrow:
                    SimpleLine line2 = new SimpleLine(point1, point2);
                    AggregateTip tip2 = new AggregateTip(point2, -10, 10, -10, -10); // в зависимости от нужного поворота принимаются разные параметры
                    Arrow arrow2 = new Arrow(line2.GetPolyline(), tip2.GetPolyline());
                    arrow2.draw(MainCanvas);
                    break;
                case Mode.ImplementationArrow:
                    DottedLine line3 = new DottedLine(point1, point2);
                    DerivTip tip3 = new DerivTip(point2, -10, 10, -10, -10); // в зависимости от нужного поворота принимаются разные параметры
                    Arrow arrow3 = new Arrow(line3.GetPolyline(), tip3.GetPolyline());
                    arrow3.draw(MainCanvas);
                    break;
            }
        }
    }
}
