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
using System.Windows.Shapes;
using UMLClassEditor.DrawElements;
using UMLClassEditor.DrawElements.Blocks;

namespace UMLClassEditor
{
    /// <summary>
    /// Логика взаимодействия для ParentDependencyWindow.xaml
    /// </summary>
    public partial class ParentDependencyWindow : Window
    {
        private List<UMLBlockLine> filedList;
        private List<UMLBlockLine> methodsList;
        public ParentDependencyWindow(List<UMLBlockLine> filedList,List<UMLBlockLine> methodsList)
        {
            InitializeComponent();
            this.filedList = filedList;
            this.methodsList = methodsList;
            this.Loaded += OnLoaded;

        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in filedList)
            {
                
                FiledListBox.Items.Add(new FItem(){LinkObj = textBox,Title = textBox.Name.Text });
                
            }
            foreach (var textBox in methodsList)
            {
                MethodsList.Items.Add(new FItem() { LinkObj = textBox, Title = textBox.Name.Text });
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Hide();

        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Hide();
        }

        public UMLClassBox generateParent(Point point,List<string> clases)
        {
            UMLClassBox box = new UMLClassBox("NewParentClass",UMLClassBox.BoxType.Class,new Point(point.X-150,point.Y-200), clases);
            foreach (var fitem in FiledListBox.SelectedItems)
            {
                var umlBlockLine = ((FItem) fitem).LinkObj;
             TextBox text = new TextBox();
             ComboBox combo = new ComboBox();
                text.Name = combo.Name = umlBlockLine.GUID;
                text.Text = umlBlockLine.Name.Text;
                combo.ItemsSource = umlBlockLine.Type.ItemsSource;
                combo.Text = umlBlockLine.Type.Text;
             box.getFieldsList().Add(new UMLBlockLine(){GUID = umlBlockLine.GUID,Name = text,Type = combo});
            }
            foreach (var fitem in MethodsList.SelectedItems)
            {
                var umlBlockLine = ((FItem)fitem).LinkObj;
                TextBox text = new TextBox();
                ComboBox combo = new ComboBox();
                text.Name = combo.Name = umlBlockLine.GUID;
                text.Text = umlBlockLine.Name.Text;
                combo.ItemsSource = umlBlockLine.Type.ItemsSource;
                combo.Text = umlBlockLine.Type.Text;
                box.getMethodsList().Add(new UMLBlockLine() { GUID = umlBlockLine.GUID, Name = text, Type = combo });
            }
            box.updateGUI();
            return box;
        }
    }
    //Не бейте я так больше никогда не буду
    public class FItem
    {
        public string Title { get; set; }
        public UMLBlockLine LinkObj { get; set; }
    }
}
