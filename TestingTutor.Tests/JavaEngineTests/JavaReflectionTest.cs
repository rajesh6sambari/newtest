using System.Collections.Generic;
using System.IO;
using System.Threading;
using TestingTutor.JavaEngine.Engine;
using TestingTutor.JavaEngine.Engine.Interfaces;
using TestingTutor.JavaEngine.Models;
using TestingTutor.Tests.Utilities;
using Xunit;

namespace TestingTutor.Tests.JavaEngineTests
{
    public class JavaReflectionTest
    {
        public string WorkspacePath = Path.Combine(Directory.GetCurrentDirectory(), "TempWorkspace");
        public const string JavaExtensions = ".java";
        public const string ValidXmlCodeName = "LoanTest";
        public const string Package = "Questions";
        public const string PackageDirectory = "\\Questions";

        public const string ValidStudentLoanTestJavaCode = "/*\r\n * To change this license header, choose License Headers in Project Properties.\r\n * To change this template file, choose Tools | Templates\r\n * and open the template in the editor.\r\n */\r\npackage Questions;\r\n\r\nimport org.junit.After;\r\nimport org.junit.AfterClass;\r\nimport org.junit.Before;\r\nimport org.junit.BeforeClass;\r\nimport org.junit.Test;\r\nimport static org.junit.Assert.*;\r\n\r\n/**\r\n *\r\n * @author Alex Radermacher\r\n */\r\npublic class LoanTest {\r\n    \r\n   \r\n    @Test\r\n    public void testMethod_setHolder()\r\n    {\r\n        Loan testLoan = new Loan(\"Bank of America\", 0, 0, 0, 0); \r\n        \r\n        testLoan.setHolder(\"Wells Fargo\");\r\n        \r\n        assertEquals(\"Wells Fargo\", testLoan.getHolder());\r\n        \r\n        testLoan.setHolder(\"Bank of America\");\r\n        \r\n        assertEquals(\"Bank of America\", testLoan.getHolder());\r\n    }\r\n    \r\n    @Test\r\n    public void testMethod_calculateInterest()\r\n    {\r\n        Loan testLoan = new Loan(\"Bank of America\", 25000.0, 0.05, 12, 10); \r\n        \r\n        double expected = 41175.24;\r\n        double actual   = testLoan.calculateInterest();\r\n        \r\n        assertEquals(expected, actual, 0.01);\r\n        \r\n        testLoan = new Loan(\"Wells Fargo\", 1000.00, 0.00, 12, 10);\r\n        \r\n        expected = 1000.00;\r\n        actual   = testLoan.calculateInterest();\r\n        \r\n        assertEquals(expected, actual, 0.01);\r\n    }\r\n    \r\n}\r\n";
        public const string ValidInstructorLoanTestJavaCode = "/*\r\n * To change this license header, choose License Headers in Project Properties.\r\n * To change this template file, choose Tools | Templates\r\n * and open the template in the editor.\r\n */\r\npackage Questions;\r\n\r\nimport org.junit.After;\r\nimport org.junit.AfterClass;\r\nimport org.junit.Before;\r\nimport org.junit.BeforeClass;\r\nimport org.junit.Test;\r\nimport static org.junit.Assert.*;\r\n\r\nimport java.lang.annotation.Retention;\r\nimport java.lang.annotation.RetentionPolicy;\r\n\r\nimport TT.TestingTutor;\r\n\r\n/**\r\n *\r\n * @author Alex Radermacher\r\n */\r\npublic class LoanTest {\r\n   \r\n    @Test\r\n    @TestingTutor(equivalenceClass = \"Setters\", concepts = {\"DATA_INTEGRITY\"})\r\n    public void testMethod_setHolder()\r\n    {\r\n        Loan testLoan = new Loan(\"Bank of America\", 0, 0, 0, 0); \r\n        \r\n        testLoan.setHolder(\"Wells Fargo\");\r\n        \r\n        assertEquals(\"Wells Fargo\", testLoan.getHolder());\r\n        \r\n        testLoan.setHolder(\"Bank of America\");\r\n        \r\n        assertEquals(\"Bank of America\", testLoan.getHolder());\r\n    }\r\n    \r\n    @Test\r\n    @TestingTutor(equivalenceClass = \"Calculations\", concepts = {\"CALCULATIONS\", \"BOUNDARY_CONDITION\",  \"EQUIVALENCE_CLASS\"})\r\n    public void testMethod_calculateInterest()\r\n    {\r\n        Loan testLoan = new Loan(\"Bank of America\", 25000.0, 0.05, 12, 10); \r\n        \r\n        double expected = 41175.24;\r\n        double actual   = testLoan.calculateInterest();\r\n        \r\n        assertEquals(expected, actual, 0.01);\r\n        \r\n        testLoan = new Loan(\"Wells Fargo\", 1000.00, 0.00, 12, 10);\r\n        \r\n        expected = 1000.00;\r\n        actual   = testLoan.calculateInterest();\r\n        \r\n        assertEquals(expected, actual, 0.01);\r\n    }\r\n   \r\n    \r\n    @Test\r\n    @TestingTutor(equivalenceClass = \"Over $25,000\", concepts = {\"BOUNDARY_CONDITION\", \"EQUIVALENCE_CLASS\"})\r\n    public void testMethod_qualifiesForBonus_RightRange()\r\n    {\r\n        Loan testLoan = new Loan(\"Bank of America\", 25000.0, 0.05, 12, 10);\r\n        \r\n        boolean qualifies = testLoan.qualifiesForBonus(testLoan.calculateInterest());\r\n        \r\n        assertTrue(qualifies);\r\n    }\r\n    \r\n}\r\n";
        public const string ValidLoanJavaCode = "package Questions;\r\n\r\n/**\r\n *\r\n * @author araderma\r\n */\r\npublic class Loan \r\n{\r\n    private String holder; \r\n    private double principal; \r\n    private double interestRate;\r\n    private double numberOfPeriodsPerYear; \r\n    private double numberOfYears; \r\n    \r\n    public Loan()\r\n    {\r\n        holder                  = \"Bank of America\";\r\n        principal               = 25000.00;\r\n        interestRate            = 0.05; \r\n        numberOfPeriodsPerYear  = 12;\r\n        numberOfYears           = 10; \r\n    }\r\n    \r\n    public Loan (String holder, double principal, double interestRate, double numberOfPeriodsPerYear, double numberOfYears)\r\n    {\r\n        this.holder = holder; \r\n        this.principal = principal; \r\n        this.interestRate = interestRate; \r\n        this.numberOfPeriodsPerYear = numberOfPeriodsPerYear; \r\n        this.numberOfYears = numberOfYears; \r\n    }\r\n    \r\n    public void setHolder(String newHolder)\r\n    {\r\n        holder = newHolder;\r\n        \r\n        \r\n    }\r\n    \r\n    public String getHolder()\r\n    {\r\n        return holder; \r\n    }\r\n    \r\n    public boolean qualifiesForBonus(double interest)\r\n    {\r\n        if (interest > 10000 && interest <= 30000)\r\n        {\r\n            return false;\r\n        }\r\n        else if (interest > 30000 && interest <= 50000)\r\n        {\r\n            return true;\r\n        }\r\n        else\r\n        {\r\n            return false;\r\n        }\r\n    }\r\n    \r\n    \r\n    public double calculateInterest()\r\n    {\r\n        // should use the following formula\r\n        // A = P (1 + r/n)^nt where\r\n        //  P is the principal of the loan\r\n        //  r is the interest rate\r\n        //  n is the number of periods per year\r\n        //  t is the duration of the loan in years\r\n\r\n        //return 0.0;  // needs implementation \r\n        \r\n        return this.principal * Math.pow((1 + this.interestRate / this.numberOfPeriodsPerYear), this.numberOfPeriodsPerYear * this.numberOfYears);\r\n    }    \r\n\r\n    @Override\r\n    public String toString()\r\n    {\r\n        return \"Loan: (holder: \" + holder + \", principal: \" + principal + \", future value: \" + this.calculateInterest() + \")\"; \r\n    }\r\n}\r\n";
        public const string ValidMainJavaCode = "package Questions;\r\n\r\n/**\r\n *\r\n * @author araderma\r\n */\r\npublic class Main \r\n{\r\n    public static void main(String[] args) \r\n    {\r\n        // Example of a loan using default constructor\r\n        Loan defaultLoan = new Loan(); \r\n        System.out.println(\"Total due for default loan: \" + defaultLoan.calculateInterest());\r\n        \r\n        // Example of a custom loan using alternate constructor \r\n        Loan customLoan = new Loan(\"Wells Fargo\", 30000, 0.06, 12, 10); \r\n        System.out.println(\"Total due for custom loan: \" + customLoan.calculateInterest());\r\n        \r\n        // CreateDatabase and print out your own loan here: \r\n        \r\n          \r\n    }\r\n}\r\n";

        private IJavaReflector GetJavaReflector()
        {
            return new JavaReflector();
        }

        private List<JavaTestClass> OneTestFile(string dir, string text)
        {
            var javaFile = $"{ValidXmlCodeName}{JavaExtensions}";
            Directory.CreateDirectory(Path.Combine(dir, Package));
            var directory = Path.Combine(dir, Package);
            var loanTest = Path.Combine(directory, javaFile);
            WriteToFile(text, loanTest);
            WriteToFile(ValidLoanJavaCode, Path.Combine(directory, "Loan.java"));
            WriteToFile(ValidMainJavaCode, Path.Combine(directory, "Main.java"));
            new JavaCompiler2().Compile(directory, new List<string>()
            {
                loanTest,
                Path.Combine(directory, "Loan.java"),
                Path.Combine(directory, "Main.java"),
            });
            return new List<JavaTestClass>()
            {
                new JavaTestClass()
                {
                    Name = ValidXmlCodeName,
                    Package = Package,
                    PackageDirectory = PackageDirectory,
                }
            };
        }


        private void WriteToFile(string code, string name)
        {
            using (var fileStream = File.Open(name, FileMode.Append, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
            {
                fileWriter.WriteLine(code);
                fileWriter.Flush();
            }
        }

        public string GeneratedDirector()
        {
            return $"{WorkspacePath}{Thread.CurrentThread.ManagedThreadId}";
        }

        [Fact]
        public void ShouldBeValidStudentJavaCode()
        {
            var reflector = GetJavaReflector();
            var dir = GeneratedDirector();
            using (var workspace = new DisposableWorkspace(dir))
            {
                var java = OneTestFile(dir, ValidStudentLoanTestJavaCode);
                reflector.Reflect(dir, $"{dir}\\", ref java);

            }
        }

        [Fact]
        public void ShouldBeValidInstructorJavaCode()
        {
            var reflector = GetJavaReflector();
            var dir = GeneratedDirector();
            using (var workspace = new DisposableWorkspace(dir))
            {
                var java = OneTestFile(dir, ValidInstructorLoanTestJavaCode);
                reflector.Reflect(dir, $"{dir}\\", ref java);
            }
        }
        
    }
}