using System.Windows.Controls;

namespace meteov3.Service
{
   
       
           public class WeatherForecastDisplay
        {
            // Les TextBlocks utilisés pour afficher les prévisions
            private readonly TextBlock _tbPrevisionjour;
            private readonly TextBlock _tbPrevision1jour;
            private readonly TextBlock _tbPrevision2jour;
            private readonly TextBlock _tbPrevision3jour;

            // Constructeur qui initialise les TextBlocks nécessaires pour afficher les prévisions
            public WeatherForecastDisplay(TextBlock tbPrevisionjour, TextBlock tbPrevision1jour, TextBlock tbPrevision2jour, TextBlock tbPrevision3jour)
            {
                // On initialise les TextBlocks
                _tbPrevisionjour = tbPrevisionjour;
                _tbPrevision1jour = tbPrevision1jour;
                _tbPrevision2jour = tbPrevision2jour;
                _tbPrevision3jour = tbPrevision3jour;
            }

            // Méthode qui affiche les prévisions
            public void DisplayForecast(dynamic weatherData)
            {
                // On affiche les prévisions dans les TextBlocks
                _tbPrevisionjour.Text = weatherData.fcst_day_1.day_long;
                _tbPrevision1jour.Text = weatherData.fcst_day_2.day_long;
                _tbPrevision2jour.Text = weatherData.fcst_day_3.day_long;
                _tbPrevision3jour.Text = weatherData.fcst_day_4.day_long;
            }
        }
    }


