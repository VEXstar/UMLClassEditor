using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using UMLClassEditor.Interfaces;

namespace UMLClassEditor.DrawElements.Blocks
{
    public class UMLClassBox:UMLElement,IObservable
    {
       
        private int  type;
        private TextBox className;
        private List<TextBox> fields  = new List<TextBox>();
        private List<TextBox> methods = new List<TextBox>();
        Point[] twoStartPoints = new Point[2];
        public const int TYPE_CLASS = 1;
        public const int TYPE_INTERAFCE = 2;
        private Border element;
        private int standartHeight = 18;

        public UMLClassBox(int type,string className,Point st)
        {
            this.className = new TextBox();
            this.type = type;
            this.className.Text = className;
            TextBox text = new TextBox();
            text.Text = "-field";
            fields.Add(text);
            text = new TextBox();
            text.Text = "+void()";
            methods.Add(text);

            element = new Border();
            Canvas.SetTop(element, st.Y);
            Canvas.SetLeft(element, st.X);
            generateBorder(element);

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
                element.BorderBrush = (type == TYPE_CLASS) ? Brushes.Black : Brushes.Blue;
            }
        }

        public override void draw(Canvas canvas)
        {
            canvas.Children.Remove(element);
            canvas.Children.Add(element);
            
        }

        public override void deleteFrom(Canvas canvas)
        {
            canvas.Children.Remove(element);
            // NotifyAll(canvas);
            NotifyAboutDelete();
            removeAll();
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
            border.BorderBrush = (type == TYPE_CLASS) ? Brushes.Black : Brushes.Blue;
            border.Width = 180;
            border.Height = 134;

            int widthset = (int)border.Width - 6;
            border.Child = canvas;
            canvas.Margin = new Thickness(0);
            canvas.Background = Brushes.White;

            Label label = new Label();
            label.FontSize = 11;
            label.Height = standartHeight+20;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            label.Width = widthset;
            label.Content = String.Format("<<{0}>>", (type == TYPE_CLASS) ? "class" : "interface");
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
                textBox.Name = "field";
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
                textBox.Name = "method";
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
                if (((TextBox) sender).Name == "method")
                {
                    methods.Remove((TextBox) sender);
                }
                else
                {
                    fields.Remove((TextBox)sender);
                }
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
            text.Text = "+void()";
            methods.Add(text);
            generateBorder(element);
        }

        private void NewFieldsBtnOnClick(object sender, RoutedEventArgs e)
        {
            TextBox text = new TextBox();
            text.Text = "-field";
            fields.Add(text);
            generateBorder(element);
           
        }

        Point last = new Point(-666,-666);
        public override void move(Point point)
        {
            if (last.X == -666 && last.Y == -666)
            {
                last = point;
                return;
                
            }
                
            Canvas.SetTop(element, Canvas.GetTop(element) + point.Y - last.Y );
            Canvas.SetLeft(element, Canvas.GetLeft(element) +point.X - last.X);

            NotifyAll(new Point(point.X - last.X, point.Y - last.Y));
            last = point;
            updateStartPoints();
        }

        public override bool canPick(Point point)
        {
            double left = Canvas.GetLeft(element);
            double top = Canvas.GetTop(element);
            return (left < point.X) && (left + element.Width > point.X) && (top < point.Y) &&
                   (top + 40 > point.Y);
        }

        public override void update(Canvas canvas)
        {
            
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

        public void removeAll()
        {
            observers.Clear();
        }

        public void NotifyAll(object e)
        {
            foreach (var observer in observers)
            {
               observer.onEvent(e);
            }
        }

        public void NotifyAboutDelete() {
            foreach (var observer in observers) {
                observer.UpdateForDelete();
            }
        }
    }
}