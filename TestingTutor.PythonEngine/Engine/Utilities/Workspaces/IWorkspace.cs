using System;

namespace TestingTutor.PythonEngine.Engine.Utilities.Workspaces
{
    public interface IWorkspace : IDisposable
    {
        string CreateDirectory();
    }
}
