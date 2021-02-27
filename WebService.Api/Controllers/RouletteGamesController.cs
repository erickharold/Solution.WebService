using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Api.Data;
using WebService.Api.Entities;

namespace WebService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteGamesController : ControllerBase
    {
        private readonly RouletteGamesData _dataRouletteGames;
        public RouletteGamesController(RouletteGamesData rouletteGamesData)
        {
            this._dataRouletteGames = rouletteGamesData ?? throw new ArgumentNullException(nameof(rouletteGamesData));
        }

        [HttpGet("[action]/{idRoulette}/{nberGames}")]
        public async Task<ActionResult<IEnumerable<RouletteGames>>> PlayRoulette(int idRoulette, int nberGames)
        {
            Random numeroAleatorio = new Random();

            int numeroGanador = numeroAleatorio.Next(0, 36);
            var color = GetWonColor(numeroGanador);

            var listWinnerObj = await _dataRouletteGames.ListPlayersXNberGames(idRoulette, nberGames);
             
            for (int i = 0; i <= listWinnerObj.Count - 1; i++)
            {
                if (listWinnerObj[i].KindBet == "NUMERO")
                {
                    listWinnerObj[i].ValueWinner = Convert.ToString(numeroGanador);
                    listWinnerObj[i].ConditionMove = true;

                    if (listWinnerObj[i].ValueKindBet == Convert.ToString(numeroGanador))
                    {
                        listWinnerObj[i].Was_Winner= true;
                        listWinnerObj[i].AmountEarned = (listWinnerObj[i].AmountBet * 5);
                    }
                    else
                    {
                        listWinnerObj[i].Was_Winner = false;
                        listWinnerObj[i].AmountEarned = 0;
                    }

                    await _dataRouletteGames.UpdateRouletteGames(listWinnerObj[i]);
                }
                else
                {
                    listWinnerObj[i].ValueWinner = color;
                    listWinnerObj[i].ConditionMove = true;

                    if (listWinnerObj[i].ValueKindBet == color)
                    {
                        listWinnerObj[i].Was_Winner = true;
                        listWinnerObj[i].AmountEarned = (listWinnerObj[i].AmountBet * Convert.ToDecimal(1.8));
                    }
                    else
                    {
                        listWinnerObj[i].Was_Winner = false;
                        listWinnerObj[i].AmountEarned = 0;
                    }

                    await _dataRouletteGames.UpdateRouletteGames(listWinnerObj[i]);
                }

            }

            return Ok(listWinnerObj);
        }

        private string GetWonColor(int nberWinner)
        {
            var colorWinner = "";

            if (nberWinner % 2 == 0)
            {
                //ES PAR
                colorWinner = "ROJO";
            }
            else
            {
                //ES INPAR
                colorWinner = "NEGRO";
            }
            return colorWinner;
        }
    }
}
