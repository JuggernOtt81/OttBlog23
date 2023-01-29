using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OttBlog23.Data;
using OttBlog23.Models;

[assembly: HostingStartup(typeof(OttBlog23.Areas.Identity.IdentityHostingStartup))]
namespace OttBlog23.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHRqVVhjVFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF5jSHxadkNiUXxXc3dSQg==;Mgo+DSMBPh8sVXJ0S0J+XE9HflRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS31Td0dqWXlfeHdcRWVdVQ==;ORg4AjUWIQA/Gnt2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkRhUH9YcXxWT2NbVUA=;MTAyMzc2M0AzMjMwMmUzNDJlMzBLN1lseEFZOTlnYUVyZ2kxTWRoak1vM2pCSk9XdXhlQjdOWVNnWGFpcEtBPQ==;MTAyMzc2NEAzMjMwMmUzNDJlMzBJelFtbVV5anJKUHQvY2R4MytCekw0MHJXTDYzOTN2R2dGck5kSy9wdUJ3PQ==;NRAiBiAaIQQuGjN/V0Z+WE9EaFxKVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdUViW3ZednVdRWlYVUJz;MTAyMzc2NkAzMjMwMmUzNDJlMzBRa3UvUnR6cFRRN1NsclIzUkFyckdrUlFOemdxdG5IaElaeVg5NlVRY3k0PQ==;MTAyMzc2N0AzMjMwMmUzNDJlMzBkU2hUdk50ZVNhb3VweDNUTmkvLy9DaFV4MGE2S2Q3SmdyUkR3NTVyc0w4PQ==;Mgo+DSMBMAY9C3t2VVhkQlFadVdJXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxQdkRhUH9YcXxWT2VeUUE=;MTAyMzc2OUAzMjMwMmUzNDJlMzBmcnYwMW8reXFqK1NhYWxsdGdRYWVmc2pXdHNyQ05TOC9oendRUXczemxVPQ==;MTAyMzc3MEAzMjMwMmUzNDJlMzBnQ2xOZW1xSlAxSGk3ekRDTFE1VURBRjFCamt2aTZYMVhySXkrTGt6biswPQ==;MTAyMzc3MUAzMjMwMmUzNDJlMzBRa3UvUnR6cFRRN1NsclIzUkFyckdrUlFOemdxdG5IaElaeVg5NlVRY3k0PQ==");
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}