using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using NLog.Config;
using NLog.Targets;
using System.IO;
using System.Reflection;
using Loregroup.Core;


namespace Loregroup.Core.Logmodels
{
    public static class LogMe
    {
        /// <summary>
        /// Setup the nLog for logging
        /// </summary>
        /// <param name="directoryToSaveLogs">${basedir} to save in the application directory or specify path.</param>
        /// <param name="commonNameSpace">common namespaces to get logs from .</param>
        public static void SetupLogger(string directoryToSaveLogs, string commonNameSpace)
        {
            // Creating configuration object 
            LoggingConfiguration config = new LoggingConfiguration();

            //Get Assemblies for which we have to handle logs for.
            string executingDirectorypath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Declare list type of object to hold assemblies info.
            List<AssemblyName> assemblies = new List<AssemblyName>();

            //Set Directory path to search dlls.
            DirectoryInfo directory = new DirectoryInfo(executingDirectorypath);

            //First get all the dlls and add into the assemblies list.
            foreach (FileInfo dll in directory.GetFiles("*.dll"))
            {
                //get the assembly name object from assembly.
                AssemblyName assemblyName = AssemblyName.GetAssemblyName(dll.FullName);
                //add assembly into the assembly list.
                assemblies.Add(assemblyName);
            }

            //include cuz exe file is the executable assembly.
            assemblies.Add(Assembly.GetCallingAssembly().GetName());

            // Filter only needed assemblies as there might be any other assemblies in the project.
            assemblies = assemblies.Where(x => x.FullName.Contains(commonNameSpace)).ToList();

            // Set nlog target and rules for each assemblies
            foreach (AssemblyName assembly in assemblies)
            {
                FileTarget fileTarget = new FileTarget();
                config.AddTarget("file", fileTarget);

                // Setting target properties
                //fileTarget.FileName = String.Format("{0}/{1}-{2}.txt", directoryToSaveLogs, assembly.Name, "${shortdate}");
                fileTarget.FileName = String.Format("{0}/{1}.txt", directoryToSaveLogs, assembly.Name);
                fileTarget.FileName = String.Format("{0}/{1}.txt", directoryToSaveLogs, assembly.Name);
                fileTarget.Layout = @"${date:format=dd-MMM-yyyy HH\:mm\:ss} :: ${logger} :: ${message}";

                // Creating Rule
                LoggingRule rule = new LoggingRule(assembly.Name, LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule);
            }

            // Activate the configuration
            LogManager.Configuration = config;
        }

        /// <summary>
        /// To log any type of entry.
        /// <para>for example Accubatch.Logging.LogMe.Log("Title", Common.Enumerations.LogType.Error, "In Dal Error Message");</para>
        /// </summary>
        /// <param name="title">Title of the Class or Method which can be used for debugging errors. </param>
        /// <param name="type">enum for LogType</param>
        /// <param name="message">Message to write.</param>
        public static void Log(String title, LogMeCommonMng.LogType type, String message)
        {
            //Get the calling assembly title.
            string assemblyTitle = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title, Assembly.GetCallingAssembly());

            //  QualityInspectionSys
            //set logger info.
            Logger logger = LogManager.GetLogger(assemblyTitle);

            //set message to write into log.
            string messageToLog = String.Format("{0} :: {1} :: {2}", title, type.ToString(), message);

            //log the message according to its log type
            switch (type)
            {
                case LogMeCommonMng.LogType.Trace:
                    {
                        logger.Trace(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Debug:
                    {
                        logger.Debug(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Info:
                    {
                        logger.Info(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Warn:
                    {
                        logger.Warn(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Error:
                    {
                        logger.Error(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Fatal:
                    {
                        logger.Fatal(messageToLog);
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// To log any type of entry.
        /// <para>for example Accubatch.Logging.LogMe.Log("Title", Common.Enumerations.LogType.Error, "In Dal Error Message");</para>
        /// </summary>
        /// <param name="title">Title of the Class or Method which can be used for debugging errors. </param>
        /// <param name="methodName">Name of the Method causing this log.</param>
        /// <param name="type">enum for LogType</param>
        /// <param name="message">Message to write.</param>
        public static void Log(String title, String methodName, LogMeCommonMng.LogType type, String message)
        {
            //Get the calling assembly title.
            string assemblyTitle = GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title, Assembly.GetCallingAssembly());

            //set logger info.
            Logger logger = LogManager.GetLogger(assemblyTitle);

            //set message to write into log.
            string messageToLog = String.Format("{0} :: {1} :: {2}", title, type.ToString(), message);

            //log the message according to its log type
            switch (type)
            {
                case LogMeCommonMng.LogType.Trace:
                    {
                        logger.Trace(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Debug:
                    {
                        logger.Debug(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Info:
                    {
                        logger.Info(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Warn:
                    {
                        logger.Warn(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Error:
                    {
                        logger.Error(messageToLog);
                        break;
                    }
                case LogMeCommonMng.LogType.Fatal:
                    {
                        logger.Fatal(messageToLog);
                        break;
                    }
                default: break;
            }
        }

        /// <summary>
        /// Get the assembly title.
        /// </summary>
        /// <typeparam name="T">Assembly Attribute Type</typeparam>
        /// <param name="value"></param>
        /// <param name="assembly">Assembly to get Attribute for.</param>
        /// <returns>String Title of the Assembly.</returns>
        public static string GetAssemblyAttribute<T>(Func<T, string> value, Assembly assembly)
        where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(assembly, typeof(T));
            return value.Invoke(attribute);
        }
    }
}
