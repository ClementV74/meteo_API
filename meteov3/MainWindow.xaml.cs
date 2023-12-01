using meteov3.Service;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace meteov3
{

    

    public partial class MainWindow : Window
    {

        WeatherForecastDisplay prevision;
        // On crée un objet Api
        Apimeteo api;
        // On crée un objet WeatherForecastDisplay
        public readonly WeatherForecastDisplay _forecastDisplay;

        // On crée un objet Ville
        Ville ville;

        // On crée une variable qui contient l'URL de l'API
        public string apiUrl;

        public MainWindow()
        {
            InitializeComponent();
            ville = new Ville(); // On crée l'objet Ville
            ComboBox.ItemsSource = ville.LsVille; // On ajoute les villes dans la ComboBox
            _forecastDisplay = new WeatherForecastDisplay(TB_Previsionjour, TB_Prevision1jour, TB_Prevision2jour, TB_Prevision3jour); // On crée l'objet WeatherForecastDisplay
            DataContext = this; // On définit le DataContext de la fenêtre

            apiUrl = "https://www.prevision-meteo.ch/services/json/Annecy"; // On définit l'URL de l'API
            GetWeather(apiUrl); // On récupère les données météo
            ComboBox.SelectionChanged += ComboBox_SelectionChanged;
            api = new Apimeteo(); // On crée l'objet Api

        }

        // Méthode qui met à jour les données météo
        private void UpdateWeatherData(string selectedVille)
        {
            // On définit l'URL de l'API
            apiUrl = $"https://www.prevision-meteo.ch/services/json/{selectedVille}";
            // On récupère les données météo
            GetWeather(apiUrl);
        }

        public async void GetWeather(string apiUrl) // On récupère les données météo
        {
            HttpClient client = new HttpClient(); // On crée un objet HttpClient
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl); // On récupère les données de l'API
                if (response.IsSuccessStatusCode) // Si la requête a réussi
                {
                    var content = await response.Content.ReadAsStringAsync(); // On récupère le contenu de la réponse
                    dynamic weatherData = JsonConvert.DeserializeObject(content); // On désérialise le contenu de la réponse


                    Jour1.Source = await DownloadImage(weatherData.fcst_day_1.icon_big.ToString()); // On affiche l'icône du jour 1
                    Jour2.Source = await DownloadImage(weatherData.fcst_day_2.icon_big.ToString()); // On affiche l'icône du jour 2
                    Jour3.Source = await DownloadImage(weatherData.fcst_day_3.icon_big.ToString()); // On affiche l'icône du jour 3
                    Jour4.Source = await DownloadImage(weatherData.fcst_day_4.icon_big.ToString()); // On affiche l'icône du jour 4

                    TB_temperature.Text = "Température " + weatherData.current_condition.tmp.ToString() + "°C"; // On affiche la température
                    TB_precipitation.Text = weatherData.current_condition.condition.ToString(); // On affiche la condition
                    TB_max.Text = weatherData.fcst_day_0.tmax.ToString() + "°C"; // On affiche la température maximale
                    TB_min.Text = weatherData.fcst_day_0.tmin.ToString() + "°C"; // On affiche la température minimale
                    TB_Ville.Text = weatherData.city_info.name.ToString(); // On affiche le nom de la ville
                    TB_vent.Text = weatherData.current_condition.wnd_spd.ToString() + "km/h"; // On affiche la vitesse du vent

                    _forecastDisplay.DisplayForecast(weatherData); // On affiche les prévisions

                    ChangeBackgroundImage(weatherData.current_condition.condition_key.ToString()); // On change l'image de fond en fonction de la condition météo
                }
                else
                {
                    MessageBox.Show("Erreur lors de la récupération des données météo."); // On affiche un message d'erreur
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erreur : veuillez entrer une ville valide ");


                ComboBox.ItemsSource = null;
                ComboBox.ItemsSource = ville.LsVille;


                ville.RemoveVille(TB_recherche.Text);

                ville.RemoveVille(TB_recherche.Text);


            }
        }

        private async Task<ImageSource> DownloadImage(string imageUrl) // On télécharge l'image
        {
            try
            {
                WebClient client = new WebClient(); // On crée un objet WebClient
                byte[] imageData = await client.DownloadDataTaskAsync(new Uri(imageUrl)); // On télécharge l'image
                BitmapImage bitmapImage = new BitmapImage(); // On crée un objet BitmapImage

                using (MemoryStream stream = new MemoryStream(imageData)) // On crée un objet MemoryStream
                {
                    bitmapImage.BeginInit(); // On initialise l'objet BitmapImage
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // On définit l'option de cache
                    bitmapImage.StreamSource = stream; // On définit la source de l'image
                    bitmapImage.EndInit(); // On termine l'initialisation de l'objet BitmapImage
                }

                return bitmapImage; // On retourne l'image
            }
            catch (Exception ex) // Si une erreur est survenue
            {
                MessageBox.Show("Erreur lors du téléchargement de l'image : " + ex.Message); // On affiche un message d'erreur
                return null;
            }
        }

        private void ChangeBackgroundImage(string weatherCondition) // On change l'image de fond en fonction de la condition météo
        {
            try
            {
                string imagePath = "pack://application:,,,/meteov3;component/Ressources/Background/" + weatherCondition.ToLower() + ".png";
                // On définit le chemin de l'image en schéma d'URI 
                Uri uri = new Uri(imagePath, UriKind.Absolute); // On crée un objet Uri
                System.Windows.Media.ImageSource imgSource = new System.Windows.Media.Imaging.BitmapImage(uri); // On crée un objet ImageSource
                this.Background = new System.Windows.Media.ImageBrush(imgSource); // On définit l'image de fond
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Erreur lors du chargement de l'image : " + ex.Message);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Lorsque l'utilisateur sélectionne une ville dans la ComboBox
        {
            if (ComboBox.SelectedItem != null)
            {
                string selectedVille = ComboBox.SelectedItem.ToString(); // On récupère la ville sélectionnée
                UpdateWeatherData(selectedVille); // On met à jour les données météo
            }
        }

        private void BTN_ADD_Click(object sender, RoutedEventArgs e) // Lorsque l'utilisateur clique sur le bouton "Ajouter"
        {

            ville.AddVille(TB_recherche.Text);  // On ajoute la ville dans la liste des villes

            ComboBox.ItemsSource = null; // On vide la ComboBox
            ComboBox.ItemsSource = ville.LsVille; // On ajoute les villes dans la ComboBox

        }


        private void BTN_RM_Click(object sender, RoutedEventArgs e) // Lorsque l'utilisateur clique sur le bouton "Supprimer"
        {

            string selectedVille = ComboBox.SelectedItem.ToString(); // On récupère la ville sélectionnée

            if (!string.IsNullOrEmpty(selectedVille))
            {
                ville.RemoveVille(selectedVille); // On supprime la ville de la liste des villes

                ComboBox.ItemsSource = null; // On vide la ComboBox
                ComboBox.ItemsSource = ville.LsVille; // On ajoute les villes dans la ComboBox
            }

            ville.RemoveVille(TB_recherche.Text); // On supprime la ville de la liste des villes
        }

    }
}



