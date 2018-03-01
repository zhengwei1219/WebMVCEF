using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.DALFactory;
using ZhengWei.ExtractOil.IDAL;

namespace ZhengWei.ExtractOil.BLL
{
   public abstract class BaseService<T> where T:class,new()
    {
       public IDbSession DbSession
       {
           get { return new DbSession(); }
       }

       public IBaseDal<T> CurrentDal {get;set;}
       public abstract void SetCurrentDal();

       public BaseService()
       {
         SetCurrentDal();
       }

       public IQueryable<T> LoadEntities(Expression<Func<T,bool>> whereLambda)
       {
           return CurrentDal.LoadEntities(whereLambda);
       }

       public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc)
       {
           return CurrentDal.LoadPageEntities(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
       }

       public bool DeleteEntity(T entity)
       {
           CurrentDal.DeleteEntity(entity);
           return this.DbSession.SaveChanges();
       }

       public bool UpdateEntity(T entity)
       {
           CurrentDal.UpdateEntity(entity);
           return this.DbSession.SaveChanges();
       }

       public T AddEntity(T entity) 
       {
           CurrentDal.AddEntity(entity);
           this.DbSession.SaveChanges();
           return entity;
       }

    }
}
