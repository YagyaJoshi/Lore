using Loregroup.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
    public class OrderListDisplayViewModel
    {
        public OrderListDisplayViewModel()
        {
            OrderDisplayList = new List<OrderListDisplayViewModel>();
        }

        public string CustomerFullName { get; set; }
        public string ShopName { get; set; }
        public string CurrencyName { get; set; }
        public string MobileNo { get; set; }
        public string ZipCode { get; set; }
        public string EmailId { get; set; }
        public string OrderNo { get; set; }
        public string OrderDate { get; set; }
        public string WareHouseName { get; set; }
        public string DeliveryDate { get; set; }
       // public string DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountString { get; set; }
        public decimal Amount { get; set; }
        public Int64 TotalProducts { get; set; }
        public string Edit { get; set; }
        public string Delete { get; set; }
        public string PrintPreview { get; set; }
        public string ChangeStatus { get; set; }
        public OrderStatus OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public string IsPOPlaced { get; set; }
        public string Receive { get; set; }
        public string DispatchOrder { get; set; }
        public string OrderRefrence { get; set; }
        public string OrderNotes { get; set; }
        public string WearDate { get; set; }
        public string UserSelectDeliveryDate { get; set; }
        public string BridesName { get; set; }
        public string IsPayment { get; set; }
        public List<OrderListDisplayViewModel> OrderDisplayList { get; set; }
      //  public string RoleId { get; set; }
        public bool IsActive { get; set; }
    }
}
