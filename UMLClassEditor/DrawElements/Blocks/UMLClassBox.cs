using System.Collections.Generic;
using System.Windows.Controls;

namespace UMLClassEditor.DrawElements.Blocks
{
    public class UMLClassBox:UMLElement
    {
        private string  type;
        private TextBox className;
        private List<TextBox> fields  = new List<TextBox>();
        private List<TextBox> methods = new List<TextBox>();

        public UMLClassBox(string type,string className)
        {
            this.type = type;
            this.className.Text = className;
            TextBox text = new TextBox();
            text.Text = "-filed";
            fields.Add(text);
            text = new TextBox();
            text.Text = "+method()";
            methods.Add(text);
        }

        public override void draw()
        {
            Border border = new Border();
            border
            
        }
    }
}