using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZhengWei.ExtractOil.BLL;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;
using System.Web.Security;
namespace ZhengWei.ExtractOil.WebApp
{
    public class TopicsController : Controller
    {
        //
        // GET: /Topics/
        ITopicsService topicsService = new TopicsService();
        IReplysService replyService = new ReplysService();
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新增
        /// </summary>
        public void AddTopics()
        {
            string author = Request["author"];
            string title = Request["title"];
            string content = Request["content"];
            if (string.IsNullOrEmpty(author)) author = "匿名";
            Topics one = new Topics();
            one.Title = title.Trim();
            one.Author = author.Trim();
            one.Content = content.Trim();
            one.UserHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            one.AddDate = DateTime.Now;
            one.LastModifyDate = DateTime.Now;
            one.UserName = System.Web.HttpContext.Current.User.Identity.Name;
            topicsService.AddEntity(one);
        
        }
        /// <summary>
        /// 修改
        /// </summary>
        public void UpdateTopics()
        {
            int id = int.Parse(Request["Id"]);
            string author=Request["author"];
            string title=Request["title"];
            string content = Request["content"];
        }

        /// <summary>
        /// 展示留言
        /// </summary>
        /// <returns></returns>
        public string ShowTopicsList()
        {
            int currentPage = int.Parse(Request["page"]);
            StringBuilder sb = new StringBuilder();
            int totalCount = 0;
            int pageSize = 5;
            List<Topics> topics = topicsService.LoadPageEntities(currentPage, pageSize, out totalCount,s => !string.IsNullOrEmpty(s.Content), s => s.AddDate, false).ToList();
            ListTopics(sb, topics);
            sb.Append(PageList(pageSize, "showList", totalCount, currentPage));
            return sb.ToString();
        }
        private static void ListTopics(StringBuilder sb, List<Topics> topics)
        {
            for (int i = 0; i < topics.Count; i++)
            {
                sb.Append("<dl><dt>" + topics[i].Title + "</dt>");
                sb.Append("<dd>作者：" + topics[i].Author + " 最后修改时间：" + topics[i].LastModifyDate);
                    sb.Append("  　IP地址：" + topics[i].UserHostAddress);
                sb.Append("</dd>");
                sb.Append("<dd>" + topics[i].Content + "</dd></dl>");
                sb.Append("<dl id='msgFoot'><dd><a href='javascript:void(0)' onclick='showReply(" + topics[i].Id + ")'>查看回复</a>");
                if (topics[i].UserName == System.Web.HttpContext.Current.User.Identity.Name)
                    sb.Append(" <a href='javascript:void(0)' onclick='modiTopic(" + topics[i].Id + ",\"" + topics[i].Title + "\"" + ",\"" + topics[i].Author + "\"" + ",\"" + topics[i].Content + "\")'>修改 </a>");

                if (topics[i].UserName == System.Web.HttpContext.Current.User.Identity.Name)
                    sb.Append(" <a href='javascript:void(0)' onclick='delTopic(" + topics[i].Id + ")'>删除 </a>");
                    sb.Append("<a href='javascript:void(0)' onclick='showReplyBar(" + topics[i].Id + ")'>回复</a>");
                sb.Append("</dd><dd id='reply" + topics[i].Id.ToString() + "' class='reply'></dd></dl>");
            }
        }
        /// <summary>
        /// 实现数据集的分页
        /// </summary>
        /// <param name="size">每页留言条数</param>
        /// <param name="funClick">函数名称</param>
        /// <param name="countNum">记录总数</param>
        /// <param name="currentPage">当前第几页</param>
        /// <returns></returns>
        public static string PageList(int size, string funClick, int countNum, int currentPage)
        {
            int pageCount = 0;
            int stepNum = 3;
            int pageRoot = 1;
            string pageStr = "";
            int pageSize = size;
            if (countNum % pageSize == 0)
            {
                pageCount = countNum / pageSize;
            }
            else
            {
                pageCount = countNum / pageSize + 1;
            }
            pageStr = "<div id='pager'>分页：" + currentPage.ToString() + "/" + pageCount.ToString() + "页";
            if (currentPage - stepNum > 1)
            {
                pageRoot = currentPage - stepNum;
            }
            int pageFoot = pageCount;
            if (currentPage + stepNum < pageCount)
            {
                pageFoot = currentPage + stepNum;
            }
            if (pageRoot == 1)
            {
                if (currentPage == 1)
                {
                    pageStr += "<font color=888888 face=webdings>9</font></a>";
                    pageStr += "<font color=888888 face=webdings>7</font></a> ";
                }
                else
                {
                    pageStr += "<a href='#page.1' onclick='" + funClick + "(1)' title='首页'><font face=webdings>9</font></a>";
                    pageStr += "<a href='#page." + Convert.ToString(currentPage - 1) + "' onclick='" + funClick + "(" + Convert.ToString(currentPage - 1) + ")' title='上页'><font face=webdings>7</font></a> ";
                }
            }
            else
            {
                pageStr += "<a href='#page.1' onclick='" + funClick + "(1)' title='首页'><font face=webdings>9</font></a>";
                pageStr += "<a href='#page." + Convert.ToString(currentPage - 1) + "' onclick='" + funClick + "(" + Convert.ToString(currentPage - 1) + ")' title='上页'><font face=webdings>7</font></a>...";
            }
            for (int i = pageRoot; i <= pageFoot; i++)
            {
                if (i == currentPage)
                {
                    pageStr += "<font color='red'>[" + i.ToString() + "]</font>&nbsp;";
                }
                else
                {
                    pageStr += "<a href='#page." + i.ToString() + "' onclick='" + funClick + "(" + i.ToString() + ")'>[" + i.ToString() + "]</a>&nbsp;";
                }
                if (i == pageCount)
                    break;
            }
            if (pageFoot == pageCount)
            {
                if (pageCount == currentPage)
                {
                    pageStr += "<font color=888888 face=webdings>8</font></a>";
                    pageStr += "<font color=888888 face=webdings>:</font></a>";
                }
                else
                {
                    pageStr += "<a href='#page." + Convert.ToString(currentPage + 1) + "' onclick='" + funClick + "(" + Convert.ToString(currentPage + 1) + ")' title='下页'><font face=webdings>8</font></a>";
                    pageStr += "<a href='#page." + pageCount.ToString() + "' onclick='" + funClick + "(" + pageCount.ToString() + ")' title='尾页'><font face=webdings>:</font></a>";
                }
            }
            else
            {
                pageStr += "... <a href='#page." + Convert.ToString(currentPage + 1) + "' onclick='" + funClick + "(" + Convert.ToString(currentPage + 1) + ")' title='下页'><font face=webdings>8</font></a>";
                pageStr += "<a href='#page." + pageCount.ToString() + "' onclick='" + funClick + "(" + pageCount.ToString() + ")' title='尾页'><font face=webdings>:</font></a>";
            }
            pageStr += "</div>";
            return pageStr;
        }
        /// <summary>
        /// 显示回复条
        /// </summary>
        /// <returns></returns>
        public string ShowReplyBar()
        {

            int tid = int.Parse(Request["id"]);
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='replyMsg" + tid.ToString() + "'></div><textarea id='replyContent" + tid.ToString() + "' cols='80' rows='4'></textarea><br><input type='button' onclick='addReply(" + tid + ")' value='发表回复'>");
            return sb.ToString();
        }
        /// <summary>
        /// 增加回复
        /// </summary>
        public bool AddReply()
        {
            int id = int.Parse(Request["id"]);
            string content = Request["content"];
            Topics topics = topicsService.LoadEntities(s => s.Id == id).FirstOrDefault();
            if (topics != null)
            {
                Replys one = new Replys();
                one.ReplyDate = DateTime.Now;
                one.TopicId = id;
                one.Content = content;
                one.ReplyName = System.Web.HttpContext.Current.User.Identity.Name;
                one.UserHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
                return replyService.AddEntity(one) != null;
            }
            return false;
        }

    }
}
