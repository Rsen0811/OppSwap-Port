using System.Collections.ObjectModel;

using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OppSwap.ViewModels
{

    public partial class NickNamePageViewModel : ObservableObject
    {
        [ObservableProperty]
        String nick;

         public NickNamePageViewModel()
        {


        }
        [RelayCommand]
        async Task SetNickName()
        {
            if (!string.IsNullOrWhiteSpace(nick))
            {
                ClientInterconnect.SetName(nick);
            }
            nick = "";
        }
            

    }
}

