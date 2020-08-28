using System.IO;
using System.Linq;
using Xunit;

namespace TestingTutor.Tests.JavaEngine
{
    public class ZipUtilitiesTests : TestBase
    {


        [Fact]
        private void ConvertToByteArray()
        {
            var data = File.ReadAllBytes(@"C:\Temp\Questions.zip");
            //File.WriteAllText(@"C:\Temp\src.txt", data.ToString());
            var to = new int[data.Length];

            for (int i = 0; i < data.Length; to[i] = data[i++]) ;

            var joined = string.Join(",", to.Select(x => x.ToString()).ToArray());

            File.WriteAllText(@"C:\Temp\test.txt", joined.ToString());
        }

        //[Fact]
        //public void Testing()
        //{
        //    var result = ZipUtilities.UnZipToMemory(SrcBytes);
        //    var javaFiles = result.ToList().Where(f => f.Key.Contains(".java")).ToList();

        //    ////result.ToList().ForEach(f => File.WriteAllBytes(Path.Combine(@"c:\\Temp\\Test", f.Key.Substring(0, f.Key.Length-1)), f.Value.ToArray()));
        //    foreach (var file in javaFiles)
        //    {
        //        var filename = file.Key.Split(@"/");
        //        var name = filename[filename.Length - 1];
        //        File.WriteAllBytes(@"C:\Temp\Test\" + name, file.Value.GetBuffer());
        //    }
        //}
    }
}
