using MarioWebAPP.Datas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

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
            string queryResult = GetSqlQuery("query1");
            Console.WriteLine(queryResult);

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<GetSalesList>("select * from SalesData");
                Console.Write(result.ToList());
            }
        }

        public string GetSqlQuery(string x)
        {
            string sqlQuery = string.Empty;
            //string connectionString = ""; 

            switch (x)
            {
                case "GetSales":
                    sqlQuery = "select Sales from SalesData";
                    break;
                case "GetCountry":
                    sqlQuery = "select Country from CountryInfo group by Country having count(*) > 1 ";
                    break;
                case "GetCity":
                    sqlQuery = "select City from CountryInfo where Country = @countries";
                    break;
                case "SubmitUser":
                    sqlQuery = @"insert into UserInfo (MemberNo,CreateDate,Name,Account,Country,City,Gender,Remark,UpdateTime,UpdateBy) 
                                values                 
                                (@MemberNo,@CreateDate,@Name,@Account,@Country,@City,@Gender,@Remark,@UpdateTime,@UpdateBy)";
                    break;
                case "SubmitInterest":
                    sqlQuery = @"insert into UserInfoMappingInterest
                                (MemberNo,UpdateTime,UpdateBy,InterestItem) 
                                values 
                                (@MemberNo,@UpdateTime,@UpdateBy,@InterestItem)";
                    break;
                case "SubmitSales":
                    sqlQuery = @"insert into UserInfoMappingSales 
                                (MemberNo,Sales,UpdateTime,UpdateBy) 
                                values 
                                (@MemberNo,@Sales,@UpdateTime,@UpdateBy)";
                    break;
                case "SetID":
                    sqlQuery = @"select max(cast(substring(MemberNo,12,3) as int)) from UserInfo where MemberNo like @Prefix";
                    break;

                default:
                    return "Query not found.";
            }
            return sqlQuery;
        }


            [HttpPost]
        public IActionResult OnPostGetSales()
        {
            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sqlTest= GetSqlQuery("GetSales");
            //var sql = "select Sales from SalesData";
            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var result = con.Query<string>(sqlTest).ToList();
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


        public async Task<IActionResult> OnPostSubmit(IFormCollection memberCollection)
        {
            //NEW
            var results=new Dictionary<string, string>();

            var conn = new DapperConnections.ConnectionOptions();
            Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var sql = @"insert into UserInfo 
                (MemberNo,CreateDate,Name,Account,Country,City,Gender,Remark,UpdateTime,UpdateBy) 
                values 
                (@MemberNo,@CreateDate,@Name,@Account,@Country,@City,@Gender,@Remark,@UpdateTime,@UpdateBy)";
            var sql2 =
                @"insert into UserInfoMappingSales 
                (MemberNo,Sales,UpdateTime,UpdateBy) 
                values 
                (@MemberNo,@Sales,@UpdateTime,@UpdateBy)";
            var sql3 =
                @"insert into UserInfoMappingInterest
                (MemberNo,UpdateTime,UpdateBy,InterestItem) 
                values 
                (@MemberNo,@UpdateTime,@UpdateBy,@InterestItem)";
            var success = "";

            using (var con = new SqlConnection(conn.RookieServerContext))
            {
                var curDate = DateTime.Now.ToString("yyyyMMdd");
                var createDate = DateTime.Now;
                var updateTime = DateTime.Now;
                var maxID = await con.ExecuteScalarAsync<int?>("select max(cast(substring(MemberNo,12,3) as int)) from UserInfo where MemberNo like @Prefix", new { Prefix = $"Mem{curDate}%" }) ?? 0;
                var newSequence = (maxID + 1).ToString().PadLeft(3, '0');
                var newId = $"Mem{curDate}{newSequence}";
                //var Name = memberCollection["Name"].ToString();
                var SalesList = new List<string>();
                var InterestedList = new List<string>();
                SalesList = JsonSerializer.Deserialize<List<string>>(memberCollection["salesList"]);
                InterestedList = JsonSerializer.Deserialize<List<string>>(memberCollection["interested"]);

                var sales = new List<SalesList>();
                for (int i = 0; i < SalesList.Count; i++)
                {
                    sales.Add(new SalesList()
                    {
                        MemberNo = newId,
                        UpdateBy = "System",
                        UpdateTime = createDate,
                        Sales = SalesList[i]
                    });
                }

                var interest = new List<Interest>();
                for (int i = 0; i < InterestedList.Count; i++)
                {
                    interest.Add(new Interest()
                    {
                        MemberNo = newId,
                        UpdateBy = "System",
                        UpdateTime = createDate,
                        InterestItem = InterestedList[i]
                    });
                }

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
                    var result = await con.ExecuteAsync(sql2, sales);
                    var result1 = await con.ExecuteAsync(sql3, interest);

                    results.Add("result", "1");
                    
                }
                else
                {
                    //return new BadRequestResult();
                    results.Add("result", "0");
                }
            }
            
            return new JsonResult(results);
        }




    }
}

