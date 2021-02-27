using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebService.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Api.Data
{
    public class RouletteData
    {
        private readonly string _connectionString;

        public RouletteData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Cnx");
        }
        private Roulette MapToValue(SqlDataReader reader)
        {
            return new Roulette()
            {
                IdRoulette = (int)reader["IdRuleta"],
                conditionOpened = (bool)reader["EstadoAbierto"],
                IdUser = (int)reader["IdUsuario"],
                UserRegistration = reader["UsuarioRegistro"].ToString(),
                DateaRegistration = (DateTime)reader["FechaRegistro"],
                UserUpdate = reader["UsuarioActualizacion"].ToString(),
                DateUpdate = (DateTime)reader["FechaActualizacion"]
            };
        }

        public async Task<Roulette> InsertRoulette(Roulette roulette)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("usp_InsertarRuleta", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@EstadoAbierto", roulette.conditionOpened));
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", roulette.IdUser));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioRegistro", roulette.UserRegistration));
                    cmd.Parameters.Add(new SqlParameter("@FechaRegistro", roulette.DateaRegistration));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioActualizacion", roulette.UserUpdate));
                    cmd.Parameters.Add(new SqlParameter("@FechaActualizacion", roulette.DateUpdate));
                    await sql.OpenAsync();
                    roulette.IdRoulette = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return roulette;
        }

        public async Task<Roulette> GetRoulette(int idRuleta)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerRuleta", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", idRuleta));
                    Roulette response = null;
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = (MapToValue(reader));
                        }
                    }
                    return response;
                }
            }
        }

        public async Task UpdateRoulette(Roulette roulette)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarRuleta", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdRuleta", roulette.IdRoulette));
                    cmd.Parameters.Add(new SqlParameter("@EstadoAbierto", roulette.conditionOpened));
                    cmd.Parameters.Add(new SqlParameter("@IdUsuario", roulette.IdUser));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioRegistro", roulette.UserRegistration));
                    cmd.Parameters.Add(new SqlParameter("@FechaRegistro", roulette.DateaRegistration));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioActualizacion", roulette.UserRegistration));
                    cmd.Parameters.Add(new SqlParameter("@FechaActualizacion", roulette.DateUpdate));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }

        public async Task<List<Roulette>> ListRoulette()
        {
            List<Roulette> listRoulette = new List<Roulette>();
            Roulette roulette = null;
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ListRoulette", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            roulette = MapToValue(reader);
                            roulette.MessageCondition = Convert.ToString(reader["Estado"]);
                            listRoulette.Add(roulette);
                        }
                    }
                    return listRoulette;
                }
            }
        }

    }
}
