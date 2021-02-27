using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebService.Api.Entities;

namespace WebService.Api.Data
{
    public class CreditData
    {
        private readonly string _connectionString;
        public CreditData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Cnx");
        }

        private Credit MapToValue(SqlDataReader reader)
        {
            return new Credit()
            {
                IdCredit = (int)reader["IdCredito"],
                IdPerson = (int)reader["IdPersona"],
                Card = reader["Tarjeta"].ToString(),
                NberAccount = reader["NroCuenta"].ToString(),
                MoneyBalance = (decimal)reader["Saldo"],
                UserRegistration = reader["UsuarioRegistro"].ToString(),
                DateaRegistration = (DateTime)reader["FechaRegistro"],
                UserUpdate = reader["UsuarioActualizacion"].ToString(),
                DateUpdate = (DateTime)reader["FechaActualizacion"]
            };
        }

        public async Task<Credit> GetPersonCredit(int idperson)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ObtenerCreditoPersona", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdPersona", idperson));
                    Credit response = null;
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

        public async Task UpdateCreditPerson(Credit credit)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ActualizarCreditoPersona", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("IdCredito", credit.IdCredit));
                    cmd.Parameters.Add(new SqlParameter("@IdPersona", credit.IdPerson));
                    cmd.Parameters.Add(new SqlParameter("@Tarjeta", credit.Card));
                    cmd.Parameters.Add(new SqlParameter("@NroCuenta", credit.NberAccount));
                    cmd.Parameters.Add(new SqlParameter("@Saldo", credit.MoneyBalance));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioRegistro", credit.UserRegistration));
                    cmd.Parameters.Add(new SqlParameter("@FechaRegistro", credit.DateaRegistration));
                    cmd.Parameters.Add(new SqlParameter("@UsuarioActualizacion", credit.UserUpdate));
                    cmd.Parameters.Add(new SqlParameter("@FechaActualizacion", credit.DateUpdate));
                    await sql.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return;
                }
            }
        }
    }
}
