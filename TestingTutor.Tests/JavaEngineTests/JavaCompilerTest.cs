using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.Tests.Utilities;
using Xunit;
using Xunit.Sdk;

namespace TestingTutor.Tests.JavaEngineTests
{
    public class JavaCompilerTest
    {
        public string WorkspacePath = Path.Combine(Directory.GetCurrentDirectory(), "TempWorkspace");
        public const string JavaExtension = ".java";
        public const string ClassExtension = ".class";
        public const string ValidJavaCodeName = "Main";
        public const string InValidJavaCodeName = "invalid";
        public const string ValidJavaCode = "package MyPackage;\r\n\r\npublic class Main\r\n{\r\n\tpublic static void main(String[] args)\r\n\t{\r\n\t\tSystem.out.println(\"Hello World\");\r\n\t}\r\n}";
        public const string InvalidJavaCode = "package MyPackage;\r\n\r\npublic class Main\r\n{\r\n\tpublic static void main(String[] args)\r\n\t{\r\n\t\tSystem.out(\"Hello World\");\r\n\t}\r\n}";
         
        private IJavaCompiler GetCompiler()
        {
            return new JavaCompiler2();
        }

        private IList<string> OneSourceFileOfValidJavaCode(string dir)
        {
            var file = $"{ValidJavaCodeName}{JavaExtension}";
            var path = Path.Combine(dir, file);
            WriteToFile(ValidJavaCode, path);
            return new List<string>()
            {
                path
            };
        }

        private IList<string> OneSourceFileOfInvalidJavaCode(string dir)
        {
            var file = $"{InValidJavaCodeName}{JavaExtension}";
            var path = Path.Combine(dir, file);
            WriteToFile(InvalidJavaCode, path);
            return new List<string>()
            {
                path
            };
        }
        private void WriteToFile(string text, string name)
        {
            using (var fileStream = File.Open(name, FileMode.Append, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
            {
                fileWriter.Write(text);
                fileWriter.Flush();
            }
        }

        public string GeneratedDirector()
        {
            return $"{WorkspacePath}{Thread.CurrentThread.ManagedThreadId}";
        }

        [Fact]
        public void ShouldPassForValidJavaCode()
        {
            var compiler = GetCompiler();
            var dir = GeneratedDirector();
            using (var workspace = new DisposableWorkspace(dir))
            {
                compiler.Compile(workspace.Path, OneSourceFileOfValidJavaCode(dir));
            }
        }

        [Fact]
        public void ShouldCreateMainClassForValidJavaCode()
        {
            var compiler = GetCompiler();
            var dir = GeneratedDirector();
            using (var workspace = new DisposableWorkspace(dir))
            {
                compiler.Compile(workspace.Path, OneSourceFileOfValidJavaCode(dir));

                Assert.True(File.Exists(Path.Combine(workspace.Path, $"{ValidJavaCodeName}{ClassExtension}")));
            }
        }

        [Fact]
        public void ShouldThrowForInvalidJavaCode()
        {
            var compiler = GetCompiler();
            var dir = GeneratedDirector();
            using (var workspace = new DisposableWorkspace(dir))
            {
                var exception =
                    Record.Exception(() => compiler.Compile(dir, OneSourceFileOfInvalidJavaCode(dir)));
                Assert.IsAssignableFrom<EngineExceptionDto>(exception);
                Assert.True(((EngineExceptionDto) exception).Report != null);
            }
        }
        

    }
}
