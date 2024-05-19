namespace SwapQuest;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();

    }

    //Caminhos para os menus
    private async void VerColecoes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("colecoes");
    }
    private async void ProcurarCartas(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("ProcuraCartas");
    }
    private async void Definicoes(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("Definicoes");
    }
    private async void VendaCLicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CartasVenda");
    }
    private async void TrocaCLicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("CartasTroca");
    }
    private async void PropostasCLicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("PropostasRecebidas");
    }
    private async void logoutClicked(object sender, EventArgs e)
    {
        // Limpa as informações do usuário logado da sessão
        UserSession.LoggedInUser = null;
        await DisplayAlert("Sucesso", "Logout realizado com sucesso", "OK");
        // Navega de volta para a página de login
        await Navigation.PushAsync(new MainPage());
    }
}