using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebService.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Api.Data
{
    public class RouletteGamesData
    {
        private readonly string _connectionString;
        public RouletteGamesData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Cnx");
        }
        private RouletteGames MapToValue(SqlDataReader reader)
        {
            return new RouletteGames()
            {
                IdRouletteGames = (int)reader["IdJugada"],
                IdRoulette = (int)reader["IdRuleta"],
                NberGames = (int)reader["NroJuego"],
                IdPlayer = (int)reader["IdJugador"],
                KindBet = reader["TipoApuesta"].ToString(),
                ValueKindBet = reader["ValorTipoApuesta"].ToString(),
                AmountBet = (decimal)reader["MontoApuesta"],
                ValueWinner = reader["ValorGanador"].ToString(),
                AmountEarned = (decimal)reader["MontoGanado"],
                Was_Winner = (bool)reader["Fue_Ganador"],
                ConditionMove = (bool)reader["EstadoJugada"]
            };
        }

        public async Task<RouletteGames> InsertRouletteGames(RouletteGames rouletteGames)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_InsertarJugadas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", rouletteGames.IdRoulette));
                    cmd.Parameters.Add(new SqlParameter("@NroJuego", rouletteGames.NberGames));
                    cmd.Parameters.Add(new SqlParameter("@IdJugador", rouletteGames.IdPlayer));
                    cmd.Parameters.Add(new SqlParameter("@TipoApuesta", rouletteGames.KindBet));
                    cmd.Parameters.Add(new SqlParameter("@ValorTipoApuesta", rouletteGames.ValueKindBet));
                    cmd.Parameters.Add(new SqlParameter("@MontoApuesta", rouletteGames.AmountBet));
                    cmd.Parameters.Add(new SqlParameter("@ValorGanador", rouletteGames.ValueWinner));
                    cmd.Parameters.Add(new SqlParameter("@MontoGanado", rouletteGames.AmountEarned));
                    cmd.Parameters.Add(new SqlParameter("@Fue_Ganador", rouletteGames.Was_Winner));
                    cmd.Parameters.Add(new SqlParameter("@EstadoJugada", rouletteGames.ConditionMove));
                    await sql.OpenAsync();
                    rouletteGames.IdRoulette = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return rouletteGames;
        }

        public async Task<List<RouletteGames>> ListPlayersXNberGames(int IdRoulette, int nberPlay)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListarJugdoresXNroJuego", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", IdRoulette));
                    cmd.Parameters.Add(new SqlParameter("@NroJuego", nberPlay));
                    var response = new List<RouletteGames>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        public async Task UpdateRouletteGames(RouletteGames rouletteGames)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarJugada", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdJugada", rouletteGames.IdRouletteGames));
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", rouletteGames.IdRoulette));
                    cmd.Parameters.Add(new SqlParameter("@NroJuego", rouletteGames.NberGames));
                    cmd.Parameters.Add(new SqlParameter("@IdJugador", rouletteGames.IdPlayer));
                    cmd.Parameters.Add(new SqlParameter("@TipoApuesta", rouletteGames.KindBet));
                    cmd.Parameters.Add(new SqlParameter("@ValorTipoApuesta", rouletteGames.ValueKindBet));
                    cmd.Parameters.Add(new SqlParameter("@MontoApuesta", rouletteGames.AmountBet));
                    cmd.Parameters.Add(new SqlParameter("@ValorGanador", rouletteGames.ValueWinner));
                    cmd.Parameters.Add(new SqlParameter("@MontoGanado", rouletteGames.AmountEarned));
                    cmd.Parameters.Add(new SqlParameter("@Fue_Ganador", rouletteGames.Was_Winner));
                    cmd.Parameters.Add(new SqlParameter("@EstadoJugada", rouletteGames.ConditionMove));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }



        public async Task<List<RouletteGames>> GetWinnerRouletteGames(int IdRoulette)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_MostrarGanadorJuegoRuleta", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", IdRoulette));
                    var response = new List<RouletteGames>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

    }
}
