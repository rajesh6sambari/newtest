using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TestingTutor.CSharpEngine.Engine.Analysis.Parser;

namespace TestingTutor.CSharpEngine.Engine.Analysis.ModuleHandlers
{
    public class ModuleHandler
    {
        public IList<ModuleName> GetModuleNames(IList<IndividualTest> individualTests)
        {
            var names = new List<ModuleName>();

            foreach(IndividualTest test in individualTests)
            {
                var n = test.TestName.Split('.');
                var nm = names.Where(x => x.Name == n[1]);

                if (nm.Count() == 0)
                {
                    names.Add(new ModuleName()
                    {
                        Name = n[1],
                    });
                }
            }
            return names;
        }
    }
}
