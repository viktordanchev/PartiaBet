namespace Core.Interfaces.Service
{
    public interface IMemoryCacheService
    {
        void Add(string key, string value, TimeSpan time);
        bool isExist(string key);
        string GetValue(string key);
    }
}