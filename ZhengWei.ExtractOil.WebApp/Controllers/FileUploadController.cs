using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;


namespace ZhengWei.ExtractOil.WebApp.Controllers
{
    public class FileUploadController : Controller
    {
        IDocumentInfoService docS = new DocumentInfoService();
        #region 上传文件至FTP
        /// <summary>
        /// 上传文档管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult DocumentToPTF()
        {
            return View();
        }

        /// <summary>
        /// 上传文档视图页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddDoc()
        {
            return View();
        }
        /// <summary>
        /// 上传文件至PTF，并保存记录到数据库
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
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
                    //保存到数据库
                    DocumentInfo doc = new DocumentInfo();
                    doc.CreateDate = DateTime.Now;
                    doc.FileName = fileData.FileName;
                    doc.FilePath = ftppath;
                    
                    docS.AddEntity(doc);

                }
            }
            catch (Exception ex)
            {

                throw new Exception("上传至FTP文件出错");
            }
            return Json(new { IsSuccess = true, ErrorMessage = "成功" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取所有上传的文档记录
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllDoc()
        {
            try
            {
                List<DocumentInfo> docList = docS.LoadEntities(s => !string.IsNullOrEmpty(s.FilePath)).ToList();
                return Json(docList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        private string ReName(string FileName)
        {
            if (string.IsNullOrEmpty(FileName)) return "";
            string filename = FileName.Trim().Replace("&", "-").Replace(" ", "");
            filename = Guid.NewGuid().ToString() + "_" + filename;
            return filename;
        }
        #endregion
        

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
