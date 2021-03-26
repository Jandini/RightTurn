namespace RightTurn
{
    public interface ITurnDirections
    {
        T Add<T>() where T : new();
        T Add<T>(T value);
        T Get<T>();
        T TryGet<T>();
        bool Have<T>();
        bool Have<T>(out T value);
    }
}