using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;



namespace TestingTutor.CSharpEngine.Engine.Analysis.ModuleHandlers
{
    public class ModuleName
    {
        public string Name { get; set; }
        
        //public ModuleName(string name)
        //{
        //    Name = name;
        //}

        public override bool Equals(object obj)
        {
            var name = obj as ModuleName;
            return name != null &&
                   Name == name.Name;
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
