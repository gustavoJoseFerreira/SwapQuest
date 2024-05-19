namespace SwapQuest;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    async void ConfirmClicked(object sender, EventArgs e)
    {
        // Verifica se os campos obrigatórios estão preenchidos
        if (string.IsNullOrWhiteSpace(loginMail.Text) || string.IsNullOrWhiteSpace(loginPasswd.Text))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return; 
        }

        var userEmail = loginMail.Text;
        var userPassword = loginPasswd.Text;

        if (!(userEmail.Contains('@') && userEmail.Contains('.')))
        {
            await DisplayAlert("Erro", "Por favor, insira um email válido.", "OK");
            return;
        }
        // Chama o método AuthenticateAsync da classe AuthService para autenticar o utilizador
        Utilizador loggedInUser = await AuthService.AuthenticateAsync(userEmail, userPassword);

        // Verifica se a autenticação foi bem-sucedida
        if (loggedInUser != null)
        {
            // Armazena o usuário logado na sessão
            UserSession.LoggedInUser = loggedInUser;

            await DisplayAlert("Sucesso", "Login realizado com sucesso", "OK");
            // Navega para a próxima página após o login bem-sucedido
            await Navigation.PushAsync(new HomePage());
        }
        else
        {
            await DisplayAlert("Erro", "Nome de usuário ou senha incorretos", "OK");
        }

    }
}