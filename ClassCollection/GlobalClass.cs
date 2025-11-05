using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using System.Net.Mail;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using OP1_API.ClassCollection;
using OP1_API.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Text.Json;
using OP1_API.Areas.Login.Models;
using System.Security.Policy;
using System.Security.Cryptography.Xml;
//using Newtonsoft.Json;
/// <summary>
/// Summary description for GlobalClass
/// </summary>

namespace OP1_API.ClassCollection
{
    public class GlobalClass
    {

        private readonly IHostEnvironment _hostEnvironment;
        private readonly IWebHostEnvironment _env;
        public GlobalClass(IHostEnvironment hostEnvironment, IWebHostEnvironment env)
        {
            _hostEnvironment = hostEnvironment;
            _env = env;
        }


        IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        IHttpContextAccessor _Session = new HttpContextAccessor();

        public readonly IHttpContextAccessor? contxt;
        CookieOptions cookies = new CookieOptions();

        string rjstr;
        //public SqlConnection Con = new SqlConnection();
        JsonToDataTableService jsnTable = new JsonToDataTableService();
        public string SqlConString = "";

        SqlConnection Con = new SqlConnection();
        public GlobalClass()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string documentsUrl()
        {
            return "https://api.we1.tech/";
        }

        public string postImageUrl()
        {
            var rootPath = _env.ContentRootPath; // Application root path
            var webRootPath = _env.WebRootPath;   // wwwroot path

            string currentPath = webRootPath + "/";
            return currentPath;
        }
        public string oldImageUrl()
        {
            return "https://welpl.we1.tech/";
        }

        public LoginResultModel GetLiveUserDetails(string authorizationHeader)
        {
            LoginResultModel r = new LoginResultModel();
            try
            {
                string token = "";
                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorizationHeader.Substring("Bearer ".Length).Trim().ToString();

                    string headerJson = ""; string payloadJson = "";
                    var parts = token.Split('.');
                    if (parts.Length != 3)
                    {
                        r.result = "FAILED";
                        r.retval = "0";
                        r.msg = "";
                        r.token = "";
                        r.userCode = "";
                        r.userRole = "0";
                        r.finYear = "0";
                        r.userCompany = "0";
                    }
                    else
                    {
                        // Decode the header (Base64URL to JSON string)
                        headerJson = jsnTable.Base64UrlDecode(parts[0]);
                        payloadJson = jsnTable.Base64UrlDecode(parts[1]);

                        getTokenHeaderModel h = JsonConvert.DeserializeObject<getTokenHeaderModel>(headerJson);
                        getTokenPayloadModel p = JsonConvert.DeserializeObject<getTokenPayloadModel>(payloadJson);

                        r.result = p.result;
                        r.retval = p.retval;
                        r.msg = p.msg;
                        r.token = token;
                        r.userCode = p.userCode;
                        r.userRole = p.userRole;
                        r.finYear = p.finYear;
                        r.userCompany = p.userCompany;
                    }
                }
                else
                {
                    r.result = "FAILED";
                    r.retval = "0";
                    r.msg = "";
                    r.token = "";
                    r.userCode = "0";
                    r.userRole = "0";
                    r.finYear = "0";
                    r.userCompany = "0";
                }
            }
            catch (Exception ex)
            {
                r.result = "FAILED";
                r.retval = "0";
                r.msg = "";
                r.token = "";
                r.userCode = "0";
                r.userRole = "0";
                r.finYear = "0";
                r.userCompany = "0";
            }
            return r;
        }

        public string getConnectionString(string finYear)
        {
            string sno = "0";
            sno = finYear;// CheckfinYear();
            sno = rj_encryptJson(sno);
            //DataTable dt = new DataTable();
            List<GetFinYearWithConnection_model> conLst = new List<GetFinYearWithConnection_model>();
            conLst = jsnTable.GetFinYearWithConnection();

            var conFltLst = conLst.AsQueryable();
            if (!string.IsNullOrEmpty(sno))
            {
                conFltLst = conFltLst.Where(p => p.sno.Contains(sno, StringComparison.OrdinalIgnoreCase));
            }
            conLst = new List<GetFinYearWithConnection_model>();
            conLst = conFltLst.ToList();

            string server = rj_decryptJson(conLst[0].Server.ToString());
            string database = rj_decryptJson(conLst[0].Database.ToString());
            string userid = rj_decryptJson(conLst[0].User_Id.ToString());
            string password = rj_decryptJson(conLst[0].Password.ToString());

            SqlConString = "Server=" + server + ";Database=" + database + ";User Id=" + userid + ";Password=" + password + ";";

            //DataView dv = dt.DefaultView;
            //dv.RowFilter = "sno='" + sno + "'";
            //dt = dv.ToTable();

            //dv.ToTable(true, "sno");

            //if (dt.Rows.Count > 0)
            //{
            //    string server = rj_decryptJson(dt.Rows[0]["Server"].ToString());
            //    string database = rj_decryptJson(dt.Rows[0]["Database"].ToString());
            //    string userid = rj_decryptJson(dt.Rows[0]["User_Id"].ToString());
            //    string password = rj_decryptJson(dt.Rows[0]["Password"].ToString());
            //    SqlConString = "Server=" + server + ";Database=" + database + ";User Id=" + userid + ";Password=" + password + ";";
            //}
            return SqlConString;
        }


        //public DataTable GetFinYearWithConnection()
        //{
        //    JsonToDataTableService jDt = new JsonToDataTableService();
        //    DataTable dt = new DataTable();
        //    List<connectionStringModel> conList = new List<connectionStringModel>();

        //    conList = jDt.GetFinYearListwithConString();
        //    return dt;
        //}

        public DataTable Rj_LoadTableWithProc(string ProcName, SqlParameter[] param, string finYear)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConString = getConnectionString(finYear);
                using (SqlDataAdapter da = new SqlDataAdapter(ProcName, SqlConString))
                {
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.Clear();
                    if (param != null)
                    {
                        foreach (SqlParameter prm in param)
                        {
                            da.SelectCommand.Parameters.Add(prm);
                        }
                    }
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }


        public DataTable Rj_LoadTable(string query, string finYear)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConString = getConnectionString(finYear);
                using (SqlDataAdapter da = new SqlDataAdapter(query, SqlConString))
                {
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }


        }


        public DataTable Rj_LoadTable(string query, SqlParameter[] param, string finYear)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConString = getConnectionString(finYear);
                using (SqlDataAdapter da = new SqlDataAdapter(query, SqlConString))
                {
                    da.SelectCommand.CommandType = CommandType.Text;
                    da.SelectCommand.Parameters.Clear();
                    if (param != null)
                    {
                        foreach (SqlParameter prm in param)
                        {
                            da.SelectCommand.Parameters.Add(prm);
                        }
                    }
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
        }


        public string rj_Exequet(string str, string finYear)
        {
            string ret = "";
            SqlConString = getConnectionString(finYear);
            Con = new SqlConnection(SqlConString);
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand(str, Con);
                cmd.ExecuteNonQuery();
                Con.Close();
                ret = "OK";
            }
            catch (Exception ex)
            {
                Con.Close();
                ret = ex.ToString();
            }
            return ret;
        }


        public List<TwoFieldModel> Rj_LoadDropdownlist(string str, string finYear, string Remarks = "")
        {
            List<TwoFieldModel> lst = new List<TwoFieldModel>();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            SqlConString = getConnectionString(finYear);
            da = new SqlDataAdapter(str, SqlConString);
            da.Fill(dt);
            if (Remarks != "")
            {
                lst.Add(new TwoFieldModel
                {
                    Code = 0,
                    Name = Remarks
                });
            }
            else
            {
                lst.Add(new TwoFieldModel
                {
                    Code = 0,
                    Name = "< -- Select -- >"
                });
            }

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["sno"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return lst;
        }

        public List<TwoFieldModel> Rj_LoadDropdownlistwithDt(DataTable dt, string Remarks = "")
        {
            List<TwoFieldModel> lst = new List<TwoFieldModel>();
            if (Remarks != "")
            {
                lst.Add(new TwoFieldModel
                {
                    Code = 0,
                    Name = Remarks
                });
            }
            else
            {
                lst.Add(new TwoFieldModel
                {
                    Code = 0,
                    Name = "< -- Select -- >"
                });
            }

            foreach (DataRow dr in dt.Rows)
            {
                lst.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["sno"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return lst;
        }


        public string checkReLogin()
        {
            GlobalClass cl = new GlobalClass();
            string r = "OK";
            string str = "";
            str = cl.CheckfinYear_();
            if (str == "") { r = ""; return r; } else { r = "OK"; }
            str = cl.checkUser_();
            if (str == "") { r = ""; return r; } else { r = "OK"; }
            str = cl.CheckGroup_();
            if (str == "") { r = ""; return r; } else { r = "OK"; }
            //str = cl.checkUserWithAdmin_();
            //if (str == "") { r = ""; return r; } else { r = "OK"; }
            return r;
        }

        //public string CheckfinYear()
        //{
        //    string finYearSno = "";
        //    try
        //    {
        //        if (HttpContext.Current.Session["finYear"] == null || HttpContext.Current.Session["finYear"].ToString() == "")
        //        {
        //            rj_userLogin = HttpContext.Current.Request.Cookies["rj_userLogin"];
        //            if (rj_userLogin != null)
        //            {
        //                if (rj_userLogin["finYear"].ToString() != "")
        //                {
        //                    finYearSno = rj_userLogin["finYear"].ToString();
        //                    HttpContext.Current.Session["finYear"] = finYearSno;
        //                }
        //                else
        //                {
        //                    HttpContext.Current.Response.Redirect("~/Admin/Login.aspx?TP=LOGOUT", true);
        //                }
        //            }
        //            else
        //            {
        //                HttpContext.Current.Response.Redirect("~/Admin/Login.aspx?TP=LOGOUT", true);
        //            }
        //        }
        //        else
        //        {
        //            finYearSno = HttpContext.Current.Session["finYear"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Response.Redirect("~/Admin/Login.aspx?TP=LOGOUT", true);
        //    }
        //    return finYearSno;
        //}

        public string CheckfinYear()
        {

            string finYearSno = "";
            var fin = _Session.HttpContext.Session.GetString("finYear");
            var rj_userLogin = _Session.HttpContext.Request.Cookies["finYear"];
            try
            {
                if (fin != null && fin.ToString() != "")
                {
                    finYearSno = fin.ToString();
                }
                else
                {
                    if (rj_userLogin != null)
                    {
                        fin = rj_userLogin;
                        var finName = _Session.HttpContext.Request.Cookies["finYearName"];
                        if (fin != null && fin.ToString() != "")
                        {
                            finYearSno = fin.ToString();
                            _Session.HttpContext.Session.SetString("finYear", fin.ToString());
                            _Session.HttpContext.Session.SetString("finYearName", finName.ToString());
                        }
                        else
                        {
                            // Perform the redirect by manually setting the response
                            _httpContextAccessor.HttpContext.Response.Redirect("/");

                        }
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Response.Redirect("/");
                    }
                }
            }
            catch (Exception ex)
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/");
            }
            return finYearSno;
        }
        public string CheckfinYear_()
        {
            string finYearSno = "";
            var fin = _Session.HttpContext.Session.GetString("finYear");
            var rj_userLogin = _Session.HttpContext.Request.Cookies["finYear"];
            try
            {
                if (fin != null && fin.ToString() != "")
                {
                    finYearSno = fin.ToString();
                }
                else
                {
                    if (rj_userLogin != null)
                    {
                        fin = rj_userLogin;
                        var finName = _Session.HttpContext.Request.Cookies["finYearName"];
                        if (fin != null && fin.ToString() != "")
                        {
                            finYearSno = fin.ToString();
                            _Session.HttpContext.Session.SetString("finYear", fin.ToString());
                            _Session.HttpContext.Session.SetString("finYearName", finName.ToString());
                        }
                        else
                        {
                            // Perform the redirect by manually setting the response
                            finYearSno = "";

                        }
                    }
                    else
                    {
                        finYearSno = "";
                    }
                }
            }
            catch (Exception ex)
            {
                finYearSno = "";
            }
            return finYearSno;
        }

        //public string CheckfinYear_()
        //{
        //    string finYearSno = "";
        //    try
        //    {
        //        if (HttpContext.Current.Session["finYear"] == null || HttpContext.Current.Session["finYear"].ToString() == "")
        //        {
        //            rj_userLogin = HttpContext.Current.Request.Cookies["rj_userLogin"];
        //            if (rj_userLogin != null)
        //            {
        //                if (rj_userLogin["finYear"].ToString() != "")
        //                {
        //                    finYearSno = rj_userLogin["finYear"].ToString();
        //                    HttpContext.Current.Session["finYear"] = finYearSno;
        //                }
        //                else
        //                {
        //                    finYearSno = "";
        //                }
        //            }
        //            else
        //            {
        //                finYearSno = "";
        //            }
        //        }
        //        else
        //        {
        //            finYearSno = HttpContext.Current.Session["finYear"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        finYearSno = "";
        //    }
        //    return finYearSno;
        //}


        public string CheckGroup()
        {

            string opGroup = "";
            if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
            {
                var rj_userLogin = _Session.HttpContext.Request.Cookies["cusergroup"];
                if (rj_userLogin != null)
                {
                    if (rj_userLogin.ToString() != "")
                    {
                        opGroup = rj_userLogin;
                        _Session.HttpContext.Session.SetString("usergroup", opGroup);
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Response.Redirect("/");
                    }
                }
                else
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/");
            }
            return opGroup;
        }

        public string CheckGroup_()
        {
            string opGroup = "";
            if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
            {
                var rj_userLogin = _Session.HttpContext.Request.Cookies["cusergroup"];
                if (rj_userLogin != null)
                {
                    if (rj_userLogin.ToString() != "")
                    {
                        opGroup = rj_userLogin;
                        _Session.HttpContext.Session.SetString("usergroup", opGroup);
                    }
                    else
                    {
                        opGroup = "";
                    }
                }
                else
                {
                    opGroup = "";
                }
            }
            else
            {
                opGroup = "";
            }
            return opGroup;
        }

        public string checkUser()
        {

            CheckfinYear();
            string user = "";
            if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
            {
                var rj_userLogin = _Session.HttpContext.Request.Cookies["cuserid"];
                if (rj_userLogin != null)
                {
                    if (rj_userLogin.ToString() != "")
                    {
                        user = rj_userLogin.ToString();
                        _Session.HttpContext.Session.SetString("userid", user);
                    }
                    else
                    {
                        _httpContextAccessor.HttpContext.Response.Redirect("/");
                    }
                }
                else
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }
            }
            else
            {
                _httpContextAccessor.HttpContext.Response.Redirect("/");
            }
            return user;
        }

        public string checkUser_()
        {
            string user = "";
            if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
            {
                var rj_userLogin = _Session.HttpContext.Request.Cookies["cuserid"];
                if (rj_userLogin != null)
                {
                    if (rj_userLogin.ToString() != "")
                    {
                        user = rj_userLogin;
                        _Session.HttpContext.Session.SetString("userid", user);
                    }
                    else
                    {
                        user = "";
                    }
                }
                else
                {
                    user = "";
                }
            }
            else
            {
                user = _Session.HttpContext.Session.GetString("userid").ToString();
            }
            return user;
        }



        public string checkUserID()
        {
            string user = "";
            if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
            {
                var rj_userLogin = _Session.HttpContext.Request.Cookies["cuserid"];
                if (rj_userLogin != null)
                {
                    if (rj_userLogin != "")
                    {
                        user = rj_userLogin.ToString();
                        _Session.HttpContext.Session.SetString("userid", user);
                    }
                    else
                    {
                        return user;
                    }
                }
                else
                {
                    return user;
                }
            }
            else
            {
                user = _Session.HttpContext.Session.GetString("userid").ToString();
            }
            return user;
        }
        //public string checkUserWithAdmin()
        //{

        //    string useridwithAdmin = "";
        //    if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
        //    {
        //        var rj_userLogin = _Session.HttpContext.Request.Cookies["cuserid"];
        //        if (rj_userLogin != null)
        //        {
        //            if (rj_userLogin.ToString() != "")
        //            {
        //                useridwithAdmin = rj_userLogin.ToString();
        //                _Session.HttpContext.Session.SetString("userid", useridwithAdmin);
        //            }
        //            else
        //            {
        //                _httpContextAccessor.HttpContext.Response.Redirect("/");
        //            }
        //        }
        //        else
        //        {
        //            _httpContextAccessor.HttpContext.Response.Redirect("/");
        //        }
        //    }
        //    else
        //    {
        //        useridwithAdmin = findMgr(_Session.HttpContext.Session.GetString("userid").ToString());
        //    }
        //    return useridwithAdmin;
        //}

        //public string checkUserWithAdmin_()
        //{
        //    string useridwithAdmin = "";
        //    try
        //    {
        //        if (_Session.HttpContext.Session.GetString("userid") == null || _Session.HttpContext.Session.GetString("userid").ToString() == "" || _Session.HttpContext.Session.GetString("usergroup") == null || _Session.HttpContext.Session.GetString("usergroup").ToString() == "")
        //        {
        //            var rj_userLogin = _Session.HttpContext.Request.Cookies["cuserid"];
        //            if (rj_userLogin != null)
        //            {
        //                if (rj_userLogin.ToString() != "")
        //                {
        //                    useridwithAdmin = rj_userLogin.ToString();
        //                    _Session.HttpContext.Session.SetString("userid", useridwithAdmin);
        //                }
        //                else
        //                {
        //                    useridwithAdmin = "";
        //                }
        //            }
        //            else
        //            {
        //                useridwithAdmin = "";
        //            }
        //        }
        //        else
        //        {
        //            useridwithAdmin = findMgr(_Session.HttpContext.Session.GetString("userid").ToString());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        useridwithAdmin = "";
        //    }
        //    return useridwithAdmin;
        //}

        //public string findMgr(string userId, string finYear)
        //{
        //    string mgrId = "";
        //    SqlParameter _userid = new SqlParameter("@userid", userId);
        //    rjstr = "select isnull(b.userid, '0') as userid from Operator as a left join employee as b on a.sno=b.userid left join ReportManager as c on b.empid=c.Employee where b.userid=@userid or a.Opgroup=1";
        //    DataTable dt = new DataTable();
        //    dt = Rj_LoadTable(rjstr, new SqlParameter[] { _userid }, finYear);
        //    if (dt.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            if (dt.Rows[i]["userid"].ToString() != "")
        //            {
        //                mgrId = userId + ", " + dt.Rows[i]["userid"].ToString();
        //            }
        //        }
        //        //mgrId = userId + ", " + dt.Rows[0]["Reportmngr"].ToString();
        //    }
        //    else
        //    {
        //        mgrId = userId;
        //    }
        //    return mgrId;
        //}

        public void rj_checkQuerystring(string opgroup, string tp = "")
        {

            if (opgroup != "1")
            {
                if (tp == "1")
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }
                else if (tp == "2")
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }
                else if (tp == "3")
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }
                else
                {
                    _httpContextAccessor.HttpContext.Response.Redirect("/");
                }

            }
        }

        //        public string checkActivity(string Mno, string subMno, string userid)
        //        {
        //            string Activity = "";



        //            rjstr = @"select (
        //select top 1 CONVERT(nvarchar(10), a.accept) + ',' + CONVERT(nvarchar(10), a.[modify]) + ',' + CONVERT(nvarchar(10), a.[delete]) as [Action] from userRight as a 
        //left join employee as b on a.empId = b.empid where mno = @mno and a.subMno = @submno and b.userid = @userId) as [Action], 
        //(case when @submno <> '' then (select mname from subMenu where mno=@mno and ordBy=@submno) else (select mname from Menu where mno=@mno) end) as mname";

        //            SqlParameter _mno = new SqlParameter("@mno", Mno);
        //            SqlParameter _subMno = new SqlParameter("@submno", subMno);
        //            SqlParameter _userid = new SqlParameter("@userId", userid);

        //            DataTable dt = new DataTable();
        //            dt = Rj_LoadTable(rjstr, new SqlParameter[] { _mno, _subMno, _userid });
        //            if (dt.Rows.Count > 0)
        //            {
        //                Activity = dt.Rows[0]["Action"].ToString().Trim() + "|" + dt.Rows[0]["mname"].ToString().Trim();
        //            }
        //            if (Activity == "")
        //            {
        //                Activity = "0,0,0|";
        //            }
        //            return Activity;
        //        }

        public string rj_withoutPrifix(string pr)
        {
            try
            {
                var numbers = Regex.Matches(pr, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
                if (numbers.Length > 0)
                {
                    pr = numbers[numbers.Length - 1].ToString();
                }
                else
                {
                    pr = "";
                }
                //string[] str;
                //if (rj_prifix(Con).Trim() != "")
                //{
                //    str = pr.Split('-');
                //    //pr = pr.Replace(rj_prifix(Con).Trim(), "");
                //    pr = str[str.Length - 1].ToString();
                //}
            }
            catch (Exception ex) { }
            return pr;
        }

        public string TextFilter(string str)
        {
            str = str.Replace("&nbsp;", "");
            str = str.Replace("&#39;", "'");
            str = str.Replace("'", "''");
            str = str.Replace("&amp;", "&");
            str = str.Replace("&#160;", "");
            str = str.Replace("&quot;", "''");
            str = str.Replace("&apos;", "''");
            str = str.Replace("&cent;", "¢");
            str = str.Replace("&pound;", "£");

            return str;
        }

        public string Rj_CurrentDate()
        {
            string zoneId = "India Standard Time";
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
            DateTime cu = result.ToUniversalTime();
            DateTime cur = cu.ToLocalTime();
            string dat = "";
            dat = string.Format("{0:yyyy-MM-dd}", cur);
            return dat;
        }

        public string Rj_CurrentDatemmddyyyy()
        {
            string zoneId = "India Standard Time";
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
            DateTime cu = result.ToUniversalTime();
            DateTime cur = cu.ToLocalTime();
            //string dat = "";
            //dat = string.Format("{0:MM/dd/yyyy}", cur);
            return cur.ToString();
        }

        public string Rj_CurrentTime()
        {
            string zoneId = "India Standard Time";
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
            DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);
            DateTime cu = result.ToUniversalTime();
            DateTime cur = cu.ToLocalTime();
            string tim = "";
            tim = string.Format("{0:HH:mm:ss}", cur);
            return tim;
        }


        public string UniqueNumber12Digit()
        {
            StringBuilder s = new StringBuilder();
            System.Threading.Thread.Sleep(1);//make everything unique while looping
            long ticks = (long)(DateTime.UtcNow.Subtract(new DateTime(1980, 06, 12, 0, 0, 0, 0))).TotalMilliseconds;//EPOCH
            char[] baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[ticks % targetBase];
                ticks = ticks / targetBase;
            }
            while (ticks > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);
            //s.Append(result);
            return new string(result);
            //}
            //return s.ToString();
            //string shortUrl = System.Web.Security.Membership.GeneratePassword(11, 0).Replace("[^a-zA-Z0-9]", "");
            //shortUrl= Regex.Replace(shortUrl, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            ////xstr.replaceAll("[^a-zA-Z0-9]", " ")
            //return shortUrl;
        }

        public void WriteUserlog(string ModuleName, string EntryNo, string UserID, string Descriptions, string finYear)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = this.Rj_LoadTableWithProc("sp_userlog", new SqlParameter[] { new SqlParameter("@ModuleName", ModuleName),
                new SqlParameter("@EntryNo", EntryNo), new SqlParameter("@UserID", UserID), new SqlParameter("@Descriptions", Descriptions) }, finYear);
            }
            catch (Exception ex)
            {

            }
        }

        public string getKey()
        {
            string st = "";
            try
            {
                st = "RJMV2SPBNI54616M";
                //DataTable dt = new DataTable();
                //dt = Rj_LoadTableWithProc("getKey", new SqlParameter[] { });
                //if (dt.Rows.Count > 0)
                //{
                //    st = dt.Rows[0][0].ToString().Trim();
                //}
                //else
                //{
                //    st = "R&M";
                //}
            }
            catch (Exception ex)
            {
                st = "R&M";
            }
            return st;
        }

        public void pwdEnc(string username, string password, string finYear)
        {
            rjstr = "select username, Oppassword from operator where username=@username and Oppassword=@Oppassword";
            DataTable dt = new DataTable();
            dt = Rj_LoadTable(rjstr, new SqlParameter[] { new SqlParameter("@username", username), new SqlParameter("@Oppassword", password) }, finYear);
            if (dt.Rows.Count > 0)
            {
                string st = dt.Rows[0]["Oppassword"].ToString().Trim();
                string chk = rj_chkdecrypt(st);
                if (chk == "0")
                {
                    st = rj_encrypt(st);
                    rjstr = "update operator set Oppassword='" + st + "' where username='" + username + "' and Oppassword='" + password + "'";
                    rj_Exequet(rjstr, finYear);
                }

            }
            //else {

            //    rjstr = "select username, Oppassword from operator where username='" + username + "' and Oppassword='" + password + "'";
            //    dt = new DataTable();
            //    dt = Rj_LoadTable(rjstr, Con);
            //}
        }

        public string rj_chkdecrypt(string cipherText)
        {
            string st = "0";
            try
            {
                string EncryptionKey = getKey();
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                st = "1";
            }
            catch (Exception ex)
            {
                st = "0";
            }
            return st;
        }

        public string rj_encrypt(string rj)
        {
            try
            {
                string EncryptionKey = getKey();
                byte[] clearBytes = Encoding.Unicode.GetBytes(rj);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        rj = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex) { }
            return rj;
        }

        public string rj_decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = getKey();
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex) { }
            return cipherText;
        }



        public string _rj_encrypt(string rj)
        {
            try
            {
                string EncryptionKey = getKey();
                byte[] clearBytes = Encoding.Unicode.GetBytes(rj);

                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32); // AES-256
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }

                        byte[] encryptedBytes = ms.ToArray();
                        string base64String = Convert.ToBase64String(encryptedBytes);

                        // URL-safe base64 encoding
                        return base64String.Replace("+", "-").Replace("/", "_").Replace("=", "");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log exception
                return null;
            }
            return rj;
        }

        public string _rj_decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = getKey();
                // URL-safe base64 decoding
                string base64String = cipherText.Replace("-", "+").Replace("_", "/");
                // Add padding if necessary (Base64 decoding needs correct padding)
                base64String = base64String.PadRight(base64String.Length + (4 - base64String.Length % 4) % 4, '=');

                byte[] encryptedBytes = Convert.FromBase64String(base64String);

                using (Aes decryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    decryptor.Key = pdb.GetBytes(32); // AES-256
                    decryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                            cs.Close();
                        }

                        byte[] decryptedBytes = ms.ToArray();
                        return Encoding.Unicode.GetString(decryptedBytes); // Convert back to original string
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle or log exception
                return null;
            }
            return cipherText;
        }

        //public string _rj_encrypt(string rj) 
        //{
        //    using (var aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Encoding.UTF8.GetBytes(_key);
        //        aesAlg.IV = Encoding.UTF8.GetBytes(_iv);
        //        aesAlg.Mode = CipherMode.CBC;
        //        aesAlg.Padding = PaddingMode.PKCS7;

        //        using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
        //        using (var ms = new MemoryStream())
        //        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        //        using (var sw = new StreamWriter(cs))
        //        {
        //            sw.Write(rj);
        //            return Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //}
        //public string _rj_decrypt(string encryptedRj) {
        //    using (var aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = Encoding.UTF8.GetBytes(_key);
        //        aesAlg.IV = Encoding.UTF8.GetBytes(_iv);
        //        aesAlg.Mode = CipherMode.CBC;
        //        aesAlg.Padding = PaddingMode.PKCS7;

        //        using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
        //        using (var ms = new MemoryStream(Convert.FromBase64String(encryptedRj)))
        //        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
        //        using (var sr = new StreamReader(cs))
        //        {
        //            return sr.ReadToEnd();
        //        }
        //    }
        //}


        //public string _rj_encrypt(string rj)
        //{
        //    try
        //    {
        //        string EncryptionKey = getKey();  
        //        byte[] clearBytes = Encoding.UTF8.GetBytes(rj);  
        //        using (Aes encryptor = Aes.Create())
        //        {
        //            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //            encryptor.Key = pdb.GetBytes(32);
        //            encryptor.IV = pdb.GetBytes(16);
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(clearBytes, 0, clearBytes.Length);
        //                    cs.Close();
        //                }
        //                rj = Convert.ToBase64String(ms.ToArray()); 
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Encryption failed: {ex.Message}");
        //    }
        //    return rj;
        //}

        //public string _rj_decrypt(string encryptedRj)
        //{
        //    try
        //    {
        //        string EncryptionKey = getKey(); 
        //        byte[] cipherBytes = Convert.FromBase64String(encryptedRj);  

        //        using (Aes decryptor = Aes.Create())
        //        {
        //            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //            decryptor.Key = pdb.GetBytes(32);
        //            decryptor.IV = pdb.GetBytes(16);

        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                using (CryptoStream cs = new CryptoStream(ms, decryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //                {
        //                    cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                    cs.Close();
        //                }
        //                byte[] decryptedBytes = ms.ToArray();
        //                return Encoding.UTF8.GetString(decryptedBytes);  
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exception: log or throw it
        //        Console.WriteLine($"Decryption failed: {ex.Message}");
        //    }
        //    return string.Empty;  // Return an empty string in case of failure
        //}


        public string rj_encryptJson(string rj)
        {
            try
            {
                string EncryptionKey = "RJMV2SPBNI54616M";
                byte[] clearBytes = Encoding.Unicode.GetBytes(rj);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        rj = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex) { }
            return rj;
        }

        public string rj_decryptJson(string cipherText)
        {
            try
            {
                string EncryptionKey = "RJMV2SPBNI54616M";
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex) { }
            return cipherText;
        }

        public string GetIpValue()
        {
            string ipAdd = "Not Available";
            ipAdd = ""; // HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAdd))
            {
                ipAdd = ""; // HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ipAdd;
        }

        public string loadModule(string opGroup, DataTable dt, string st = "")
        {
            string str = "";
            string path = "";
            if (st == "0")
            {
                path = "";
            }
            else if (st == "1")
            {
                path = "../";
            }
            else
            {
                path = st;
            }

            if (opGroup == "2")
            {
                if (dt.Rows.Count > 0)
                {
                    str = "<b>";
                    //str += "<a href='" + path + "Attendance/markattendance.aspx'>" +
                    //        "<img src='"+ path + "Dashboard/att.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;ATT</a>";
                    if (dt.Rows[0]["mdc"].ToString() == "1")
                    {
                        str += "<a href='" + path + "MDC/mdcDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/mdc-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;MDC</a>";
                    }
                    //if (dt.Rows[0]["hr"].ToString() == "1")
                    //{
                    str += "<a href='" + path + "HRMS/hrmsDashboard.aspx'>" +
                                    "<img src='" + path + "assets/img/hexa/hrm-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;HRM</a>";
                    //}
                    if (dt.Rows[0]["lAndD"].ToString() == "1")
                    {
                        str += "<a href='" + path + "LND/lndDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/landD-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;L&D</a>";
                    }
                    if (dt.Rows[0]["crm"].ToString() == "1")
                    {
                        str += "<a href='" + path + "HR/hrDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/crm-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;CRM</a>";
                    }
                    if (dt.Rows[0]["bAndA"].ToString() == "1")
                    {
                        str += "<a href='" + path + "BNA/bnaDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/fandA-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;F&A</a>";
                    }
                    if (dt.Rows[0]["wms"].ToString() == "1")
                    {
                        str += "<a href='" + path + "adminDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/wms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;WMS</a>";
                    }
                    if (dt.Rows[0]["wms"].ToString() == "1")
                    {
                        str += "<a href='" + path + "Report/REPORTDASHBOARD.ASPX'>" +
                                        "<img src='" + path + "assets/img/hexa/erp-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;ERP</a>";
                    }
                    if (dt.Rows[0]["tms"].ToString() == "1")
                    {
                        str += "<a href='" + path + "TMS/tmsDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/tms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;TMS</a>";
                    }
                    if (dt.Rows[0]["bAndP"].ToString() == "1")
                    {
                        str += "<a href='" + path + "BNP/bnpDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/bsp-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;SPS</a>";
                    }
                    if (dt.Rows[0]["vms"].ToString() == "1")
                    {
                        str += "<a href='" + path + "VMS/vmsDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/vms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;VMS</a>";
                    }
                    if (dt.Rows[0]["pod"].ToString() == "1")
                    {
                        str += "<a href='" + path + "POD/Dashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/pod-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;POD</a>";
                    }
                    if (dt.Rows[0]["op1"].ToString() == "1")
                    {
                        str += "<a href='" + path + "OP1/opDashboard.aspx'>" +
                                        "<img src='" + path + "assets/img/hexa/op1-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;OP1</a>";
                    }
                    str += "<a href='#'>" +
                                        "<img src='" + path + "assets/img/hexa/app-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;APP</a>";

                    str += "<a href='" + path + "HELP/helpDashboard.aspx'>" +
                                    "<img src='" + path + "assets/img/hexa/help-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;Help</a>" +
                           "<a href='" + path + "Login.aspx?TP=LOGOUT'>" +
                                    "<img src='" + path + "assets/img/hexa/exit-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;Exit</a>" +
                "</b>";

                }

            }
            else
            {
                str = "<b>" +
                                               //"<a href='" + path + "Attendance/markattendance.aspx'>" +
                                               //    "<img src='" + path + "Dashboard/att.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;ATT</a>" +
                                               "<a href='" + path + "MDC/mdcDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/mdc-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;MDC</a>" +
                                               "<a href='" + path + "HRMS/hrmsDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/hrm-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;HRM</a>" +
                                                "<a href='" + path + "LND/lndDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/landD-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;L&D</a>" +
                                                "<a href='" + path + "HR/hrDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/crm-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;CRM</a>" +
                                                "<a href='" + path + "BNA/bnaDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/fandA-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;F&A</a>" +
                                                "<a href='" + path + "adminDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/wms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;WMS</a>" +
                                               "<a href='" + path + "Report/REPORTDASHBOARD.ASPX'>" +
                                                   "<img src='" + path + "assets/img/hexa/erp-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;ERP</a>" +
                                               "<a href='" + path + "TMS/tmsDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/tms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;TMS</a>" +
                                               "<a href='" + path + "BNP/bnpDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/bsp-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;SPS</a>" +
                                               "<a href='" + path + "VMS/vmsDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/vms-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;VMS</a>" +
                                                "<a href='" + path + "POD/Dashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/pod-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;POD</a>" +
                                               "<a href='" + path + "OP1/opDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/op1-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;OP1</a>" +
                                               "<a href='#'>" +
                                                   "<img src='" + path + "assets/img/hexa/app-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;APP</a>" +
                                               "<a href='" + path + "HELP/helpDashboard.aspx'>" +
                                                   "<img src='" + path + "assets/img/hexa/help-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;Help</a>" +
                                               "<a href='" + path + "Login.aspx?TP=LOGOUT'>" +
                                                   "<img src='" + path + "assets/img/hexa/exit-icon.png' style='width: 25px;' />&nbsp;&nbsp;&nbsp;Exit</a>" +
                                           "</b>";
            }

            return str;

        }

    }
}