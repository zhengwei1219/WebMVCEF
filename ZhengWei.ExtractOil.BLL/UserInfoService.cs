using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.IBLL;
using ZhengWei.ExtractOil.IDAL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.BLL
{
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal =  this.DbSession.UserInfoDal;
        }

        public IQueryable<UserInfo> LoadSearchPageEntities(UserInfoSearch us)
        {
           return this.DbSession.UserInfoDal.LoadSearchPageEntities(us);
        }


        public bool SaveUserRoleInfo(UserInfo userInfo, List<int> roleIds)
        {
            userInfo.RoleInfo.Clear();
            List<RoleInfo> roles = new List<RoleInfo>();
            foreach (var item in roleIds)
            {
               RoleInfo role= this.DbSession.RoleInfoDal.LoadEntities(s=>s.ID==item).FirstOrDefault();
                roles.Add(role);
            }
            userInfo.RoleInfo = roles;
            return this.DbSession.SaveChanges();
        }
    }
}
