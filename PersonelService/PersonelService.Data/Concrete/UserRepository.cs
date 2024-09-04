using Microsoft.EntityFrameworkCore;
using PersonelService.Data.Abstract;
using PersonelService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Data.Concrete
{
    public class UserRepository : Repository<Kullanici> , IUserRepository
    {
        public UserRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<List<Kullanici>> GetCustomList()
        {
            return await _dbSet.AsNoTracking().Include(x => x.Rol).ToListAsync();
        }

        public async Task<List<Kullanici>> GetCustomList(Expression<Func<Kullanici, bool>> expression)
        {
            return await _dbSet.Where(expression).AsNoTracking().Include(x => x.Rol).ToListAsync();
        }
    }
}
