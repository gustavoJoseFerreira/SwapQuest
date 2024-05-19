

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

        // Verifica se os campos obrigatórios estão preenchidos
        if (string.IsNullOrWhiteSpace(UserName.Text) || string.IsNullOrWhiteSpace(UserEmail.Text) || string.IsNullOrWhiteSpace(UserPassword.Text))
        {
            await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
            return; 
        }
        // Verifica se o email é válido
        if (!(mail.Contains(arroba) && mail.Contains(ponto)))
        {
            await DisplayAlert("Erro", "Por favor, insira um email válido.", "OK");
            return;
        }
        string pass = UserPassword.Text;
        string confirmarPass = RepetirPassword.Text;
        //verifica se a pass tem o tamanha necessário
        if ( pass.Length < 8)
        {
            await DisplayAlert("Erro", "Por favor, insira uma palavra chave com pelo menos 8 caracteres.", "OK");
            return;
        }
        //verifica se as passwords coincidem
        if (!(pass == confirmarPass)){
            await DisplayAlert("Erro", "As palavras chave não coincidem, por favor volte a tentar.", "OK");
            return;
        }
        //verifica se o email já está em uso
        using (var dbContext = new ApplicationDbContext())
        {
            if (dbContext.Utilizador.Any(u => u.UserEmail == UserEmail.Text))
            {
                await DisplayAlert("Erro", "O email inserido já se encontra em uso.", "OK");
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
        // Chama o método de registro na classe de serviço AuthService
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
        await DisplayAlert("Alerta", "Faça login para utilizar a aplicação!", "OK");


    }
}