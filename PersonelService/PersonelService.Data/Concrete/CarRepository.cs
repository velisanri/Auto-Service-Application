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
    public class CarRepository : Repository<Arac>, ICarRepository
    {
        public CarRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Arac> GetCustomCar(int id)
        {
            return await _dbSet.AsNoTracking().Include(x => x.Marka).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Arac>> GetCustomCarList()
        {
            return await _dbSet.AsNoTracking().Include(x=>x.Marka).ToListAsync();
        }

        public async Task<List<Arac>> GetCustomCarList(Expression<Func<Arac, bool>> expression)
        {
            return await _dbSet.Where(expression).AsNoTracking().Include(x => x.Marka).ToListAsync();
        }

        public async Task<Arac> GetByIdAsync(int id)
        {
            return await _context.Araclar.FindAsync(id);
        }
    }
}
