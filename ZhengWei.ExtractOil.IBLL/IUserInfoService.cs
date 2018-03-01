using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.IBLL
{
   public interface IUserInfoService:IBaseService<UserInfo>
    {
       IQueryable<UserInfo> LoadSearchPageEntities(UserInfoSearch us);
       bool SaveUserRoleInfo(UserInfo userInfo,List<int> roleIds);
    }
}
