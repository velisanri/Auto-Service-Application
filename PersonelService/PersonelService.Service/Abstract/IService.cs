using PersonelService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonelService.Data.Abstract;


namespace PersonelService.Service.Abstract
{
    public interface IService<T> :  IRepository<T> where T : class , IEntity, new()
    {

    }
}
