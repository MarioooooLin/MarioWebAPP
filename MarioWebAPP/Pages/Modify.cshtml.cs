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
                    membernumber=memberNo
                }).ToList();
                return new JsonResult(result);
            }

        }
    }
}
