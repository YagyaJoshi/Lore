using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface ILoreProvider
    {
        
        List<CategoryViewModel> GetAllCategoryList();
        String SaveAddresses(AddressViewModel model);
        String SaveAccountDetailsFirst(FrontendAccountdetailViewModel model);
        //String SaveProducts(TempCartViewModel model);
        List<AccountDetailsViewModel> GetAllAccountList();
        String SaveBillingAddresses(AddressViewModel model);


        List<TempCartViewModel> GetTempcartList(Int64 userid);
        string SaveFrontendOrder(CheckoutViewModel Model);
        int GetTempcartCount(Int64 userid);

        ShippingViewModel GetUserShippingDetails(Int64 userid);

    }
}
