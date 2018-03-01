using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.IDAL
{
   public interface IUserInfoDal:IBaseDal<UserInfo>
    {
       IQueryable<UserInfo> LoadSearchPageEntities(UserInfoSearch us);
    }
}
