using System.Data.SqlClient;
using System.Data;
using OP1_API.Areas.Login.Models;
using OP1_API.ClassCollection;
using OP1_API.Models;

namespace OP1_API.Areas.Common.Data
{
    public class DropDownDataLayer
    {
        GlobalClass cl = new GlobalClass();
        public List<ThreeTextFieldModel> branchList(LoginResultModel _userDetails, long company = 0)
        {
            List<ThreeTextFieldModel> branchList = new List<ThreeTextFieldModel>();

            string rjstr = "";
            string? opGroup = _userDetails.userRole;
            if (opGroup == "1")
            {
                rjstr = @"select * from (
select sno, upper(warehousename) as sname,  
STUFF((select '|' + CONVERT(nvarchar(12), aa.pno) + ';;' + aa.pname from party as aa left join partydetail as b on aa.pno=b.pno where b.whname=a.sno order by aa.pname FOR XML PATH ('')), 1, 1, '') as details 
from warehousMaster as a left join partydetail as b on a.sno=b.whname where (isnull(b.pno, 0)=@company or @company=0)
) as r group by r.sno, r.sname, r.details order by r.sname";
            }
            else if (opGroup == "2")
            {
                rjstr = @"select * from (
select a.warehouse as sno, upper(w.warehousename) as sname, 
                    STUFF((select '|' + CONVERT(nvarchar(12), c.pno) + ';;' + c.pname from empalloutclient as a 
                    inner join party as c on a.pno=c.pno 
                    inner join (select empid from employee where userid=@userid) as p on a.empid=p.empid 
                    where a.warehouse=w.sno order by c.pname 
                    FOR XML PATH ('')), 1, 1, '') as details 
                    from empalloutclient as a 
                    inner join warehousMaster as w on a.warehouse=w.sno 
                    inner join (select empid from employee where userid=@userid) as e on a.empid=e.empid 
					where (a.pno=@company or @company=0)
					) as r group by r.sno, r.sname, r.details order by r.sname";
            }
            else
            {
                rjstr = @"select b.sno as sno, b.warehousename as sname, 
(select top 1 CONVERT(nvarchar(12), pno) + ';;' + pname from party where opUserid=@userid) as details from partydetail as a left join warehousMaster as b on a.whname=b.sno left join party as c on a.pno=c.pno 
where c.opUserid=@userid and (a.pno=@company or @company=0) and isnull(b.active, 0)=0 order by sname";
            }

            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] {
                new SqlParameter("@userid", _userDetails.userCode),
                new SqlParameter("@company", company)
            }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                branchList.Add(new ThreeTextFieldModel
                {
                    Value1 = dr["sno"].ToString(),
                    Value2 = dr["sname"].ToString(),
                    Value3 = dr["details"].ToString()
                });
            }
            return branchList;
        }

        public List<TwoFieldModel> partyList(LoginResultModel _userDetails, long branch = 0)
        {
            List<TwoFieldModel> branchList = new List<TwoFieldModel>();

            string rjstr = "";
            string? opGroup = _userDetails.userRole;
            if (opGroup == "1")
            {
                rjstr = @"select a.pno as sno, upper(a.pname) as sname from party as a left join partydetail as b on a.pno=b.pno 
where isnull(DeActive, 0)=0 and (b.whname=@warehouse or @warehouse=0) 
group by a.pno, a.pname order by pname";
            }
            else if (opGroup == "2")
            {
                rjstr = @"select isnull(c.pno, 0) as sno, c.pname as sname from empalloutclient as a inner join party as c on a.pno=c.pno 
inner join (select empid from employee where userid=@userid) as e on a.empid=e.empid 
where (a.warehouse=@warehouse or @warehouse=0) and ISNULL(c.DeActive, 0)=0 
group by c.pno, c.pname
order by sname";
            }
            else
            {
                rjstr = @"select isnull(b.pno, 0) as sno, b.pname as sname from partydetail as a left join party as b on a.pno=b.pno 
where b.opUserid=@userid and (b.whhouse=@warehouse or @warehouse=0) and ISNULL(b.DeActive, 0)=0 order by sname";
            }

            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] {
                new SqlParameter("@userid", _userDetails.userCode),
                new SqlParameter("@warehouse", branch)
            }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                branchList.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["sno"].ToString()),
                    Name = dr["sname"].ToString(),

                });
            }
            return branchList;
        }

        public List<TwoFieldModel> employeeList(LoginResultModel _userDetails, long branch = 0)
        {
            List<TwoFieldModel> branchList = new List<TwoFieldModel>();

            string rjstr = "";
            string? opGroup = _userDetails.userRole;
            if (opGroup == "1")
            {
                rjstr = @"select a.pno as sno, upper(a.pname) as sname from party as a left join partydetail as b on a.pno=b.pno 
where isnull(DeActive, 0)=0 and (b.whname=@warehouse or @warehouse=0) 
group by a.pno, a.pname order by pname";
            }
            else if (opGroup == "2")
            {
                rjstr = @"select isnull(c.pno, 0) as sno, c.pname as sname from empalloutclient as a inner join party as c on a.pno=c.pno 
inner join (select empid from employee where userid=@userid) as e on a.empid=e.empid 
where (a.warehouse=@warehouse or @warehouse=0) and ISNULL(c.DeActive, 0)=0 
group by c.pno, c.pname
order by sname";
            }
            else
            {
                rjstr = @"select isnull(b.pno, 0) as sno, b.pname as sname from partydetail as a left join party as b on a.pno=b.pno 
where b.opUserid=@userid and (b.whhouse=@warehouse or @warehouse=0) and ISNULL(b.DeActive, 0)=0 order by sname";
            }

            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] {
                new SqlParameter("@userid", _userDetails.userCode),
                new SqlParameter("@warehouse", branch)
            }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                branchList.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["sno"].ToString()),
                    Name = dr["sname"].ToString(),

                });
            }
            return branchList;
        }

        public List<TwoFieldModel> stateList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            string rjstr = @"select Statecode as code, upper(Statename) as sname from StateMaster order by Statename";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }



        public List<TwoFieldModel> cityList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            string rjstr = @"select sno as code, upper(cityname) as sname from city order by cityname";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }


        public List<TwoFieldModel> languageList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            string rjstr = @"select sno as code, upper([language]) as sname from languagemaster order by [language]";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }

        public List<TwoFieldModel> countryList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            string rjstr = @"select sno as code, upper(contyname) as sname from nationalityMaster order by [contyname]";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }

        public List<TwoFieldModel> bloodGroupList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();


            string rjstr = @"select sno as code, upper(bloodgroupname) as sname from bloodgroup order by [bloodgroupname]";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }

        public List<TwoFieldModel> bankList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();


            string rjstr = @"select sno as code, upper(bankname) as sname from bankmaster order by bankname";
            DataTable dt = new DataTable();
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["code"].ToString()),
                    Name = dr["sname"].ToString()
                });
            }
            return r;
        }

        public List<TwoFieldModel> genderStatusList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            r.Add(new TwoFieldModel { Code = 0, Name = "Male" });
            r.Add(new TwoFieldModel { Code = 1, Name = "Female" });
            r.Add(new TwoFieldModel { Code = 2, Name = "Other" });

            return r;
        }

        public List<TwoFieldModel> maritalStatusList(LoginResultModel _userDetails)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();

            r.Add(new TwoFieldModel { Code = 0, Name = "Single" });
            r.Add(new TwoFieldModel { Code = 1, Name = "Married" });
            r.Add(new TwoFieldModel { Code = 2, Name = "Widowed" });
            r.Add(new TwoFieldModel { Code = 3, Name = "Divorced" });
            r.Add(new TwoFieldModel { Code = 4, Name = "Separated" });

            return r;
        }

        public List<TwoFieldModel> unitList(LoginResultModel _userDetails, long code = 0)
        {
            List<TwoFieldModel> r = new List<TwoFieldModel>();
            DataTable dt = new DataTable();
            string rjstr = "select sno, unit from unitMaster as a where (sno=@sno or @sno=0) and ISNULL(status, 0)=0 order by unit";
            dt = cl.Rj_LoadTable(rjstr, new SqlParameter[] { new SqlParameter("@sno", code) }, _userDetails.finYear);
            foreach (DataRow dr in dt.Rows)
            {
                r.Add(new TwoFieldModel
                {
                    Code = Convert.ToInt32(dr["sno"].ToString()),
                    Name = dr["unit"].ToString()
                });
            }

            return r;
        }

    }
}
