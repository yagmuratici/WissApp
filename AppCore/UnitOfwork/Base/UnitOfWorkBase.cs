using AppCore.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.UnitOfwork.Base
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        protected DbContext db; //protected çünkü UnitOfWork.cs in  override ında db yi kullanabilmek için.

        public UnitOfWorkBase(DbContext _db) //dışarıdan alacağımız için ctor oluşturduk
        {
            db = _db;
        }

        public virtual int SaveChanges()
        {
            try
            {
                int result = db.SaveChanges();
                return result;
            }
            catch (Exception exc)
            {

                throw exc;
            }
        }


        #region Dispose
        protected bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
