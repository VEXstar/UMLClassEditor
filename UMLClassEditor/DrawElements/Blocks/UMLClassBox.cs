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
        private List<TextBox> fields  = new List<TextBox>();
        private List<TextBox> methods = new List<TextBox>();
        
        Point[] twoStartPoints = new Point[2];
        private Border element;
        private int standartHeight = 18;
        private string guid;

        public UMLClassBox(string className,BoxType type,Point st)
        {
            guid = Guid.NewGuid().ToString();
            this.className = new TextBox();
            this.type = type;
            this.className.Text = className;
            TextBox text = new TextBox();
            if (type != BoxType.Interface)
            {
                text.TextChanged += SOnTextChanged;
                text.Text = "#field";
                text.Name = "f" + Guid.NewGuid().ToString().Replace("-", "");
                fields.Add(text);
            }
            text = new TextBox();
            text.TextChanged += SOnTextChanged;
            text.Name = "f"+Guid.NewGuid().ToString().Replace("-","");
            text.Text = "+void()";
            methods.Add(text);

            
            element = new Border();
            Canvas.SetTop(element, st.Y);
            Canvas.SetLeft(element, st.X);
            generateBorder(element);

        }

        public void initMenu(EventBridge bridge)
        {
            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Создать потомка";
            item.Name = "f" + guid.Replace("-","_");
            item.Click += bridge.ContextMenuAddChild;
            menu.Items.Add(item);
            item = new MenuItem();
            item.Name = "f" + guid.Replace("-", "_");
            item.Header = "Добавить предка";
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
                    textBox.Height = standartHeight;
                    textBox.Width = widthset;
                    textBox.KeyUp += TextBoxOnKeyUp;
                    textBox.BorderBrush = Brushes.Transparent;
                    textBox.BorderThickness = new Thickness(0);
                    Canvas.SetTop(textBox, drop);
                    drop += standartHeight;
                    canvas.Children.Add(textBox);
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
                textBox.Height = standartHeight;
                textBox.Width = widthset;
                textBox.KeyUp += TextBoxOnKeyUp;
                textBox.BorderBrush = Brushes.Transparent;
                textBox.BorderThickness = new Thickness(0);
                Canvas.SetTop(textBox, drop);
                drop += standartHeight;
                canvas.Children.Add(textBox);
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

        private void TextBoxOnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete&& sender is TextBox)
            {
                if (methods.Contains(((TextBox) sender)))
                {
                    methods.Remove((TextBox)sender);
                }
                else
                {
                    fields.Remove((TextBox)sender);
                }
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
            methods.Add(text);
            generateBorder(element);
            NotifyAll(text, NotifyType.AddM);
        }

        private void NewFieldsBtnOnClick(object sender, RoutedEventArgs e)
        {
            TextBox text = new TextBox();
            text.Text = "#field"+fields.Count;
            text.Name = "f" + Guid.NewGuid().ToString().Replace("-", "");
            fields.Add(text);
            generateBorder(element);
            
            NotifyAll(text, NotifyType.AddF);
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

        public List<TextBox> getMethodsList()
        {
            return methods;
        }
        public List<TextBox> getFieldsList()
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
                info.AddValue("field"+(i),textBox.Text,typeof(string));
                info.AddValue("fieldG"+(i++),textBox.Name,typeof(string));
            }
             i = 0;
            foreach (var textBox in methods)
            {
                info.AddValue("method" + (i), textBox.Text, typeof(string));
                info.AddValue("methodG" + (i++), textBox.Name, typeof(string));
            }
        }
        public UMLClassBox(SerializationInfo info, StreamingContext context)
        {
            for (int i = 0; i < (int) info.GetValue("fieldsCount", typeof(int)); i++)
            {
                TextBox s = new TextBox();
                s.Text = (string)info.GetValue("field" + i,typeof(string));
                s.Name = (string)info.GetValue("fieldG" + i, typeof(string));
                s.TextChanged += SOnTextChanged;
                fields.Add(s);
            }
            for (int i = 0; i < (int)info.GetValue("methodCount", typeof(int)); i++)
            {
                TextBox s = new TextBox();
                s.Text = (string)info.GetValue("method" + i, typeof(string));
                s.Name = (string)info.GetValue("methodG" + i, typeof(string));
                s.TextChanged += SOnTextChanged;
                methods.Add(s);
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

        private void SOnTextChanged(object sender, TextChangedEventArgs e)
        {
           NotifyAll(sender,NotifyType.Change);
        }

        public void imitateEvent(object sender,object type)
        {
            NotifyAll(sender,type);
        }
    }
}