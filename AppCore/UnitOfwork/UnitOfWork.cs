using AppCore.Repository;
using AppCore.Repository.Base;
using AppCore.UnitOfwork.Base;
using System.Data.Entity;

namespace AppCore.UnitOfwork
{
    public class UnitOfWork : UnitOfWorkBase
    {
        public UnitOfWork(DbContext _db) : base(_db)
        {

        }
    }
}
