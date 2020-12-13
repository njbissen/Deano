using Deano.Azure.Entities;
using Deano.Models.Prototypes.Shared;
using Deano.Models.Prototypes.Users;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using prototypes = Deano.Models.Prototypes.Users;

namespace Deano.Azure
{
    public class Storage
    {
        public async Task<Catalog<prototypes.Library, prototypes.Report>> GetReportArchive(int? reportId)
        {
            IList<prototypes.Report> reports = new List<prototypes.Report>();
            //previousId = 0;
            //nextId = 0;
            //IQueryable<Deano.Data.Report> reports = data.GetReports(string.Empty, HttpContext.Current.User.Identity.Name);
            //return ReportTransformation.Transform(reports.AsQueryable(), null, null, string.Empty).ToList();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["storage"]);
            string reportsTableName = "reports";
            // Create a blob client for interacting with the blob service.
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(reportsTableName);
            TableQuery<ReportEntity> query = new TableQuery<ReportEntity>();

            // Print the fields for each customer.
            TableContinuationToken token = null;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (var entity in resultSegment.Results)
                {
                    reports.Add(new prototypes.Report()
                    {
                        Subject = entity.Subject,
                        Body = entity.Body,
                        CreatedBy = entity.CreatedBy,
                        CreatedDate = entity.CreatedDate.ToString()
                    });
                }
            } while (token != null);



            Catalog<prototypes.Library, prototypes.Report> catalog = new Catalog<prototypes.Library, prototypes.Report>();
            catalog.Header = new Library() { LibraryId = 2020, Title = "Library" };

            int? firstIndex = 0;
            int? selectedIndex = 0;
            int pageSize = 10;
            //  IQueryable<Deano.Data.Report> reports = data.GetReports(search, HttpContext.Current.User.Identity.Name);
            catalog.Items = reports.ToList(); // ReportTransformation.Transform(reports.AsQueryable(), yearId, monthId, string.Empty).ToList();
            catalog.Pager = new Pager(firstIndex.GetValueOrDefault(), catalog.Items.Count(), selectedIndex.GetValueOrDefault(), pageSize);
            catalog.Items = catalog.Items.Skip(((int)catalog.Pager.Available) * selectedIndex.GetValueOrDefault()).Take((int)catalog.Pager.Available).ToList();
           // catalog.Items.ForEach(m => m.Images.ForEach(p => p.GalleryPath = "/Pictures/gallery" + p.Name));
           // catalog.Items.ForEach(m => m.Images.ForEach(p => p.Path = "/Pictures/" + p.Name));
           // catalog.Items.ForEach(m => m.AllowBookmark = HttpContext.Current.Request.IsAuthenticated);
            catalog.Header.Reports = catalog.Items;
            return catalog;
        }
    }
}
