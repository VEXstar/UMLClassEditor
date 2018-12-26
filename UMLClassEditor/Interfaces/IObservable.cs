namespace UMLClassEditor.Interfaces
{
    public interface IObservable
    {
        void addObserver(IObserver observer);
        void removeObserver(IObserver observer);
        void removeAllObservers();
        void NotifyAll(object e);
    }
}