
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZhengWei.ExtractOil.WebApp
{
    /// <summary>
    /// 日志记录服务
    /// </summary>
    public class LogService
    {
        public static ILog logger;
        /// <summary>
        /// 常规操作记录
        /// </summary>
        /// <param name="operationString"></param>
        public static void OperationLog(string operationString)
        {
            logger = log4net.LogManager.GetLogger("OperationLogger");
            logger.Info(operationString);
        }
        /// <summary>
        /// 邮件发送记录
        /// </summary>
        /// <param name="EmailString"></param>
        public static void EmailLog(string EmailString)
        {
            logger = log4net.LogManager.GetLogger("EmailLogger");
            logger.Info(EmailString);
        }
        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="warn"></param>
        public static void WarnLog(string warn)
        {
            logger = log4net.LogManager.GetLogger("WarnLogger");
            logger.Warn(warn);
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="error"></param>
        public static void ErrorLog(string error)
        {
            logger = log4net.LogManager.GetLogger("ErrorLogger");
            logger.Error(error);
        }
        /// <summary>
        /// FTP操作信息
        /// </summary>
        /// <param name="operationString"></param>
        internal static void FtpOperationString(string operationString)
        {
            logger = log4net.LogManager.GetLogger("FTPLogger");
            logger.Info(operationString);
        }
    }
}