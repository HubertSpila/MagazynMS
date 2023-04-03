using NLog;

namespace WarehouseManagmentAPI.Tools
{
    class NLogConfig
    {
        public static Logger log;

        public static void InitLog()
        {
            try
            {
                log = LogManager.GetLogger("log");
                LogManager.Configuration.Variables["logDir"] = @"C:\Users\Hubert\Desktop\Praca inżynierska\import\Log\";
                //LogManager.Configuration.Variables["logDir"] = @"D:\Studia\Log\";
                LogManager.Configuration.Variables["logName"] = "Errors";
                LogManager.ReconfigExistingLoggers();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message, ex.InnerException);
            }
        }

        public static void WriteLog(string text)
        {
            try
            {
                log.Error(text);
            }
            catch(Exception ex)
            {
                InitLog();
                log.Error(text);
            }
        }
    }
}