namespace Kliens.Interfaces
{
    public interface IOptionCreator
    {
        bool CreateOption(string name, string item1, string item2, string item3, string settingKey, string settingValue);
    }
}