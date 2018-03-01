using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ZhengWei.ExtractOil.DALFactory
{
   public class DbSessionFactory
    {
       public DbSession CreateDbSessoin()
       {
           DbSession dbSession = CallContext.GetData("dbSession") as DbSession;
           if (dbSession == null)
           {
               dbSession = new DbSession();
               CallContext.SetData("dbSession",dbSession);
           }
           return dbSession;
       }
    }
}
