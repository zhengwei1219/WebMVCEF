using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.DAL
{
   public class DbContextFactory
    {
       /// <summary>
       /// 保证一次请求只创建一个EF上下文
       /// </summary>
       /// <returns></returns>
       public static DbContext CreateDbContext()
       {
           DbContext dbContext = CallContext.GetData("dbContext") as DbContext;
           if (dbContext == null)
           {
               dbContext = new ExtractOilEntities();
               CallContext.SetData("dbContext",dbContext);
           }
           return dbContext;
       }
    }
}
