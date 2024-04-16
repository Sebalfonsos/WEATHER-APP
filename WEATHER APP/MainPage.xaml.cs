using System.Globalization;
using WEATHER_APP.Model;
using WEATHER_APP.Services;

namespace WEATHER_APP
{
    public partial class MainPage : ContentPage
    {
       
        private readonly IWeatherService _weatherService;
        public MainPage(IWeatherService weatherService)
        {
            InitializeComponent();
            _weatherService = weatherService;
            SearchBtn.Text = "\uf689";
            cleanAll();
            Facrylic2.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.Dark;
            Facrylic.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.ExtraDark;
            EText.PlaceholderColor = Color.FromHex("#B0BEC5");
            EText.TextColor = Color.FromHex("#FFFFFF");
            BV1.Color = Color.FromHex("#FFFFFF");
            BV2.Color = Color.FromHex("#FFFFFF");
            Ibackground.Source = "defaultplanet.png";
        }

        private async void OnSearchBtnClicked(object sender, EventArgs e)
        {
            cleanAll();
            Loader.IsVisible = true;
            EText.IsEnabled = false; // Por alguna razon desabilitar el campo de texto y volverlo a habilitar oculta el teclado IQ 1000
            EText.IsEnabled = true;
            string location = string.IsNullOrEmpty(EText.Text) ? "." : EText.Text;

            List<WeatherResponse.Rootobject> data = await _weatherService.Get(location);

            if (data != null && data.Count > 0)
            {
                // Accede a los datos solo si la lista no está vacía

                LName.Text = data[0].name;
                Ldesc.Text = ToTitleCase(data[0].weather[0].description);
                Lcoord.Text = "Lon: " + data[0].coord.lon + "° - Lat: " + data[0].coord.lat + "° ";
                var iconCode = data[0].weather[0].icon;

                Ibackground.Source = "a" + iconCode;

                float temperatureKelvin = data[0].main.temp;
                float temperatureCelsius = temperatureKelvin - 273.15f;  //Se convierte de kelvin a celsius
                float roundedTemperature = (float)Math.Round(temperatureCelsius, 3);
                Ltemp.Text = "" + roundedTemperature.ToString() + "°C";

                var baseUrl = "https://openweathermap.org/img/wn";
                var fullUrl = $"{baseUrl}/{iconCode}@2x.png";
                Icon.WidthRequest = 80;
                Icon.Source = fullUrl;

                if (iconCode[2].Equals('n')) {
                    Facrylic3.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.Light;
                    Facrylic2.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.Light;
                    Facrylic.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.Light;
                } else {
                    Facrylic3.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.ExtraDark;
                    Facrylic2.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.Dark;
                    Facrylic.EffectStyle = Xe.AcrylicView.Controls.EffectStyle.ExtraDark;
                }

                long sunriseTimestamp = data[0].sys.sunrise;
                long sunsetTimestamp = data[0].sys.sunset;

                DateTimeOffset sunriseTime = DateTimeOffset.FromUnixTimeSeconds(sunriseTimestamp);
                DateTimeOffset sunsetTime = DateTimeOffset.FromUnixTimeSeconds(sunsetTimestamp);

                DateTimeOffset localSunriseTime = sunriseTime.ToOffset(TimeSpan.FromSeconds(data[0].timezone));
                DateTimeOffset localSunsetTime = sunsetTime.ToOffset(TimeSpan.FromSeconds(data[0].timezone));

                Lsunrise.Text = "Sunrise: " + localSunriseTime.ToString("HH:mm:ss");
                Lsunset.Text = "Sunset: " + localSunsetTime.ToString("HH:mm:ss");

                float temperatureKelvin2 = data[0].main.feels_like;
                float temperatureCelsius2 = temperatureKelvin2 - 273.15f;  //Se convierte de kelvin a celsius
                float roundedTemperature2 = (float)Math.Round(temperatureCelsius2, 3);
                Ltermic.Text = "Termic Sensation: " + roundedTemperature2.ToString() + "°C";

                Lpressure.Text = "Pressure: " + data[0].main.pressure + " hPa";

                Lhumidity.Text = "Humidity: " + data[0].main.humidity + "%";

                Ltime.Text = "Time Zone: " + FormatTimezoneOffset(data[0].timezone) + " ";

                Lwindspeed.Text = "Wind Speed: " + data[0].wind.speed + " m/s";

                Lwinddegrees.Text = "Wind Degrees: " + data[0].wind.deg + "° ";

                Lvisibility.Text = "Visiblity: " + (data[0].visibility / 1000) + " km";


            }
            else
            {
                // Maneja el caso en el que la lista esté vacía (por ejemplo, ciudad no encontrada)
                LName.Text = "";
                Ldesc.Text = "The requested city was not found :(";
                Ltemp.Text = "";
                Icon.WidthRequest = 250;
                Icon.Source = "citynotfound.png";
                Ibackground.Source = "defaultplanet.png";

            }

            Loader.IsVisible = false;
            Facrylic.IsVisible = true;
            Facrylic3.IsVisible = true;


        }

        private string ToTitleCase(string input)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(input);
        }

        public string FormatTimezoneOffset(int offset)
        {
            int hours = offset / 3600;
            int minutes = Math.Abs((offset % 3600) / 60);
            string sign = (hours < 0) ? "-" : "+";
            hours = Math.Abs(hours);
            return $"UTC {sign} {hours:00}:{minutes:00}";
        }

        private void cleanAll()
        {
            Icon.WidthRequest = 120;
            Facrylic.IsVisible = false;
            Facrylic3.IsVisible = false;
            Lhumidity.Text = "";
            Lpressure.Text = "";
            Lsunrise.Text = "";
            Lsunset.Text = "";
            Ltermic.Text = "";
            Lcoord.Text = "";
            LName.Text = "";
            Ldesc.Text = "";
            Ltemp.Text = "";
            Ltime.Text = "";
            Lwinddegrees.Text = "";
            Lwindspeed.Text = "";
            Lvisibility.Text = "";
            Icon.Source = "";
        }
    }

}
