using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace ZhengWei.ExtractOil.WebApp
{
    public class FTPProcessRecord
    {
        internal static string UploadMap(Stream stream, string FileName)
        {
           
            FTPProcess ftp = new FTPProcess(1);
            string ftppath = ftp.Upload(stream, FileName, false);
            if (string.IsNullOrEmpty(ftppath)) return "";
            try
            {

                string targetPath = GetFtpRoot() + ftp.currentDir + ftppath;
                
                return targetPath;
            }
            //文件记录上传失败的话 删除上传的文件
            catch (Exception ex)
            {
                ftp.Delete(ftppath);
                throw new Exception("上传到FTP服务器失败，原因:" + ex.Message);
                return "";
            }
        }

        /// <summary>
        ///该系统使用的ftp根目录  带'\' 
        /// </summary>
        /// <returns>System.String.</returns>
        internal static string GetFtpRoot()
        {
            string Temp = ConfigurationManager.AppSettings["FTPRoot"];
            if (!string.IsNullOrEmpty(Temp) && !Temp.EndsWith("\\"))
            {
                return Temp += "\\";
            }
            return Temp;
        }
    }
}