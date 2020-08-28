using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using TestingTutor.EngineModels;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;
using TestingTutor.JavaEngine.Utilities;

namespace TestingTutor.JavaEngine.Engine
{
    public class JavaReflector : IJavaReflector
    {
        private const string Command = "java.exe";
        private const string SubCommand = @"JUnitClassReflection";
        private const string ClassPathOption = "-classpath";
        private const string ClassPath = @"c:\TestingTutorTools\junit-4.12.jar;c:\TestingTutorTools\hamcrest-core-1.3.jar;C:\TestingTutorTools\TestingTutorAnnotation.jar;C:\TestingTutorTools\xstream\lib\xstream-1.4.11.1.jar;C:\TestingTutorTools;";

        public void Reflect(string codeDirectory, string reflectionDirectory, ref List<JavaTestClass> testClasses)
        {
            GenerateXml(codeDirectory, reflectionDirectory, ref testClasses);
            VerifyXml(reflectionDirectory, testClasses.Count);
            MapTestMethods(reflectionDirectory, ref testClasses);
        }

        public void VerifyXml(string reflectionDirectory, int expectedCount)
        {
            var files = Directory.GetFiles(reflectionDirectory, "*xml");
            if (expectedCount != files.Length)
            {
                throw new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForReflectionVerifyXml(expectedCount, files)
                };
            }
        }

        public void GenerateXml(string codeDirectory, string reflectionDirectory, ref List<JavaTestClass> testClasses)
        {
            foreach (var javaClass in testClasses)
            {
                GenerateXmlForJavaClass(codeDirectory, reflectionDirectory, javaClass);
            }
        }
        

        public void GenerateXmlForJavaClass(string codeDirectory, string reflectionDirectory, JavaTestClass javaTestClass)
        {
            var process = new EngineProcess(Command, GetCommandOptions(codeDirectory, javaTestClass, reflectionDirectory), codeDirectory);
            
            try
            {
                process.Run();
            }
            catch (Exception e)
            {
                var exception = new EngineExceptionDto()
                {
                    Report = JavaEngineReportExceptionFactory
                        .GenerateReportForReflectionProcess(process, e, javaTestClass)
                };
                process.Stop();
                throw exception;
            }
            process.Stop();
        }

        public void MapTestMethods(string reflectionDirectory, ref List<JavaTestClass> testClasses)
        {
            foreach (var testClass in testClasses)
            {
                try
                {
                    MapTestMethodsImplementation(reflectionDirectory, testClass);
                }
                catch (Exception e)
                {
                    throw new EngineExceptionDto()
                    {
                        Report = JavaEngineReportExceptionFactory
                            .GenerateReportForReflectionMapping(e, testClass)
                    };
                }
            }
        }

        public void MapTestMethodsImplementation(string reflectionDirectory, JavaTestClass testTestClass)
        {
            var document = GetXmlDocument(reflectionDirectory, testTestClass);
            AddTestMethodsToTestClass(document, testTestClass);
        }

        public void AddTestMethodsToTestClass(XmlDocument document, JavaTestClass testTestClass)
        {
            var nodeList = SelectTestMethodList(document);
            foreach (XmlNode node in nodeList)
            {
                testTestClass.AddMethod(GetJavaMethod(node));
            }
        }

        public JavaTestMethod GetJavaMethod(XmlNode node)
        {
            var name = GetNameFromTestMethod(node);
            if (node.ChildNodes.Count > 1)
            {
                return new JavaTestMethod()
                {
                    Name = name,
                    EquivalenceClass = GetEquivalenceClass(node),
                    LearningConcepts = GetConcepts(node) 
                };
            }
            return new JavaTestMethod()
            {
                Name = name
            };
        }

        public XmlNodeList SelectTestMethodList(XmlDocument document)
        {
            return document.SelectNodes("/TestClass/testMethods/TestMethod");
        }

        public string GetNameFromTestMethod(XmlNode node)
        {
            return node["name"].InnerText;
        }

        public string GetEquivalenceClass(XmlNode node)
        {
            return node["equivalenceClass"].InnerText;
        }

        public string[] GetConcepts(XmlNode node)
        {
            var concepts = new List<string>();
            foreach (XmlNode concept in node["learningConcepts"].ChildNodes)
            {
                concepts.Add(concept.InnerText);
            }
            return concepts.ToArray();
        }


        public XmlDocument GetXmlDocument(string reflectionDirectory, JavaTestClass javaTestClass)
        {
            var className = javaTestClass.Name;
            var xmlFileUri = $"{reflectionDirectory}{className}.xml";
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFileUri);
            return xmlDocument;
        }

        private static string GetCommandOptions(string workingDirectory, JavaTestClass javaTestClass, string reflectionDirectory)
            => $"{ClassPathOption} {ClassPath} {SubCommand} {workingDirectory}{javaTestClass.PackageDirectory}\\ {javaTestClass.Package}.{javaTestClass.Name} {reflectionDirectory}";

        private static bool VerifyReflect(string reflectionDirectory, int count)
            => Directory.GetFiles(reflectionDirectory, "*.xml").Length == count;
    }
}