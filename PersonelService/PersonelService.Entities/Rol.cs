using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelService.Entities
{
    public class Rol : IEntity
    {

        public int Id { get; set; }
        [StringLength(50)]
        [Display(Name = "Adı"), Required(ErrorMessage = "{0} boş bırakılamaz")]
        public string Adi { get; set; }
    }
}
