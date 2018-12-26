using System.Windows;
using System.Windows.Controls;

namespace UMLClassEditor.Interfaces
{
    public interface IMovable
    {
     void move(Point offset,Canvas canvas);
    }
}