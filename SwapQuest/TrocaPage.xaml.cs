using Microsoft.EntityFrameworkCore;

namespace SwapQuest;

public partial class TrocaPage : ContentPage
{
    public int idCarta2;
    public TrocaPage(int idCarta)
	{
		InitializeComponent();
        //Id da carta do utilizador
        idCarta2 = idCarta;
        Utilizador utilizadorLogado = UserSession.LoggedInUser;
        int idUtilizador = utilizadorLogado.Id_User;

        var label = new Label
        {
            Text = "Insira a quantidade e opção de troca:",
            Margin = new Thickness(0, 0, 0, 50)
        };
        var quantidadeEntry = new Entry
        {
            Placeholder = "Quantidade",
            Keyboard = Keyboard.Numeric, 
            Margin = new Thickness(0, 0, 0, 50)
        };

        var opcaotroca = new Entry
        {
            Placeholder = "O que pretende receber",
            Keyboard = Keyboard.Default, 
            Margin = new Thickness(0, 0, 0, 50)
        };
        var trocaButton = new Button
        {
            Text = "Criar troca",
            HorizontalOptions = LayoutOptions.Center,
            Margin = new Thickness(0, 20, 0, 0)
        };
        //Tratar o clique do botão para criar troca
        trocaButton.Clicked += async (sender, e) =>
        {
            var quantidade = int.Parse(quantidadeEntry.Text);
            var opcao = opcaotroca.Text;

            // Criando uma nova Troca
            var novaTroca = new Troca
            {
                QTD_Troca = quantidade,
                OpcaoTroca = opcao,
                Id_User = idUtilizador,
                Id_Card = idCarta2 
            };

            // Criar a troca 
            using (var dbContext = new ApplicationDbContext())
            {
                var qtdUtilizador = await dbContext.CartasUtilizador
                            .Where(cu => cu.Id_Carta == idCarta2)
                            .Select(cu => cu.Quantidade)
                            .FirstOrDefaultAsync();
                if (!(quantidade > qtdUtilizador))
                {
                    dbContext.Troca.Add(novaTroca);
                    await dbContext.SaveChangesAsync();
                    
                    await DisplayAlert("Sucesso", "Troca registrada com sucesso!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Quantidade superior ao permitido.", "OK");
                }
            }

            await Navigation.PopAsync();
        };
        var stackLayout = new Microsoft.Maui.Controls.StackLayout
        {
            WidthRequest = 200,
            HeightRequest = 500,
            Children = { label, quantidadeEntry, opcaotroca, trocaButton },
            Margin = new Thickness(20)
        };

        Content = stackLayout;
    }
}