

namespace SwapQuest;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}


    async void ButtonRegister_Clicked(object sender, EventArgs e)
    {
        string mail = UserEmail.Text;
        char arroba = '@';
        char ponto = '.';

        // Verifica se os campos obrigat�rios est�o preenchidos
        if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(UserEmail.Text) || string.IsNullOrWhiteSpace(UserPassword.Text))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigat�rios.", "OK");
            return; 
        }
        // Verifica se o email � v�lido
        if (!(mail.Contains(arroba) && mail.Contains(ponto)))
        {
            await DisplayAlert("Erro", "Por favor, insira um email v�lido.", "OK");
            return;
        }
        string pass = UserPassword.Text;
        string confirmarPass = RepetirPassword.Text;
        //verifica se a pass tem o tamanha necess�rio
        if ( pass.Length < 8)
        {
            await DisplayAlert("Erro", "Por favor, insira uma palavra chave com pelo menos 8 caracteres.", "OK");
            return;
        }
        //verifica se as passwords coincidem
        if (!(pass == confirmarPass)){
            await DisplayAlert("Erro", "As palavras chave n�o coincidem, por favor volte a tentar.", "OK");
            return;
        }
        //verifica se o email j� est� em uso
        using (var dbContext = new ApplicationDbContext())
        {
            if (dbContext.Utilizador.Any(u => u.UserEmail == UserEmail.Text))
            {
                await DisplayAlert("Erro", "O email inserido j� se encontra em uso.", "OK");
                return;
            }
        }

        string username = UserName.Text;
        string userEmail = UserEmail.Text;
        string userpassword = UserPassword.Text;
        string userDistrito = picker.SelectedItem.ToString();
        DateTime userdn = datePicker.Date;

        Utilizador newUtilizador = new Utilizador
        {
            UserName = username,
            UserPassword = userpassword,
            UserEmail = userEmail,
            UserDistrito = userDistrito,
            UserDN = userdn
        };
        // Chama o m�todo de registro na classe de servi�o AuthService
        bool isSuccess = await AuthService.RegisterAsync(newUtilizador);

        if (isSuccess)
        {
            await DisplayAlert("Sucesso", "Conta criada com sucesso", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Erro", "Erro ao criar conta HHHHHHHHHH" , "OK");
        }
        
        await Shell.Current.GoToAsync("LoginPage");
        await DisplayAlert("Alerta", "Fa�a login para utilizar a aplica��o!", "OK");


    }
}