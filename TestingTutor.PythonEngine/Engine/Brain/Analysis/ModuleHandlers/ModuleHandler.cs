using System.Collections.Generic;
using System.IO;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.ModuleHandlers
{
    public class ModuleHandler
    {
        public IList<ModuleName> GetModuleNames(string directory)
        {
            var names = new List<ModuleName>();

            var files = Directory.GetFileSystemEntries(directory);

            foreach (var file in files)
            {
                if (Path.GetExtension(file) == ".py")
                {
                    names.Add(new ModuleName()
                    {
                        Name = file
                    });
                }
            }
            return names;
        }
    }
}
