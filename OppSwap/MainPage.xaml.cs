using OppSwap.ViewModels;

namespace OppSwap;

public partial class MainPage : ContentPage
{
    //Client c = new Client();
    int count = 0;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public MainPage()
    {

    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
       // c.Ping();
        count++;

        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}