namespace Gun.Core
{
    public interface IDuplicateManager
    {
        bool Check(string id);
        string Track(string id);
    }
}