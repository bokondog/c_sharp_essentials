using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MovieBudget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DateTime currentDate = DateTime.Now;
        const int YEAR_OFFSET = 30;
        decimal totalBudget = 0;
        StringBuilder stringBuilder = new StringBuilder();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LblDatum.Content = currentDate.ToString("yyyy-MM-dd");
            InitializeStringBuilder();
            InitializeSlider();
        }

        private void InitializeStringBuilder()
        {
            stringBuilder.AppendLine("Moviebudget");
            stringBuilder.AppendLine("===========");
        }

        private void InitializeSlider()
        {
            SliderReleaseYear.Maximum = currentDate.Year;
            SliderReleaseYear.Minimum = currentDate.Year - YEAR_OFFSET;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if(TxtLogin.Text.Length > 2) { 
                if(TxtLogin.Text.ToLower() == "admin")
                {
                    ImgUser.Source = new BitmapImage(new Uri("/MovieBudget;component/img/admin.png", UriKind.Relative));
                } else
                {
                    ImgUser.Source = new BitmapImage(new Uri("/MovieBudget;component/img/user.png", UriKind.Relative));
                }
                Login();
            } 
            else 
            {
                Logout();
                MessageBox.Show("Username moet minstens 3 karakters bevatten");
            }
        }

        private void Login()
        {
            ImgUser.Visibility = Visibility.Visible;
            PanelMovie.Visibility = Visibility.Visible;
        }

        private void Logout()
        {
            ImgUser.Visibility = Visibility.Hidden;
            PanelMovie.Visibility = Visibility.Hidden;
        }

        private void TxtMovieName_KeyUp(object sender, KeyEventArgs e)
        {
            if (TxtMovieName.Text.Length > 0)
            {
                SliderReleaseYear.IsEnabled = true;
            }
            else
            {
                SliderReleaseYear.IsEnabled = false;
            }
        }

        private void SliderReleaseYear_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LblReleaseYear.Content = Math.Round(SliderReleaseYear.Value);
        }

        private void BtnAddMovie_Click(object sender, RoutedEventArgs e)
        {
            TxtResultaat.Visibility = Visibility.Visible;
            AddNewBudget();
            ShowBudget();
        }

        private decimal BerekenBudget(int releaseJaar)
        {
            decimal initialBudget = 1000m;
            decimal multiplier = 1.01m;
            int movieAge = currentDate.Year - releaseJaar;
            if(movieAge > 0)
            {
                return initialBudget * (multiplier * (currentDate.Year - releaseJaar));
            } else
            {
                return initialBudget;
            }
        }

        private void AddNewBudget()
        {
            decimal budget = BerekenBudget((int)SliderReleaseYear.Value);
            stringBuilder.AppendLine($"Moviename: {TxtMovieName.Text}");
            stringBuilder.AppendLine($" Release year: {Math.Round(SliderReleaseYear.Value)}");
            stringBuilder.AppendLine($"  Budget: {BerekenBudget((int)SliderReleaseYear.Value):c}");
            totalBudget += budget;
        }
        private void ShowBudget()
        {
            TxtResultaat.Text = stringBuilder.ToString();
            TxtResultaat.Visibility = Visibility.Visible;
            LblBudget.Content = totalBudget.ToString("c");
            BtnWissen.Visibility = Visibility.Visible;
        }

        private void BtnWissen_Click(object sender, RoutedEventArgs e)
        {
            TxtResultaat.Visibility= Visibility.Hidden;
            TxtResultaat.Text = "";
            InitializeStringBuilder();

            BtnWissen.Visibility = Visibility.Hidden;
            ImgUser.Visibility = Visibility.Hidden;
            TxtMovieName.Text = "";
            InitializeSlider();
            totalBudget = 0;
            LblBudget.Content = "";
            TxtLogin.Text = "";
            Logout();
        }
    }
}
