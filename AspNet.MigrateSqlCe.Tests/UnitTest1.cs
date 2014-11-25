using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace AspNet.MigrateSqlCe.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task MigrateCbmDatabaseTest()
        {
            var client = new RestClient("http://sqlcemigration.azurewebsites.net/");
            var request = new RestRequest("api/sqlcemigration/", Method.POST);

            var assemblyFileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var file = assemblyFileInfo.DirectoryName + "\\Checkbook.sdf";
            request.AddFile("Checkbook.sdf", file);

            var response = await client.ExecuteTaskAsync(request);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
