using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace SwapQuest
{
    public partial class CartasPage : ContentPage
    {
        private readonly int idColecaoUtilizador;

        public ObservableCollection<CartaUtiViewModel> Cartas { get; set; }

        public CartasPage(int idColecaoUtilizador)
        {
            InitializeComponent();

            BindingContext = new CartasViewModel(idColecaoUtilizador);

            //variavel que guarda o id da coleção do utilizador
            this.idColecaoUtilizador = idColecaoUtilizador;

            //Coleção para apresentar as cartas do utilizador
            Cartas = new ObservableCollection<CartaUtiViewModel>();

            var cartasLayout = new Microsoft.Maui.Controls.StackLayout();


            // Configurar a coleção de cartas
            var collectionView = new CollectionView();
            collectionView.SetBinding(CollectionView.ItemsSourceProperty, new Binding(nameof(Cartas)));
            Button adicionar = new Button
            {
                Text = "Adicionar cartas à coleção",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 0, 40)

            };


            // Configurar o modelo para a CollectionView
            collectionView.ItemTemplate = new DataTemplate(() =>
            {
                var image = new Image();
                image.SetBinding(Image.SourceProperty, "Imagem");
                image.WidthRequest = 300;

                var nomeLabel = new Label();
                nomeLabel.SetBinding(Label.TextProperty, "NomeCarta");
                nomeLabel.FontAttributes = FontAttributes.Bold;
                nomeLabel.HorizontalOptions = LayoutOptions.Center;
                nomeLabel.FontSize = 20;

                var precoRefLabel = new Label();
                precoRefLabel.SetBinding(Label.TextProperty, "PrecoRef", stringFormat: "{0} €");
                precoRefLabel.HorizontalOptions = LayoutOptions.Center;
                precoRefLabel.FontAttributes = FontAttributes.Bold;
                precoRefLabel.FontSize = 18;

                var quantidadeLabel = new Label();
                quantidadeLabel.SetBinding(Label.TextProperty, "Quantidade", stringFormat: "Qtd: {0}");
                quantidadeLabel.TextColor = Colors.Black;
                quantidadeLabel.Margin = new Thickness(0, 0, 100, 0);
                quantidadeLabel.HorizontalOptions = LayoutOptions.Center;
                quantidadeLabel.FontSize = 16;

                var qualidadeLabel = new Label();
                qualidadeLabel.SetBinding(Label.TextProperty, "Qualidade");
                qualidadeLabel.TextColor = Colors.Black;
                qualidadeLabel.HorizontalOptions = LayoutOptions.Center;
                qualidadeLabel.FontSize = 16;

                var idiomaLabel = new Label();
                idiomaLabel.SetBinding(Label.TextProperty, "Idioma");
                idiomaLabel.Margin = new Thickness(60, 0, 0, 0);
                idiomaLabel.HorizontalOptions = LayoutOptions.Center;
                idiomaLabel.TextColor = Colors.Black;
                idiomaLabel.FontSize = 16;

                var stackItem = new Microsoft.Maui.Controls.StackLayout();
                stackItem.Children.Add(image);

                var venderButton = new Button
                {
                    Text = "Vender",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                var trocarButton = new Button
                {
                    Text = "Trocar",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10, 0, 0)
                };
                var removerButton = new Button
                {
                    Text = "Remover",
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10, 0, 0),
                    WidthRequest = 250
                };

                Border borda = new Border()
                {
                    StrokeShape = new RoundRectangle
                    {
                        CornerRadius = new CornerRadius(30, 30, 30, 30)
                    },
                };

                var gridAlinhar = new Grid();
                gridAlinhar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Primeira coluna
                gridAlinhar.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Segunda coluna 
                gridAlinhar.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Primeira linha 
                gridAlinhar.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });// Segunda linha
                gridAlinhar.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//Terceira Linha
                gridAlinhar.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });//Quarta Linha
                gridAlinhar.Padding = new Thickness(20); // Adicionar um espaçamento interno
                gridAlinhar.BackgroundColor = Colors.LightGrey;
                gridAlinhar.WidthRequest = 350;
                gridAlinhar.Margin = new Thickness(0, -50, 0, 50);


                gridAlinhar.Children.Add(nomeLabel);
                gridAlinhar.Children.Add(precoRefLabel);
                gridAlinhar.SetColumn(precoRefLabel, 1);
                gridAlinhar.Children.Add(quantidadeLabel);
                gridAlinhar.SetRow(quantidadeLabel, 1);
                gridAlinhar.Children.Add(qualidadeLabel);
                gridAlinhar.SetColumn(qualidadeLabel, 1);
                gridAlinhar.SetRow(qualidadeLabel, 1);
                gridAlinhar.Children.Add(idiomaLabel);
                gridAlinhar.SetRow(idiomaLabel, 1);
                gridAlinhar.Children.Add(venderButton);
                gridAlinhar.SetRow(venderButton, 2);
                gridAlinhar.Children.Add(trocarButton);
                gridAlinhar.SetRow(trocarButton, 2);
                gridAlinhar.SetColumn(trocarButton, 2);
                gridAlinhar.Children.Add(removerButton);
                gridAlinhar.SetRow(removerButton, 3);
                gridAlinhar.SetColumn(removerButton, 0);
                gridAlinhar.SetColumnSpan(removerButton, 2);

                stackItem.Children.Add(gridAlinhar);
                //botão de venda
                venderButton.Clicked += async (sender, e) =>
                {
                    var button = (Button)sender;

                    if (button.BindingContext is CartaUtiViewModel viewModel)
                    {
                        var idCartaClicada = viewModel.Id;
                        var vendaPage = new VendaPage(idCartaClicada);
                        
                        await Navigation.PushAsync(vendaPage);
                    }
                };
                //botão de troca
                trocarButton.Clicked += async (sender, e) =>
                {
                    var buttontroca = (Button)sender;
                    if (buttontroca.BindingContext is CartaUtiViewModel viewModel)
                    {
                        var idCartaClicada = viewModel.Id;
                        var trocaPage = new TrocaPage(idCartaClicada);
                        await Navigation.PushAsync(trocaPage);
                    }
                };
                //botão para remover carta da coleção
                removerButton.Clicked += async (sender, e) =>
                {
                    if (sender is Button removerButton && removerButton.BindingContext is CartaUtiViewModel carta)
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            var utilizador = UserSession.LoggedInUser.Id_User;

                            // Encontre a carta a ser removida
                            var cartaParaRemover = await dbContext.CartasUtilizador.FirstOrDefaultAsync(c => c.Id_Carta == carta.Id && c.Qualidade == carta.Qualidade && c.Id_Utilizador == utilizador && c.Idioma == carta.Idioma);

                            if (cartaParaRemover != null)
                            {
                                dbContext.CartasUtilizador.Remove(cartaParaRemover); // Remova a carta do banco de dados
                                await dbContext.SaveChangesAsync();

                                Cartas.Remove(carta); // Remova a carta da coleção na interface do usuário
                                await DisplayAlert("Alerta", "Carta removida com sucesso!", "OK");
                            }
                            else
                            {
                                await DisplayAlert("Alerta", "Não foi possível encontrar a carta para remover!", "OK");
                            }
                        }
                    }
                };



                return stackItem;
            });

            //caminho para a página Adicionar Cartas
            adicionar.Clicked += async (sender, e) =>
            {
                
                await Navigation.PushAsync(new AdicionarCartasPokemon());
            };
            cartasLayout.Children.Add(adicionar);

            cartasLayout.Children.Add(collectionView);

            ScrollView scrollView = new ScrollView
            {
                Margin = new Thickness(10, 20, 10, 0),
                Content = cartasLayout
            };

            Content = scrollView;
        }


        //view model para cartas a mostrar
        public class CartasViewModel : BindableObject
        {
            private int idColecaoUtilizador;

            public ObservableCollection<CartaUtiViewModel> Cartas { get; set; }

            public CartasViewModel(int idColecaoUtilizador)
            {
                this.idColecaoUtilizador = idColecaoUtilizador;
                Cartas = new ObservableCollection<CartaUtiViewModel>();
                LoadCartas();
            }

            //Função para carregar as cartas da base de dados
            private async void LoadCartas()
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var utilizador = await dbContext.ColecaoUtilizador
                        .Where(cu => cu.Id_ColecaoUtilizador == idColecaoUtilizador)
                        .Select(cu => cu.Id_Utilizador)
                        .FirstOrDefaultAsync();
                    

                    if (utilizador != null)
                    {
                        var cartasDoUtilizador =  await dbContext.CartasUtilizador
                          .Where(cu => cu.Id_Utilizador == utilizador && cu.Quantidade > 0)
                          .ToListAsync();


                        foreach (var cartaUtilizador in cartasDoUtilizador)
                        {
                          
                            var idcolecao = await dbContext.ColecaoUtilizador
                                .Where(cu => cu.Id_ColecaoUtilizador == idColecaoUtilizador)
                                .Select(cu => cu.Id_Colecao)
                                .FirstOrDefaultAsync();
                           
                            var colecaocarta = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == cartaUtilizador.Id_Card)
                                .Select(ca => ca.Id_Collection)
                                .FirstOrDefaultAsync();
                            //verifica se a carta pertence à coleção selecionada
                          if(colecaocarta == idcolecao) { 
                            var cartaID = cartaUtilizador.Id_Card;
                            var cartaIdUtil = cartaUtilizador.Id_Carta;
                            
                            var qualidade = cartaUtilizador.Qualidade;
                            var quantidade = cartaUtilizador.Quantidade;

                            
                            var nomeCarta = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == cartaID)
                                .Select(ca => ca.PersonagemCarta)
                                .FirstOrDefaultAsync();

                            var imagem = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == cartaID)
                                .Select(ca => ca.Imagem)
                                .FirstOrDefaultAsync();

                            var precoref = await dbContext.CartasAPI
                                .Where(ca => ca.Id_Carta == cartaID)
                                .Select(ca => ca.PrecoRef)
                                .FirstOrDefaultAsync();

                            var idioma = await dbContext.CartasUtilizador
                                .Where(ca => ca.Id_Carta == cartaIdUtil)
                                .Select(ca => ca.Idioma)
                                .FirstOrDefaultAsync();

                            var precoFormatado = precoref.ToString("0.00");

                            Cartas.Add(new CartaUtiViewModel
                            {
                                Id = cartaIdUtil,
                                NomeCarta = nomeCarta,
                                Imagem = imagem,
                                PrecoRef = precoFormatado,
                                Quantidade = quantidade,
                                Qualidade = qualidade,
                                Idioma = idioma,
                            });
                          }  
                        }
                    }
                }
            }
        }

    }

}