namespace SwapQuest
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            
            Routing.RegisterRoute("HomePage", typeof(HomePage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("colecoes", typeof(ColecoesPage1));
            Routing.RegisterRoute("ProcuraCartas", typeof(ProcuraPage));
            Routing.RegisterRoute("Definicoes", typeof(Definicoes));
            Routing.RegisterRoute("AdicionarCartasPokemon", typeof(AdicionarCartasPokemon));
            Routing.RegisterRoute("CartasPage", typeof(CartasPage));
            Routing.RegisterRoute("VendaPage", typeof(VendaPage));
            Routing.RegisterRoute("CartasVenda", typeof(CartasVenda));
            Routing.RegisterRoute("CartasTroca", typeof(CartasTroca));
            Routing.RegisterRoute("PropostasRecebidas", typeof(PropostasRecebidas));
            Routing.RegisterRoute("HistoricoVendasPopUp", typeof(HistoricoVendasPopUp)); 
            Routing.RegisterRoute("HistoricoTrocasPopUp", typeof(HistoricoTrocasPopUp));
        }
    }
}
