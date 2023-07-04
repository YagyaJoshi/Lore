using Loregroup.Core.Enumerations;
using Loregroup.Core.ViewModels;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Core.Interfaces.Providers
{
   public  interface IUploadFileProvider
    {

       Int64 SaveConsumer(List<ConsumersViewModel> modellist);
       ConsumersViewModel ToConsumerViewModel(Consumer consumer);
       string ConsumerRegistration(List<ConsumersViewModel> datalist);
       List<ConsumersViewModel> GetAllConsumers(Status? status, int records, int page = 0);
       List<ConsumersViewModel> GetAllConsumersAccStatus(string status);
       List<ConsumersViewModel> GetAllConsumersAccBookinDate(string startDate, string endDate);
       List<ConsumersViewModel> GetAllCon(string status, string startDate, string endDate);
    }
}
