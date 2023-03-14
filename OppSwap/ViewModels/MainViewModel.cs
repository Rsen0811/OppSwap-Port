

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KemperTestCodeMaui.ViewModels
{
    public partial class MainViewModel : ObservableObject 
    {
        public MainViewModel()
        {
            Items = new ObservableCollection<string>();
        }

        [ObservableProperty]
        ObservableCollection<string> items;

        [ObservableProperty]
        string text;

        [RelayCommand]
        void Add()
        {
            if(string.IsNullOrWhiteSpace(Text))
            {
                return;
            }
            Items.Add(Text);
            Text = string.Empty;
        }

        [RelayCommand]
        async Task NextPage(string s)
        {
            await Shell.Current.GoToAsync(nameof(HUD));
        }

        [RelayCommand]
        async Task Tap(string s)
        {
            await Shell.Current.GoToAsync(nameof(HUD));
        }

    }
}
