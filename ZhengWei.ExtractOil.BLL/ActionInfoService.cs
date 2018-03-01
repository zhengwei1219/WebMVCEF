using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWeil.ExtractOil.Model;
using ZhengWei.ExtractOil.IBLL;
namespace ZhengWei.ExtractOil.BLL
{
   public class ActionInfoService:BaseService<ActionInfo>,IActionInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.ActionInfoDal;
        }
    }
}
