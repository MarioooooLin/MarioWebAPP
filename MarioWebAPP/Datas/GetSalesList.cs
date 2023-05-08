namespace MarioWebAPP.Datas
{
    public class GetSalesList
    {
        public int RecordId { get; set; }
        public string Sales { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class UserInfo
    {
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public int MemberNo { get; set; }
    }

    public class Interest
    {
        public int RecordId { get; set; }
        public string MemberNo { get; set; }
        public DateTime UpdateTime { get; set; }

        public string UpdateBy { get; set; }

        public string InterestItem { get; set; }
    }

    public class SalesList
    {
        public int RecordId { get; set; }

        public string MemberNo { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Sales { get; set; }
        public string UpdateBy { get; set; }

    }

    public class UpdateData
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public string Sales { get; set; }
        public string InterestItem { get; set; }
        public string Remark { get; set; }
        public DateTime UpdateTime { get; set; }

        public string UpdateBy { get; set; }

        public string Account { get; set; }
    }
}