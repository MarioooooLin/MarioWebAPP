namespace MarioWebAPP.Datas
{
    public class DapperConnections
    {
        public class ConnectionOptions
        {
            public const string Position = "ConnectionStrings";
            public string EmpServerContext { get; set; }
            public string RookieServerContext { get; set; }

            internal IDisposable BeginTransaction()
            {
                throw new NotImplementedException();
            }
            
        }

    }
}
