using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TestingTutor.CSharpEngine.Models;
using McMaster.NETCore.Plugins;


namespace TestingTutor.CSharpEngine.Engine.Analysis.Parser
{
    public class AnnotatedTestParser
    {
        private List<string> test = new List<string>();
        private List<string> nameSpaceStrings = new List<string>();
        public List<AnnotatedTest> Setup(string arguments, string output, EngineWorkingDirectories workingDirectory)
        {
            string dllPath = arguments;
            string[] argString = { "", "" ,""};

            //Can configure to use .csproj if given the correct path
            arguments = GetBuildFile(arguments, ".csproj");

            argString[0] = arguments;

            //Can configure for .sln
            arguments = GetBuildFile(dllPath, ".sln");
            argString[1] = arguments;

            string addPath = @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\PublicAssemblies";

            var process = new System.Diagnostics.Process();
            var startInfo =
                new ProcessStartInfo()
                {
                    FileName = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    Arguments = argString[1]+ " /p:"+'"'+ "ReferencePath=" + addPath+'"',
                    WorkingDirectory = workingDirectory.ParentDirectory
                };
            process.StartInfo = startInfo;
            process.Start();
            var buildSummary = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            var exitCode = process.ExitCode;
            process.Close();
            process.Dispose();

            if (exitCode == 1 || !buildSummary.Contains("Build succeeded."))
            {
                throw new Exception(".csproj file Build failed. Path: " + arguments + " could not be executed properly.");
            }
          
            DirectoryInfo di = new DirectoryInfo(dllPath);
            var newPath = Path.Combine(dllPath +"\\bin\\Debug\\" + di.Name +".dll");

            PluginLoader loader = PluginLoader.CreateFromAssemblyFile(newPath);

            var annotations = GetAnnotations(loader.LoadDefaultAssembly());

            return annotations;
        }

        public List<AnnotatedTest> GetAnnotations(Assembly dll)
        {
            List<AnnotatedTest> annotatedTests = new List<AnnotatedTest>();
            var dType = dll.DefinedTypes;

            //Assumes custom attributes will have the word "custom" in them!
            var type = dType
                .Where( x => !x.ToString().Contains("Custom"));

            foreach (var t in type)
            {

                //Find all method names in test file
                var methodNames = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly )
                                    .Select(x => x.Name)
                                    .Distinct()
                                    .OrderBy(x => x);

                MethodInfo methodInfo;
                List<Tuple<List<Attribute>, string>> attributes = new List<Tuple<List<Attribute>, string>>();

                //Loop through names grabbing only ones with CustomAttribute
                foreach (string s in methodNames)
                {
                    methodInfo = t.GetMethod(s, BindingFlags.Public | BindingFlags.Instance,
                    null,
                    CallingConventions.Any,
                    new Type[] { },
                    null);

                    if (methodInfo != null)
                    {
                        var AnnotatedTest = new AnnotatedTest();
                        List<Attribute> attrList = new List<Attribute>();

                        var testinfo = methodInfo.GetCustomAttributes();
                        if (methodInfo.GetCustomAttributes().Count() > 1)
                        {
                            foreach (Attribute at in methodInfo.GetCustomAttributes())
                            {
                                attrList.Add(at);
                            }
                        }

                        test.Add(type.ToString() + '.' + methodInfo.Name);
                        nameSpaceStrings.Add(methodInfo.Name);

                        foreach (var attr in attrList)
                        {
                            //Assumes that custome attributes will contain "Custom" in the name!
                            if (attr.ToString().Contains("Custom"))
                            {
                                dynamic c = Activator.CreateInstance(attr.GetType());

                                c = attr;
                                AnnotatedTest.Concepts = c.concepts;
                                AnnotatedTest.EquivalanceClass = c.equivalenceClass;
                            }
                        }

                        AnnotatedTest.IndividualTest.TestName = t.ToString() + '.' + methodInfo.Name;
                        annotatedTests.Add(AnnotatedTest);
                    }
                }
            }
            return annotatedTests;
        }

        public string GetBuildFile(string arguments, string ext)
        {
            string newPath = arguments;

            if (ext == ".sln")
            {
                 newPath = Path.Combine(arguments, @"..\");
            }
            
            var files = Directory.GetFiles(newPath);

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file.Contains(ext))
                    {
                        return file;
                    }
                }
            }
            else
            {
                throw new Exception("Missing .csproj " +ext);
            }
            return "No "+ext+" file!";
        }
    }
}