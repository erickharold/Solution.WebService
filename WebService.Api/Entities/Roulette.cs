using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Api.Entities
{
    public class Roulette
    {
        [Key]
        public int IdRoulette { get; set; }
        public bool conditionOpened { get; set; }
        public int IdUser { get; set; }
        public string UserRegistration { get; set; }
        public DateTime DateaRegistration { get; set; }
        public string UserUpdate { get; set; }
        public DateTime DateUpdate { get; set; }

        #region Otras Propiedades
        public string MessageCondition { get; set; }
        #endregion
    }
}
