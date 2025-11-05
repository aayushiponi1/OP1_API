namespace OP1_API.Areas.OP1.Models
{
    public class DashboardModel
    {

        public int? OrderCount { get; set; }
        public int? InvoiceCount { get; set; }
        public int? TotalAmount { get; set; }
        public int? DuesAmt { get; set; }


    }

    public class DashboardModelParam
    {
        public string? entryBy { get; set; }

    }

    public class GetSalesSummaryModel
    {
        public int? SalesTarget { get; set; }
        public int? SalesTargetForToday { get; set; }
        public int? SalesTargetForWeek { get; set; }
        public int? SalesTargetForMonth { get; set; }
        public int? SalesTargetForLast3Months { get; set; }
        public int? TotalSales { get; set; }
    }

    public class GetSalesSummaryModelParam
    {
        public string? UserId { get; set; }

    }
}
