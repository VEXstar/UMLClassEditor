namespace UMLClassEditor.Interfaces
{
    public interface IObservable
    {
        void addObserver(IObserver observer);
        void removeObserver(IObserver observer);
        void removeAll();
        void NotifyAll(object e);
        void NotifyAboutDelete();
    }
}