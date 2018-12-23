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

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public enum Mode {
            Nothing, Box, EditBox, AssotiationArrow
        }

        Mode state = new Mode();
        Point point1 = new Point(0, 0);
        Point point2 = new Point(0, 0);

        public MainWindow() {
            InitializeComponent();
            state = Mode.AssotiationArrow;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) {
            point1.X = e.GetPosition((Canvas)sender).X;
            point1.Y = e.GetPosition((Canvas)sender).Y;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e) {
            point2.X = e.GetPosition((Canvas)sender).X;
            point2.Y = e.GetPosition((Canvas)sender).Y;
        }
    }
}
