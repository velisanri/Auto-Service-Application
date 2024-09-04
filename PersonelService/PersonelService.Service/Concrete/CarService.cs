using Microsoft.EntityFrameworkCore;
using PersonelService.Data;
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
    public class CarService : CarRepository , ICarService
    {
        public CarService(DatabaseContext context) : base(context)
        {
            
        }

    }
}
