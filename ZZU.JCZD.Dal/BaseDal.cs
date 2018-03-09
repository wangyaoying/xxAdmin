using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.Model;

namespace ZZU.JCZD.DAL
{

    public class BaseDal<T> where T : class ,new()
    {
        DbContext db = DBContexFactory.CreateDbContext() as JCZDEntities;

        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> wherelambda)
        {
            return db.Set<T>().Where<T>(wherelambda);

        }

        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> wherelambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            var temp = db.Set<T>().Where<T>(wherelambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, s>(orderbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
        }

        public bool DeleteEntity(T entity)
        {
            db.Entry<T>(entity).State = System.Data.EntityState.Deleted;
            return true;
        }

        public bool EditEntity(T entity)
        {
            db.Entry<T>(entity).State = System.Data.EntityState.Modified;
            return true;
        }

        public T AddEntity(T entity)
        {
            db.Set<T>().Add(entity);
            return entity;
        }
    }
}
