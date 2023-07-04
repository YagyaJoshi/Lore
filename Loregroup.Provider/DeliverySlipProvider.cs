using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Data;
using Loregroup.Data.Entities;
using Loregroup.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Provider
{
    public class DeliverySlipProvider :IDeliverySlipProvider
    {

        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ISession _session;

        public DeliverySlipProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _session = session;
        }

        #region : CheckStatus

        public Consumer GetConsumerStatus(String ConsumerN) {

            Consumer c = new Consumer();
            try
            {
                c = _context.Consumers.FirstOrDefault(x => x.ConsumerNo == ConsumerN);
                
                return c;
            }
            catch (Exception ex) 
            {
                return null;
            }

        }

        public Consumer GetConsumerStatusForAPI(String ConsumerN)
        {

            Consumer c = new Consumer();
            try
            {
                c = _context.Consumers.Where(x => x.ConsumerNo.Contains(ConsumerN)).FirstOrDefault();

                return c;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public decimal GetRefillAmount(Int64 p)
        {
            decimal amnt = 0;
            try
            {

                RefillRate a = _context.RefillRates.FirstOrDefault(x => x.PackageId == p);
                amnt = a.NewRefillRate;
                return amnt;
                //amnt = _context.RefillRates.FirstOrDefault(x => x.PackageId == p).NewRefillRate;
                
            }
            catch (Exception ex) { return amnt; }
        }
        #endregion


    }
}
