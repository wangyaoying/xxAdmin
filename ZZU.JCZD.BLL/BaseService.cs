using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZU.JCZD.DAL;
using ZZU.JCZD.Model;


namespace ZZU.JCZD.BLL
{
    public abstract class BaseService<T> where T :class ,new()
    {
        JCZDEntities db = DBContexFactory.CreateDbContext() as JCZDEntities;
        public DAL.BaseDal<T> currentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            SetCurrentDal();//子类必须实现该抽象方法，指定当前的dal
        }
        public IQueryable<T> LoadEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return currentDal.LoadEntities(whereLambda);
        }
        public IQueryable<T> LoadPageEntities<s>(int pageIndex, int pageSize, out int totalCount, System.Linq.Expressions.Expression<Func<T, bool>> wherelambda, System.Linq.Expressions.Expression<Func<T, s>> orderbyLambda, bool isAsc)
        {
            return currentDal.LoadPageEntities<s>(pageIndex, pageSize, out totalCount, wherelambda, orderbyLambda,isAsc);
        }
        public bool DeleteEntity(T entity)
        {
             currentDal.DeleteEntity(entity);
             return db.SaveChanges()>0;
        }

        public bool EditEntity(T entity)
        {
            currentDal.EditEntity(entity);
            return db.SaveChanges()>0;
        }

        public T AddEntity(T entity)
        {
            currentDal.AddEntity(entity);
            db.SaveChanges();
            return entity;
        }
    }
}
