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


        //public IActionResult OnPostGetCountryCity()
        //{
        //    List<string> City = new List<string>();
        //    var results = new Dictionary<string, List<string>>();

        //    try
        //    {
        //        var conn = new DapperConnections.ConnectionOptions();
        //        Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
        //        var sql = "SELECT Country, City FROM CountryInfo";

        //        using (var con = new SqlConnection(conn.RookieServerContext))
        //        {
        //            var countryCityResult = con.Query(sql).ToList();
        //            var result = new List<object>();

        //            foreach (var item in countryCityResult.GroupBy(x => x.Country))
        //            {
        //                var country = item.Key;
        //                var cities = item.Select(x => x.City).ToList();
        //                var obj = new { name = country, city = cities };
        //                result.Add(obj);
        //            }

        //            return new JsonResult(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return new JsonResult(null);
        //}

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

        public async Task<IActionResult> OnPostSubmit(IFormCollection memberCollection)
        {

            //NEW
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql =
                @"insert into UserInfo 
                (MemberNo,CreateDate,Name,Account,Country,City,Gender,Remark,UpdateTime,UpdateBy) 
                values 
                (@MemberNo,@CreateDate,@Name,@Account,@Country,@City,@Gender,@Remark,@UpdateTime,@UpdateBy)";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var curDate = DateTime.Now.ToString("yyyyMMdd");
                var createDate = DateTime.Now;
                var updateTime = DateTime.Now;
                var maxID = await con.ExecuteScalarAsync<int?>("select max(cast(substring(MemberNo,12,3) as int)) from UserInfo where MemberNo like @Prefix", new { Prefix = $"Mem{curDate}%" }) ?? 0;

                var newSequence = (maxID + 1).ToString().PadLeft(3, '0');
                var newId = $"Mem{curDate}{newSequence}";
                var rowsAffected = await con.ExecuteAsync(sql, new
                {
                    MemberNo = newId,
                    CreateDate = createDate,
                    Name = memberCollection["Name"].ToString(),
                    Account = memberCollection["Account"].ToString(),
                    Country = memberCollection["Country"].ToString(),
                    City = memberCollection["City"].ToString(),
                    Gender = memberCollection["Gender"].ToString(),
                    Remark = memberCollection["Remark"].ToString(),
                    UpdateTime = updateTime,
                    UpdateBy = "System"
                });
                if (rowsAffected == 1)
                {
                    return new OkResult();
                }
                else
                {
                    return new BadRequestResult();
                }
            }

        }




        }
    }

