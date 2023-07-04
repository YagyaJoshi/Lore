
using Loregroup.Core.Enumerations;
using Loregroup.Core.Helpers;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using Loregroup.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loregroup.Provider
{


    public class UploadFileProvider : IUploadFileProvider
    {

        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ISession _session;


        #region : Consumer

        public ConsumersViewModel ToConsumerViewModel(Consumer consumer)
        {
            string dd = "";
            string cd = "";
            string bd = "";
            string dt = "";

            if (consumer.CashMemoDate != null)
            {
                cd = consumer.CashMemoDate.ToString();
            }
            if (consumer.DeliveryDate != null)
            {
                dd = consumer.DeliveryDate.ToString();
            }
            if (consumer.BookingDate != null)
            {
                bd = consumer.BookingDate.ToString();
            }
            if (consumer.BookTime != null)
            {
                dt = consumer.BookTime.ToString();
            }

            return new ConsumersViewModel()
            {
                BookNo = consumer.BookNo,
                ConsumerId = consumer.ConsumerId,
                ConsumerNo = consumer.ConsumerNo,
                DBTLStatus = consumer.DBTLStatus,
                //BookingDate = consumer.BookingDate,
                BookTime = consumer.BookTime,
                //CashMemoDate = consumer.CashMemoDate,
                CashMemoNo = consumer.CashMemoNo,
                CreatedById = consumer.CreatedById,
                CreationDate = consumer.CreationDate,
                //DeliveryDate = consumer.DeliveryDate,
                DeliveryTime = consumer.DeliveryTime,
                Equipment = consumer.Equipment,
                Id = consumer.Id,
                InstallationOnBooking = consumer.InstallationOnBooking,
                IvrsReferenceNo = consumer.IvrsReferenceNo,
                ModificationDate = consumer.ModificationDate,
                ModifiedById = consumer.ModifiedById,
                Name = consumer.Name,
                NoOfCycles = consumer.NoOfCycles,
                Scheme = consumer.Scheme,
                StatusId = consumer.StatusId,
                UserId = consumer.UserId,
                BStatus = consumer.BStatus,
                //deliverydate = consumer.DeliveryDate.Value.ToString("dd-MM-yyyy"),
                //cashmemodate = consumer.CashMemoDate.Value.ToString("dd-MM-yyyy"),
                //bookingdate = consumer.BookingDate.Value.ToString("dd-MM-yyyy"),

                deliverydate = dd,
                cashmemodate = cd,
                bookingdate = bd,
                deliverytime = dt
                


            };
        }

        public List<ConsumersViewModel> GetAllConsumers(Status? status, int records, int page = 0)
        {
            var Predicate = PredicateBuilder.True<Consumer>();
            if (status != null)
            {
                Predicate.And(x => x.StatusId == (int)status);
            }

            if (page > 0)
            {
                //return _context.Cities.Where(x => x.StatusId == 1)
                //   .ToList()
                //   .Select(ToCityViewModel)
                //   .ToList();
            }
            if (records == 0)
            {
                return _context.Consumers.Where(x => x.StatusId !=4 && x.CreatedById == _session.CurrentUser.Id)
                       .ToList()
                       .Select(ToConsumerViewModel)
                       .ToList();
            }
            else
            {
                return _context.Consumers
                   .ToList()
                   .Select(ToConsumerViewModel)
                   .ToList();
            }
            throw new NotImplementedException();

        }

        public UploadFileProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _session = session;
        }

        public Int64 SaveConsumer(List<ConsumersViewModel> modellist)
        {

            long i = 0;
            try
            {
                foreach (ConsumersViewModel model in modellist)
                {
                    Consumer consumers = new Consumer()
                    {

                        BookNo = model.BookNo,
                        // BookTime = model.BookTime,
                        // CashMemoDate = model.CashMemoDate,
                        CashMemoNo = model.CashMemoNo,
                        ConsumerId = model.ConsumerId,
                        ConsumerNo = model.ConsumerNo,
                        DBTLStatus = model.DBTLStatus,
                        // DeliveryDate = model.DeliveryDate,
                        // DeliveryTime = model.DeliveryTime,
                        Equipment = model.Equipment,
                        InstallationOnBooking = model.InstallationOnBooking,
                        // IvrsReferenceNo = model.IvrsReferenceNo,
                        Name = model.Name,
                        NoOfCycles = model.NoOfCycles,
                        Scheme = model.Scheme,
                        BStatus = model.BStatus,
                        StatusId = model.StatusId,
                        //UserId = model.UserId,



                    };
                    _context.Consumers.Add(consumers);
                    _context.SaveChanges();
                    i = consumers.Id;
                }
            }
            catch (Exception ex)
            { }
            return i;


        }

        public DataTable ToDataTable<T>(IList<T> data)// T is any generic type
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                //    dt.Columns.Add(pi.Name, Nullable.GetUnderlyingType(
                //pi.PropertyType) ?? pi.PropertyType);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public string ConsumerRegistration(List<ConsumersViewModel> datalist)
        {
            string c = "";
            try
            {
                bool check = false;
                
                string ConnString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                string TimeStamp = DateTime.Today.Date.Year.ToString() + DateTime.Today.Date.Month.ToString() + DateTime.Today.Date.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                DataTable dt = new DataTable();
                dt = ToDataTable(datalist);
                dt.Columns.Add("TimeStamp");
                // timstamp = TimeStamp;
                int CheckStatus = (int)CheckingStatus.Pending;// that means  Pending
                foreach (DataRow dr in dt.Rows)
                {
                    dr["TimeStamp"] = TimeStamp;
                }
                SqlConnection con = new SqlConnection(ConnString);
                con.Open();
                if (dt != null && dt.Rows.Count > 0)
                {                  
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(con);
                    bulkCopy.BatchSize = 500;
                    bulkCopy.ColumnMappings.Add("Equipment", "Equipment");
                    bulkCopy.ColumnMappings.Add("ConsumerNo", "ConsumerNo");
                    bulkCopy.ColumnMappings.Add("ConsumerId", "ConsumerId");
                    bulkCopy.ColumnMappings.Add("Name", "Name");
                    bulkCopy.ColumnMappings.Add("Scheme", "Scheme");
                    bulkCopy.ColumnMappings.Add("BookingDate", "BookingDate");
                    bulkCopy.ColumnMappings.Add("BookNo", "BookNo");
                    bulkCopy.ColumnMappings.Add("BookTime", "BookTime");
                    bulkCopy.ColumnMappings.Add("BStatus", "BStatus");
                    bulkCopy.ColumnMappings.Add("NoOfCycles", "NoOfCycles");
                    bulkCopy.ColumnMappings.Add("CashMemoNo", "CashMemoNo");
                    bulkCopy.ColumnMappings.Add("CashMemoDate", "CashMemoDate");
                    bulkCopy.ColumnMappings.Add("DeliveryDate", "DeliveryDate");
                    bulkCopy.ColumnMappings.Add("DeliveryTime", "DeliveryTime");
                    bulkCopy.ColumnMappings.Add("UserId", "UserId");
                    bulkCopy.ColumnMappings.Add("InstallationOnBooking", "InstallationOnBooking");
                    bulkCopy.ColumnMappings.Add("IvrsReferenceNo", "IvrsReferenceNo");
                    bulkCopy.ColumnMappings.Add("DBTLStatus", "DBTLStatus");
                    bulkCopy.ColumnMappings.Add("StatusId", "StatusId");
                    bulkCopy.ColumnMappings.Add("CreatedById", "CreatedById");
                    bulkCopy.ColumnMappings.Add("ModifiedById", "ModifiedById");
                    bulkCopy.ColumnMappings.Add("CreationDate", "CreationDate");
                    bulkCopy.ColumnMappings.Add("ModificationDate", "ModificationDate");
                    bulkCopy.ColumnMappings.Add("TimeStamp", "TimeStamp");
                    bulkCopy.DestinationTableName = "ConsumersTemp";
                    try
                    {
                     bulkCopy.WriteToServer(dt);
                     check = true;
                    if (check == true)
                    {
                       c = "File has been uploaded";
                    }
                       
                    }
                    
                    catch (Exception)
                    {
                        return c = "Excel File Template does not match";
                    }
                    SqlCommand cmd = new SqlCommand("SP_Upsert_Customer_Master", con);//"SP_Insert_Item_Class_Attribute_Mapping", con);
                    cmd.CommandTimeout = 3600;
                    cmd.Parameters.AddWithValue("@TimeStamp", TimeStamp);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    
                    SqlCommand cmd1 = new SqlCommand("SP_Delete_CustomerTemp", con);
                    cmd1.CommandTimeout = 3600;
                    cmd1.Parameters.AddWithValue("@TimeStamp", TimeStamp);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }

                return c;
            }
            catch (Exception ex)
            {
                return c;
            }

        }

        public List<String> GetAllStatus(){

            List<String> AllStatus = _context.Consumers.Select(x => x.BStatus).Distinct().ToList();
            return AllStatus;

        }

        public List<ConsumersViewModel> GetAllConsumersAccStatus(string status) 
        {

            List<ConsumersViewModel> c_list = new List<ConsumersViewModel>();
            try
            { 
            c_list = _context.Consumers.Where(x => x.BStatus == status).ToList().Select(ToConsumerViewModel).ToList();
            return c_list;
            }
            catch(Exception ex)
            {
                return c_list;
            }
        }

        public List<ConsumersViewModel> GetAllConsumersAccBookinDate(string startDate, string endDate)
        {
            DateTime start = Convert.ToDateTime(startDate);

            DateTime end = Convert.ToDateTime(endDate);
            try
            {
                return _context.Consumers.Where(x => x.BookingDate >= start && x.BookingDate <= end).ToList().Select(ToConsumerViewModel).ToList();

            }
            catch(Exception ex){
                return null;
            }
        }

        public List<ConsumersViewModel> GetAllCon(string status, string startDate, string endDate)
        {
            List<ConsumersViewModel> c_list = new List<ConsumersViewModel>();
            DateTime start = Convert.ToDateTime(startDate);

            DateTime end = Convert.ToDateTime(endDate);

            //Int64 s = Convert.ToInt32(status);

            try
            {

                c_list = _context.Consumers.Where(x => x.BStatus == status && (x.BookingDate >= start && x.BookingDate <= end)).ToList().Select(ToConsumerViewModel).ToList(); ;

                return c_list;
            }
            catch (Exception ex)
            {
                return c_list;

            }
        }

        #endregion 
    }
}
