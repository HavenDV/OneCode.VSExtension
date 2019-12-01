using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OneCode.Core.Tests.Utilities;

namespace OneCode.Core.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void GetMethodsTest()
        {
            var text = ResourcesUtilities.ReadFile("Repository.cs");
            var code = Code.Load(text);

            foreach (var method in code.Methods)
            {
                Console.WriteLine(method.Name);
            }

            Assert.AreEqual(2, code.Methods.Count);

            Assert.AreEqual(Version.Parse("1.1.1.1"), code.Methods[0].Version);
            Assert.AreEqual(Version.Parse("1.0.0.0"), code.Methods[1].Version);
        }

        [TestMethod]
        public void RepositoryLoadTest()
        {
            var repository = Repository.Load("../../../../OneCode.Core");

            foreach (var file in repository.Files)
            {
                Console.WriteLine($"Path: {file.RelativePath}");

                foreach (var method in file.Code.Methods)
                {
                    Console.WriteLine($"- {method.Name}");
                }
            }

            Assert.IsTrue(repository.Files.Any());
        }

        [TestMethod]
        public void CodeSaveTest()
        {
            var text = ResourcesUtilities.ReadFile("Repository.cs");
            var code = Code.Load(text);

            Assert.AreEqual(2, code.Methods.Count);
            Assert.AreEqual("OneCode.Core", code.NamespaceName);

            code.NamespaceName = "TEST";
            code.Methods.RemoveAt(0);

            var newCodeText = code.Save();
            Console.WriteLine(newCodeText); 
            
            code = Code.Load(newCodeText);

            Assert.AreEqual(1, code.Methods.Count);
            Assert.AreEqual("TEST", code.NamespaceName);
        }
    }
}
