using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Platform;

namespace SwapQuest;

public partial class Definicoes : ContentPage
{
    private readonly ApplicationDbContext _context;
    public Definicoes()
	{
        _context = new ApplicationDbContext();
        InitializeComponent();
		
        if (UserSession.LoggedInUser != null)
        {
            // Obt�m o nome do usu�rio logado
            string nome = UserSession.LoggedInUser.UserName;

            // Cria uma label com o nome do usu�rio
            Label minhaLabel = new Label
            {
                Text = $"Aqui pode atualizar a sua informa��o {nome}",
                FontSize = 25,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(30, 0, 30, 0)
            };

            Label minhaLabel2 = new Label
            {
                Text = $"Alterar palavra passe: ",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 75, 0, 0)
            };

            Entry passAntiga = new Entry
            {
                Placeholder = "Introduzir palavra passe atual",
                MinimumWidthRequest = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 0)
            };
            Entry novaPass = new Entry
            {
                Placeholder = "Introduzir nova palavra passe ",
                MinimumWidthRequest = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            Entry confirmaPass = new Entry
            {
                Placeholder = "Confirmar nova palavra passe",
                MinimumWidthRequest = 0,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 5, 0, 0)
            };
            Button alteraPass = new Button
            {
                Text = "Alterar palavra passe",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0,20,0,0)
                
            };
            Label minhaLabel3 = new Label
            {
                Text = $"Alterar localiza��o: ",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(10, 50, 0, 0)
            };

            Picker novaLocal = new Picker
            {
                Title = "Selecione o novo Distrito",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };
            // Adicionando itens ao Picker
            novaLocal.Items.Add("Aveiro");
            novaLocal.Items.Add("Beja");
            novaLocal.Items.Add("Braga");
            novaLocal.Items.Add("Bragan�a");
            novaLocal.Items.Add("Castelo Branco");
            novaLocal.Items.Add("Coimbra");
            novaLocal.Items.Add("�vora");
            novaLocal.Items.Add("Faro");
            novaLocal.Items.Add("Guarda");
            novaLocal.Items.Add("Leiria");
            novaLocal.Items.Add("Lisboa");
            novaLocal.Items.Add("Portalegre");
            novaLocal.Items.Add("Porto");
            novaLocal.Items.Add("Santar�m");
            novaLocal.Items.Add("Set�bal");
            novaLocal.Items.Add("Viana do Castelo");
            novaLocal.Items.Add("Vila Real");
            novaLocal.Items.Add("Viseu");
            novaLocal.Items.Add("Madeira");
            novaLocal.Items.Add("A�ores");

            // Manipulando o evento de sele��o de item (opcional)
            novaLocal.SelectedIndexChanged += (sender, e) =>
            {
                var itemSelecionado = novaLocal.SelectedItem;
                
            };

            Button alteraLocal = new Button
            {
                Text = "Alterar o distrito",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 20, 0, 0)

            };
            // Adiciona a l�gica para alterar a palavra-passe
            alteraPass.Clicked += async (sender, e) =>
            {
                if (UserSession.LoggedInUser != null)
                {
                    string username = UserSession.LoggedInUser.UserName;
                    string oldPassword = passAntiga.Text;
                    string newPassword = novaPass.Text;

                    // Verificar se a senha antiga est� correta
                    var user = await _context.Utilizador.FirstOrDefaultAsync(u => u.UserName == username && u.UserPassword == oldPassword);
                    if (user != null)
                    {
                        // Atualizar a palavra-passe
                        user.UserPassword = newPassword;
                        await _context.SaveChangesAsync();
                        await DisplayAlert("Sucesso", "Palavra-passe alterada com sucesso.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Palavra-passe antiga incorreta.", "OK");
                    }
                }
            };

            // Adiciona a l�gica para alterar a localiza��o
            alteraLocal.Clicked += async (sender, e) =>
            {
                if (UserSession.LoggedInUser != null)
                {
                    string username = UserSession.LoggedInUser.UserName;
                    string novoDistrito = novaLocal.SelectedItem.ToString();

                    // Verificar se o usu�rio existe
                    var user = await _context.Utilizador.FirstOrDefaultAsync(u => u.UserName == username);
                    if (user != null)
                    {
                        // Atualizar o distrito do usu�rio
                        user.UserDistrito = novoDistrito;
                        await _context.SaveChangesAsync();
                        await DisplayAlert("Sucesso", "Distrito alterado com sucesso.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Usu�rio n�o encontrado.", "OK");
                    }
                }
            };

            // Adiciona a label e a Entry a um StackLayout
            StackLayout layout = new StackLayout();
            layout.Children.Add(minhaLabel);
            layout.Children.Add(minhaLabel2);
            layout.Children.Add(passAntiga);
            layout.Children.Add(novaPass);
            layout.Children.Add(confirmaPass);
            layout.Children.Add(alteraPass);
            layout.Children.Add(minhaLabel3);
            layout.Children.Add(novaLocal);
            layout.Children.Add(alteraLocal);

            // Define o StackLayout como o conte�do da p�gina
            Content = layout;
        }
        else
        {
            // Caso n�o haja usu�rio logado, exibe uma mensagem de erro
            Label erroLabel = new Label
            {
                Text = "Nenhum usu�rio logado.",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            // Define a label de erro como o conte�do da p�gina
            Content = new StackLayout
            {
                Children = { erroLabel }
            };
        }

    }
}