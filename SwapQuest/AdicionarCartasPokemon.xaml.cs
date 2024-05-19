using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace SwapQuest
{
    public partial class AdicionarCartasPokemon : ContentPage
    {
        
        public ObservableCollection<AdicionarCartasViewModel> Cartas { get; set; }
       

        public AdicionarCartasPokemon()
        {           
            InitializeComponent();
            Cartas = new ObservableCollection<AdicionarCartasViewModel>();
            
            LoadCartas();
            BindingContext = this;
            
        }

        //FUnção para carregar as cartas da tabela CartasAPI
        private async void LoadCartas()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var cartasAPI = await dbContext.CartasAPI.ToListAsync();

                foreach (var carta in cartasAPI)
                {
                        var idcarta = await dbContext.CartasAPI
                             .Where(ca => ca.Id_Carta == carta.Id_Carta)
                             .Select(ca => ca.Id_Carta)
                             .FirstOrDefaultAsync(); 
                        var numCarta = await dbContext.CartasAPI
                             .Where(ca => ca.Id_Carta == carta.Id_Carta)
                             .Select(ca => ca.NumeroCarta)
                             .FirstOrDefaultAsync();
                        var imagem = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == carta.Id_Carta)
                            .Select(ca => ca.Imagem)
                            .FirstOrDefaultAsync();
                        var personagem = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == carta.Id_Carta)
                            .Select(ca => ca.PersonagemCarta)
                            .FirstOrDefaultAsync();
                        var serie = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == carta.Id_Carta)
                            .Select(ca => ca.NomeSerie)
                            .FirstOrDefaultAsync();
                        var lancamento = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == carta.Id_Carta)
                            .Select(ca => ca.AnoLancamento)
                            .FirstOrDefaultAsync();
                        var preco = await dbContext.CartasAPI
                            .Where(ca => ca.Id_Carta == carta.Id_Carta)
                            .Select(ca => ca.PrecoRef)
                            .FirstOrDefaultAsync();
                    
                        Cartas.Add(new AdicionarCartasViewModel
                        {
                                Id_Carta = idcarta,
                                NumeroCarta = numCarta,
                                Imagem = imagem,
                                PersonagemCarta = personagem,
                                NomeSerie = serie,
                                AnoLancamento = lancamento,
                                PrecoRef = preco
                        });
                    
                }
            }
        }
        private async void OnAdicionarCartaClicked(object sender, EventArgs e)
        {
            // Obtém o Id_Utilizador do utilizador logado
            Utilizador utilizadorLogado = UserSession.LoggedInUser;
            int idUtilizador = utilizadorLogado.Id_User;

            // Recupera os detalhes da carta selecionada
            AdicionarCartasViewModel cartaSelecionada = (sender as Button)?.BindingContext as AdicionarCartasViewModel;
            if (cartaSelecionada == null)
            {
                
                return;
            }

            // Cria a Entry para introduzir a quantidade
            var quantidadeEntry = new Entry
            {
                Placeholder = "Quantidade",
                Keyboard = Keyboard.Numeric
            };

            // Cria os pickers para seleção de qualidade e do idioma
            var qualidadePicker = new Picker
            {
                Title = "Selecione a Qualidade",
                ItemsSource = new List<string> { "Excelente", "Boa", "Usada" },
                SelectedIndex = 0
            };
            var idiomaPicker = new Picker
            {
                Title = "Selecione o Idioma",
                ItemsSource = new List<string> { "Português", "Inglês", "Japonês" },
                SelectedIndex = 0
            };
            var infoLabelTitulo = new Label
            {

                Text = "Aviso!",
                Margin = new Thickness(20, 30, 20, 0),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };
            var infoLabel = new Label
            {
                
                Text = "Para terminar de adicionar a carta é so retornar ao menú anterior e a carta será guardada na sua coleção.",
                Margin = new Thickness(20, 5, 20, 0)
            };

            
            var layout = new StackLayout();
            layout.Children.Add(quantidadeEntry);
            layout.Children.Add(qualidadePicker);
            layout.Children.Add(idiomaPicker);
            layout.Children.Add(infoLabelTitulo);
            layout.Children.Add(infoLabel);

            var page = new ContentPage
            {
                Content = layout
            };

            // Assina o evento de fecho da página para tratar os valores selecionados
            page.Disappearing += async (s, ev) =>
            {
                // Verifica se a quantidade é válida
                if (!int.TryParse(quantidadeEntry.Text, out int quantidade) || quantidade <= 0)
                {
                    await DisplayAlert("Atenção", "Por favor, insira uma quantidade válida.", "OK");
                    return;
                }

                // Verifica se a qualidade e o idioma foram selecionados
                string qualidadeSelecionada = qualidadePicker.SelectedItem as string;
                string idiomaSelecionado = idiomaPicker.SelectedItem as string;

                if (string.IsNullOrEmpty(qualidadeSelecionada) || string.IsNullOrEmpty(idiomaSelecionado))
                {
                    await DisplayAlert("Atenção", "Por favor, selecione a qualidade e o idioma da carta.", "OK");
                    return;
                }

                // Verifica se a carta já existe na coleção do usuário com a mesma qualidade e mesmo idioma
                using (var dbContext = new ApplicationDbContext())
                {
                    var cartaExistente = await dbContext.CartasUtilizador
                        .FirstOrDefaultAsync(cu => cu.Id_Utilizador == idUtilizador && cu.Id_Card == cartaSelecionada.Id_Carta && cu.Qualidade == qualidadeSelecionada && cu.Idioma == idiomaSelecionado);

                    if (cartaExistente != null)
                    {
                        // Se a carta já existir com a mesma qualidade, atualiza apenas a quantidade
                        cartaExistente.Quantidade += quantidade;
                    }
                    else
                    {
                        // Se a carta não existir ou existir com qualidade diferente, adiciona à coleção do usuário
                        CartasUtilizador cartaUtilizador = new CartasUtilizador
                        {
                            Id_Utilizador = idUtilizador,
                            Id_Card = cartaSelecionada.Id_Carta,
                            Quantidade = quantidade,
                            Qualidade = qualidadeSelecionada,
                            Idioma = idiomaSelecionado
                        };

                        dbContext.CartasUtilizador.Add(cartaUtilizador);
                    }

                    int result = await dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        await DisplayAlert("Sucesso", "Carta adicionada com sucesso! Para recarregar a coleção volte a abrir a coleção.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Falha", "Falha ao adicionar carta. Tente novamente mais tarde.", "OK");
                    }
                }
            };

            await Navigation.PushAsync(page);
        }

    }
}

