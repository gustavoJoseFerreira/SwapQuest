using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
namespace SwapQuest
{
    public partial class ProcuraPage : ContentPage
    {
        public ProcuraPage()
        {
            InitializeComponent();
            BindingContext = new CartasTrocaEVendaViewModel();
        }
    }
    //View model para apresentar as cartas disponíveis
    public class CartasTrocaEVendaViewModel
    {
        public ObservableCollection<TrocaComNomeCarta> CartasTrocadas { get; set; }
        public ObservableCollection<VendaComNomeCarta> CartasVendidas { get; set; }
        public ICommand PropostaCommand { get; private set; }
        public ICommand TrocaCommand { get; private set; }
        

        public CartasTrocaEVendaViewModel()
        {
            CartasTrocadas = new ObservableCollection<TrocaComNomeCarta>();
            CartasVendidas = new ObservableCollection<VendaComNomeCarta>();
            
            LoadCartas();
            PropostaCommand = new Command<VendaComNomeCarta>(async (venda) => await PropostaButtonClicked(venda));
            TrocaCommand = new Command<TrocaComNomeCarta>(async (troca) => await TrocaButtonClicked(troca));
        }
        //Função para criação da proposta de venda na base de dados
        private async Task PropostaButtonClicked(VendaComNomeCarta venda)
        {
            int idlogin = UserSession.LoggedInUser.Id_User;
            var contraproposta = await App.Current.MainPage.DisplayPromptAsync("Fazer Proposta", "Insira o seu contacto:");

            if (!string.IsNullOrEmpty(contraproposta))
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var idvendedor = await dbContext.Venda
                            .Where(v => v.Id_Venda == venda.Id_Venda)
                            .Select(v => v.Id_User)
                            .FirstOrDefaultAsync();


                        var propostaCompra = new PropostaCompra
                        {
                            ContactoComprador = contraproposta,
                            Id_User = idvendedor,
                            Id_Sale = venda.Id_Venda,
                            Id_Comprador = idlogin
                        };

                        dbContext.PropostaCompra.Add(propostaCompra);
                        await dbContext.SaveChangesAsync();

                        await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta registrada com sucesso!", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao registrar a proposta: {ex.Message}");
                }
            }
        }
        //Função para criação da proposta de troca na base de dados
        private async Task TrocaButtonClicked(TrocaComNomeCarta troca)
        {
            int idlogin = UserSession.LoggedInUser.Id_User;
            var contactoComprador = await App.Current.MainPage.DisplayPromptAsync("Fazer Proposta", "Insira o seu contacto:");
            
            if (!string.IsNullOrEmpty(contactoComprador))
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var idvendedor = await dbContext.Troca
                            .Where(v => v.Id_Troca== troca.Id_Troca)
                            .Select(v => v.Id_User)
                            .FirstOrDefaultAsync();


                        var propostaTroca = new PropostaTroca
                        {
                            ContactoComprador = contactoComprador,
                            Id_Vendedor = idvendedor,
                            Id_Troca = troca.Id_Troca,
                            Id_Comprador = idlogin
                        };

                        dbContext.PropostaTroca.Add(propostaTroca);
                        await dbContext.SaveChangesAsync();

                        await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta registrada com sucesso!", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao registrar a proposta: {ex.Message}");
                }
            }
        }
        //Carregar as cartas da base de dados
        public async void LoadCartas()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    int idlogin = UserSession.LoggedInUser.Id_User;

                    // Carregar cartas para troca
                    var cartasParaTroca = await dbContext.Troca
                        .Where(cu => cu.Id_User != idlogin)
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

                        var trocaButton = new Button
                        {
                            Text = "Proposta",
                            VerticalOptions = LayoutOptions.End,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        var trocaItem = new TrocaComNomeCarta { };

                        CartasTrocadas.Add(new TrocaComNomeCarta
                        {
                            Id_Troca = cartaParaTroca.Id_Troca,
                            OpcaoTroca = opcao,
                            QTD_Troca = qtd,
                            Id_User = idlogin,
                            Id_Card = id,
                            NomeCarta = nome,
                            imagem = image
                        });
                    }

                    // Carregar cartas para venda
                    var cartasParaVenda = await dbContext.Venda
                        .Where(cu => cu.Id_User != idlogin)
                        .ToListAsync();
                    foreach (var cartaParaVenda in cartasParaVenda)
                    {
                        var valor = cartaParaVenda.ValorVenda;
                        var valorFormatado = valor.ToString("0.00");
                        var qtd = cartaParaVenda.QTD_Venda;
                        var id = cartaParaVenda.Id_Card;
                        var idcarta = await dbContext.CartasUtilizador
                            .Where(cu => cu.Id_Carta== id)
                            .Select(cu => cu.Id_Card)
                            .FirstOrDefaultAsync();  

                        var nome = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == idcarta)
                            .Select(ca => ca.PersonagemCarta)
                            .FirstOrDefaultAsync();
                        var image = await dbContext.CartasAPI
                           .Where(ca => ca.Id_Carta == idcarta)
                           .Select(ca => ca.Imagem)
                           .FirstOrDefaultAsync();


                        // Criar botão "Proposta"
                        var propostaButton = new Button
                        {
                            Text = "Proposta",
                            VerticalOptions = LayoutOptions.End,
                            HorizontalOptions = LayoutOptions.Center,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        var vendaItem = new VendaComNomeCarta{};
                       
                        // Adicionar item de venda à coleção
                        CartasVendidas.Add(new VendaComNomeCarta
                        {
                            Id_Venda = cartaParaVenda.Id_Venda,
                            ValorVenda = valorFormatado,
                            QTD_Venda = qtd,
                            Id_User = idlogin,
                            Id_Card = id,
                            NomeCarta = nome,
                            imagem = image,

                        });


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar as cartas: {ex.Message}");
            }
        }
    }

}