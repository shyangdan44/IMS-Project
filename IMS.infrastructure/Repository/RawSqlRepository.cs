using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.infrastructure.Irepository;
using Microsoft.EntityFrameworkCore;

namespace IMS.infrastructure.Repository
{


    public class RawSqlRepository : IRawSqlRepository
    {
     
        private readonly IMSDbContext _context;

        public RawSqlRepository(IMSDbContext context)
        {
            _context = context;
        }


        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            var result = _context.Set<TEntity>().FromSqlRaw(sql, parameters);
            return result;
        }
    }
}
