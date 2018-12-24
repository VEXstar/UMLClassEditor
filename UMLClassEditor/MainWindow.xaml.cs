using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UMLClassEditor.DrawElements;
using UMLClassEditor.DrawElements.Blocks;

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public enum State {
            Editing, ClassBox, InterfaceBox, AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
                
        }

        private State picked;
        private  Rectangle rectangle = new Rectangle();
        List<UMLElement> elements = new List<UMLElement>();
        public MainWindow() {
            InitializeComponent();

            this.KeyUp += OnKeyUp;
            drawCanvas.PreviewMouseMove += DrawCanvasOnPreviewMouseMove;
            rectangle.Width = 180;
            rectangle.Stroke = Brushes.RoyalBlue;
            rectangle.Fill = Brushes.Transparent;
            rectangle.Height = 134;


        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                List<UMLElement> s = new List<UMLElement>();
                foreach (var umlElement in elements)
                {
                    if (umlElement.getPicked())
                    {
                        s.Add(umlElement);
                        umlElement.deleteFrom(drawCanvas);
                    }
                }
                foreach (var umlElement in s)
                {
                    elements.Remove(umlElement);
                }
            }
        }

        private bool isMoving = false;
        private void DrawCanvasOnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            Point now = e.GetPosition((UIElement) sender);
            if (picked == State.ClassBox || picked == State.InterfaceBox)
            {
                if (!drawCanvas.Children.Contains(rectangle))
                {
                    drawCanvas.Children.Add(rectangle);

                }
                Canvas.SetTop(rectangle, now.Y - rectangle.Height / 2);
                Canvas.SetLeft(rectangle, now.X - rectangle.Width /2);
            }
            else if(e.LeftButton == MouseButtonState.Pressed)
            {
                foreach (var elem in elements)
                {
                    if (elem.canPick(now))
                    {
                        isMoving = true;
                        elem.move(now);
                        break;
                        
                    }
                      

                }
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point now = e.GetPosition((UIElement)sender);
            if (picked == State.ClassBox || picked == State.InterfaceBox)
            {
               drawCanvas.Children.Remove(rectangle);
               now = new Point(now.X - rectangle.Width / 2, now.Y - rectangle.Height / 2);
               UMLClassBox box = new UMLClassBox((picked==State.ClassBox)? UMLClassBox.TYPE_CLASS:UMLClassBox.TYPE_INTERAFCE,"SomeClass",now);
                box.draw(drawCanvas);
                picked = State.Editing;
                ClassSelected.Background = InterfaceSelected.Background = DependeceSelected.Background =
                    DeriveSelected.Background = AccationSelected.Background = Brushes.Transparent;
                elements.Add(box);
            }
            else if (picked == State.Editing&&!isMoving)
            {
                foreach (var umlElement in elements)
                {
                    if (umlElement.canPick(now))
                    {
                        umlElement.setPicked(!umlElement.getPicked());
                        return;
                    }
                }
            }

            isMoving = false;
        }

        private void ClassSelected_Click(object sender, RoutedEventArgs e)
        {
            picked = State.ClassBox;
            ClassSelected.Background = Brushes.RoyalBlue;
            InterfaceSelected.Background = DependeceSelected.Background =
                DeriveSelected.Background = AccationSelected.Background = Brushes.Transparent;
        }

        private void InterfaceSelected_OnClick(object sender, RoutedEventArgs e)
        {
            picked = State.InterfaceBox;
            InterfaceSelected.Background = Brushes.RoyalBlue;
            AccationSelected.Background = ClassSelected.Background =
                DependeceSelected.Background = DeriveSelected.Background = Brushes.Transparent;
        }
        

        private void DependeceSelected_OnClick(object sender, RoutedEventArgs e)
        {
            picked = State.DependenceArrow;
            DependeceSelected.Background = Brushes.RoyalBlue;
            InterfaceSelected.Background = ClassSelected.Background =
                DeriveSelected.Background = AccationSelected.Background = Brushes.Transparent;
        }

        private void DeriveSelected_OnClick(object sender, RoutedEventArgs e)
        {
            picked = State.DerivArrow;
            DeriveSelected.Background = Brushes.RoyalBlue;
            InterfaceSelected.Background = ClassSelected.Background =
                DependeceSelected.Background = AccationSelected.Background = Brushes.Transparent;
        }

        private void AccationSelected_OnClick(object sender, RoutedEventArgs e)
        {
            picked = State.AssotiationArrow;
            AccationSelected.Background = Brushes.RoyalBlue;
            InterfaceSelected.Background = ClassSelected.Background =
                DependeceSelected.Background = DeriveSelected.Background = Brushes.Transparent;
        }
    }
}
