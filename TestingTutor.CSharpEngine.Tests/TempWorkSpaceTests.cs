using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestingTutor.CSharpEngine.Engine.Utilities.Workspace;

namespace TestingTutor.CSharpEngine.Tests
{
    [TestClass]
    public class TempWorkspaceTest
    {
        protected string CurrentDirectory;

        [TestInitialize]
        public void Init()
        {
            CurrentDirectory = Directory.GetCurrentDirectory();
        }

        [TestMethod]
        public void CreatesADirectory()
        {
            // Arrange
            var path = Path.Combine(CurrentDirectory, "CreatesADirectory");
            using (var workspace = new TempWorkspace(path))
            {
                // Act
                workspace.CreateDirectory();

                // Assert
                Assert.IsTrue(Directory.Exists(path));
            }
        }

        [TestMethod]
        public void RemoveDirectory()
        {
            // Arrange
            var path = Path.Combine(CurrentDirectory, "CreatesADirectory");
            using (var workspace = new TempWorkspace(path))
            {
                // Act
                workspace.CreateDirectory();
            }

            // Assert
            Assert.IsFalse(Directory.Exists(path));
        }
    }
}
