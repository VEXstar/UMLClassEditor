using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Blocks
{
    [Serializable()]
    public class UMLClassBox:UMLElement,IObservable, ISerializable
    {
       public enum BoxType
       {
           Class,Interface
       }
        public enum NotifyType
        {
            Move,Delete,Change,AddF,AddM,DeleteL
        }
        private BoxType  type;
        private TextBox className;
        private List<UMLBlockLine> fields  = new List<UMLBlockLine>();
        private List<UMLBlockLine> methods = new List<UMLBlockLine>();
        private List<string> avableTypes;


        Point[] twoStartPoints = new Point[2];
        private Border element;
        private int standartHeight = 18;
        private string guid;

        public string getClassName()
        {
            return className.Text;
        }
        public UMLClassBox(string className,BoxType type,Point st,List<string> types)
        {
            guid = Guid.NewGuid().ToString();
            this.className = new TextBox();
            this.type = type;
            this.className.Text = className;
            avableTypes = types;
            types.Add(className);
            
            element = new Border();
            Canvas.SetTop(element, st.Y);
            Canvas.SetLeft(element, st.X);
            generateBorder(element);

        }

        private EventBridge bridge;
        public void initMenu(EventBridge bridge)
        {
            this.bridge = bridge;
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Создать производный класс";
            item.Name = "f" + guid.Replace("-","_");
            item.Click += bridge.ContextMenuAddChild;
            menu.Items.Add(item);
            item = new MenuItem();
            item.Name = "f" + guid.Replace("-", "_");
            item.Header = "Создать базовый класс";
            item.Click += bridge.ContextMenuAddParent;
            menu.Items.Add(item);
            element.ContextMenu = menu;

        }

        public BoxType getType()
        {
            return type;
        }
        public override void setPicked(bool set)
        {
            isPicked = set;
            if (set)
            {
                element.BorderBrush = Brushes.OrangeRed;
            }
            else
            {
                element.BorderBrush = (type == BoxType.Class) ? Brushes.Black : Brushes.Blue;
            }
        }

        public override void removeGraphicFromCanvas(Canvas canvas)
        {
            canvas.Children.Remove(element);
            removeAllObservers();
        }

        public override void updateGraphicPoints(Point[] points)
        {
            throw new NotImplementedException();
        }

        public override void draw(Canvas canvas)
        {
            canvas.Children.Remove(element);
            canvas.Children.Add(element);
            
        }

        public UIElement getGraph()
        {
            return element;
        }

        private void generateBorder(Border border)
        {
            ((Canvas) border.Child)?.Children.RemoveRange(0, ((Canvas)border.Child).Children.Count);
            border.Child = null;
            Canvas canvas = new Canvas();
            border.BorderThickness = new Thickness(2);
            border.BorderBrush = (type == BoxType.Class) ? Brushes.Black : Brushes.Blue;
            border.Width = 180;
            border.Height = 134;

            int widthset = (int)border.Width - 6;
            border.Child = canvas;
            canvas.Margin = new Thickness(0);
            canvas.Background = Brushes.White;

            Label label = new Label();
            label.FontSize = 11;
            label.Cursor = Cursors.Hand;
            label.Height = standartHeight+20;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Width = widthset;
            label.Content = String.Format("<<{0}>>", (type == BoxType.Class) ? "class" : "interface");
            Canvas.SetTop(label,0);
            canvas.Children.Add(label);

            int drop = standartHeight+5;
            
            className.HorizontalContentAlignment = HorizontalAlignment.Center;
            className.BorderBrush = Brushes.Transparent;
            className.BorderThickness = new Thickness(0);
            className.Height = standartHeight;
            className.TextChanged+= ClassNameOnTextChanged;
            className.Width = widthset;
            Canvas.SetTop(className,drop);
            canvas.Children.Add(className);
            drop += standartHeight;
            Separator s = new Separator();
            if (type == BoxType.Class)
            {
                s.Height = 5;
                s.Width = widthset;
                s.BorderBrush = Brushes.Black;
                Canvas.SetTop(s, drop);
                canvas.Children.Add(s);
                drop += (int)s.Height;

                foreach (var textBox in fields)
                {

                    textBox.Name.Height = standartHeight;
                    textBox.Type.Width = 60;
                    
                    textBox.Name.Width = widthset - 60;
                    textBox.Name.KeyUp += TextBoxOnKeyUp;
                    textBox.Name.BorderBrush = Brushes.Transparent;
                    textBox.Name.BorderThickness = new Thickness(0);
                    textBox.Name.TextChanged += SOnTextChanged;

                    textBox.Type.BorderBrush = Brushes.Transparent;
                    textBox.Type.BorderThickness = new Thickness(0);
                    textBox.Type.SelectionChanged += TypeOnSelectionChanged;
                    
                    Canvas.SetTop(textBox.Name, drop);
                    Canvas.SetTop(textBox.Type, drop);
                    Canvas.SetLeft(textBox.Name, 62);
                    drop += standartHeight;
                    canvas.Children.Add(textBox.Name);
                    canvas.Children.Add(textBox.Type);
                }
                Button newFieldsBtn = new Button();

                newFieldsBtn.Content = "+";
                newFieldsBtn.Height = standartHeight;
                newFieldsBtn.Width = widthset;
                newFieldsBtn.BorderBrush = Brushes.Transparent;
                newFieldsBtn.BorderThickness = new Thickness(0);
                newFieldsBtn.Click += NewFieldsBtnOnClick;
                Canvas.SetTop(newFieldsBtn, drop);
                canvas.Children.Add(newFieldsBtn);
                drop += standartHeight;
            }

            s = new Separator();
            s.Height = 6;
            s.Width = widthset;
            s.BorderBrush = Brushes.Black;
            Canvas.SetTop(s, drop);
            canvas.Children.Add(s);
            drop += (int) s.Height;
            foreach (var textBox in methods)
            {

                textBox.Name.Height = standartHeight;
                textBox.Type.Width = 60;

                textBox.Name.Width = widthset - 60;
                textBox.Name.KeyUp += TextBoxOnKeyUp;
                textBox.Name.BorderBrush = Brushes.Transparent;
                textBox.Name.BorderThickness = new Thickness(0);
                textBox.Name.TextChanged += SOnTextChanged;
                textBox.Type.SelectionChanged += TypeOnSelectionChanged;
                textBox.Type.BorderBrush = Brushes.Transparent;
                textBox.Type.BorderThickness = new Thickness(0);

                Canvas.SetTop(textBox.Name, drop);
                Canvas.SetTop(textBox.Type, drop);
                Canvas.SetLeft(textBox.Name, 62);
                drop += standartHeight;
                canvas.Children.Add(textBox.Name);
                canvas.Children.Add(textBox.Type);
            }
            Button newMethodButton = new Button();
            newMethodButton.Content = "+";
            newMethodButton.Height = standartHeight;
            newMethodButton.Width = widthset;
            newMethodButton.BorderBrush = Brushes.Transparent;
            newMethodButton.BorderThickness = new Thickness(0);
            newMethodButton.Click += NewMethodButtonOnClick;
            Canvas.SetTop(newMethodButton, drop);
            canvas.Children.Add(newMethodButton);
            border.Height = drop+standartHeight+10;
            updateStartPoints();
        }

        private void ClassNameOnTextChanged(object sender, TextChangedEventArgs e)
        {
         bridge.generateClassesList();
        }

        private void TypeOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NotifyAll(sender, NotifyType.Change);
        }

        private void TextBoxOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete&& sender is TextBox)
            {
                bool compl = false;
                foreach (var umlBlockLine in methods)
                {
                    if (umlBlockLine.Name == (TextBox) sender)
                    {
                        methods.Remove(umlBlockLine);
                        compl = true;
                        break;
                    }
                }

                if (!compl)
                {
                    foreach (var umlBlockLine in fields)
                    {
                        if (umlBlockLine.Name == (TextBox)sender)
                        {
                            fields.Remove(umlBlockLine);
                            compl = true;
                            break;
                        }
                    }
                }
                if(!compl)
                    return;
                NotifyAll(sender,NotifyType.DeleteL);
                generateBorder(element);
            }
        }

        private void updateStartPoints()
        {
            twoStartPoints[0].X = Canvas.GetLeft(element);
            twoStartPoints[0].Y = Canvas.GetTop(element) + +element.Height / 2;
            twoStartPoints[1].Y = Canvas.GetTop(element)+element.Height/2;
            twoStartPoints[1].X = Canvas.GetLeft(element)+ element.Width;
        }

        public  Point[] getStartPoints()
        {
            return  twoStartPoints;
        }

        public Point GetCenterPoint()
        {
            return new Point(Canvas.GetLeft(element)+element.Width/2, Canvas.GetTop(element) + element.Height / 2);
        }

        private void NewMethodButtonOnClick(object sender, RoutedEventArgs e)
        {
            TextBox text = new TextBox();
            text.Text = "+void"+methods.Count+"()";
            text.Name = "f" + Guid.NewGuid().ToString().Replace("-", "");
            UMLBlockLine block = new UMLBlockLine();
            block.Name = text;
            ComboBox box = new ComboBox();
            box.Name = text.Name;
            box.ItemsSource = avableTypes;
            box.Text = "void";
            block.Type  = box;
            block.GUID = text.Name;
            methods.Add(block);
            generateBorder(element);
            NotifyAll(block, NotifyType.AddM);
        }

        private void NewFieldsBtnOnClick(object sender, RoutedEventArgs e)
        {
            TextBox text = new TextBox();
            text.Text = "#field"+fields.Count;
            text.Name = "f" + Guid.NewGuid().ToString().Replace("-", "");
            UMLBlockLine block = new UMLBlockLine();
            block.Name = text;
            ComboBox box = new ComboBox();
            box.Name = text.Name;
            box.ItemsSource = avableTypes;
            box.Text = "int";
            block.Type = box;
            block.GUID = text.Name;
            fields.Add(block);
            generateBorder(element);
            
            NotifyAll(block, NotifyType.AddF);
        }

        public override void move(Point point, Canvas canvas)
        {
            Canvas.SetTop(element,Canvas.GetTop(element)+point.Y);
            Canvas.SetLeft(element,Canvas.GetLeft(element)+point.X);
            moveStruct sMoveStruct = new moveStruct();
            sMoveStruct.offset = point;
            sMoveStruct.WorkCanvas = canvas;
            updateStartPoints();
            NotifyAll(sMoveStruct,NotifyType.Move);
        }

        public override bool canPick(Point point)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            return (left < point.X) && (left + element.Width > point.X) && (top < point.Y) &&
                   (top + 40 > point.Y);
        }

        public override string getGuid()
        {
            return guid;
        }

        public override void updateGUI()
        {
            generateBorder(element);
        }

        List<IObserver> observers = new List<IObserver>();
        public void addObserver(IObserver observer)
        {
           observers.Add(observer);
        }

        public void removeObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void removeAllObservers()
        {
            observers.Clear();
        }

        public List<UMLBlockLine> getMethodsList()
        {
            return methods;
        }
        public List<UMLBlockLine> getFieldsList()
        {
            return fields;
        }

        public void NotifyAll(object e,object type)
        {
            foreach (var observer in observers)
            {
               observer.onEvent(this,e,type);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("fieldsCount",fields.Count,typeof(int));
            info.AddValue("methodCount",methods.Count,typeof(int));
            info.AddValue("className", className.Text,typeof(string));
            info.AddValue("guid", guid,typeof(string));
            info.AddValue("BoxType", type,typeof(BoxType));
            info.AddValue("pos",new Point(Canvas.GetTop(element), Canvas.GetLeft(element)),typeof(Point));
            int i = 0;
            foreach (var textBox in fields)
            {
                info.AddValue("fieldN"+(i),textBox.Name.Text,typeof(string));
                info.AddValue("fieldT"+(i),textBox.Type.Text, typeof(string));
                info.AddValue("fieldG"+(i++),textBox.GUID,typeof(string));
            }
             i = 0;
            foreach (var textBox in methods)
            {
                info.AddValue("methodN" + (i), textBox.Name.Text, typeof(string));
                info.AddValue("methodT" + (i), textBox.Type.Text, typeof(string));
                info.AddValue("methodG" + (i++), textBox.GUID, typeof(string));
            }
        }
        private Dictionary<string,string> typesLoad = new Dictionary<string, string>();
        public UMLClassBox(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < (int) info.GetValue("fieldsCount", typeof(int)); i++)
            {
                TextBox s = new TextBox();
                ComboBox box = new ComboBox();
              //  box.ItemsSource = avableTypes;
                s.Text = (string)info.GetValue("fieldN" + i,typeof(string));
                string typeB = (string)info.GetValue("fieldT" + i,typeof(string));
                s.Name = (string)info.GetValue("fieldG" + i, typeof(string));
                box.Name = s.Name;
                typesLoad.Add(box.Name,typeB);
                s.TextChanged += SOnTextChanged;
                fields.Add(new UMLBlockLine(){GUID = s.Name,Name =s ,Type = box});
            }
            for (int i = 0; i < (int)info.GetValue("methodCount", typeof(int)); i++)
            {
                TextBox s = new TextBox();
                ComboBox box = new ComboBox();
               // box.ItemsSource = avableTypes;
                s.Text = (string)info.GetValue("methodN" + i, typeof(string));
                string typeB = (string)info.GetValue("methodT" + i, typeof(string));

                s.Name = (string)info.GetValue("methodG" + i, typeof(string));
                box.Name = s.Name;
                typesLoad.Add(box.Name, typeB);
                s.TextChanged += SOnTextChanged;
                methods.Add(new UMLBlockLine() { GUID = s.Name, Name = s, Type = box });
            }
            type =(BoxType)info.GetValue("BoxType",typeof(BoxType));
            className = new TextBox();
            className.Text = (string)info.GetValue("className", typeof(string));
            element = new Border();
            Point st = (Point) info.GetValue("pos", typeof(Point));
            guid = (string)info.GetValue("guid", typeof(string));
            Canvas.SetTop(element,st.X);
            Canvas.SetLeft(element,st.Y);
            generateBorder(element);
            twoStartPoints = new[] {new Point(0, 0), new Point(0, 0)};
            updateStartPoints();
        }

        public void beforeLoadSets(List<string> calsses)
        {
            List<UMLBlockLine> line = new List<UMLBlockLine>(methods);
            line.AddRange(fields);
            avableTypes = calsses;
            foreach (var umlBlockLine in line)
            {
                if (typesLoad.ContainsKey(umlBlockLine.GUID))
                {
                    umlBlockLine.Type.ItemsSource = avableTypes;
                    umlBlockLine.Type.Text = typesLoad[umlBlockLine.GUID];
                }
            }

        }

        private void SOnTextChanged(object sender, TextChangedEventArgs e)
        {
           NotifyAll(sender,NotifyType.Change);
        }

        public void imitateEvent(object sender,object type)
        {
            NotifyAll(sender,type);
        }
    }

    public class UMLBlockLine
    {
        public TextBox Name { get; set; }
        public ComboBox Type { get; set; }
        public string GUID { get; set; }
    }
}