using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using MarioWebAPP.Datas;
using Microsoft.Data.SqlClient;

namespace MarioWebAPP.Pages.Test
{
    public class IndexModel : PageModel

    {
        private readonly IConfiguration Configuration;
        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void OnGet()
        {

            //var conn = new DapperConnections.ConnectionOptions();
            //Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            //using (var con = new SqlConnection(conn.EmpServerContext))
            //{
            //    var sqlStr = string.Format(@" SELECT[Dep_Id]
            //                              ,[Enable]
            //                              ,[Departmen_TC]
            //                              ,[Department]
            //                              ,[EMP_Id]
            //                              ,[EMP_Account]
            //                              ,[FST_Name]
            //                              ,[LST_Name]
            //                              ,[EMP_Name_TC]
            //                              ,[JobTitle_ID]
            //                              ,[JobTitle_TC]
            //                              ,[JobTitle]
            //                              ,[Office_phone]
            //                              ,[Email]
            //                              ,[Manager_Id]
            //                              ,[Manager2nd_Id]
            //                              ,[SystemRole]
            //                              ,[Office]
            //    FROM[EmpServer].[dbo].[EMP_Profile]");
                
            //    var tResults = con.Query(sqlStr, new { }).ToList();

            //}

            #region Test
            //    decimal InValue = 123;

            //    string x=InValue.ToString();

            //    string OutValue = "";
            //    OutValue = '0'+ x;
            //    Console.WriteLine(OutValue);

            //    List<string> y = new List<string>();
            //    y.Add("aa");
            //    y.Add("bb");
            //    y.Add("cc");
            //    y.Add("dd");
            //    y.Add("ee");

            //    for(int a = 0; a < y.Count; a++)
            //    {
            //        Console.WriteLine(y[a]);

            //    }


            //    int j = 0;
            //    int k = 0;

            //    for (int i = 2; i < 100; i++)
            //    {
            //        //2.3.5.7本身也是質數
            //        if (i % 2 == 0 || i % 3 == 0 || i % 5 == 0 || i % 7 == 0)
            //        {
            //            if(i == 2 || i == 3 || i == 5 || i == 7)
            //            {
            //                j += i;
            //                Console.WriteLine(j);
            //            }
            //            continue;
            //        }
            //        else
            //        {
            //            Console.WriteLine(i);
            //            j = i + j;
            //        }
            //    }

            //    Console.WriteLine(j);
            //    EMP_Info tEMP = new();
            //    tEMP = CheckEmployee("Mario");
            //    Console.WriteLine("Chinese Name:" + tEMP.ChineseName);
            //    Console.WriteLine(tEMP.EnglishName);

            //    for(int q = 0; q < 5; q++)
            //    {
            //        for (int e=0;e<5+1;e++)
            //        {
            //            Console.WriteLine("*");
            //        }
            //        Console.WriteLine("\n");
            //    }

            //}
            //public EMP_Info CheckEmployee(string myName)
            //{
            //    EMP_Info eMP_Info = new EMP_Info();

            //    if (myName == "Mario")
            //    {
            //        eMP_Info.ChineseName = "OOO";
            //        eMP_Info.EnglishName = "abc";

            //    }
            //    else
            //    {

            //    }
            //    return eMP_Info;
            //}
            //public class EMP_Info
            //{
            //    public string ChineseName { get; set; }
            //    public string EnglishName { get; set; }
            //    public int EmployeeNums { get; set; }
            //    public string Department { get; set; }
            //    public int Telephone { get; set; }
            //    public string JobTitle { get; set; }
            //    public string Email { get; set; }
            //}
            #endregion 
        }
        public IActionResult OnPostIndex()
        {

            var result = "test index";
            return new JsonResult(result);
        }
        //[HttpPost]
        public IActionResult OnPostTest1()
        {
                var conn = new DapperConnections.ConnectionOptions();
                Configuration.GetSection(DapperConnections.ConnectionOptions.Position).Bind(conn);
            var result=new Dictionary<string, object>();
           try
            {
                using (var con = new SqlConnection(conn.RookieServerContext))
                {
                    //    var sqlStr = string.Format(@" SELECT[Dep_Id]
                    //                          ,[Enable]
                    //                          ,[Departmen_TC]
                    //                          ,[Department]
                    //                          ,[EMP_Id]
                    //                          ,[EMP_Account]
                    //                          ,[FST_Name]
                    //                          ,[LST_Name]
                    //                          ,[EMP_Name_TC]
                    //                          ,[JobTitle_ID]
                    //                          ,[JobTitle_TC]
                    //                          ,[JobTitle]
                    //                          ,[Office_phone]
                    //                          ,[Email]
                    //                          ,[Manager_Id]
                    //                          ,[Manager2nd_Id]
                    //                          ,[SystemRole]
                    //                          ,[Office]
                    //FROM[EmpServer].[dbo].[EMP_Profile]");
                    var sqlStr = @"select [obj],[process] from [RookieServer].[dbo].[Table_2] ";
                    var sesult = con.Query<string>(sqlStr, new { tEnable = "Active"}).ToList();

                }
            }
            catch { }

            return new JsonResult(result);
        }

    }
}
