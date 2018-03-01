using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace ZhengWei.ExtractOil.WebApp
{
    /// <summary>
    /// Ftp上传文件/图件类
    /// 需要Appsetting设置ftp用户访问参数：
    /// 网址AppSettings["FTPServer"] 
    /// 端口(默认21)AppSettings["FTPPoint"]
    /// 用户名AppSettings["FTPUid"]
    /// 密码AppSettings["FTPPwd"]
    /// 起始子目录AppSettings["FTPDocDir"]
    /// 默认为AppSettings["FTPDocDir"] 如果上传其他请上传前设置路径 dir等
    /// </summary>
    public class FTPProcess
    {
        #region 变量声明

        /// <summary>
        /// 服务器连接地址
        /// </summary>
        public string server;

        /// <summary>
        /// 登陆帐号
        /// </summary>
        public string user;

        /// <summary>
        /// 登陆口令
        /// </summary>
        public string pass;

        /// <summary>
        /// 端口号
        /// </summary>
        public int port;
        /// <summary>
        /// ftp顶目录下的存放子目录
        /// </summary>
        public string dir;
        /// <summary>
        /// ftp当前的访问目录(相对于服务器根目录);上传文件到不同目录时 注意切换; 并带上‘\’
        /// </summary>
        public string currentDir;
        /// <summary>
        ///ftpURI 当前根目录带'/'分隔符(每次处理注意记录)  
        ///初始化默认为服务器顶级根 ftp路径(包括端口):"ftp://" + server + ":" + port + "/" ;
        /// </summary>
        private string ftpURI;
        /// <summary>
        /// upload初始上传 需要检查目录
        /// </summary> 
        #endregion
        /// <summary>
        /// 构造函数(自动提取配置文件中的 连接参数 上传文档)
        /// </summary>
        public FTPProcess()
        {
            server = ConfigurationManager.AppSettings["FTPServer"];
            user = ConfigurationManager.AppSettings["FTPUid"];
            pass = ConfigurationManager.AppSettings["FTPPwd"];
            port = string.IsNullOrEmpty(ConfigurationManager.AppSettings["FTPPoint"]) ? 21 : int.Parse(ConfigurationManager.AppSettings["FTPPoint"]);
            dir = ConfigurationManager.AppSettings["FTPDocDir"];
            ftpURI = "ftp://" + server + ":" + port + "/";
            currentDir = "";
        }
        /// <summary>
        /// 构造函数(自动提取配置文件中的 连接参数)，如果是文档。
        /// type:0 上传的子目录为AppSettings["FTPDocDir"];图件type:1为AppSettings["FTPMapDir"],
        /// 原始数据体type:2为AppSettings["FTPOriginVolDir"],3：解编AppSettings["FTPDemultiplexVolDir"]；4：预处理AppSettings["FTPPrePreProcessVolDir"]
        /// 5：处理AppSettings["FTPProcessVolDir"]；6：解释AppSettings["FTPInterpVolDir"];7:服务商现场处理AppSettings["FTPSiteProcessVolDir"];8:地震测井原始数据体AppSettings["FTPSeisLogVolDir"]
        /// </summary>
        /// <param name="type">上传文件的类型(默认为文档)</param>
        public FTPProcess(int type)
        {
            server = ConfigurationManager.AppSettings["FTPServer"]; ;
            user = ConfigurationManager.AppSettings["FTPUid"].Trim();
            pass = ConfigurationManager.AppSettings["FTPPwd"].Trim();
            port = string.IsNullOrEmpty(ConfigurationManager.AppSettings["FTPPoint"]) ? 21 : int.Parse(ConfigurationManager.AppSettings["FTPPoint"]);
            switch (type)
            {
                case 0:
                    dir = ConfigurationManager.AppSettings["FTPDocDir"];
                    break;
                case 1:
                    dir = ConfigurationManager.AppSettings["FTPMapDir"];
                    break;
                case 2:
                    dir = ConfigurationManager.AppSettings["FTPOriginVolDir"];
                    break;
                case 3:
                    dir = ConfigurationManager.AppSettings["FTPDemultiplexVolDir"];
                    break;
                case 4:
                    dir = ConfigurationManager.AppSettings["FTPPreProcessVolDir"];
                    break;
                case 5:
                    dir = ConfigurationManager.AppSettings["FTPProcessVolDir"];
                    break;
                case 6:
                    dir = ConfigurationManager.AppSettings["FTPInterpVolDir"];
                    break;
                case 7:
                    dir = ConfigurationManager.AppSettings["FTPSiteProcessVolDir"];
                    break;
                case 8:
                    dir = ConfigurationManager.AppSettings["FTPSeisLogVolDir"];
                    break;
                default:
                    dir = ConfigurationManager.AppSettings["FTPDocDir"];
                    break;
            }
            ftpURI = "ftp://" + server + ":" + port + "/";
            currentDir = "";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FTPProcess" /> class.
        /// </summary>
        /// <param name="Server">服务地址/IP</param>
        /// <param name="UserName">访问用户名</param>
        /// <param name="PassWord">访问用户密码</param>
        /// <param name="Point">FTP端口</param>
        /// <param name="Dir">指定目录</param>
        public FTPProcess(string Server, string UserName, string PassWord, int Port, string Dir)
        {
            server = Server;
            user = UserName;
            pass = PassWord;
            port = Port;
            dir = Dir;
            ftpURI = "ftp://" + server + ":" + port + "/";
            currentDir = "";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FTPProcess"/> class.
        /// </summary>
        /// <param name="Server">The server.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="PassWord">The pass word.</param>
        public FTPProcess(string Server, string UserName, string PassWord)
        {
            server = Server;
            user = UserName;
            pass = PassWord;
            port = 21;
            dir = "";
            ftpURI = "ftp://" + server + "/";
            currentDir = "";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FTPProcess"/> class.
        /// </summary>
        /// <param name="Server">The server.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="PassWord">The pass word.</param>
        /// <param name="Dir">The dir.</param>
        public FTPProcess(string Server, string UserName, string PassWord, string Dir)
        {
            server = Server;
            user = UserName;
            pass = PassWord;
            dir = Dir;
            port = 21;
            ftpURI = "ftp://" + server + "/";
            currentDir = "";
        }
        /// <summary>
        /// Uploads the specified file stream.
        /// </summary>
        /// <param name="FileStream">The file stream.</param>
        /// <param name="TargetFileNameWithExtend">The target file name with extend.</param>
        /// <returns>System.String.</returns>
        public string Upload(Stream FileStream, string TargetFileNameWithExtend)
        {
            return Upload(FileStream, TargetFileNameWithExtend, true);
        }
        /// <summary>
        /// 上传文件流(到 dir文件目录并 自动新增时间文件夹),并返回重命名后的文件名；失败则返回null,如果目标文件名称不执行上传并返回""
        /// </summary>
        /// <param name="FileStream">上传文件的文件流</param>
        /// <param name="TargetFileNameWithExtend">目标文件名</param>
        /// <param name="Rename">是否重命名文件 (再文件名前 添加 guid_)</param>
        /// <returns>System.String.</returns>
        public string Upload(Stream FileStream, string TargetFileNameWithExtend, bool Rename)
        {
            // 初次上传 需定为目标目录
            if (string.IsNullOrEmpty(currentDir))
            {
                if (string.IsNullOrEmpty(TargetFileNameWithExtend)) return "";
                TargetFileNameWithExtend.Replace(" ", "");
                //判断当前目录是否有 顶级 dir目录
                if (!DirectoryExist(dir))
                {
                    //没有就新建配置顶级目录
                    MakeDir(dir);
                }
                //自动加上年文件目录
                string YearDir = DateTime.Now.Year.ToString();
                if (!DirectoryExist(YearDir))
                {
                    //没有就新建时间目录
                    MakeDir(YearDir);
                }
                //自动加上日期文件目录
                string dateDir = DateTime.Now.ToString("MM-dd");
                if (!DirectoryExist(dateDir))
                {
                    //没有就新建时间目录
                    MakeDir(dateDir);
                }
            }
            string TargetPath;
            if (Rename) TargetPath = Guid.NewGuid().ToString() + "_" + TargetFileNameWithExtend;
            else TargetPath = TargetFileNameWithExtend.Replace(" ", "");
            Uri uri = new Uri(Path.Combine(ftpURI, currentDir, TargetPath));
            // 建立Ftp 连接
            FtpWebRequest fwr = (FtpWebRequest)WebRequest.Create(uri);
            fwr.Credentials = new NetworkCredential(user, pass);
            fwr.Method = WebRequestMethods.Ftp.UploadFile;
            fwr.UseBinary = true;
            fwr.UsePassive = true;
            fwr.ContentLength = FileStream.Length;
            try
            {
                // 建上传的文件流 写入ftp链接
                Stream stream = fwr.GetRequestStream();
                int bufferlength = 1024 * 10;
                byte[] b = new byte[bufferlength];
                int contentLen = FileStream.Read(b, 0, bufferlength);
                while (contentLen != 0)
                {
                    stream.Write(b, 0, contentLen);
                    contentLen = FileStream.Read(b, 0, bufferlength);
                }
                stream.Close();
                LogService.FtpOperationString(string.Format("FTP上传文件成功，文件名为：{0} !", TargetFileNameWithExtend));
                return TargetPath;
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString(string.Format("FTP上传文件失败：文件名为：{0}；原因：{1}!", TargetFileNameWithExtend, ex.Message));
                return null;
            }
        }

        /// <summary>
        /// 通过文件名 从当前目录下载到指定路径
        /// </summary>
        /// <param name="filePath">目标文件目录</param>
        /// <param name="fileName">源文件名,开头不能带‘\’</param>
        public void Download(string filePath, string fileName)
        {
            fileName = TrimSpliter(fileName);
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
                string temppath;
                if (string.IsNullOrEmpty(currentDir))
                {
                    temppath = Path.Combine(ftpURI, fileName);
                }
                else
                {
                    temppath = Path.Combine(ftpURI, currentDir, fileName);
                }
                Uri uri = new Uri(temppath);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                int bufferSize = 1024 * 10;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString(string.Format("FtpWeb Download Error --> FileName:{0} ;Error Info:{1}", fileName, ex.Message));
            }
        }
        /// <summary>
        /// 通过文件名下载当前目录的文档(非图件)
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Stream.</returns>
        public Stream Download(string fileName)
        {
            return Download(fileName, true);
        }
        /// <summary>
        /// 通过文件名下载指定目录的文档(非图件)，若果不是当前目录，直接从服务器节点定位
        /// </summary>
        /// <param name="fileName">文档名称(可包括相对路径)</param>
        /// <param name="IsCurrentDir">是否为相对于当前目录</param>
        /// <returns>Stream.</returns>
        public Stream Download(string fileName, bool IsCurrentDir)
        {
            fileName = TrimSpliter(fileName);
            MemoryStream outputStream = new MemoryStream();
            FtpWebRequest reqFTP;
            try
            {
                string temppath;
                if (IsCurrentDir)
                {
                    temppath = Path.Combine(ftpURI, currentDir, fileName);
                }
                else
                {
                    temppath = Path.Combine(ftpURI, fileName);
                }
                Uri uri = new Uri(temppath);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                int bufferSize = 1024 * 10;
                int readCount;
                byte[] buffer = new byte[bufferSize];
                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                ftpStream.Close();
                LogService.FtpOperationString("FtpWeb Download Success --> " + "  文件名:" + fileName);
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString(string.Format("FtpWeb Download Error --> FileName:{0} ;Error Info:{1}", fileName, ex.Message));
            }
            return outputStream;
        }
        /// <summary>
        /// 通过文件名删除文件当前目录中的文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            Delete(fileName, true);
        }
        /// <summary>
        /// 通过文件名删除文件当前目录中的文件,不切换当前目录
        /// </summary>
        /// <param name="fileName">文件名(不包含'/').</param>
        /// <param name="IsCurrentDir">是否为当前目录路径</param>
        public void Delete(string fileName, bool IsCurrentDir)
        {
            fileName = TrimSpliter(fileName);
            try
            {
                string temppath;
                if (IsCurrentDir)
                {
                    temppath = Path.Combine(ftpURI, currentDir, fileName);
                }
                else
                {
                    temppath = Path.Combine(ftpURI, fileName);
                }
                Uri uri = new Uri(temppath);
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Credentials = new NetworkCredential(user, pass);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
                LogService.FtpOperationString("FtpWeb Delete Success --> " + "  文件名:" + fileName);
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString("FtpWeb Delete Error --> " + ex.Message + "  文件名:" + fileName);
            }
        }
        /// <summary>
        /// 判断当前目录下指定的子目录是否存在,如果有,当前目录定为子目录,否则还是定位在当前目录
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (!string.IsNullOrEmpty(str) && str.Trim() == RemoteDirectoryName.Trim())
                {
                    currentDir = currentDir + RemoteDirectoryName + "\\";
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// 默认FTP访问文件属性 最后一项问文件名称并且以空格‘ ’与前面属性分隔
        /// </summary>
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            List<string> result = new List<string>();
            foreach (string str in drectory)
            {
                if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    result.Add(str.Split(new char[] { ' ' }).Last());
                }
            }
            return result.ToArray();
        }
        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            string[] downloadFiles;
            try
            {
                List<string> result = new List<string>();
                FtpWebRequest ftp;
                string path = Path.Combine(ftpURI, currentDir);
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                ftp.Credentials = new NetworkCredential(user, pass);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Add(line);
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return result.ToArray();
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                LogService.FtpOperationString("FtpWeb GetFilesDetailList Error --> " + ex.Message);
            }
            return downloadFiles;
        }
        /// <summary>
        /// 获取当前目录下文件列表(仅文件)  
        /// 如果当前目录不是最底层目录，列出的文件列表自动包含父级目录
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(string mask)
        {
            string[] downloadFiles;
            List<string> result = new List<string>();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(Path.Combine(ftpURI, currentDir)));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pass);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {
                        string mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Add(line);
                        }
                    }
                    else
                    {
                        result.Add(line);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return result.ToArray();
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                //if (ex.Message.Trim() != "远程服务器返回错误: (550) 文件夹不可用(例如，未找到文件夹，无法访问文件夹)。")
                //{
                LogService.FtpOperationString(string.Format("FtpWeb GetFileList Error -->Error Info:{1}", ex.Message.ToString()));
                //}
                return downloadFiles;
            }
        }
        /// <summary>
        /// 判断当前目录下指定的文件是否存在
        /// </summary>
        /// <param name="RemoteFileName">文件名</param>
        public bool FileExist(string RemoteFileName)
        {
            string[] fileList = GetFileList("*.*");
            foreach (string str in fileList)
            {
                if (str == RemoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 在当前目录创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                string temppath;
                temppath = ftpURI + currentDir + dirName + "\\";
                Uri uri = new Uri(temppath);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                currentDir = currentDir + dirName + "\\";
                LogService.FtpOperationString(string.Format("FtpWeb MakeDir Success -->{0} ", dirName));
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString("FtpWeb MakeDir Error --> " + ex.Message);
            }
        }
        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void ReName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                string temppath = Path.Combine(ftpURI, currentDir, currentFilename);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(temppath));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(user, pass);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                ftpStream.Close();
                response.Close();
                LogService.FtpOperationString(string.Format("FtpWeb ReName:Success --> {0} to {1} ", currentFilename, newFilename));
            }
            catch (Exception ex)
            {
                LogService.FtpOperationString("FtpWeb ReName Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void MovieFile(string currentFilename, string newDirectory)
        {
            ReName(currentFilename, newDirectory);
        }
        /// <summary>
        /// 切换当前目录
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <param name="IsRoot">相对路径</param> 
        public void GotoDirectory(string DirectoryName)
        {
            currentDir = DirectoryName;
        }
        /// <summary>
        /// 去掉路径的前分隔符 ‘/’ '\'
        /// </summary>
        /// <param name="Str">The STR.</param>
        /// <returns>System.String.</returns>
        public string TrimSpliter(string Str)
        {
            if (!string.IsNullOrEmpty(Str))
            {
                return Str.TrimStart(new char[] { '\\', '/' });
            }
            return Str;
        }
    }
}