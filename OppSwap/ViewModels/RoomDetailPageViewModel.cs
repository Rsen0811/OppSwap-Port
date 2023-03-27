using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OppSwap.ViewModels
{
	public partial class RoomDetailPageViewModel: ObservableObject
	{
		[ObservableProperty]
		double arrowAngle = 90;

        public RoomDetailPageViewModel()
		{
		}
	}
}

