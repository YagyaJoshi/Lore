using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace Loregroup.Core.Logmodels
{
    public static class LogMeCommonMng
    {
        // public static readonly string LogPath = "C:\\ProgramData\\logs";
        public static readonly string LogPath = (AppDomain.CurrentDomain.BaseDirectory) + "\\pwalogs\\Logs\\";

        public enum LogType
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal
        }

        /// <summary>
        /// Class To Get Enum Description
        /// </summary>
        public static string GetDesc(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        public enum enm_Log_Title
        {
            [Description("Batch")]
            Batch,
            [Description("Batch Sequence")]
            BatchSequence,
            [Description("Event")]
            Event,
            [Description("Ingredient")]
            Ingredient,
            [Description("Ink")]
            Ink,
            [Description("Key")]
            Key,
            [Description("Lab")]
            Lab,
            [Description("Language")]
            Language,
            [Description("ProdSchedule")]
            ProdSchedule,
            [Description("Property")]
            Property,
            [Description("Report Header")]
            ReportHeader,
            [Description("Report Sequence")]
            ReportSequence,
            [Description("Sequence")]
            Sequence,
            [Description("Substrate")]
            Substrate,
            [Description("User")]
            User,
            //
            [Description("Security Manager")]
            SecurityManager,
            [Description("Report Manager")]
            ReportManager,
            [Description("Authorization Manager")]
            AuthorizationManager,
            [Description("Language Manager")]
            LanguageManager,
            [Description("Property Manager")]
            PropertyManager,
            [Description("BackupRoutine Manager")]
            BackupRoutineManager,
        }

        public enum enm_Log_Key
        {
            [Description("Unable To Check Licence Key")]
            Check_LicenceKey,
            [Description("Unable To Update Key")]
            Update_Key,
            [Description("Unable To Save Key")]
            Save_Key,
            [Description("Unable To Select Key")]
            Select_Key,

            [Description("Update Key")]
            Update_Key_,
            [Description("Save Key")]
            Save_Key_,
            [Description("Select Key")]
            Select_Key_,
        }
    }
}
