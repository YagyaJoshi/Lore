using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface IOrderProvider
    {
        string SaveOrder(OrderViewModel Model);
        string UpdateOrder(OrderViewModel Model);
        OrderViewModel GetOrderById(Int64 Id);
        OrderViewModel GetPrintPreview(Int64 Id);
        void DeleteOrder(Int64 id, Int64 ModifiedById);
        List<OrderListDisplayViewModel> GetAllOrders(int start, int length, string search, int filtercount, Int64 rid, Int64 warehouseId, Int64 orderlocId);

        List<OrderListDisplayViewModel> GetAllCustmerOrder(int start, int length, string search, int filtercount, Int64 current, Int64 rid, Int64 orderlocId);

        List<OrderListDisplayViewModel> GetAllOrdersBtCustomerId(int start, int length, string search, int filtercount, Int64 rid, Int64 warehouseId,Int64 CuatomerId);

        List<ColourViewModel> GetColoursForProduct(Int64 ProductId);
        OrderViewModel GetOrderByOrderNo(string OrderNo);
        void SendMailToCustomer(OrderViewModel Model);
        List<OrderDetailViewModel> GetDetailsListForDispatch(Int64 id);
        bool DispatchOrderDetails(OrderViewModel Model);


        bool UpdatePurchaseOrder(PurchaseOrderViewModel Model);
        bool SavePurchaseOrder(PurchaseOrderViewModel Model);
        List<OrderListDisplayViewModel> GetAllPurchaseOrders(int start, int length, string search, int filtercount, Int64 rid, Int64 cusid,Int64 warehouseId);
        void DeletePurchaseOrder(Int64 id, Int64 ModifiedById);
        PurchaseOrderViewModel GetPurchaseOrderById(Int64 Id);
        PurchaseOrderViewModel GetPOPrintPreview(Int64 Id);
        List<SizeViewModel> GetSizeModelForReceivePO(PurchaseOrderViewModel model);
        List<SizeViewModel> GetSizeModelForDispatchOrder(OrderViewModel model);
        bool ReceivePO(PurchaseOrderViewModel Model);
        bool DispatchOrder(OrderViewModel Model);
        List<PurchaseOrderDetailViewModel> GetDetailsListForReceivePO(Int64 PoMasterid);
        bool ReceivePODetails(PurchaseOrderViewModel Model);

        List<SizeViewModel> GetSizeModelForInventory(PurchaseOrderViewModel model);

        List<WareHouseViewModel> GetAllWareHouse();
        void SendInprogressMailToCustomer(OrderViewModel Model);

        void SendMailToReadyShip(PurchaseOrderViewModel Model);
        void SendMail(PurchaseOrderViewModel Model);

        List<OrderViewModel> GetAllOrderList();
        OrderViewModel GetOrderByStatusId(int id);
        List<OrderViewModel> GetAllOrderListByUserid(Int64 userid);

        List<ProductImage> GetAllOrderImageListByUserid(Int64 userid);
        List<ProductImage> GetDownloadImagesbyProduct(Int64 ab);
        List<ProductListModel> GetPOProductDetailList(Int64 POMasterId);
        //CS 03-05-2020
        List<List<SizeViewModel>> GetProductInventoryCount(Int64 productId, int warehouseId);
        List<SizeViewModel> GetStockReportForInventory(Int64 productId, string styleNumber, Int64 colorId, Int64 warehouseId);
        List<SizeViewModel> GetSalebyShowReportInventory(Int64 OrderLocaterId,Int64 warehouseId, string OrderDate, string OrderDateString);
        List<SizeViewModel> GetSalebyProductReportInventory(Int64 productId, string productName, Int64 colorId, Int64 warehouseId, string OrderDate,string OrderDateString);
        List<SizeViewModel> GetSizeModelForShortfallInventory(long productId, string productName, long colorId, long warehouseId);
    }
}
