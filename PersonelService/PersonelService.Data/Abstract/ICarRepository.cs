using PersonelService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Data.Abstract
{
    public interface ICarRepository : IRepository<Arac>
    {
        Task<List<Arac>> GetCustomCarList();
        Task<List<Arac>> GetCustomCarList(Expression<Func<Arac, bool>> expression);

        Task<Arac> GetCustomCar(int id);

         Task<Arac> GetByIdAsync(int id);

    }
}
