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
    public class AddModel : PageModel
    {
        private readonly IConfiguration Configuration;
        public AddModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void OnGet()
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<GetSalesList>("select * from SalesData");
                Console.Write(result.ToList());
            }
        }

        [HttpGet]
        //public IEnumerable<GetSalesList> GetSales() {
        //    var conn = new DapperConnections.ConnectionOptions();
        //    Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //    var sql = "select Sales from SalesData";
        //    using (var con=new SqlConnection(conn.RookieServerContext))
        //    {
        //        var result= con.Query<GetSalesList>(sql);
        //        return result;
        //    }
        //}


        [HttpPost]
        public IActionResult OnPostGetSales()
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = "select Sales from SalesData";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql).ToList();
                return new JsonResult(result);
            }
        }

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

        //public IActionResult OnPostGetCity()
        //{
        //    //var conn = new DapperConnections.ConnectionOptions();
        //    //Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //    //var sql = @"select City from CountryInfo where Country = @countries";
        //    //using (var con = new SqlConnection(conn.RookieServerContext))
        //    //{
        //    //    var result = con.Query<string>(sql,new {countries =new []
        //    //    {
        //    //        "USA","Japan","Spain"
        //    //    } }).ToList();
        //    //    return new JsonResult(result);
        //    //}

        //}


        public IActionResult OnPostGetCountryCity()
        {
            List<string> City = new List<string>();
            var results = new Dictionary<string, List<string>>();

            try
            {
                var conn = new DapperConnections.ConnectionOptions();
                Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
                var sql = "select Country,City from CountryInfo";
                
                using(var con=new SqlConnection(conn.RookieServerContext))
                {
                    var result=con.Query(sql).ToList();
                    var resultDict=result
                        .GroupBy(r=>r.Country,r=>r.City)
                        .ToDictionary(g=>g.Key,g=>g.ToList());

                    return new JsonResult(resultDict);
                }


            }
            catch (Exception ex)
            {

            }
            return new JsonResult(null);
        }

        //public IActionResult OnPostTest()
        //{
        //    var conn = new DapperConnections.ConnectionOptions();
        //    Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);

        //    var sql = "select Country from CountryInfo group by Country having count(*) > 1 ";
        //    using (var con = new SqlConnection(conn.RookieServerContext))
        //    {
        //        var countryResult = con.Query<string>(sql).ToList();
        //    }


        //}


    }
}
