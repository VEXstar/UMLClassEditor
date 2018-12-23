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
using UMLClassEditor.DrawElements.Blocks;

namespace UMLClassEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += LoadedForm;
        }

        private UMLClassBox box;
        private void LoadedForm(object sender, RoutedEventArgs e)
        {
            box = new UMLClassBox(UMLClassBox.TYPE_CLASS,"SUKA");
            drawPanel.Children.Add(box.getGraph());
            
            this.PreviewMouseMove += DrawPanelOnMouseMove;
        }

        private Point last = new Point(-1,-1);
        private void DrawPanelOnMouseMove(object sender, MouseEventArgs e)
        {
            Point now = e.GetPosition(drawPanel);
            if (last.X==-1)
            {
                last = new Point(now.X, now.Y); ;
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                box.move((int)(now.X - last.X), (int)(now.Y - last.Y));
            }

            last  = new Point(now.X,now.Y);


        }
    }
}
