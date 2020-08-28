using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestingTutor.CSharpEngine.Engine.Utilities.Workspace
{
    public interface IWorkspace : IDisposable
    {
        string CreateDirectory();
    }
}
