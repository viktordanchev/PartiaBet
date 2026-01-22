namespace Core.Models.Match
{
    public sealed class RedisLockHandle
    {
        public string Key { get; }
        public string Value { get; }

        public RedisLockHandle(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
