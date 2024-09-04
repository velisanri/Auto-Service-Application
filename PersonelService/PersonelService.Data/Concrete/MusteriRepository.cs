using PersonelService.Data.Abstract;
using PersonelService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Data.Concrete
{
    public class MusteriRepository : Repository<Musteri>, IMusteriRepository
    {
        public MusteriRepository(DatabaseContext context) : base(context)
        {

        }
    }
}
