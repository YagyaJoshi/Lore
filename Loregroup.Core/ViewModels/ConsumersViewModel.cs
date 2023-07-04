using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.ViewModels
{
   public  class ConsumersViewModel : BaseViewModel
    {
       
       public string Equipment { get; set; }
       
       public string Scheme { get; set; }

       public DateTime? BookingDate { get; set; }
       public string bookingdate { get; set; }
        
       public string BookNo { get; set; }

       public DateTime? BookTime { get; set; }

        public string BStatus { get; set; }

        public Int64 ConsumerId { get; set; }

        public string ConsumerNo { get; set; }

        public string Name { get; set; }

        public int NoOfCycles { get; set; }

        public string CashMemoNo { get; set; }

        public DateTime? CashMemoDate { get; set; }
        public string cashmemodate { get; set; }

        public DateTime? DeliveryDate { get; set; }
        public string deliverydate { get; set; }

        public DateTime? DeliveryTime { get; set; }
        public string deliverytime { get; set; }

        public string UserId { get; set; }

        public string InstallationOnBooking { get; set; }

        public string IvrsReferenceNo { get; set; }

        public string DBTLStatus { get; set; }

    }
}
