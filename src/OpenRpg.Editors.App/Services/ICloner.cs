namespace OpenRpg.Editors.App.Services
{
    public interface ICloner
    {
        T Clone<T>(T source) where T : new();
    }
}