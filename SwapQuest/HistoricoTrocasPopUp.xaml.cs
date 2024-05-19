using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace SwapQuest
{
    public partial class HistoricoTrocasPopUp : ContentPage
    {
        public HistoricoTrocasPopUp()
        {
            InitializeComponent();
            BindingContext = new HistoricoTrocasViewModel();
        }
    }

    public class HistoricoTrocasViewModel
    {
        public ObservableCollection<DetalhesTrocaViewModel> Trocas { get; set; }

        public HistoricoTrocasViewModel()
        {
            Trocas = new ObservableCollection<DetalhesTrocaViewModel>();
            LoadHistoricoTrocas();
        }

        private async void LoadHistoricoTrocas()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var trocas = await dbContext.ConfirmacaoTroca.ToListAsync();

                    foreach (var troca in trocas)
                    {
                        var utilizadorLogado = UserSession.LoggedInUser.Id_User;
                        if (utilizadorLogado == troca.Id_Vendedor)
                        {
                            var nomeVendedor = await dbContext.Utilizador
                                .Where(u => u.Id_User == troca.Id_Vendedor)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync();
                            var nomeComprador = await dbContext.Utilizador
                                .Where(u => u.Id_User == troca.Id_Comprador)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync();
                            var idCarta = await dbContext.CartasUtilizador
                                .Where(u => u.Id_Carta == troca.Id_CardTrocado)
                                .Select(u => u.Id_Card)
                                .FirstOrDefaultAsync();
                            var nomeCarta = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == idCarta)
                                .Select(ca => ca.PersonagemCarta)
                                .FirstOrDefaultAsync();
                            Console.WriteLine(nomeCarta);
                            var detalhesTroca = new DetalhesTrocaViewModel
                            {
                                Id_ConfTroca = troca.Id_ConfTroca,
                                QTD_Trocada = troca.QTD_Trocada,
                                ContactoComprador = troca.ContactoComprador,
                                OpcaoTroca = troca.OpcaoTroca,
                                Id_Comprador = troca.Id_Comprador,
                                Id_Vendedor = troca.Id_Vendedor,
                                Id_CardTrocada = troca.Id_CardTrocado,
                                Id_Troca = troca.Id_Troca,
                                NomeComprador = nomeComprador,
                                NomeVendedor = nomeVendedor,
                                NomeCartaTrocada = nomeCarta
                            };

                            // Adicione o objeto DetalhesTrocaViewModel à sua ObservableCollection
                            Trocas.Add(detalhesTroca);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar o histórico de trocas: {ex.Message}");
            }
        }
    }

    public class DetalhesTrocaViewModel
    {
        public int Id_ConfTroca { get; set; }
        public int QTD_Trocada { get; set; }
        public string ContactoComprador { get; set; }
        public string OpcaoTroca { get; set; }
        public int Id_Comprador { get; set; }
        public int Id_Vendedor { get; set; }
        public int Id_CardTrocada { get; set; }
        public int Id_Troca { get; set; }
        public string NomeComprador { get; set; }
        public string NomeVendedor { get; set; }
        public string NomeCartaTrocada { get; set; }
    }
}
