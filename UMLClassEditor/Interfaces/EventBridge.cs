using System.Windows;

namespace UMLClassEditor.Interfaces
{
    public interface EventBridge
    {
         void ContextMenuAddChild(object sender, RoutedEventArgs e);
         void ContextMenuAddParent(object sender, RoutedEventArgs e);
    
    }
}