using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public class AuthService
    {
        public static async Task<bool> RegisterAsync(Utilizador newUtilizador)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
            
                    dbContext.Utilizador.Add(newUtilizador);
                    await dbContext.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
                // Tratar qualquer exceção e registrar o erro, se necessário
                Console.WriteLine("Erro ao registrar usuário: " + ex.Message);

                return false;
            }

        }

        // Método para autenticar um usuário
        public static async Task<Utilizador> AuthenticateAsync(string userEmail, string userPassword)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    // Busca o usuário no banco de dados com base no e-mail e senha fornecidos
                    Utilizador user = await dbContext.Utilizador.FirstOrDefaultAsync(u => u.UserEmail == userEmail && u.UserPassword == userPassword);
                    return user;
                }
            }
            catch (Exception ex)
            {
                // Tratar qualquer exceção e registrar o erro, se necessário
                Console.WriteLine("Erro ao autenticar usuário: " + ex.Message);
                return null;
            }


        }
    }
}
