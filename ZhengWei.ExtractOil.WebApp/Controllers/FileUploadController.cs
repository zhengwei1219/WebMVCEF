using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/

        public ActionResult Document()
        {
            return View();
        }

        //新增文档
        public ActionResult AddDoc()
        {
            return View();
        }

        public ActionResult ImportDoc(HttpPostedFileBase fileData)
        {
            try
            {
                if (fileData != null)
                {
                    string fileName = fileData.FileName;
                    string filesAndPath = ReName(fileName);
                    //上传记录文件至FTP
                    string ftppath = FTPProcessRecord.UploadMap(fileData.InputStream, filesAndPath);
                }
            }
            catch (Exception ex)
            {

                throw new Exception("上传至FTP文件出错");
            }
            return Json(new { IsSuccess = true, ErrorMessage = "成功"}, JsonRequestBehavior.AllowGet);
        }

        private string ReName(string FileName)
        {
            if (string.IsNullOrEmpty(FileName)) return "";
            string filename = FileName.Trim().Replace("&", "-").Replace(" ", "");
            filename = Guid.NewGuid().ToString() + "_" + filename;
            return filename;
        }

        /// <summary>
        /// 无刷新文件上传
        /// </summary>
        /// <returns></returns>
        public ViewResult DocumentNoFulsh()
        {
            return View();
        }

    }
}
