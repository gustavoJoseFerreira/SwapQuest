using Microsoft.EntityFrameworkCore;

namespace SwapQuest;

public partial class ColecoesPage1 : ContentPage
{
    public ColecoesPage1()
    {
        InitializeComponent();
        CarregarColecoesDoUtilizador();
    }

    //Função para carregar as coleções do utilizador da base de dados
    private async void CarregarColecoesDoUtilizador()
    {
        using (var dbContext = new ApplicationDbContext())
        {
            var utilizador = dbContext.Utilizador.Find(UserSession.LoggedInUser.Id_User);
            
            if (utilizador != null)
            {
                // Carregar as coleções do utilizador 
                var colecoesDoUtilizador = dbContext.ColecaoUtilizador
                    .Where(cu => cu.Id_Utilizador == utilizador.Id_User)
                    .ToList();
                
                
                
                // Criar e exibir o botão correspondentes à coleção do utilizador
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
                        // Caminho para a página correspondente à coleção
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

    //Função para criar novas coleções do utilizador
    private void CriarColecao(object sender, EventArgs e)
    {
        string selectedText = AdicionarColecao.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(selectedText))
        {
            // Verifica se já existe um botão para a coleção selecionada
            bool botaoExistente = false;
            foreach (Button button in buttonContainer.Children)
            {
                if (button.Text == selectedText)
                {
                    botaoExistente = true;
                    break;
                }
            }

            // Se não houver botão para a coleção selecionada, cria um novo botão
            if (!botaoExistente)
            {
                // Chama o método AdicionarColecaoAoUtilizador para adicionar a coleção à conta do utilizador
                ColecaoManager.AdicionarColecaoAoUtilizador(selectedText);

                Button newButton = new Button
                {
                    Text = selectedText,
                    WidthRequest = 200,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                buttonContainer.Children.Add(newButton);
                DisplayAlert("Aviso", "Coleção adicionada com sucesso! Para recarregar volte a abrir o menu Coleção", "OK");
            }
            else
            {
                DisplayAlert("Aviso", "Esta coleção já existe", "OK");
            }
        }
    }

}