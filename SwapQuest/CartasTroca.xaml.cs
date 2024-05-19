using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;

namespace SwapQuest
{

    public partial class CartasTroca : ContentPage
    {
        public CartasTroca()
        {
            InitializeComponent();
            BindingContext = new CartasTrocadasViewModel();
        }
        //Função do botão para eliminar a proposta
        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            try
            {
                // Lógica para eliminar a troca
                if (sender is Button button && button.CommandParameter is int idTroca)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var trocaParaEliminar = await dbContext.Troca.FirstOrDefaultAsync(v => v.Id_Troca == idTroca);
                        if (trocaParaEliminar != null)
                        {
                            dbContext.Troca.Remove(trocaParaEliminar);
                            await dbContext.SaveChangesAsync();
                            await DisplayAlert("Alerta", "Troca eliminada com sucesso! Volte atrás para atualizar as suas trocas", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao eliminar a carta: {ex.Message}");
            }
        }
    }
    //View model para mostar as cartas que estão para troca
        public class CartasTrocadasViewModel
        {
            public ObservableCollection<TrocaComNomeCarta> CartasTrocadas { get; set; }

            public CartasTrocadasViewModel()
            {
                CartasTrocadas = new ObservableCollection<TrocaComNomeCarta>();
                LoadCartasTrocadas();
            }
            //Carrega as cartas da base de dados
            public async void LoadCartasTrocadas()
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        int idlogin = UserSession.LoggedInUser.Id_User;
                        var idut = await dbContext.Troca
                                   .Where(ve => ve.Id_User == idlogin)
                                   .Select(ve => ve.Id_User)
                                   .FirstOrDefaultAsync();

                        if (idlogin == idut)
                        {
                            var cartasParaTroca = await dbContext.Troca
                                    .Where(cu => cu.Id_User == idlogin)
                                    .ToListAsync();
                            foreach (var cartaParaTroca in cartasParaTroca)
                            {
                                var opcao = cartaParaTroca.OpcaoTroca;
                                var qtd = cartaParaTroca.QTD_Troca;
                                var id = cartaParaTroca.Id_Card;
                                var idcarta = await dbContext.CartasUtilizador
                                    .Where(ve => ve.Id_Carta == id)
                                    .Select(ve => ve.Id_Card)
                                    .FirstOrDefaultAsync();
                                var nome = await dbContext.CartasAPI
                                    .Where(ca => ca.Id_Carta == idcarta)
                                    .Select(ca => ca.PersonagemCarta)
                                    .FirstOrDefaultAsync();
                                var image = await dbContext.CartasAPI
                                   .Where(ca => ca.Id_Carta == idcarta)
                                   .Select(ca => ca.Imagem)
                                   .FirstOrDefaultAsync();
                                Console.WriteLine(image);

                                CartasTrocadas.Add(new TrocaComNomeCarta
                                {
                                    Id_Troca = cartaParaTroca.Id_Troca,
                                    OpcaoTroca = opcao,
                                    QTD_Troca = qtd,
                                    Id_User = idut,
                                    Id_Card = id,
                                    NomeCarta = nome,
                                    imagem = image
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar as cartas vendidas: {ex.Message}");
                }
            }
        }

        
        public class TrocaComNomeCarta
        {
            public int Id_Troca { get; set; }
            public string OpcaoTroca { get; set; }
            public int QTD_Troca { get; set; }
            public int Id_User { get; set; }
            public int Id_Card { get; set; }
            public string NomeCarta { get; set; }
            public string imagem { get; set; }
        }
    }
