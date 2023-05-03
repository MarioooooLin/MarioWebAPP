using MarioWebAPP.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


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

        public IActionResult OnPostSearchResult(IFormCollection formCollection)
        {
            var beginDate = formCollection["beginDate"].ToString();
            var endDate = formCollection["endDate"].ToString();
            var serialNumber = formCollection["serialNumber"].ToString();
            var city = formCollection["selectedCity"];
            //var result=new Dictionary<string, object>();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"SELECT CreateDate,Name,Account,Country,City,Gender,MemberNo FROM UserInfo ";
            //@"SELECT CreateDate,Name,Account,Country,City,Gender,MemberNo FROM UserInfo where 1=1"  可不用寫下面的else部分
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(city))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += "AND City=@city";
                }
                else
                {
                    sql += " WHERE City = @city";
                }
                parameters.Add("@city", city);
            }
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " AND CreateDate BETWEEN @startDate AND @endDate";
                }
                else
                {
                    sql += " WHERE CreateDate BETWEEN @startDate AND @endDate";
                }
                parameters.Add("@startDate", beginDate);
                parameters.Add("@endDate", endDate);
            }
            if (!string.IsNullOrEmpty(serialNumber))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " AND SerialNumber LIKE @serialNumber";
                }
                else
                {
                    sql += " WHERE SerialNumber LIKE @serialNumber";
                }
                parameters.Add("@serialNumber", $"%{serialNumber}%");
            }

            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query(sql, parameters).ToList();
                //var result = con.Query<UserInfo>(sql, parameters).ToList();
                //result.FirstOrDefault().
                return new JsonResult(result);
            }
        }


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

        //public void Delete(IFormCollection formCollection)
        //{
        //    var memberNo = formCollection["MemberNo"];
        //    var conn = new DapperConnections.ConnectionOptions();
        //    Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //    var sql = @"DELETE FROM UserInfo WHERE MemberNo=@MemberNumber";
        //    using (var con = new SqlConnection(conn.RookieServerContext))
        //    {
        //        con.Execute(sql, new { MemberNumber = memberNo });
        //    }

        //}

        public void OnPostDelete(IFormCollection formcollection)
        {
            var mem = formcollection["MemberNo"].ToString();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"DELETE FROM UserInfo WHERE MemberNo=@MemberNumber";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                con.Execute(sql, new { MemberNumber = mem });
            }
        }
    }
}
