﻿namespace OppSwap
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(JoinPage), typeof(JoinPage));
        }
    }
}