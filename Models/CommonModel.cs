using System.Data;

namespace OP1_API.Models
{
    public class CommonModel
    {
    }

    public class TwoFieldModel
    {
        public int? Code { get; set; }
        public string? Name { get; set; }
    }
    public class TwoFieldNumberModel
    {
        public int? Number1 { get; set; }
        public int? Number2 { get; set; }
    }
    public class ThreeTextFieldModel
    {
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public string? Value3 { get; set; }
    }

    public class ReturnResult
    {
        public string? result { get; set; }
        public string? retval { get; set; }
        public string? msg { get; set; }
        public string? userCode { get; set; }
        public string? userRole { get; set; }
        public string? userCompany { get; set; }
    }

    public class GetLiveUserDetailsMmodel
    {
        public string? result { get; set; }
        public string? userCode { get; set; }
        public string? userRole { get; set; }
        public string? finYear { get; set; }
        public string? userCompany { get; set; }
    }

    public class getUserModel
    {
        public string? result { get; set; }
        public string? userCode { get; set; }
        public string? userRole { get; set; }
        public string? userCompany { get; set; }
    }


    public class ReturnDataTable
    {
        public Int32? count { get; set; }
        public string? result { get; set; }
        public DataTable? dataTable { get; set; }
    }
    public class ExecuteQuery
    {
        public int rowEffected { get; set; }
        public string? result { get; set; }          //OK OR FAILED OR ERROR
        public string? message { get; set; }
    }

    public class ErrorLogModel
    {
        public string? errId { get; set; }
        public string? errorMsg { get; set; }
    }


    public class ReturnUploadResult
    {
        public string? result { get; set; }
        public string? msg { get; set; }
        public List<ReturnUploadResultList> lst { get; set; }
    }

    public class ReturnUploadResultList
    {
        public string? result { get; set; }
        public string? flType { get; set; }
        public string? flName { get; set; }
        public string? msg { get; set; }


    }

    public class getTokenHeaderModel
    {
        public string? alg { get; set; }
        public string? typ { get; set; }
    }
    public class getTokenPayloadModel
    {
        public string? sub { get; set; }
        public string? jti { get; set; }
        public string? iat { get; set; }

        public string? result { get; set; }
        public string? retval { get; set; }
        public string? msg { get; set; }
        public string? userCode { get; set; }
        public string? userRole { get; set; }
        public string? finYear { get; set; }
        public string? userCompany { get; set; }

        public string? exp { get; set; }
        public string? iss { get; set; }
        public string? aud { get; set; }

    }

    public class UploadFilesModel
    {
        public string? type { get; set; }
        public string? flName { get; set; }
        public string? path { get; set; }
        public IFormFile? file { get; set; }
    }

    public class UploadMultipleFilesModel
    {
        public string? type { get; set; }
        public string? path { get; set; }
        public IEnumerable<IFormFile>? files { get; set; }
    }

    public class contextRouteDataModel
    {
        public string? controller { get; set; }
        public string? action { get; set; }
        public List<contextRouteDataKeyModel>? lst { get; set; }
    }
    public class contextRouteDataKeyModel
    {
        public string? Key { get; set; }
        public string? Value { get; set; }
    }

    public class we1plateforms_param
    {
        public long? code { get; set; }
        public long? ip { get; set; }
    }

    public class moduleAccessModel
    {
        public string? result { get; set; }
        public string? msg { get; set; }
        public long? sno { get; set; }
        public int? opGroup { get; set; }
        public int? status { get; set; }
        public int? wms { get; set; } = 0;
        //public string? wmsModuleCode { get; set; } = "";
        //public string? wmsText { get; set; } = "";
        //public string? wmsTitle { get; set; } = "";
        //public string? wmsImgage { get; set; } = "";
        //public string? wmsUrl { get; set; } = "";
        public int? erp { get; set; } = 0;
        //public string? erpModuleCode { get; set; } = "";
        //public string? erpText { get; set; } = "";
        //public string? erpTitle { get; set; } = "";
        //public string? erpImgage { get; set; } = "";
        //public string? erpUrl { get; set; } = "";
        public int? hr { get; set; } = 0;
        //public string? hrModuleCode { get; set; } = "";
        //public string? hrText { get; set; } = "";
        //public string? hrTitle { get; set; } = "";
        //public string? hrImgage { get; set; } = "";
        //public string? hrUrl { get; set; } = "";
        public int? sms { get; set; } = 0;
        //public string? smsModuleCode { get; set; } = "";
        //public string? smsText { get; set; } = "";
        //public string? smsTitle { get; set; } = "";
        //public string? smsImgage { get; set; } = "";
        //public string? smsUrl { get; set; } = "";
        public int? crm { get; set; } = 0;
        //public string? crmModuleCode { get; set; } = "";
        //public string? crmText { get; set; } = "";
        //public string? crmTitle { get; set; } = "";
        //public string? crmImgage { get; set; } = "";
        //public string? crmUrl { get; set; } = "";
        public int? tms { get; set; } = 0;
        //public string? tmsModuleCode { get; set; } = "";
        //public string? tmsText { get; set; } = "";
        //public string? tmsTitle { get; set; } = "";
        //public string? tmsImgage { get; set; } = "";
        //public string? tmsUrl { get; set; } = "";
        public int? mdc { get; set; } = 0;
        //public string? mdcModuleCode { get; set; } = "";
        //public string? mdcText { get; set; } = "";
        //public string? mdcTitle { get; set; } = "";
        //public string? mdcImgage { get; set; } = "";
        //public string? mdcUrl { get; set; } = "";
        public int? bAndA { get; set; } = 0;
        //public string? bAndAModuleCode { get; set; } = "";
        //public string? bAndAText { get; set; } = "";
        //public string? bAndATitle { get; set; } = "";
        //public string? bAndAImgage { get; set; } = "";
        //public string? bAndAUrl { get; set; } = "";
        public int? vms { get; set; } = 0;
        //public string? vmsModuleCode { get; set; } = "";
        //public string? vmsText { get; set; } = "";
        //public string? vmsTitle { get; set; } = "";
        //public string? vmsImgage { get; set; } = "";
        //public string? vmsUrl { get; set; } = "";
        public int? op1 { get; set; } = 0;
        //public string? op1ModuleCode { get; set; } = "";
        //public string? op1Text { get; set; } = "";
        //public string? op1Title { get; set; } = "";
        //public string? op1Imgage { get; set; } = "";
        //public string? op1Url { get; set; } = "";
        public int? lAndD { get; set; } = 0;
        //public string? lAndDModuleCode { get; set; } = "";
        //public string? lAndDText { get; set; } = "";
        //public string? lAndDTitle { get; set; } = "";
        //public string? lAndDImgage { get; set; } = "";
        //public string? lAndDUrl { get; set; } = "";
        public int? bAndP { get; set; } = 0;
        //public string? bAndPModuleCode { get; set; } = "";
        //public string? bAndPText { get; set; } = "";
        //public string? bAndPTitle { get; set; } = "";
        //public string? bAndPImgage { get; set; } = "";
        //public string? bAndPUrl { get; set; } = "";
        public int? app { get; set; } = 0;
        //public string? appModuleCode { get; set; } = "";
        //public string? appText { get; set; } = "";
        //public string? appTitle { get; set; } = "";
        //public string? appImgage { get; set; } = "";
        //public string? appUrl { get; set; } = "";
        public int? attendance { get; set; } = 0;
        //public string? attendanceModuleCode { get; set; } = "";
        //public string? attendanceText { get; set; } = "";
        //public string? attendanceTitle { get; set; } = "";
        //public string? attendanceImgage { get; set; } = "";
        //public string? attendanceUrl { get; set; } = "";

        public int? help { get; set; } = 0;
        //public string? helpModuleCode { get; set; } = "";
        //public string? helpText { get; set; } = "";
        //public string? helpTitle { get; set; } = "";
        //public string? helpImgage { get; set; } = "";
        //public string? helpUrl { get; set; } = "";

        public int? allprj { get; set; } = 0;
        public string? datetime { get; set; }
        public List<moduleAccessListModel>? moduleList { get; set; }
    }

    public class moduleAccessListModel
    {
        public int? srNo { get; set; } = 0;
        public int? status { get; set; } = 0;
        public string? moduleCode { get; set; } = "";
        public string? moduleText { get; set; } = "";
        public string? moduleTitle { get; set; } = "";
        public string? moduleImgage { get; set; } = "";
        public string? moduleUrl { get; set; } = "";
    }

    public class we1plateformsMenuParam
    {
        public string? moduleCode { get; set; }
    }

    public class we1plateformsSubIconParam
    {
        public string? moduleCode { get; set; }
        public string? mno { get; set; }
    }

    public class we1Menu
    {
        public string? mno { get; set; }
        public string? mname { get; set; }
        public string? url { get; set; }
        public string? status { get; set; }
        public string? type { get; set; }
        public string? ordBy { get; set; }
        public string? story { get; set; }
        public string? def { get; set; }
        public string? projectName { get; set; }
        public string? moduleUrl { get; set; }
        public string? moduleimg { get; set; }
        public string? moduleshortImg { get; set; }
        public List<we1SubMenu> submenu { get; set; }
    }

    public class we1SubMenu
    {
        public string? mno { get; set; }
        public string? submno { get; set; }
        public string? mname { get; set; }
        public string? url { get; set; }
        public string? status { get; set; }
        public string? type { get; set; }
        public string? ordBy { get; set; }
        public string? story { get; set; }
        public string? def { get; set; }
        public string? projectName { get; set; }
        public string? moduleUrl { get; set; }
        public string? moduleimg { get; set; }
        public string? moduleshortImg { get; set; }
    }


    public class we1Icon
    {
        public string? code { get; set; }
        public string? viewType { get; set; }
        public string? viewTypeName { get; set; }
        public string? mainIcon { get; set; }
        public string? url { get; set; }
        public string? iconName { get; set; }
        public string? serialNo { get; set; }
        public string? story { get; set; }
        public string? defaultIcon { get; set; }
        public List<we1SubIcon> subicon { get; set; }
    }

    public class we1SubIcon
    {
        public string? code { get; set; }
        public string? subCode { get; set; }
        public string? viewType { get; set; }
        public string? viewTypeName { get; set; }
        public string? mainIcon { get; set; }
        public string? url { get; set; }
        public string? iconName { get; set; }
        public string? serialNo { get; set; }
        public string? story { get; set; }
        public string? defaultIcon { get; set; }

    }
    public class uploadDocumentsParam
    {
        public IFormFile? file { get; set; }
        public string? rootPath { get; set; } = "";
        public string fileName { get; internal set; }
    }
    public class uploadmultipleDocumentsParam
    {
        public List<IFormFile>? files { get; set; }
        public string? rootPath { get; set; }
    }



}
