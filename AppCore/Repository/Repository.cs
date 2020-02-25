using AppCore.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Repository
{
    public class Repository<TEntity> : RepositoryBase<TEntity> where TEntity : class , new()
    {
        public Repository(DbContext _db) : base(_db) //db= _db demenın başka hali abstract olduğu için base e gönderebilirsin.
        {
                
        }
    }
}
