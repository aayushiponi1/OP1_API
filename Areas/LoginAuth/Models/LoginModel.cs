namespace OP1_API.Areas.Login.Models
{
    public class LoginModel
    {
    }

    public class userProfileparam
    {
        public string? sno { get; set; }
        public string? opGroup { get; set; }

    }


    public class loginDetailModel
    {
        public string? result { get; set; }
        public string? pwdChange { get; set; }
        public string? opgroup { get; set; }
        public string? sno { get; set; }
        public string? code { get; set; }
        public string? username { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? image { get; set; }
        public string? companyId { get; set; }
        public string? companyname { get; set; }
        public string? companyImg { get; set; }
        public string? companyAddress { get; set; }
        public string? companyPhone { get; set; }
        public string? companyEmail { get; set; }
        public string? companyGst { get; set; }
        public string? status { get; set; }


    }

    public class LoginResultModel
    {
        public string? result { get; set; }
        public string? retval { get; set; }
        public string? msg { get; set; }
        public string? token { get; set; }
        public string? userCode { get; set; }
        public string? userRole { get; set; }
        public string? finYear { get; set; }
        public string? userCompany { get; set; }

        public string? buyerCode { get; set; }
        public string? pwdChange { get; set; }
        public string? username { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? image { get; set; }
        public string? companyId { get; set; }
        public string? companyname { get; set; }
        public string? companyImg { get; set; }
        public string? companyAddress { get; set; }
        public string? companyPhone { get; set; }
        public string? companyEmail { get; set; }
        public string? companyGst { get; set; }
        public string? status { get; set; }
        public string? sdate { get; set; }
    }

    public class LoginParam 
    {
        public string? finYear { get; set; } = "1";
        public string? finYearName { get; set; } = "";
        public string? username { get; set; }
        public string? password { get; set; }
        public string? ipAddress { get; set; } = "";
        public int? remember { get; set; } = 1;
    }

    public class logoutModel { 
    public string? str { get; set; }
    }
}
