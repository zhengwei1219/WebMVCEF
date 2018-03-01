using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ZhengWei.ExtractOil.IDAL
{
    /// <summary>
    /// 基类中通用的CRUD方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public interface IBaseDal<T> where T:class,new()
    {
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, s>> orderbyLambda, bool isAsc);

        bool DeleteEntity(T entity);

        bool UpdateEntity(T entity);

        T AddEntity(T entity);
    }
}
