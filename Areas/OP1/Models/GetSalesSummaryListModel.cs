namespace OP1_API.Areas.OP1.Models
{
    public class GetSalesSummaryListModel
    {
        public int CompanyCode { get; set; }
        public List<GetTopTenCustomerModel>? GetTopTenCustomerModels { get; set; }
        public List<LowStockLevelsModel>? LowStockLevelsModels {  get; set; }
        public List<CurrentStockStatusModel>? CurrentStockStatusModels {  get; set; }
        public List<CancelledOrdersModel>? cancelledOrdersModels { get; set; }
        public List<SellingProductsModel>? SellingProductsModels {  get; set; }

    }

    public class GetTopTenCustomerModel
    {
        public string? CustomerName { get; set; }
        public string? TotalAmount { get; set; }

    }

    public class LowStockLevelsModel
    {
        public string? Items { get; set; }
        public string? quantity { get; set; }
    }
    public class CurrentStockStatusModel
    {
        public string? Items { get; set; }
        public string? quantity { get; set; }

    }

    public class CancelledOrdersModel
    {
        public string? OrderNo { get; set; }
        public string? CustomerName { get; set; }
        public string? TotalAmount { get; set; }

    }
    public class SellingProductsModel
    {
        public string? Items { get; set; }
        public string? Qty { get; set; }
        public string? Amount {  get; set; }

    }
}