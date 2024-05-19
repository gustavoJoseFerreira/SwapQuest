using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;


namespace SwapQuest
{
    public partial class CartasVenda : ContentPage
    {
        public CartasVenda()
        {
            InitializeComponent();
            BindingContext = new CartasVendidasViewModel();

        }
        //Função do botão para eliminar a venda
        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            try
            {
                // Lógica para eliminar a venda
                if (sender is Button button && button.CommandParameter is int idVenda)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var vendaParaEliminar = await dbContext.Venda.FirstOrDefaultAsync(v => v.Id_Venda == idVenda);
                        if (vendaParaEliminar != null)
                        {
                            dbContext.Venda.Remove(vendaParaEliminar);
                            await dbContext.SaveChangesAsync();
                            await DisplayAlert("Alerta", "Venda eliminada com sucesso! Volte atrás para atualizar as suas vendas", "OK");
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

    //Viw model para mostar as cartas à venda
    public class CartasVendidasViewModel
    {
        public ObservableCollection<VendaComNomeCarta> CartasVendidas { get; set; }

        public CartasVendidasViewModel()
        {
            CartasVendidas = new ObservableCollection<VendaComNomeCarta>();
            LoadCartasVendidas();
        }

        //Carregar as cartas da base de dados
        private async void LoadCartasVendidas()
        {
            try
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    int idlogin = UserSession.LoggedInUser.Id_User;
                    var idut = await dbContext.Venda
                               .Where(ve => ve.Id_User == idlogin)
                               .Select(ve => ve.Id_User)
                               .FirstOrDefaultAsync();

                    if (idlogin == idut)
                    {
                        var cartasParaVenda = await dbContext.Venda
                                .Where(cu => cu.Id_User == idlogin)
                                .ToListAsync();
                        foreach (var cartaParaVenda in cartasParaVenda)
                        {
                            var valor = cartaParaVenda.ValorVenda;
                            var valorFormatado = valor.ToString("0.00");
                            var qtd = cartaParaVenda.QTD_Venda;
                            var id = cartaParaVenda.Id_Card;
                            var idcartaapi = await dbContext.CartasUtilizador
                                .Where(ca => ca.Id_Carta == id)
                                .Select(ca => ca.Id_Card)
                                .FirstOrDefaultAsync();
                            var nome = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == idcartaapi)
                                .Select(ca => ca.PersonagemCarta)
                                .FirstOrDefaultAsync();
                            var image = await dbContext.CartasAPI
                               .Where(ca => ca.Id_Carta == idcartaapi)
                               .Select(ca => ca.Imagem)
                               .FirstOrDefaultAsync();
                            Console.WriteLine(image);
                            Console.WriteLine(nome + id);

                            CartasVendidas.Add(new VendaComNomeCarta
                            {
                                Id_Venda = cartaParaVenda.Id_Venda,
                                ValorVenda = valorFormatado,
                                QTD_Venda = qtd,
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

    public class VendaComNomeCarta
    {
        public int Id_Venda { get; set; }
        public string ValorVenda { get; set; }
        public int QTD_Venda { get; set; }
        public int Id_User { get; set; }
        public int Id_Card { get; set; }
        public string NomeCarta { get; set; }
        public string imagem { get; set; }
    }
}