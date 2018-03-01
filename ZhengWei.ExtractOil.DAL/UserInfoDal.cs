using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.IDAL;
using ZhengWeil.ExtractOil.Model;


namespace ZhengWei.ExtractOil.DAL
{
   public class UserInfoDal:BaseDal<UserInfo>,IUserInfoDal
    {

        public IQueryable<UserInfo> LoadSearchPageEntities(UserInfoSearch us)
        {
            var temp = LoadEntities(c=>true);
            if (!string.IsNullOrEmpty(us.Name))
            {
                temp = temp.Where(w=>w.Name.Contains(us.Name));
            }
            if (!string.IsNullOrEmpty(us.Remark))
            {
                temp = temp.Where(w=>w.Remark.Contains(us.Name));
            }
            us.TotalCount = temp.Count();
            return temp.OrderBy<UserInfo,string>(o=>o.Name).Skip<UserInfo>((us.PageIndex - 1) * us.PageSize).Take<UserInfo>(us.PageSize).OrderBy(s=>s.ID);
        }
    }
}
