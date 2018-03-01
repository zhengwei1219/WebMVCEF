using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.BLL
{
   public class RoleInfoService:BaseService<RoleInfo>,IRoleInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.RoleInfoDal;
        }


        public void DeleteEntitys(List<int> Ids)
        {
            var roleList = this.DbSession.RoleInfoDal.LoadEntities(s=>Ids.Contains(s.ID));
            foreach (var role in roleList)
            {
                this.DbSession.RoleInfoDal.DeleteEntity(role);
            }
            this.DbSession.SaveChanges();
        }
    }
   
}
