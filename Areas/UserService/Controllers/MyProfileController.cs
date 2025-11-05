using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using OP1_API.Areas.Login.Models;
using OP1_API.Areas.UserService.Models;
using OP1_API.ClassCollection;
using OP1_API.Models;

namespace OP1_API.Areas.UserService.Controllers
{
    [Route("user-service/")]
    [ApiController]
    [Authorize]
    public class MyProfileController : ControllerBase
    {
        GlobalClass cl;
        public MyProfileController(GlobalClass _cl) { 
            cl = _cl;
        }


        [Route("My-Profile")]
        [HttpPost]
        [Authorize]
        public MyProfileModel myProfile(we1plateformsMenuParam p) 
        {
            MyProfileModel pr = new MyProfileModel();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel r = cl.GetLiveUserDetails(authorizationHeader);

            string rjstr = @"select a.empid as empid, convert(varchar(12), a.empdate, 103) as empdate,convert(varchar(12),isnull(a.doj,''),103) as doj,ISNULL(a.payroletypeName,'') as payroletype, a.empName empfirstname, a.dob as dob,
a.maritalStatus as maritalstatus,
a.genderName as gender,
a.[languageName] as [language],a.bloodgroupname as bloodgrp,a.identificationMarks as identificationMarks,a.religion as religion,a.fathername as fathername, a.localphoneNo as lphone1,
a.localphoneNo as lphone2,a.pPhoneNo as pphone1,a.pPhoneNo as pphone2,a.email as email,a.offmailid as offmailid,a.localaddress as localaddress,
a.Statename as lstate,a.lcityName as lcity, 0 as lpin, a.permaddress as paddress,pstatename as pstate,pCityName as pcity, 0 as ppin,
a.nationlityName as nationality, a.docName3 as docName3,a.docName4 as docName4,a.AadharNo as docName5,a.panNo as docName6,a.doc1 as doc1,a.doc2 as doc2,
a.doc3 as doc3,a.doc4 as doc4,a.doc5 as doc5,a.doc6 as doc6,a.epic as epic,a.chequebook as chequebook,
a.bankname as bankname,a.bankbranch as bankbranch,isnull(a.AccHolderName,'')  as AccHolderName,a.accountno as accountno,a.ifsccode as ifsccode,a.departmentname as dept,a.designationname as designation,
a.gradename as grade,a.branchName as branch,a.pcompany as pcompany,a.pdesignaion as pdesignaion,a.[reson] as [reson],a.[pexperiance] as [pexperiance],
a.shiftName as [shift], 'WELPL' + convert(nvarchar(12), a.empid) as cardno,a.[st] as status
from view_employee as a where userid=@userid";
            DataTable dt= new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { new SqlParameter("@userid", r.userCode) }, r.finYear);
            if (dt.Rows.Count > 0)
            {
                pr.userid = r.userCode.ToString();
                pr.empid = dt.Rows[0]["empid"].ToString();
                pr.doj = dt.Rows[0]["doj"].ToString();
                pr.empfirstname = dt.Rows[0]["empfirstname"].ToString();
                pr.empdate = dt.Rows[0]["empdate"].ToString();
                pr.payroletype = dt.Rows[0]["payroletype"].ToString();
                pr.fathername = dt.Rows[0]["fathername"].ToString();
                pr.docName5 = dt.Rows[0]["docName5"].ToString();
                pr.docName6 = dt.Rows[0]["docName6"].ToString();
                pr.dob = dt.Rows[0]["dob"].ToString();
                pr.gender = dt.Rows[0]["gender"].ToString();
                pr.maritalstatus = dt.Rows[0]["maritalstatus"].ToString();
                pr.bloodgrp = dt.Rows[0]["bloodgrp"].ToString();
                pr.language = dt.Rows[0]["language"].ToString();
                pr.identificationMarks = dt.Rows[0]["identificationMarks"].ToString();
                pr.religion = dt.Rows[0]["religion"].ToString();
                pr.nationality = dt.Rows[0]["nationality"].ToString();
                pr.lphone1 = dt.Rows[0]["lphone1"].ToString();
                pr.lphone2 = dt.Rows[0]["pphone1"].ToString();
                pr.pphone1 = dt.Rows[0]["lphone1"].ToString();
                pr.pphone2 = dt.Rows[0]["pphone1"].ToString();
                pr.email = dt.Rows[0]["email"].ToString();
                pr.offmailid = dt.Rows[0]["offmailid"].ToString();
                pr.localaddress = dt.Rows[0]["localaddress"].ToString();
                pr.paddress = dt.Rows[0]["paddress"].ToString();
                pr.lstate = dt.Rows[0]["lstate"].ToString();
                pr.lcity = dt.Rows[0]["lcity"].ToString();
                pr.lpin = dt.Rows[0]["lpin"].ToString();

                pr.pstate = dt.Rows[0]["pstate"].ToString();
                pr.pcity = dt.Rows[0]["pcity"].ToString();
                pr.ppin = dt.Rows[0]["ppin"].ToString();
                
                pr.doc1 = dt.Rows[0]["doc1"].ToString();
                pr.doc2 = dt.Rows[0]["doc2"].ToString();
                pr.doc3 = dt.Rows[0]["doc3"].ToString();
                pr.doc4 = dt.Rows[0]["doc4"].ToString();
                pr.doc5 = dt.Rows[0]["doc5"].ToString();
                pr.doc6 = dt.Rows[0]["doc6"].ToString();
                
                pr.epic = dt.Rows[0]["epic"].ToString();
                
                pr.pcompany = dt.Rows[0]["pcompany"].ToString();
                pr.pdesignaion = dt.Rows[0]["pdesignaion"].ToString();
                pr.reson = dt.Rows[0]["reson"].ToString();
                pr.pexperiance = dt.Rows[0]["pexperiance"].ToString();
                
                pr.bankname = dt.Rows[0]["bankname"].ToString();
                pr.AccHolderName = dt.Rows[0]["AccHolderName"].ToString();
                pr.accountno = dt.Rows[0]["accountno"].ToString();
                pr.ifsccode = dt.Rows[0]["ifsccode"].ToString();
                pr.chequebook = dt.Rows[0]["chequebook"].ToString();
                pr.bankbranch = dt.Rows[0]["bankbranch"].ToString();
                
                pr.branch = dt.Rows[0]["branch"].ToString();
                pr.dept = dt.Rows[0]["dept"].ToString();
                pr.designation = dt.Rows[0]["designation"].ToString();
                pr.grade = dt.Rows[0]["grade"].ToString();
                pr.shift = dt.Rows[0]["shift"].ToString();
                pr.cardno = dt.Rows[0]["cardno"].ToString();
                pr.status = dt.Rows[0]["status"].ToString();
            }

            return pr;
        }


        [Route("update-Profile-detail")]
        [HttpPost]
        [Authorize]
        public ReturnResult updateProfileDetail(updateProfileDetailParam p) {
            ReturnResult ret = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel r = cl.GetLiveUserDetails(authorizationHeader);

            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTableWithProc("sp_updateEmployeeDetails", new SqlParameter[] {
            new SqlParameter("@tp", "1"),
            new SqlParameter("@empid", p.empid),
            new SqlParameter("@fathername", p.fatherName),
            new SqlParameter("@dob", p.dob),
            new SqlParameter("@gender", p.gender),
            new SqlParameter("@maritalStatus", p.maritalstatus),
            new SqlParameter("@bloodgrp", p.bloodGroup),
            new SqlParameter("@language", p.language),
            new SqlParameter("@identificationMarks", p.identityMarks),
            new SqlParameter("@aadharNo", p.aadharNo),
            new SqlParameter("@panNo", p.panNo),
            new SqlParameter("@nationality", p.nationality),
            new SqlParameter("@lphone2", p.ePhone),
            new SqlParameter("@email", p.pEmail),
            new SqlParameter("@offmailid", p.oEmail),
            new SqlParameter("@localaddress", p.lAddress),
            new SqlParameter("@lcity", p.lCity),
            new SqlParameter("@lstate", p.lstate),
            new SqlParameter("@lpin", p.lPin),
            new SqlParameter("@permaddress", p.pAddress),
            new SqlParameter("@pcity", p.pCity),
            new SqlParameter("@pstate", p.pstate),
            new SqlParameter("@ppin", p.pPin),
        }, r.finYear);

            if (dt.Rows.Count > 0)
            {
                ret.result = dt.Rows[0]["type"].ToString();
                ret.retval = dt.Rows[0]["des"].ToString();
                ret.msg = dt.Rows[0]["msg"].ToString();
            }
            else {
                ret.result = "FAILED";
                ret.retval = "0";
                ret.msg = "Profile Updation FAILED !";
            }

            return ret;
        }


        [Route("update-user-bank-detail")]
        [HttpPost]
        [Authorize]
        public ReturnResult updateUserBankDetail(updateUserBankDetailParam p)
        {
            ReturnResult ret = new ReturnResult();
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            LoginResultModel r = cl.GetLiveUserDetails(authorizationHeader);

            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTableWithProc("sp_updateEmployeeDetails", new SqlParameter[] {
            new SqlParameter("@tp", "2"),
            new SqlParameter("@empid", p.empid),
            new SqlParameter("@bankname", p.bankname),
            new SqlParameter("@AccHolderName", p.AccHolderName),
            new SqlParameter("@accountno", p.accountno),
            new SqlParameter("@ifsccode", p.ifsccode),
            new SqlParameter("@bankbranch", p.bankbranch)
        }, r.finYear);

            if (dt.Rows.Count > 0)
            {
                ret.result = dt.Rows[0]["type"].ToString();
                ret.retval = dt.Rows[0]["des"].ToString();
                ret.msg = dt.Rows[0]["msg"].ToString();
            }
            else
            {
                ret.result = "FAILED";
                ret.retval = "0";
                ret.msg = "Bank Detail Updation FAILED !";
            }
            return ret;
        }


        //[Route("upload-user-profile-pic")]
        //[HttpPost]
        //[Authorize]
        //public ReturnResult uploadUserProfilePic(IFormFile file) {
        //    ReturnResult ret = new ReturnResult();
        //    var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        //    LoginResultModel r = cl.GetLiveUserDetails(authorizationHeader);


        //    if (file == null || file.Length == 0)
        //    {
        //        ret.result = "FAILED";
        //        ret.retval = "0";
        //        ret.msg = "No File uploaded !";
        //    }
        //    else {
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        //        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        //        if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
        //        {
        //            ret.result = "FAILED";
        //            ret.retval = "0";
        //            ret.msg = "Invalid File Type !";
        //        }
        //        else {
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), _uploadFolder, file.FileName);

        //            // Save the file to the server
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //        }
                
        //    }

        //    return ret;
        //}



    }
}
