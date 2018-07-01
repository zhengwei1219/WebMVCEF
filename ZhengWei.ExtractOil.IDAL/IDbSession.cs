using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhengWei.ExtractOil.IDAL
{
   public interface IDbSession
    {
       IUserInfoDal UserInfoDal { get; set; }
       IRoleInfoDal RoleInfoDal { get; set; }
       IActionInfoDal ActionInfoDal { get; set; }
       IReplysDal ReplysDal { get; set; }
       ITopicsDal TopicsDal { get; set; }
       IDocumentInfoDal DocumentInfoDal { get; set; }
       bool SaveChanges();
    }
}
