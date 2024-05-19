using System;
using Microsoft.Maui.Controls;
using Microsoft.EntityFrameworkCore;

namespace SwapQuest
{
    public partial class VendaPage : ContentPage
    {
        public int idCarta1;

        public VendaPage(int idCarta)
        {
            InitializeComponent();
            //id da carta do utilizador
            idCarta1 = idCarta;

            Utilizador utilizadorLogado = UserSession.LoggedInUser;
            int idUtilizador = utilizadorLogado.Id_User;
           

            var label = new Label
            {
                Text = "Insira a quantidade e o preço de venda que deseja:",
                Margin = new Thickness(0, 0, 0, 50)
            };
            
            var precoEntry = new Entry
            {
                Placeholder = "Preço da Carta",
                Keyboard = Keyboard.Numeric 
            };

            var quantidadeEntry = new Entry
            {
                Placeholder = "Quantidade",
                Keyboard = Keyboard.Numeric, 
                Margin = new Thickness(0, 0, 0, 50)
            };

            
            var venderButton = new Button
            {
                Text = "Criar venda",
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 0)
            };

            // Tratar o clique para criar a venda
            venderButton.Clicked += async (sender, e) =>
            {
                try
                {
                    
                    var preco = decimal.Parse(precoEntry.Text);
                    var quantidade = int.Parse(quantidadeEntry.Text);
                    
                    // Criando uma nova Venda
                    var novaVenda = new Venda
                    {
                        ValorVenda = preco,
                        QTD_Venda = quantidade,
                        Id_User = idUtilizador, 
                        Id_Card = idCarta1 
                    };

                    // Criar a Venda
                    using (var dbContext = new ApplicationDbContext())
                    {   
                        var qtdUtil = await dbContext.CartasUtilizador
                            .Where(cu => cu.Id_Carta == idCarta1)
                            .Select(cu => cu.Quantidade)
                            .FirstOrDefaultAsync();
                        if ( !(quantidade > qtdUtil)) { 
                        dbContext.Venda.Add(novaVenda);
                        await dbContext.SaveChangesAsync();
                            // Exibindo uma mensagem de confirmação
                            await DisplayAlert("Sucesso", "Venda registrada com sucesso!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("Erro","Quantidade superior ao permitido.", "OK");
                        }
                    }

                    

                    await Navigation.PopAsync();
                }
                catch (Exception ex)
                {
                    // Exibir a mensagem de erro 
                    var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    await DisplayAlert("Erro", $"Ocorreu um erro ao registrar a venda: {errorMessage}", "OK");
                }
            };

            
            var stackLayout = new Microsoft.Maui.Controls.StackLayout
            {
                WidthRequest = 200,
                HeightRequest = 500,
                Children = { label, precoEntry, quantidadeEntry, venderButton },
                Margin = new Thickness(20)
            };

            
            Content = stackLayout;
        }
    }
}
