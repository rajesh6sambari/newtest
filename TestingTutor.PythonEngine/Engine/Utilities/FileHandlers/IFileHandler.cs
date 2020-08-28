namespace TestingTutor.PythonEngine.Engine.Utilities.FileHandlers
{
    public interface IFileHandler
    {
        void UnzipByteArray(byte[] bytes, string current, string identifier);
    }
}
