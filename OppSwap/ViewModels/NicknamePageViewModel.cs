using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OppSwap.ViewModels
{
	public partial class NicknamePageViewModel:ObservableObject
	{
		[ObservableProperty]
		string nickname="";
		
		public NicknamePageViewModel() { }

		[RelayCommand]
		public void changeName()
		{
			ClientInterconnect.SetName(Nickname);
			Nickname = "";
		}


	}
}

