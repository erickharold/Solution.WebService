using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Api.Entities
{
    public class RouletteGames
    {
        [Key]
        public int IdRouletteGames { get; set; }
        public int IdRoulette { get; set; }
        public int NberGames { get; set; }
        public int IdPlayer { get; set; }
        public string KindBet { get; set; }
        public string ValueKindBet { get; set; }
        public decimal AmountBet { get; set; }
        public string ValueWinner { get; set; }
        public decimal AmountEarned { get; set; }
        public bool Was_Winner { get; set; }
        public bool ConditionMove{ get; set; }
    }
}
