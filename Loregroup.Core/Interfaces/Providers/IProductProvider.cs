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
   public interface IProductProvider
    {
       string SaveProduct(ProductViewModel model);
       List<ProductViewModel> GetAllProducts(int start, int length, string search, int filtercount);
       ProductViewModel GetProduct(Int64 id);
       void DeleteProduct(Int64 id);
       List<ProductViewModel> GetAllProductsForReceivePO(Int64 POMasterId);
       List<ProductViewModel> GetAllProductsForDispatchOrder(Int64 OrderMasterId);
       List<ProductViewModel> GetProductsOfPO(Int64 POId);


        Int64 SaveAgents(AgentViewModel model);
        List<AgentViewModel> GetAllAgents(int start, int length, string search, int filtercount);
        void DeleteAgents(Int64 id);
        AgentViewModel GetAgents(Int64 id);
        List<AgentViewModel> GetAllAgentList();



        Int64 SaveBrand(BrandViewModel model);
       List<BrandViewModel> GetAllBrands(int start, int length, string search, int filtercount);
       List<BrandViewModel> GetAllBrandList();
       BrandViewModel GetBrand(Int64 id);
       void DeleteBrand(Int64 id);


        Int64 SaveOrderLocator(OrderLocatorViewModel model);
        List<OrderLocatorViewModel> GetAllOrderLocator(int start, int length, string search, int filtercount);
        List<OrderLocatorViewModel> GetAllOrderLocatorList();
        OrderLocatorViewModel GetOrderLocator(Int64 id);
        void DeleteOrderLocator(Int64 id);

        Int64 SaveCollectionYear(CollectionYearViewModel model);
       List<CollectionYearViewModel> GetAllCollectionYears(int start, int length, string search, int filtercount);
       List<CollectionYearViewModel> GetAllCollectionYearList();
       CollectionYearViewModel GetCollectionYear(Int64 id);
       void DeleteCollectionYear(Int64 id);

       Int64 SaveColour(ColourViewModel model);
       List<ColourViewModel> GetAllColours(int start, int length, string search, int filtercount);
       List<ColourViewModel> GetAllColourList();
       ColourViewModel GetColour(Int64 id);
       void DeleteColour(Int64 id);

       Int64 SaveCutOfDress(CutOfDressViewModel model);
       List<CutOfDressViewModel> GetAllCutOfDresses(int start, int length, string search, int filtercount);
       List<CutOfDressViewModel> GetAllCutOfDressList();
       CutOfDressViewModel GetCutOfDress(Int64 id);
       void DeleteCutOfDress(Int64 id);

       Int64 SaveCategory(CategoryViewModel model);
       List<CategoryViewModel> GetAllCategories(int start, int length, string search, int filtercount);
       List<CategoryViewModel> GetAllCategoryList();
       CategoryViewModel GetCategory(Int64 id);
       void DeleteCategory(Int64 id);

       Int64 SaveSize(SizeViewModel model);
       List<SizeViewModel> GetAllSizes(int start, int length, string search, int filtercount);
       List<SizeViewModel> GetAllSizeList();
       SizeViewModel GetSize(Int64 id);
       void DeleteSize(Int64 id);

       Int64 SaveFabric(FabricViewModel model);
       List<FabricViewModel> GetAllFabrics(int start, int length, string search, int filtercount);
       List<FabricViewModel> GetAllFabricList();
       FabricViewModel GetFabric(Int64 id);
       void DeleteFabric(Int64 id);       

       Int64 SaveTax(TaxViewModel model);
       List<TaxViewModel> GetAllTaxes(int start, int length, string search, int filtercount);
       List<TaxViewModel> GetAllTaxList();
       TaxViewModel GetTax(Int64 id);
       void DeleteTax(Int64 id);

       List<SizeViewModel> GetSizeModelForInventory(PurchaseOrderViewModel model);
       List<WareHouseViewModel> GetAllWareHouse();
       bool SaveUpdatedInventory(PurchaseOrderViewModel Model);

       Int64 SaveEvents(EventViewModel model);
       List<EventViewModel> GetAllEvents(int start, int length, string search, int filtercount);
       List<EventViewModel> GetAllEventsList();
       List<EventViewModel> GetAllEventsListByEventtype(string eventtype);
       EventViewModel GetEvents(Int64 id);
       void DeleteEvents(Int64 id);

       Int64 SaveContact(ContactViewModel model);
       List<ContactViewModel> GetAllContact(int start, int length, string search, int filtercount);
       List<ContactViewModel> GetAllContactList();
       ContactViewModel GetContact(Int64 id);
       void DeleteContact(Int64 id);

       Int64 SaveFaq(FaqViewModel model);
       List<FaqViewModel> GetAllFaq(int start, int length, string search, int filtercount);
       List<FaqViewModel> GetAllFaqList();
       FaqViewModel GetFaq(Int64 id);
       void DeleteFaq(Int64 id);

    }
}
