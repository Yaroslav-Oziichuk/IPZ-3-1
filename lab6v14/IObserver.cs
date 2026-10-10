namespace Lab10v14;

public interface IObserver<in T>
{
    void Update(T value);
}
