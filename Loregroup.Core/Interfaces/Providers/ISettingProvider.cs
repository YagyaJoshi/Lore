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
    public interface ISettingProvider
    {

        String SaveRegistration(FrontendCustomerViewModel model);
        CustomerViewModel GetRegistration(Int64 id);
        List<CustomerViewModel> GetRegistrationByCountry(Int64 CountryId);

        Int64 SaveBaseEntity(BaseEntityViewModel model);
        BaseEntityViewModel GetBaseEntity(Int64 id);
        BaseEntityViewModel GetBaseEntityById(Int64 id);
        BaseEntityViewModel GetBaseEntityByName(string name);

        Int64 SaveEvents(EventViewModel model);
        List<EventViewModel> GetAllEvents(int start, int length, string search, int filtercount);
        List<EventViewModel> GetAllEventsList();
        EventViewModel GetEvents(Int64 id);
        void DeleteEvents(Int64 id);

        Int64 SaveFaq(FaqViewModel model);
        List<FaqViewModel> GetAllFaq(int start, int length, string search, int filtercount);
        List<FaqViewModel> GetAllFaqList();
        FaqViewModel GetFaq(Int64 id);
        void DeleteFaq(Int64 id);

        Int64 SaveContact(ContactViewModel model);
        List<ContactViewModel> GetAllContact(int start, int length, string search, int filtercount);
        List<ContactViewModel> GetAllContactList();
        ContactViewModel GetContact(Int64 id);
        void DeleteContact(Int64 id);

        Int64 SaveAboutUs(BaseEntityViewModel model);
        BaseEntityViewModel GetAboutUs(Int64 id);
        BaseEntityViewModel GetAboutUsById (Int64 id);

        Int64 SaveReturnPolicy(BaseEntityViewModel model);
        BaseEntityViewModel GetReturnPolicy(Int64 id);
        BaseEntityViewModel GetReturnPolicyById (Int64 id);

        Int64 SaveTermsandConditions(BaseEntityViewModel model);
        BaseEntityViewModel GetTermsandConditions(Int64 id);
        BaseEntityViewModel GetTermsandConditionById (Int64 id);

        Int64 SaveGallery(GalleryViewModel model);
        List<GalleryViewModel> GetAllGallery(int start, int length, string search, int filtercount, Int64 rid);
        List<GalleryViewModel> GetAllGalleryList();
        GalleryViewModel GetGallery(Int64 id);
        void DeleteGallery(Int64 id);
        List<GalleryViewModel> GetAllGalleryList(int Typeid);

        Int64 SaveWishlist(WishlistViewModel model);
        //List<WishlistViewModel> GetAllWishlist (int start, int length, string search, int filtercount);
        List<WishlistViewModel> GetAllWishList(Int64 userid);
        WishlistViewModel GetWishlist(Int64 id);
        void DeleteWishlist(Int64 Id = 0);

        String SaveStoreLocator(StoreViewModel model);
        StoreViewModel GetStoreLocator(Int64 id);
        List<StoreViewModel> GetStoreLocatorByCountry(Int64 CountryId);
        List<StoreViewModel> GetStoreLocatorByState(Int64 StateId);
        List<StoreViewModel> GetAllShopForMap();
        List<StoreViewModel> GetAllStoreLocator(Int64 CountryId, Int64 StateId);
        List<StoreViewModel> GetAllStoreList();

        List<UserLocation> GetAllLocationsForStoreLocator(Int64 countryid, Int64 stateid, string zipcd);

        Int64 SaveSlider(SliderViewModel model);
        List<SliderViewModel> GetAllSliders(int start, int length, string search, int filtercount);
        List<SliderViewModel> GetAllSlidersList();
        SliderViewModel GetSlider(Int64 id);

    }
}
