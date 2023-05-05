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
    public class ModifyModel : PageModel
    {
        private readonly IConfiguration Configuration;
        public ModifyModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPostGetMember(IFormCollection formcollection)
        {
            var memberNo = formcollection["memberNo"].ToString();

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select * from UserInfo where MemberNo =  @membernumber";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query(sql, new
                {
                    membernumber = memberNo
                }).ToList();
                return new JsonResult(result);
            }

        }

        public IActionResult OnPostGetMemberSales(IFormCollection formcollection)
        {
            var memberNo = formcollection["memberNo"].ToString();

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select Sales from UserInfoMappingSales where MemberNo =  @membernumber";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql, new
                {
                    membernumber = memberNo
                }).ToList();
                return new JsonResult(result);
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

        public IActionResult OnPostUpdate(IFormCollection formCollection)
        {
            var country = formCollection["Country"];
            var city = formCollection["City"];
            var gender = formCollection["Gender"];
            var name = formCollection["Name"];

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"update UserInfo set Name=@Name,
                        Country=@Country,
                        City=@City,
                        Gender=@Gender,                                              
                        where Account = @Account";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {

                var result = con.Execute(sql, new
                {
                    Name = name,
                    Country = country,
                    City = city,
                    Gender = gender
                });
            }
            return new JsonResult(sql);
        }

        public IActionResult OnPostGetMemberInterest(IFormCollection formcollection)
        {
            var memberNo = formcollection["memberNo"].ToString();

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"select InterestItem from [UserInfoMappingInterest] where MemberNo =  @membernumber";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sql, new
                {
                    membernumber = memberNo
                }).ToList();
                return new JsonResult(result);
            }

        }


    }
}
