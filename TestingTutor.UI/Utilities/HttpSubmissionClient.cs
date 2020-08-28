using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestingTutor.EngineModels;

namespace TestingTutor.UI.Utilities
{
    public class HttpSubmissionClient
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<Uri> SubmitEngineJobAsync(string uri, SubmissionDto submissionDto)
        {
            //http://localhost:49829/
            var response = await Client.PostAsJsonAsync(uri, submissionDto);
            //var response = await Client.PostAsJsonAsync("http://localhost:49824/api/Submissions", submissionDto);
            response.EnsureSuccessStatusCode();
            return response.Headers.Location;
        }

        public async Task SubmitJavaEngineJobAsync(string uri, SubmissionDto submissionDto)
        {

            var submissionXml = Serialize<SubmissionDto>(submissionDto);

            var response = PostXMLData(uri, submissionXml);

            //var response = await Client.PostAsXmlAsync(uri, submissionDto);
            //return response.Headers.Location;
        }

        public static string Serialize<T>(T dataToSerialize)
        {
            try
            {
                var stringWriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, dataToSerialize);
                return stringWriter.ToString();
            }
            catch
            {
                throw;
            }
        }

        public static T Deserialize<T>(string xmlText)
        {
            try
            {
                var stringReader = new System.IO.StringReader(xmlText);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
            catch
            {
                throw;
            }
        }

        public string PostXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create(destinationUrl);
                var bytes = System.Text.Encoding.Unicode.GetBytes(requestXml);

                request.ContentType = "application/xml; encoding='utf-16'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";

                var requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();

                using (var response = (HttpWebResponse) request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK) return null;

                    var responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream ?? throw new InvalidOperationException()).ReadToEnd();
                }
                   
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed " + ex.ToString());
            }

            return null;
        }

    }
}
