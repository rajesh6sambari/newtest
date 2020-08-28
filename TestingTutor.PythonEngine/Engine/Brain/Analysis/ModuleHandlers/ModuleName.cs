using System;
using System.Collections.Generic;
using System.IO;

namespace TestingTutor.PythonEngine.Engine.Brain.Analysis.ModuleHandlers
{
    public class ModuleName
    {
        public string Name { get; set; }

        public string CoverFileName => Path.GetFileNameWithoutExtension(Name) + ".cover";

        public override bool Equals(object obj)
        {
            var name = obj as ModuleName;
            return name != null &&
                   Name == name.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, CoverFileName);
        }

        public static bool operator ==(ModuleName name1, ModuleName name2)
        {
            return EqualityComparer<ModuleName>.Default.Equals(name1, name2);
        }

        public static bool operator !=(ModuleName name1, ModuleName name2)
        {
            return !(name1 == name2);
        }
    }
}
