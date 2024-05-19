using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Controls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SwapQuest
{
    //Context que faz a ligação entre a aplicação e a base de dados
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Utilizador> Utilizador { get; set; }
        public DbSet<ClassColecao> ColecoesAPI { get; set; }
        public DbSet<CartasUtilizador> CartasUtilizador { get; set; }
        public DbSet<CartasAPI> CartasAPI { get; set; }
        public DbSet<ColecaoUtilizador> ColecaoUtilizador { get; set; }
        public DbSet<Venda> Venda { get; set; }
        public DbSet<Troca> Troca {  get; set; }
        public DbSet<PropostaCompra> PropostaCompra { get; set; }
        public DbSet<PropostaTroca> PropostaTroca { get; set; }
        public DbSet<ConfirmacaoVenda> ConfirmacaoVenda { get; set; }
        public DbSet<ConfirmacaoTroca> ConfirmacaoTroca { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Ligação com a base de dados
            optionsBuilder.UseSqlServer("Server = tcp:swapserver.database.windows.net, 1433; Initial Catalog = swapquestbd; Persist Security Info = False; User ID = swap_admin; Password = Pdi_2024; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
        }
    }
}
