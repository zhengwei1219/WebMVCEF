using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.DAL;
using ZhengWei.ExtractOil.IDAL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.DALFactory
{
   public class DbSession : IDbSession
    {
       //DbContext Db = new ExtractOilEntities();
       public DbContext Db
       {
           get { return DbContextFactory.CreateDbContext(); }
       }
       private IUserInfoDal _UserInfoDal;
       public IUserInfoDal UserInfoDal
       {
           get {
                   if (_UserInfoDal == null)
                   {
                      // _UserInfoDal = new UserInfoDal();
                       _UserInfoDal = DALAbstractFactory.CreateUserInfoDal();
                   }
                   return _UserInfoDal;          
               }
           set { _UserInfoDal = value; }
       }
       private IRoleInfoDal _RoleInfoDal;
       public IRoleInfoDal RoleInfoDal
       {
           get {
               if (_RoleInfoDal == null)
               {
                   _RoleInfoDal = DALAbstractFactory.CreateRoleInfoDal();
               }
               return _RoleInfoDal;
           }
           set { _RoleInfoDal = value; }
       }
       private IActionInfoDal _ActionInfoDal;
       public IActionInfoDal ActionInfoDal
       {
           get {
               if (_ActionInfoDal == null)
               {
                   _ActionInfoDal = DALAbstractFactory.CreateActionInfoDal();
               }
               return _ActionInfoDal;
           }
           set { _ActionInfoDal = value; }
       }

       /// <summary>
       /// 保存数据
       /// </summary>
       /// <returns></returns>
       public bool SaveChanges()
       {
           return Db.SaveChanges() > 0;
       }
    }
}
