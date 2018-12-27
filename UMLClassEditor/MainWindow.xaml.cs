using System.Collections.Generic;
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
using UMLClassEditor.Interfaces;

namespace UMLClassEditor {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,EventBridge
    {
        public enum State {
            Editing, ClassBox, InterfaceBox, AssotiationArrow, DerivArrow, ImplementationArrow, DependenceArrow, AggregationArrow, CompositionArrow
                //
        }

        private State picked;
        private List<string> classes = new List<string>();
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
            generateClassesList();
            this.Loaded+= OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        public void generateClassesList()
        {
            classes.Clear();
            classes.Add("int");
            classes.Add("double");
            classes.Add("long");
            classes.Add("string");
            classes.Add("Point");
            classes.Add("object");
            classes.Add("void");
            foreach (var umlElement in elements)
            {
                if (umlElement is UMLClassBox s)
                {
                    classes.Add(s.getClassName());
                }
            }
            
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                List<UMLElement> s = new List<UMLElement>();
                List<UMLElement> d = new List<UMLElement>();
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
                    if (umlElement is UMLClassBox f)
                    {
                        foreach (var element in elements)
                        {
                            if (element is DependencyArrow arr)
                            {
                                if (arr.getSGUID() == f.getGuid()|| arr.getFGUID() == f.getGuid())
                                    d.Add(arr);
                            }
                        }
                    }
                     
                }
                s.AddRange(d);
                foreach (var umlElement in s)
                {
                    elements.Remove(umlElement);
                    umlElement.removeGraphicFromCanvas(drawCanvas);
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
               UMLClassBox box = new UMLClassBox("SomeClass",(picked==State.ClassBox)? UMLClassBox.BoxType.Class: UMLClassBox.BoxType.Interface,now, classes);
               box.initMenu(this);
               box.draw(drawCanvas);
                elements.Add(box);
                setMode(State.Editing);
            }
            else if (picked == State.Editing&&!isMoving&&e.ChangedButton == MouseButton.Left&&e.ChangedButton != MouseButton.Right)
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
                elements = new Storage<UMLElement>(s,this,classes);
                s.Close();
                foreach (var umlElement in elements)
                {
                    umlElement.draw(drawCanvas);
                }
                generateClassesList();
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
                s.Close();
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

        public void ContextMenuAddChild(object sender, RoutedEventArgs e)
        {
            if(!(sender is MenuItem))
                return;
            string guid = ((MenuItem) sender).Name.Substring(1, ((MenuItem) sender).Name.Length - 1).Replace("_", "-");
            foreach (var umlElement in elements)
            {
                if (umlElement is UMLClassBox && ((UMLClassBox) umlElement).getGuid() == guid)
                {
                    UMLClassBox f = umlElement as UMLClassBox;
                    Point s = f.GetCenterPoint();
                    UMLClassBox n = new UMLClassBox("NewChildClass",UMLClassBox.BoxType.Class,new Point(s.X+160, s.Y), classes);
                    n.draw(drawCanvas);
                    n.initMenu(this);
                    DependencyArrow dependency = new DependencyArrow(n,f,(f.getType() == UMLClassBox.BoxType.Class)? DependencyArrow.Tips.DerivArrow: DependencyArrow.Tips.ImplementationArrow);
                    dependency.draw(drawCanvas);
                    elements.Add(n);
                    elements.Add(dependency);
                    return;
                }
            }
        }
        public void ContextMenuAddParent(object sender, RoutedEventArgs e)
        {
            if (!(sender is MenuItem))
                return;
            string guid = ((MenuItem)sender).Name.Substring(1, ((MenuItem)sender).Name.Length - 1).Replace("_", "-");
            DependencyArrow link = null;
            UMLClassBox fblock = null;
            UMLClassBox sblock = null;
            foreach (var val in elements)
            {
                if (val is DependencyArrow&&(val as DependencyArrow).getFGUID() == guid&&((val as DependencyArrow).GetTip() == DependencyArrow.Tips.ImplementationArrow || (val as DependencyArrow).GetTip() == DependencyArrow.Tips.DerivArrow))
                {
                    link = val as DependencyArrow;
                }
                else if (val is UMLClassBox && (val as UMLClassBox).getGuid() == guid)
                {
                    fblock = val as UMLClassBox;
                }
                if(link!=null&&fblock!=null)
                    break;
            }

            if (link != null)
            {
                foreach (var umlElement in elements)
                {
                    if (umlElement is UMLClassBox && (umlElement as UMLClassBox).getGuid() == link.getSGUID())
                    {
                        sblock = umlElement as UMLClassBox;
                        break;
                    }
                }
                elements.Remove(link);
                link.removeGraphicFromCanvas(drawCanvas);
            }
            ParentDependencyWindow dependencyWindow = new ParentDependencyWindow(fblock.getFieldsList(),fblock.getMethodsList());
            if(dependencyWindow.ShowDialog() != true)
                return;
            UMLClassBox newbox = dependencyWindow.generateParent(fblock.GetCenterPoint(), classes);
            fblock.move(new Point(160,0),drawCanvas);
            DependencyArrow arrow = new DependencyArrow(fblock,newbox,DependencyArrow.Tips.DerivArrow);
            arrow.draw(drawCanvas);
            newbox.draw(drawCanvas);
            newbox.beforeLoadSets(classes);
            newbox.initMenu(this);
            elements.Add(arrow);
            elements.Add(newbox);
            if (link != null)
            {
                DependencyArrow sArrow = new DependencyArrow(newbox,sblock,link.GetTip());
                sArrow.draw(drawCanvas);
                elements.Add(sArrow);
            }
            //TODO: удалять отдельно связи
        }
    }
}
