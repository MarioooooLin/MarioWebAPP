using MarioWebAPP.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;


namespace MarioWebAPP.Pages
{
    public class SearchModel : PageModel

    {
        private readonly IConfiguration Configuration;
        public SearchModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
        }

        //public IActionResult OnPostSearchResult()
        //{

        //}
        //public IActionResult OnPostGetSales()
        //{
        //    var conn = new DapperConnections.ConnectionOptions();
        //    Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //    var sql = "select Sales from SalesData";
        //    using (var con = new SqlConnection(conn.RookieServerContext))
        //    {
        //        var result = con.Query<string>(sql).ToList();
        //        return new JsonResult(result);
        //    }
        //}

        public IActionResult OnPostGetCountry()
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = "select Country from CountryInfo group by Country having count(*) > 1 ";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql).ToList();
                return new JsonResult(result);
            }
        }

        public IActionResult OnPostGetCity(IFormCollection countryCollection)
        {
            var selectCity = countryCollection["country"].ToString();

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select City from CountryInfo where Country = @countries";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql, new
                {
                    countries = selectCity
                }).ToList();
                return new JsonResult(result);
            }
        }
    }
}
