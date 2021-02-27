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
    public class RouletteController : ControllerBase
    {
        private readonly RouletteData _dataroulette;
        private readonly RouletteGamesData _datarouletteGames;
        private readonly CreditData _dataCredit;

        public RouletteController(RouletteData rouletteData, RouletteGamesData rouletteGamesData, CreditData creditData)
        {
            this._dataroulette = rouletteData ?? throw new ArgumentNullException(nameof(rouletteData));
            this._datarouletteGames = rouletteGamesData ?? throw new ArgumentNullException(nameof(rouletteGamesData));
            this._dataCredit = creditData ?? throw new ArgumentNullException(nameof(creditData));
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> InsertNewRoulette([FromBody] Roulette roulette)
        {
            try
            {
                roulette.conditionOpened = false;
                roulette.UserRegistration = "ADMIN";
                roulette.DateaRegistration = DateTime.Now;
                roulette.UserUpdate = "ADMIN";
                roulette.DateUpdate = DateTime.Now;

                await _dataroulette.InsertRoulette(roulette);

                return Ok(roulette.IdRoulette);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut("[action]/{idRoulette}")]
        public async Task<ActionResult> OpenRoulette(int idRoulette)
        {
            var rouletteObj = await _dataroulette.GetRoulette(idRoulette);

            if (rouletteObj != null)
            {
                rouletteObj.conditionOpened = true;
                await _dataroulette.UpdateRoulette(rouletteObj);
                return Ok("Operación exitosa");
            }
            else
            {
                return Ok("Operación denegada");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ToBet([FromBody] List<RouletteGames> rouletteGames)
        {
            foreach (var item in rouletteGames)
            {
                RouletteGames objRouletteGames = new RouletteGames();
                objRouletteGames.IdRoulette = item.IdRoulette;
                objRouletteGames.NberGames = item.NberGames;
                objRouletteGames.IdPlayer = item.IdPlayer;
                objRouletteGames.KindBet = item.KindBet;
                objRouletteGames.ValueKindBet = item.ValueKindBet;
                objRouletteGames.AmountBet = item.AmountBet;
                objRouletteGames.ValueWinner = "";
                objRouletteGames.AmountEarned = 0;
                objRouletteGames.Was_Winner = false;
                objRouletteGames.ConditionMove = false;
                await _datarouletteGames.InsertRouletteGames(objRouletteGames);
            }
            return Ok("Apuesta hecha satisfactoriamente");
        }

        [HttpGet("[action]/{idRoulette}")]
        public async Task<ActionResult<IEnumerable<RouletteGames>>> CloseRoulette(int idRoulette)
        {
            var rouletteGamesObj = await _dataroulette.GetRoulette(idRoulette);

            rouletteGamesObj.conditionOpened = false;
            rouletteGamesObj.UserUpdate = "ADMIN";
            rouletteGamesObj.DateUpdate = DateTime.Now;
            await _dataroulette.UpdateRoulette(rouletteGamesObj);

            var winners = await _datarouletteGames.GetWinnerRouletteGames(idRoulette);

            foreach (var item in winners)
            {
                var creditPerson = await _dataCredit.GetPersonCredit(item.IdPlayer);
                creditPerson.MoneyBalance += item.AmountEarned;
                await _dataCredit.UpdateCreditPerson(creditPerson);
            }
            return Ok(winners);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<RouletteGames>>> ListRoulette()
        {
            var listRoulette = await _dataroulette .ListRoulette();

            if (listRoulette != null)
            {
                return Ok(listRoulette);
            }
            else
            {
                return Ok("No existen registros");
            }
        }
    }
}
