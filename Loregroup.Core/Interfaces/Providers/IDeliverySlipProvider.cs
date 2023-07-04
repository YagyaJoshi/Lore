using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Providers
{
    public interface IDeliverySlipProvider 
    {
        
        
        Consumer GetConsumerStatus(String ConsumerNo);
        decimal GetRefillAmount(Int64 p);
        Consumer GetConsumerStatusForAPI(String ConsumerN);

        
    }
}
