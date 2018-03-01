using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.IDAL;

namespace ZhengWei.ExtractOil.DALFactory
{
    /// <summary>
    /// 抽象工厂，利用反射来创建对象
    /// </summary>
   public class DALAbstractFactory
    {
       private static readonly string DalNameSpace = ConfigurationManager.AppSettings["DalNameSpace"];

       private static readonly string DalAssembly = ConfigurationManager.AppSettings["DalAssembly"];

       public static IUserInfoDal CreateUserInfoDal()
       {
           string fullClassName = DalNameSpace + ".UserInfoDal";
          return CreateInstance(fullClassName, DalAssembly) as IUserInfoDal;
       }
       public static IRoleInfoDal CreateRoleInfoDal()
       {
           string fullClassName = DalNameSpace + ".RoleInfoDal";
           return CreateInstance(fullClassName,DalAssembly) as IRoleInfoDal;
       }
       public static IActionInfoDal CreateActionInfoDal()
       {
           string fullClassName = DalNameSpace + ".ActionInfoDal";
           return CreateInstance(fullClassName,DalAssembly) as IActionInfoDal;
       }
       private static Object CreateInstance(string fullClassName, string DalAssembly)
       {
           
           //加载程序集
           var assembly = Assembly.Load(DalAssembly);
           //创建实例
          return assembly.CreateInstance(fullClassName);
       }
    }
}
