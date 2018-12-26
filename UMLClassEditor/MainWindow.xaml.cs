using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using UMLClassEditor.DrawElements;
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
                //
        }

        private State picked;
        private  Rectangle rectangle = new Rectangle();
        Storage<UMLElement> elements = new Storage<UMLElement>();

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
                        umlElement.removeGraphicFromCanvas(drawCanvas);
                    }
                }
                foreach (var umlElement in s)
                {
                    elements.Remove(umlElement);
                }
            }
        }

        private bool isMoving = false;
        private Point? last;
        private void DrawCanvasOnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            
            Point now = e.GetPosition((UIElement) sender);
            if (!last.HasValue)
            {
                last = now;
                
            }
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
                UMLElement g = getPickedElement(now);
                if (g != null)
                {
                    isMoving = true;
                    g.move(new Point(now.X-last.Value.X, now.Y - last.Value.Y),drawCanvas);
                }


            }

            last = now;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private bool doubleClick = false;
        private UMLElement fblock;
        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point now = e.GetPosition((UIElement)sender);
            if (picked == State.ClassBox || picked == State.InterfaceBox)
            {
               drawCanvas.Children.Remove(rectangle);
               now = new Point(now.X - rectangle.Width / 2, now.Y - rectangle.Height / 2);
               UMLClassBox box = new UMLClassBox("SomeClass",(picked==State.ClassBox)? UMLClassBox.BoxType.Class: UMLClassBox.BoxType.Interface,now);
               box.draw(drawCanvas);
                elements.Add(box);
                setMode(State.Editing);
            }
            else if (picked == State.Editing&&!isMoving&&e.LeftButton == MouseButtonState.Released)
            {
                UMLElement g = getPickedElement(now);
                if (g != null)
                {
                    g.setPicked(!g.getPicked());
                    return;
                }
            }
            else if(picked != State.Editing)
            {
                if (!doubleClick)
                {
                    doubleClick = true;
                    fblock = getPickedElement(now);
                }
                else
                {
                    DependencyArrow.Tips s;
                    s = convertTip();
                    doubleClick = false;
                    DependencyArrow line = new DependencyArrow(fblock,getPickedElement(now),s);
                    line.draw(drawCanvas);
                    elements.Add(line);
                    setMode(State.Editing);
                }

            }

            isMoving = false;
        }

        private void ClassSelected_Click(object sender, RoutedEventArgs e)
        {
            setMode(State.ClassBox);
        }

        private void InterfaceSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.InterfaceBox);
        }
        

        private void DependeceSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.DependenceArrow);
        }

        private void DeriveSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.DerivArrow);
        }

        private void AccationSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.AssotiationArrow);
        }

        private void Implementation_selected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.ImplementationArrow);
        }

        private void AggregationSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.AggregationArrow);
        }

        private void CompositionSelected_OnClick(object sender, RoutedEventArgs e)
        {
            setMode(State.CompositionArrow);
        }
        private DependencyArrow.Tips convertTip()
        {
            DependencyArrow.Tips s = DependencyArrow.Tips.AssotiationArrow;
            if (picked == State.AggregationArrow)
            {
                s = DependencyArrow.Tips.AggregationArrow;
            }
            else if (picked == State.AssotiationArrow)
            {
                s = DependencyArrow.Tips.AssotiationArrow;
            }
            else if (picked == State.CompositionArrow)
            {
                s = DependencyArrow.Tips.CompositionArrow;
            }
            else if (picked == State.DependenceArrow)
            {
                s = DependencyArrow.Tips.DependenceArrow;
            }
            else if (picked == State.DerivArrow)
            {
                s = DependencyArrow.Tips.DerivArrow;
            }
            else if (picked == State.ImplementationArrow)
            {
                s = DependencyArrow.Tips.ImplementationArrow;
            }

            return s;
        }
        private void setMode(State set)
        {
            AccationSelected.Background = DeriveSelected.Background = InterfaceSelected.Background =
                ClassSelected.Background = DependeceSelected.Background = CompositionSelected.Background =
                    Implementation_selected.Background = AggregationSelected.Background = Brushes.Transparent;
            switch (set)
            {
                case State.AssotiationArrow:
                    AccationSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.AggregationArrow:
                    AggregationSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.ClassBox:
                    ClassSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.CompositionArrow:
                    CompositionSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.DependenceArrow:
                    DependeceSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.DerivArrow:
                    DeriveSelected.Background = Brushes.RoyalBlue;
                    break;
                case State.ImplementationArrow:
                    Implementation_selected.Background = Brushes.RoyalBlue;
                    break;
                case State.InterfaceBox:
                    InterfaceSelected.Background = Brushes.RoyalBlue;
                    break;
            }
            picked = set;
        }

        private UMLElement getPickedElement(Point find)
        {
            foreach (var umlElement in elements)
            {
                if (umlElement.canPick(find))
                {
                    return umlElement;
                }
            }

            return null;
        }

        private void Open_OnClick(object sender, RoutedEventArgs e)
        {
           OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                elements.Clear(drawCanvas);
                Stream s = File.Open(dialog.FileName, FileMode.Open);
                elements = new Storage<UMLElement>(s);
                s.Close();
                foreach (var umlElement in elements)
                {
                    umlElement.draw(drawCanvas);
                }
            }
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "TEST|*.uml";
            if (dialog.ShowDialog() == true)
            {
                Stream s = File.Open(dialog.FileName, FileMode.Create);
                elements.save(s);
            }
        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PNG File|*.png";
            if (dialog.ShowDialog() == true)
            {
                RenderTargetBitmap renderTargetBitmap =
                    new RenderTargetBitmap((int)drawCanvas.ActualWidth, (int)drawCanvas.ActualHeight, 100, 100, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(drawCanvas);
                PngBitmapEncoder pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                using (Stream fileStream = File.Create(dialog.FileName))
                {
                    pngImage.Save(fileStream);
                }
            }
        }
    }
}
