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
            var sales = formCollection["sales"];
            var account = formCollection["account"];
            var country = formCollection["selectedCountry"];
            //var result=new Dictionary<string, object>();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"SELECT a.CreateDate,a.Name,a.Account,a.Country,a.City,a.Gender,a.MemberNo FROM UserInfo as a ";
            //var sql2 = @"select ";
            //@"SELECT CreateDate,Name,Account,Country,City,Gender,MemberNo FROM UserInfo where 1=1"  ぃノ糶else场だ
            var parameters = new DynamicParameters();

            var results=new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(sales))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " left join UserInfoMappingSales as b on a.MemberNo=b.MemberNo where Sales =@sales ";
                }
                else{
                    sql += " left join UserInfoMappingSales as b on a.MemberNo=b.MemberNo where Sales =@sales ";

                }
                parameters.Add("@sales", sales);

            }
            if (!string.IsNullOrEmpty(account))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " AND Account=@account";
                }
                else //硂场だ
                {
                    sql += " WHERE Account = @account";
                }
                parameters.Add("@account", account);
            }


            if (!string.IsNullOrEmpty(city))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += "AND City=@city ";
                }
                else //硂场だ
                {
                    sql += " WHERE City = @city ";
                }
                parameters.Add("@city", city);
            }

            if (!string.IsNullOrEmpty(country))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += "AND Country=@country ";
                }
                else //硂场だ
                {
                    sql += " WHERE Country = @country ";
                }
                parameters.Add("@country", country);
            }
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " AND CreateDate >=DATEADD(DAY,DATEDIFF(day,'',@startDate),'')  AND CreateDate <= DATEADD(day,DATEDIFF(day,'',@endDate)+1,'') ";
                }
                else  //硂场だ
                {
                    sql += " WHERE CreateDate >= DATEADD(DAY,DATEDIFF(day,'',@startDate),'') AND CreateDate <= DATEADD(day,DATEDIFF(day,'',@endDate)+1,'')";
                }
                parameters.Add("@startDate", beginDate);
                parameters.Add("@endDate", endDate);
            }
            if (!string.IsNullOrEmpty(serialNumber))
            {
                if (parameters.ParameterNames.Any())
                {
                    sql += " AND a.MemberNo LIKE @serialNumber ";
                }
                else //硂场だ
                {
                    sql += " WHERE a.MemberNo LIKE @serialNumber ";
                }
                parameters.Add("@serialNumber", $"%{serialNumber}%");
            }


            using (var con = new SqlConnection(conn.RookieServerContext))
            {

                var result = con.Query(sql, parameters).ToList();
                if (result.Count == 0)
                {
                    results.Add("result", "0");
                }
                else
                {
                    results.Add("result", "1");
                    results.Add("data",result.ToArray());   
                }
                return new JsonResult(results);
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

        public void OnPostDelete(IFormCollection formcollection)
        {
            var mem = formcollection["MemberNo"].ToString();
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"DELETE FROM UserInfo WHERE MemberNo=@MemberNumber;
                        delete from UserInfoMappingSales where MemberNo=@MemberNumber;
                        delete from UserInfoMappingInterest where MemberNo=@MemberNumber";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                con.Execute(sql, new { MemberNumber = mem });
            }
        }

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

    }
}
