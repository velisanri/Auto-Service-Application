using PersonelService.Data;
using PersonelService.Data.Abstract;
using PersonelService.Data.Concrete;
using PersonelService.Entities;
using PersonelService.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Service.Concrete
{
    public class Service<T> : Repository<T>,IService<T> where T : class, IEntity, new()
    {
        public Service(DatabaseContext context) : base(context)
        {
        }
    }
}
