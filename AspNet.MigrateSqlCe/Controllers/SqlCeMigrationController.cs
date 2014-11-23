using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AspNet.MigrateSqlCe.Models;
using Newtonsoft.Json;

namespace AspNet.MigrateSqlCe.Controllers
{
    public class SqlCeMigrationController : ApiController
    {
        // /api/sqlcemigration/
        public async Task<HttpResponseMessage> Post()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            // generate a temp guid to place the request files into. 
            var requestId = Guid.NewGuid();

            // create the directory with the request id
            var requestDir = Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/App_Data/" + requestId));

            // create the data stream provider
            var provider = new MultipartFormDataStreamProvider(requestDir.FullName);

            try
            {
                // loads the provider with file data
                await Request.Content.ReadAsMultipartAsync(provider);

                // look for a file ending with .sdf
                var sdfFileData = provider.FileData.FirstOrDefault(f => f.Headers.ContentDisposition.FileName.Replace("\"", string.Empty).EndsWith(".sdf"));
                if (sdfFileData == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "A file ending in .sdf was not present.");
                }

                // use the local file path to the sdf for our data source
                var connString = string.Format("Data Source={0}", sdfFileData.LocalFileName);

                // create a new instance of SqlCeEngine so we can upgrade the database to 4.0
                using (var engine = new SqlCeEngine(connString))
                {
                    engine.Upgrade();
                }

                // declare a string to store the json output of the database
                var sdfJson = string.Empty;

                // connect to the database
                using (var conn = new SqlCeConnection(connString))
                {
                    // make sure you open!
                    conn.Open();

                    // create a new data context
                    using (var context = new CbmDataContext(conn))
                    {
                        // generate a json string from all the database's data 
                        // the Serializer settings will ignore any DataContext reference loops from object to object.
                        sdfJson = JsonConvert.SerializeObject(context, Formatting.None, new JsonSerializerSettings
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });
                    }
                }

                // delete the temp directory. if any exception occurs above, this won't be deleted for troubleshooting purposes.
                requestDir.Delete(true);

                // return the data
                return Request.CreateResponse(HttpStatusCode.OK, sdfJson);
            }
            catch (Exception e)
            {
                Trace.TraceError(requestId + " | " + e.Message + " - " + e.StackTrace);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}