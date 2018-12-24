namespace UMLClassEditor.Interfaces
{
    public interface IObservable
    {
        void addObserver(IObserver observer);
        void removeObserver(IObserver observer);
        void NotifyAll(object e);
    }
}