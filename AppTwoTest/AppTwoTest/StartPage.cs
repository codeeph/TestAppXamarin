using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppTwoTest
{
    class StartPage : ContentPage
    {
        public StartPage()
        {
            Label header = new Label() { Text = "Здарова собаки!" };
            this.Content = header;
            Button button = new Button() { Text = "Соси", BackgroundColor = Color.Red };
            this.Content = button;
        }
    }
}
