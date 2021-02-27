using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Api.Entities
{
    public class Credit
    {
        [Key]
        public int IdCredit { get; set; }
        public int IdPerson { get; set; }
        public string Card { get; set; }
        public string NberAccount { get; set; }
        public decimal MoneyBalance { get; set; }
        public string UserRegistration { get; set; }
        public DateTime DateaRegistration { get; set; }
        public string UserUpdate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
