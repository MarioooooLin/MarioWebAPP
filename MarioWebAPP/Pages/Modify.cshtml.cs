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
            //            set Sales=@Sales,
            //            UpdateTime=@UpdateTime,
            //            UpdateBy=@UpdateBy,
            //            where MemberNo=@MemberNo";
            //var sql3 = @"update UserInfoMappingInterest
            //            set InterestItem=@InterestItem,
            //            UpdateTime=@UpdateTime,
            //            UpdateBy=@UpdateBy,
            //            where MemberNo=@MemberNo";

            var getSalesSql = @"select 
                                Sales
                                from [UserInfoMappingSales]
                                where MemberNo=@MemberNo";

            var getInterestSql = @"select
                                InterestItem
                                from [UserInfoMappingInterest]
                                where MemberNo=@MemberNo";

            var updateTime = DateTime.Now;
            var updateBy = "System";
            var oldSales = new List<string>();
            var oldInterest = new List<string>();
            //var memData = new List<string>();
            var memSales = new List<string>();
            var memInterest = new List<string>();

            //memData = JsonSerializer.Deserialize<List<string>>(formCollection["results"]);
            memSales = JsonSerializer.Deserialize<List<string>>(formCollection["Sales"]);
            memInterest = JsonSerializer.Deserialize<List<string>>(formCollection["Interest"]);

            var memDatas = new List<UpdateData>();

            //處理一般DATA的部分
            var memUpdate = new UpdateData()
            {
                Name = formCollection["Name"],
                Country = formCollection["Country"],
                City = formCollection["City"],
                Gender = formCollection["Gender"],
                Remark = formCollection["Remark"],
                UpdateTime = updateTime,
                UpdateBy = updateBy,
                Account = formCollection["Account"]
            };

            //處理SALES的部分
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(getSalesSql, new
                {
                    MemberNo = formCollection["MemberNo"],
                });

                oldSales = new List<string>(result);
            }

            var addSales = memSales.Except(oldSales).ToList();
            var deleSales = oldSales.Except(memSales).ToList();

            var insertSales = new List<SalesList>();
            var deleteSales = new List<SalesList>();

            for (var i = 0; i < addSales.Count; i++)
            {
                insertSales.Add(
                    new SalesList()
                    {
                        MemberNo = formCollection["MemberNo"],
                        UpdateBy = "System",
                        UpdateTime = updateTime,
                        Sales = addSales[i],
                    });
            }

            for (var i = 0; i < deleSales.Count; i++)
            {
                deleteSales.Add(
                    new SalesList()
                    {
                        MemberNo = formCollection["MemberNo"],
                        Sales = deleSales[i],
                    }
                    );
            }

            //處理INTEREST的部分
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(getInterestSql, new
                {
                    MemberNo = formCollection["MemberNo"],
                });
                oldInterest = new List<string>(result);
            }

            var addInter = memInterest.Except(oldInterest).ToList();
            var deleInter = oldInterest.Except(memInterest).ToList();

            var insertInter = new List<Interest>();
            var deleteInter = new List<Interest>();

            for (var i = 0; i < addInter.Count; i++)
            {
                insertInter.Add(
                    new Interest()
                    {
                        MemberNo = formCollection["MemberNo"],
                        UpdateBy = "System",
                        UpdateTime = updateTime,
                        InterestItem = addInter[i],
                    });
            }

            for (var i = 0; i < deleInter.Count; i++)
            {
                deleteInter.Add(new Interest()
                {
                    MemberNo = formCollection["MemberNo"],
                    InterestItem = deleInter[i],
                });
            }

            //SALE+INTEREST的SQL
            var sqlStrDeleteSales = @"delete from UserInfoMappingSales 
                                        where MemberNo=@MemberNo
                                        and Sales=@Sales";
            var sqlStrAddSales = @"insert into UserInfoMappingSales
                                (MemberNo,Sales,UpdateTime,UpdateBy) 
                                values 
                                (@MemberNo,@Sales,@UpdateTime,@UpdateBy)";

            var sqlStrDeleteInterest = @"delete from UserInfoMappingInterest 
                                        where MemberNo=@MemberNo
                                        and InterestItem=@InterestItem";
            var sqlStrAddInterest = @"insert into UserInfoMappingInterest
                                (MemberNo,InterestItem,UpdateTime,UpdateBy) 
                                values 
                                (@MemberNo,@InterestItem,@UpdateTime,@UpdateBy)";



            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                await con.ExecuteAsync(sql, memUpdate);
                await con.ExecuteAsync(sqlStrAddSales, insertSales);
                await con.ExecuteAsync(sqlStrDeleteSales, deleteSales);
                await con.ExecuteAsync(sqlStrAddInterest, insertInter);
                var result = await con.ExecuteAsync(sqlStrDeleteInterest, deleteInter);
                return new JsonResult(result);
            }
            return new OkResult();
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
