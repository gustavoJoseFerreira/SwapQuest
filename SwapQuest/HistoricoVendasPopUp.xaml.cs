using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;


namespace SwapQuest
{
    public partial class HistoricoVendasPopUp : ContentPage
    {
        public HistoricoVendasPopUp()
        {
            InitializeComponent();
            BindingContext = new HistoricoVendasViewModel();
        }


        public class HistoricoVendasViewModel
        {
            public ObservableCollection<DetalhesVendaViewModel> Vendas { get; set; }


            public HistoricoVendasViewModel()
            {
                Vendas = new ObservableCollection<DetalhesVendaViewModel> ();
                LoadHistoricoVendas();
            }

            private async void LoadHistoricoVendas()
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var vendas = await dbContext.ConfirmacaoVenda.ToListAsync();

                        foreach (var venda in vendas)
                        {
                            var utilizadorLogado = UserSession.LoggedInUser.Id_User;
                            if (utilizadorLogado == venda.Id_Vendedor)
                            { 
                                var nomeVendedor = await dbContext.Utilizador
                                    .Where(u => u.Id_User == venda.Id_Vendedor)
                                    .Select(u => u.UserName)
                                    .FirstOrDefaultAsync();
                                var nomeComprador = await dbContext.Utilizador
                                    .Where(u => u.Id_User == venda.Id_Comprador)
                                    .Select(u => u.UserName)
                                    .FirstOrDefaultAsync();
                                var idCarta = await dbContext.CartasUtilizador
                                    .Where(u => u.Id_Carta == venda.Id_CardVendida)
                                    .Select(u => u.Id_Card)
                                    .FirstOrDefaultAsync();
                                var nomeCarta = await dbContext.CartasAPI
                                    .Where(ca => ca.Id_Carta == idCarta)
                                    .Select(ca => ca.PersonagemCarta)
                                    .FirstOrDefaultAsync(); 
                                Console.WriteLine(nomeCarta);   
                                var detalhesVenda = new DetalhesVendaViewModel
                                {
                                    Id_ConfVenda = venda.Id_ConfVenda,
                                    QTD_Comprada = venda.QTD_Comprada,
                                    ContactoComprador = venda.ContactoComprador,
                                    ValorVenda = venda.ValorVenda.ToString("0.00"),
                                    Id_Comprador = venda.Id_Comprador,
                                    Id_Vendedor = venda.Id_Vendedor,
                                    Id_CardVendida = venda.Id_CardVendida,
                                    Id_Sale = venda.Id_Sale,
                                    NomeComprador = nomeComprador,
                                    NomeVendedor = nomeVendedor,
                                    NomeCartaVendida = nomeCarta
                                };

                                // Adicione o objeto DetalhesVendaViewModel à sua ObservableCollection
                                Vendas.Add(detalhesVenda);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar o histórico de vendas: {ex.Message}");
                }
            }
        }
    }
    public class DetalhesVendaViewModel
    {
        public int Id_ConfVenda { get; set; }
        public int QTD_Comprada { get; set; }
        public int ContactoComprador { get; set; }
        public string ValorVenda { get; set; }
        public int Id_Comprador { get; set; }
        public int Id_Vendedor { get; set; }
        public int Id_CardVendida { get; set; }
        public int Id_Sale { get; set; }
        public string NomeComprador { get; set; }
        public string NomeVendedor { get; set; }
        public string NomeCartaVendida { get; set; }
    }
}
