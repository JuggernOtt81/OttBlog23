using OttBlog23;
using OttBlog23.Data;
using OttBlog23.Helpers;
using OttBlog23.Models;
using OttBlog23.Services;
using OttBlog23.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;

namespace OttBlog23.Helpers
{
    public static class ConnectionHelper
    {
        public static string GetConnectionString(string connectionString, string databaseUrl)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            //var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);

        }
    
        //build the connection string from the environment. i.e. Heroku
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }
    }
}
