using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.IBLL
{
   public interface IRoleInfoService:IBaseService<RoleInfo>
    {
       void DeleteEntitys(List<int> Ids);
    }
}
