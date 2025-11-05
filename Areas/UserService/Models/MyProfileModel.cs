namespace OP1_API.Areas.UserService.Models
{
    public class MyProfileModel
    {
        public string? empid { get; set; }
        public string? userid { get; set; }
        public string? empdate { get; set; }
        public string? doj { get; set; }
        public string? payroletype { get; set; }
        public string? empfirstname { get; set; }
        public string? dob { get; set; }
        public string? maritalstatus { get; set; }
        public string? gender { get; set; }
        public string? language { get; set; }
        public string? bloodgrp { get; set; }
        public string? identificationMarks { get; set; }
        public string? religion { get; set; }
        public string? fathername { get; set; }
        public string? lphone1 { get; set; }
        public string? lphone2 { get; set; }
        public string? pphone1 { get; set; }
        public string? pphone2 { get; set; }
        public string? email { get; set; }
        public string? offmailid { get; set; }
        public string? localaddress { get; set; }
        public string? lstate { get; set; }
        public string? lcity { get; set; }
        public string? lpin { get; set; }
        public string? paddress { get; set; }
        public string? pstate { get; set; }
        public string? pcity { get; set; }
        public string? ppin { get; set; }
        public string? nationality { get; set; }
        public string? docName3 { get; set; }
        public string? docName4 { get; set; }
        public string? docName5 { get; set; }
        public string? docName6 { get; set; }
        public string? doc1 { get; set; }
        public string? doc2 { get; set; }
        public string? doc3 { get; set; }
        public string? doc4 { get; set; }
        public string? doc5 { get; set; }
        public string? doc6 { get; set; }
        public string? epic { get; set; }
        public string? chequebook { get; set; }
        public string? bankname { get; set; }
        public string? bankbranch { get; set; }
        public string? AccHolderName { get; set; }
        public string? accountno { get; set; }
        public string? ifsccode { get; set; }
        public string? dept { get; set; }
        public string? designation { get; set; }
        public string? grade { get; set; }
        public string? branch { get; set; }
        public string? pcompany { get; set; }
        public string? pdesignaion { get; set; }
        public string? reson { get; set; }
        public string? pexperiance { get; set; }
        public string? shift { get; set; }
        public string? cardno { get; set; }
        public string? status { get; set; }
    }



    public class updateProfileDetailParam
    {
        public string? empid { get; set; }
        public string? name { get; set; }
        public string? fatherName { get; set; }
        public string? dob { get; set; }
        public string? gender { get; set; }
        public string? maritalstatus { get; set; }
        public string? bloodGroup { get; set; }
        public string? language { get; set; }
        public string? identityMarks { get; set; }
        public string? aadharNo { get; set; }
        public string? panNo { get; set; }
        public string? nationality { get; set; }
        public string? ePhone { get; set; }
        public string? pEmail { get; set; }
        public string? oEmail { get; set; }
        public string? lAddress { get; set; }
        public string? lCity { get; set; }
        public string? lstate { get; set; }
        public string? lPin { get; set; }
        public string? pAddress { get; set; }
        public string? pCity { get; set; }
        public string? pstate { get; set; }
        public string? pPin { get; set; }
    }

    public class updateUserBankDetailParam
    {
        public string? empid { get; set; }
        public string? name { get; set; }
        public string? bankname { get; set; }
        public string? AccHolderName { get; set; }
        public string? accountno { get; set; }
        public string? ifsccode { get; set; }
        public string? bankbranch { get; set; }
    }
}
