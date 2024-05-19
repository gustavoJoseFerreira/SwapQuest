namespace SwapQuest;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    async void ConfirmClicked(object sender, EventArgs e)
    {
        // Verifica se os campos obrigat�rios est�o preenchidos
        if (string.IsNullOrWhiteSpace(loginMail.Text) || string.IsNullOrWhiteSpace(loginPasswd.Text))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos.", "OK");
            return; 
        }

        var userEmail = loginMail.Text;
        var userPassword = loginPasswd.Text;

        if (!(userEmail.Contains('@') && userEmail.Contains('.')))
        {
            await DisplayAlert("Erro", "Por favor, insira um email v�lido.", "OK");
            return;
        }
        // Chama o m�todo AuthenticateAsync da classe AuthService para autenticar o utilizador
        Utilizador loggedInUser = await AuthService.AuthenticateAsync(userEmail, userPassword);

        // Verifica se a autentica��o foi bem-sucedida
        if (loggedInUser != null)
        {
            // Armazena o usu�rio logado na sess�o
            UserSession.LoggedInUser = loggedInUser;

            await DisplayAlert("Sucesso", "Login realizado com sucesso", "OK");
            // Navega para a pr�xima p�gina ap�s o login bem-sucedido
            await Navigation.PushAsync(new HomePage());
        }
        else
        {
            await DisplayAlert("Erro", "Nome de usu�rio ou senha incorretos", "OK");
        }

    }
}