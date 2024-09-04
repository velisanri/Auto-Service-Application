using PersonelService.Data;
using PersonelService.Data.Concrete;
using PersonelService.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Service.Concrete
{
    public class UserService : UserRepository , IUserService
    {
        public UserService(DatabaseContext context) : base(context)
        {
        }
    }
}
