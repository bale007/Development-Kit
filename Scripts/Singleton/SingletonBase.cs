namespace Bale007
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>, new()
    {
        public static T Instance { get; } = new T();
    }
}