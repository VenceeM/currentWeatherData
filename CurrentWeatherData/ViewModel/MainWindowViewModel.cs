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

namespace CurrentWeatherData.ViewModel
{
    public class MainWindowViewModel:BaseViewModel
    {

        public ICommand cmd { get; set; }
        public MainWindowViewModel()
        {
            cmd = new RelayCommand(Da);
            Title = "Weather";

           
           
        }

        private async void Da()
        {
            await Data();
        }

        public async Task Data()
        {
            
            try
            {
                using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync("data/2.5/weather?q=Villasis,ph&appid=517a87487b1257ffa55fc471a130585d"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonConverter = await response.Content.ReadAsStringAsync();

                        Root root = JsonConvert.DeserializeObject<Root>(jsonConverter);

                        double temp = Convert.ToInt32(root.main.temp - 273.15);
                        MessageBox.Show(temp.ToString());


                    }
                }
                    
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            




        }




    }
}
