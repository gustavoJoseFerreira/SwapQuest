using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwapQuest
{
    public static class ColecaoManager
    {
        public static void AdicionarColecaoAoUtilizador(string descricaoColecao)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // Verificar se a coleção já existe na base de dados
                var colecaoExistente = dbContext.ColecoesAPI.FirstOrDefault(c => c.DescricaoColecao == descricaoColecao);
                if (colecaoExistente == null)
                {
                    // Se a coleção não existir, cria uma nova
                    var novaColecao = new ClassColecao { DescricaoColecao = descricaoColecao };
                    dbContext.ColecoesAPI.Add(novaColecao);
                    dbContext.SaveChanges();
                    colecaoExistente = novaColecao;
                }

                // Atribuir a coleção ao utilizador logado
                var utilizador = dbContext.Utilizador.Find(UserSession.LoggedInUser.Id_User);
                if (utilizador != null)
                {
                    // Verificar se o utilizador já possui esta coleção
                    var colecaoDoUtilizador = dbContext.ColecaoUtilizador
                        .FirstOrDefault(cu => cu.Id_Utilizador == utilizador.Id_User && cu.Id_Colecao == colecaoExistente.Id_Colecao);

                    if (colecaoDoUtilizador == null)
                    {
                        // Se o utilizador não possuir esta coleção, adicioná-la à sua conta
                        var novaColecaoUtilizador = new ColecaoUtilizador
                        {
                            Id_Utilizador = utilizador.Id_User,
                            Id_Colecao = colecaoExistente.Id_Colecao
                        };
                        dbContext.ColecaoUtilizador.Add(novaColecaoUtilizador);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        
                        Console.WriteLine("O utilizador já possui esta coleção em sua conta.");
                    }
                }
                else
                {
                   
                    Console.WriteLine("O utilizador não está logado.");
                }
            }
        }
    }
}


