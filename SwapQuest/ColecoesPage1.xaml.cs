using Microsoft.EntityFrameworkCore;

namespace SwapQuest;

public partial class ColecoesPage1 : ContentPage
{
    public ColecoesPage1()
    {
        InitializeComponent();
        CarregarColecoesDoUtilizador();
    }

    //Fun��o para carregar as cole��es do utilizador da base de dados
    private async void CarregarColecoesDoUtilizador()
    {
        using (var dbContext = new ApplicationDbContext())
        {
            var utilizador = dbContext.Utilizador.Find(UserSession.LoggedInUser.Id_User);
            
            if (utilizador != null)
            {
                // Carregar as cole��es do utilizador 
                var colecoesDoUtilizador = dbContext.ColecaoUtilizador
                    .Where(cu => cu.Id_Utilizador == utilizador.Id_User)
                    .ToList();
                
                
                
                // Criar e exibir o bot�o correspondentes � cole��o do utilizador
                foreach (var colecao in colecoesDoUtilizador)
                {
                    var jogout = colecao.Id_ColecaoUtilizador;
                    var jogo = await dbContext.ColecoesAPI
                        .Where(ca => ca.Id_Colecao == colecao.Id_Colecao)
                        .Select(ca => ca.DescricaoColecao)
                        .FirstOrDefaultAsync();
                    var idcolecaoutilizador = await dbContext.ColecaoUtilizador
                   .Where(cu => cu.Id_Utilizador == utilizador.Id_User & cu.Id_Colecao == cu.Id_Colecao)
                   .Select(cu => cu.Id_ColecaoUtilizador)
                   .FirstOrDefaultAsync();
                    
                    Button newButton = new Button
                    {
                        Text = jogo,
                        WidthRequest = 200,
                        Margin = new Thickness(0, 20, 0, 0)
                    };

                    
                    newButton.Clicked += async (sender, args) =>
                    {
                        // Caminho para a p�gina correspondente � cole��o
                        if (jogo == "Pokemon")
                        {
                           
                            await Navigation.PushAsync(new CartasPage(jogout));
                        }
                        if (jogo == "Magic")
                        {

                            await Navigation.PushAsync(new CartasPage(jogout));
                        }
                        if (jogo == "Lorcana")
                        {
                            await Navigation.PushAsync(new CartasPage(jogout));
                        }
                    };

                    buttonContainer.Children.Add(newButton);
                }
            }
        }
    }

    //Fun��o para criar novas cole��es do utilizador
    private void CriarColecao(object sender, EventArgs e)
    {
        string selectedText = AdicionarColecao.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selectedText))
        {
            // Verifica se j� existe um bot�o para a cole��o selecionada
            bool botaoExistente = false;
            foreach (Button button in buttonContainer.Children)
            {
                if (button.Text == selectedText)
                {
                    botaoExistente = true;
                    break;
                }
            }

            // Se n�o houver bot�o para a cole��o selecionada, cria um novo bot�o
            if (!botaoExistente)
            {
                // Chama o m�todo AdicionarColecaoAoUtilizador para adicionar a cole��o � conta do utilizador
                ColecaoManager.AdicionarColecaoAoUtilizador(selectedText);

                Button newButton = new Button
                {
                    Text = selectedText,
                    WidthRequest = 200,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                buttonContainer.Children.Add(newButton);
                DisplayAlert("Aviso", "Cole��o adicionada com sucesso! Para recarregar volte a abrir o menu Cole��o", "OK");
            }
            else
            {
                DisplayAlert("Aviso", "Esta cole��o j� existe", "OK");
            }
        }
    }

}