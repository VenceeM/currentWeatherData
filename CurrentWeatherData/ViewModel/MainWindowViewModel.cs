using CurrentWeatherData.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CurrentWeatherData.Model;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Windows.Media.Imaging;

namespace CurrentWeatherData.ViewModel
{
    public class MainWindowViewModel:BaseViewModel
    {

        
        public MainWindowViewModel()
        {
            
            Title = "Weather";

            _Data();
           
           
        }
        private string location;

        public string Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        private string temp;

        public string Temp
        {
            get => temp;
            set => SetProperty(ref temp, value);
        }
        private string feelsLike;

        public string FeelsLike
        {
            get => feelsLike;
            set => SetProperty(ref feelsLike, value);
        }

        private BitmapImage icon;

        public BitmapImage Icon
        {
            get => icon;
            set => SetProperty(ref icon, value);
        }



        private async void _Data()
        {
            await Data();
        }

        public async Task Data()
        {
            
            try
            {
                //Insert your apikey from https://openweathermap.org/
                string apiKey = "";
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync($"data/2.5/weather?q=Villasis,ph&appid={apiKey}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonConverter = await response.Content.ReadAsStringAsync();

                        Root root = JsonConvert.DeserializeObject<Root>(jsonConverter);

                        double temp = Convert.ToInt32(root.main.temp - 273.15);
                        double feel = Convert.ToInt32(root.main.feels_like - 273.15);
                        //MessageBox.Show(root.name + "," + root.sys.country);
                        var des = string.Empty;
                        foreach (var i in root.weather)
                        {
                            //MessageBox.Show(i.main);
                            des = i.description;
                            _Icon(i.icon);
                        }
                        Temp = Convert.ToString(string.Format("{0}°C", temp));

                        FeelsLike = Convert.ToString(string.Format("Feels Like {0}°C {1}", feel, des));
                        Location = root.name + "," + root.sys.country;



                    }
                }
                    
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }

        private void _Icon(string icon)
        {
            var fullFilePath = $@"https://openweathermap.org/img/wn/{icon}@2x.png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            Icon = bitmap;
        }




    }
}
