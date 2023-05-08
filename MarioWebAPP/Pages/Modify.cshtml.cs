using MarioWebAPP.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.Json;

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

        public async Task<IActionResult> OnPostUpdate(IFormCollection formCollection)
        {

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"update UserInfo set Name=@Name,
                        Country=@Country,
                        City=@City,
                        Gender=@Gender,  
                        Remark=@Remark,
                        UpdateTime=@UpdateTime,
                        UpdateBy=@UpdateBy
                        where Account = @Account";
            //var sql2 = @"update UserInfoMappingSales 
            //            set Sales=@Sales
            //            UpdateTime=@UpdateTime
            //            UpdateBy=@UpdateBy
            //            wherer MemberNo=@MemberNo";
            var sql2 = @"update UserInfoMappingSales 
                        set Sales=@Sales,
                        UpdateTime=@UpdateTime,
                        UpdateBy=@UpdateBy,
                        where MemberNo=@MemberNo";
            var sql3 = @"update UserInfoMappingInterest
                        set InterestItem=@InterestItem,
                        UpdateTime=@UpdateTime,
                        UpdateBy=@UpdateBy,
                        where MemberNo=@MemberNo";

            var updateTime = DateTime.Now;
            var updateBy = "System";
            //var memData = new List<string>();
            //var memSales = new List<string>();
            //var memInterest = new List<string>();

            //memData = JsonSerializer.Deserialize<List<string>>(formCollection["results"]);
            //memSales = JsonSerializer.Deserialize<List<string>>(formCollection["Sales"]);
            //memInterest = JsonSerializer.Deserialize<List<string>>(formCollection["Interest"]);

            var memDatas=new List<UpdateData>();

            var memUpdate = new UpdateData()
            {
                Name = formCollection["Name"],
                Country = formCollection["Country"],
                City= formCollection["City"],
                Gender= formCollection["Gender"],
                Remark= formCollection["Remark"],
                UpdateTime=updateTime,
                UpdateBy=updateBy,
                Account = formCollection["Account"]
            };


            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = await con.ExecuteAsync(sql, memUpdate);
                ////var result = con.Execute(sql, memUpdate);
                return new JsonResult(result);

                //var result = await con.ExecuteAsync(sql,new
                //{
                //    Name= formCollection["Name"],
                //    Country = formCollection["Country"],
                //    City = formCollection["City"],
                //    Gender = formCollection["Gender"],
                //    Remark = formCollection["Remark"],
                //    UpdateTime=updateTime,
                //    UpdateBy=updateBy,
                //    Account = formCollection["Account"]
                //});
            }
            //return new OkResult();
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
