using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace SwapQuest
{
    public partial class PropostasRecebidas : ContentPage
    {
        public PropostasRecebidas()
        {
            InitializeComponent();
            BindingContext = new PropostasCompraViewModel();
            
        }
        //Caminho para histórico de vendas
        private async void MostrarInformacoes_Clicked(object sender, EventArgs e)
        {
            
             await Shell.Current.GoToAsync("HistoricoVendasPopUp");
        }

        //Caminho para histórico de trocas
        private async void MostrarInformacoesTroca_Clicked(object sender, EventArgs e)
        {
           await Shell.Current.GoToAsync("HistoricoTrocasPopUp");
        }


    }

    //view model para apresentar as propostas
    public class PropostasCompraViewModel
    {
        public ObservableCollection<PropostaCompraComDetalhes> PropostasCompra { get; set; }
        public ObservableCollection<PropostaTrocaComDetalhes> PropostasTroca { get; set; }
        public ICommand AceitarCommand { get; private set; }
        public ICommand AceitarTrocaCommand { get; private set; }
        public ICommand EliminarCommand { get; private set; }
        public ICommand EliminarTrocaCommand { get; private set; }

        public PropostasCompraViewModel()
            {
            PropostasCompra = new ObservableCollection<PropostaCompraComDetalhes>();
            PropostasTroca = new ObservableCollection<PropostaTrocaComDetalhes>();
            LoadPropostasCompra();
            LoadPropostasTroca();
            AceitarCommand = new Command<PropostaCompraComDetalhes>(async (aceitar) => await AceitarButtonClicked(aceitar));
            AceitarTrocaCommand = new Command<PropostaTrocaComDetalhes>(async (aceitarTroca) => await AceitarTrocaButtonClicked(aceitarTroca));
            EliminarCommand = new Command<PropostaCompraComDetalhes>(async (eliminar) => await EliminarButtonClicked(eliminar));
            EliminarTrocaCommand = new Command<PropostaTrocaComDetalhes>(async (eliminarTroca) => await EliminarTrocaButtonClicked(eliminarTroca));

        }
        // Função para eliminar as propostas de compra ao clicar no botão
        private async Task EliminarButtonClicked(PropostaCompraComDetalhes eliminar)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var propostaParaRemover = await dbContext.PropostaCompra.FirstOrDefaultAsync(pc => pc.Id_PrCompra == eliminar.Id_PrCompra);
                    if (propostaParaRemover != null)
                    {
                        dbContext.PropostaCompra.Remove(propostaParaRemover);
                        await dbContext.SaveChangesAsync();
                        PropostasCompra.Remove(eliminar);
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta eliminada com sucesso!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao eliminar a proposta: {ex.Message}");
            }
        }
        // Função para eliminar as propostas de troca ao clicar no botão
        private async Task EliminarTrocaButtonClicked(PropostaTrocaComDetalhes eliminarTroca)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var propostaTrocaParaRemover = await dbContext.PropostaTroca.FirstOrDefaultAsync(pt => pt.Id_PrTroca == eliminarTroca.Id_PrTroca);
                    if (propostaTrocaParaRemover != null)
                    {
                        dbContext.PropostaTroca.Remove(propostaTrocaParaRemover);
                        await dbContext.SaveChangesAsync();
                        PropostasTroca.Remove(eliminarTroca);
                        await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta de troca eliminada com sucesso!", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao eliminar a proposta de troca: {ex.Message}");
            }
        }
        //Função para aceitar as propostas de venda ao clicar no botão
        private async Task AceitarButtonClicked(PropostaCompraComDetalhes aceitar)
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var idvenda = aceitar.Id_Sale;
                        var qtd = await dbContext.Venda
                            .Where(v => v.Id_Venda == idvenda)
                            .Select(v => v.QTD_Venda)
                            .FirstOrDefaultAsync();
                        var contacto = aceitar.ContactoComprador;
                        var valor = await dbContext.Venda
                            .Where(v => v.Id_Venda == idvenda)
                            .Select(v => v.ValorVenda)
                            .FirstOrDefaultAsync();
                        var idcomprador = await dbContext.PropostaCompra
                            .Where(pc => pc.Id_Sale == idvenda)
                            .Select(pc => pc.Id_Comprador)
                            .FirstOrDefaultAsync();
                        var idvendedor = await dbContext.PropostaCompra
                            .Where(pc => pc.Id_Sale == idvenda)
                            .Select(pc => pc.Id_User)
                            .FirstOrDefaultAsync();
                        var idcard = await dbContext.Venda
                            .Where(v => v.Id_Venda == idvenda)
                            .Select(v => v.Id_Card)
                            .FirstOrDefaultAsync(); 
                        var vendaCnfirmada = new ConfirmacaoVenda
                        {
                            QTD_Comprada = qtd,
                            ContactoComprador = contacto,
                            ValorVenda = valor,
                            Id_Comprador = idcomprador,
                            Id_Vendedor = idvendedor,
                            Id_CardVendida = idcard,
                            Id_Sale = idvenda
                        };

                        dbContext.ConfirmacaoVenda.Add(vendaCnfirmada);
                        await dbContext.SaveChangesAsync();

                        // Remover a entrada na tabela PropostaCompra
                        var propostaParaRemover = await dbContext.PropostaCompra.FirstOrDefaultAsync(pc => pc.Id_Sale == idvenda);
                        if (propostaParaRemover != null)
                        {
                            dbContext.PropostaCompra.Remove(propostaParaRemover);
                        }
                        //remover a venda na table Venda
                        var vendaParaRemover = await dbContext.Venda.FirstOrDefaultAsync(v => v.Id_Venda == idvenda);
                        if(vendaParaRemover != null)
                        {
                            dbContext.Venda.Remove(vendaParaRemover);
                        }
                        // Diminuir a quantidade na tabela CartasUtilizador
                        var cartaParaAtualizar = await dbContext.CartasUtilizador.FirstOrDefaultAsync(cu => cu.Id_Carta == idcard);
                        if (cartaParaAtualizar != null)
                        {
                            cartaParaAtualizar.Quantidade -= qtd;
                        }

                        await dbContext.SaveChangesAsync();

                        await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta Aceite com sucesso!", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao registrar a proposta: {ex.Message}");
                }

            }
        //Função para aceitar as propostas de troca ao clicar no botão
        private async Task AceitarTrocaButtonClicked(PropostaTrocaComDetalhes aceitarTroca)
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var idtroca = aceitarTroca.Id_Troca;
                    Console.WriteLine(idtroca);
                    var qtd = await dbContext.Troca
                        .Where(v => v.Id_Troca == idtroca)
                        .Select(v => v.QTD_Troca)
                        .FirstOrDefaultAsync();
                    Console.WriteLine(qtd);
                    var contacto = aceitarTroca.ContactoCompradorTroca.ToString();
                    Console.WriteLine(contacto);
                    var opcao = await dbContext.Troca
                        .Where(v => v.Id_Troca == idtroca)
                        .Select(v => v.OpcaoTroca)
                        .FirstOrDefaultAsync();
                    Console.WriteLine(opcao);
                    var idcomprador = await dbContext.PropostaTroca
                        .Where(pc => pc.Id_Troca == idtroca)
                        .Select(pc => pc.Id_Comprador)
                        .FirstOrDefaultAsync();
                    Console.WriteLine(idcomprador);
                    var idvendedor = await dbContext.PropostaTroca
                        .Where(pc => pc.Id_Troca == idtroca)
                        .Select(pc => pc.Id_Vendedor)
                        .FirstOrDefaultAsync();
                    Console.WriteLine(idvendedor);
                    var idcard = await dbContext.Troca
                        .Where(v => v.Id_Troca == idtroca)
                        .Select(v => v.Id_Card)
                        .FirstOrDefaultAsync();
                    Console.WriteLine(idcard);

                    var trocaCnfirmada = new ConfirmacaoTroca
                    {
                        QTD_Trocada = qtd,
                        ContactoComprador = contacto,
                        OpcaoTroca = opcao,
                        Id_Comprador = idcomprador,
                        Id_Vendedor = idvendedor,
                        Id_CardTrocado = idcard,
                        Id_Troca = idtroca
                    };

                    dbContext.ConfirmacaoTroca.Add(trocaCnfirmada);
                    await dbContext.SaveChangesAsync();

                    // Remover a entrada na tabela PropostaCompra
                    var propostaTrocaParaRemover = await dbContext.PropostaTroca.FirstOrDefaultAsync(pc => pc.Id_Troca == idtroca);
                    if (propostaTrocaParaRemover != null)
                    {
                        dbContext.PropostaTroca.Remove(propostaTrocaParaRemover);
                    }
                    //remover a venda na table Venda
                    var trocaParaRemover = await dbContext.Troca.FirstOrDefaultAsync(v => v.Id_Troca == idtroca);
                    if (trocaParaRemover != null)
                    {
                        dbContext.Troca.Remove(trocaParaRemover);
                    }
                    // Diminuir a quantidade na tabela CartasUtilizador
                    var cartaParaAtualizar = await dbContext.CartasUtilizador.FirstOrDefaultAsync(cu => cu.Id_Carta == idcard);
                    if (cartaParaAtualizar != null)
                    {
                        cartaParaAtualizar.Quantidade -= qtd;
                    }

                    await dbContext.SaveChangesAsync();

                    await App.Current.MainPage.DisplayAlert("Sucesso", "Proposta Aceite com sucesso!", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar a proposta de troca: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }

        }
        //carregar as propostas de compra
        private async void LoadPropostasCompra()
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        int idlogin = UserSession.LoggedInUser.Id_User;
                        var propostas = await dbContext.PropostaCompra
                                   .Where(pc => pc.Id_User == idlogin)
                                   .ToListAsync();

                        foreach (var proposta in propostas)
                        {
                            var contacto = int.Parse(proposta.ContactoComprador);
                            var idUser = proposta.Id_User;
                            var idVenda = proposta.Id_Sale;
                            var idComprador = proposta.Id_Comprador;
                            var nome = await dbContext.Utilizador
                                .Where(u => u.Id_User == idUser)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync();
                            var idcardut = await dbContext.Venda
                                .Where(v => v.Id_Venda == idVenda)
                                .Select(v => v.Id_Card)
                                .FirstOrDefaultAsync();
                            var idcard = await dbContext.CartasUtilizador
                                .Where(v => v.Id_Carta == idcardut)
                                .Select(v => v.Id_Card)
                                .FirstOrDefaultAsync();
                            var nomeCarta = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == idcard)
                                .Select(ca => ca.PersonagemCarta)
                                .FirstOrDefaultAsync();
                            var nomeComprador = await dbContext.Utilizador
                                .Where(u => u.Id_User == idComprador)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync ();


                            var AceitarButton = new Button
                            {
                                Text = "Aceitar",
                                VerticalOptions = LayoutOptions.End,
                                HorizontalOptions = LayoutOptions.Center,
                                Margin = new Thickness(0, 10, 0, 0)
                            };
                            var aceitarItem = new PropostaCompraComDetalhes { };

                            PropostasCompra.Add(new PropostaCompraComDetalhes
                            {
                                
                                Id_PrCompra = proposta.Id_PrCompra,
                                ContactoComprador = contacto,
                                Id_User = idUser,
                                Id_Sale = idVenda,
                                NomeComprador = nomeComprador,
                                NomeCarta = nomeCarta
                               
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar as propostas de compra: {ex.Message}");
                }
            }
        //carregar as propostas de troca
        private async void LoadPropostasTroca()
            {
                try
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        int idlogin = UserSession.LoggedInUser.Id_User;
                        var propostasTroca = await dbContext.PropostaTroca
                                   .Where(pt => pt.Id_Vendedor == idlogin)
                                   .ToListAsync();

                        foreach (var propostaTroca in propostasTroca)
                        {
                            var contactotroca = propostaTroca.ContactoComprador.ToString();
                            var idUser = propostaTroca.Id_Vendedor;
                            var idTroca = propostaTroca.Id_Troca;
                            var idComprador = propostaTroca.Id_Comprador;
                            var nome = await dbContext.Utilizador
                                .Where(u => u.Id_User == idUser)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync();
                            var idcardut = await dbContext.Troca
                                .Where(t => t.Id_Troca == idTroca)
                                .Select(t => t.Id_Card)
                                .FirstOrDefaultAsync();
                            var idcard = await dbContext.CartasUtilizador
                                .Where(cu => cu.Id_Carta == idcardut)
                                .Select(cu => cu.Id_Card)
                                .FirstOrDefaultAsync();
                            var nomeCarta = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == idcard)
                                .Select(ca => ca.PersonagemCarta)
                                .FirstOrDefaultAsync();
                            var nomeComprador = await dbContext.Utilizador
                                .Where(u => u.Id_User == idComprador)
                                .Select(u => u.UserName)
                                .FirstOrDefaultAsync();


                            var AceitarTrocaButton = new Button
                            {
                                Text = "Aceitar",
                                VerticalOptions = LayoutOptions.End,
                                HorizontalOptions = LayoutOptions.Center,
                                Margin = new Thickness(0, 10, 0, 0)
                            };
                            var aceitarTrocaItem = new PropostaTrocaComDetalhes { };

                            PropostasTroca.Add(new PropostaTrocaComDetalhes
                            {

                                Id_PrTroca = propostaTroca.Id_PrTroca,
                                ContactoCompradorTroca = contactotroca,
                                Id_Vendedor = idUser,
                                Id_Troca = idTroca,
                                NomeComprador = nomeComprador,
                                NomeCarta = nomeCarta

                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar as propostas de troca: {ex.Message}");
                }
            }
    }

        public class PropostaCompraComDetalhes
        {
            public int Id_PrCompra { get; set; }
            public int ContactoComprador { get; set; }
            public int Id_User { get; set; }
            public string NomeComprador { get; set; }
            public int Id_Sale { get; set; }
            public string NomeCarta { get; set; }
           
        }
    public class PropostaTrocaComDetalhes
    {
        public int Id_PrTroca { get; set; }
        public string ContactoCompradorTroca { get; set; } 
        public int Id_Vendedor { get; set; }
        public string NomeComprador { get; set; }
        public int Id_Troca { get; set; }
        public string NomeCarta { get; set; }
    }
}
