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
using System.Windows.Threading;

namespace sportclub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const int INITIAL_GRAPH_WIDTH = 50;
        const int GRAPH_SHRINK_FACTOR = 10;

        private DispatcherTimer clock = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeClock()
        {
            clock.Tick += new EventHandler(ClockTickEvent);
            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Start();
        }

        private void ClockTickEvent(object sender, EventArgs e)
        {
            LabelTime.Content = DateTime.Now.ToString();
        }

        private List<string> namen = new List<string>() { "Sander", "Quirino", "Thomas",
"Cédric", "Jason", "Domenico", "Rickert", "Klaas", "Tom", "Stephan", "Alexander",
"Yannick", "Robin", "Dave", "Lynn", "Arno", "Niels", "Maxiem", "Matthijs", "Kobe",
"Michaël", "Bram", "Achraf", "Raf", "Sven", "Gerben", "Kevin", "Cem", "Steff", "Steven",
"Rani", "Djordy", "Nick", "Mikail", "Konstantin", "Asad", "Viktor", "Antonio", "Senne",
"Benjamin", "Stef", "Abdul", "Christiaan", "Abdurrahman", "Jurgen", "Kevin", "Silvio",
"Nathan", "Stijn", "Bart", "Frank", "Steven", "Matty", "Arend", "Simon", "Ziggy",
"Pascal", "Michaël", "Danny", "Robby", "Johan", "Vincent", "Wim", "Tuba", "Kristof",
"Kenneth" };

        private string[,] lidgeldPerCat = { { "Preminiem", "150" }, { "Miniem", "150" }, {
"Junior", "170" }, { "Kadet", "170" },{ "Senior", "200" } };

        private TextBox[] prognoseVakken = new TextBox[6];

        private int[] CalculateTimes(int time)
        {
            int[] times = new int[6];
            times[0] = (int)Math.Truncate(time - (0 * 0.05 - (0.005 * 0)));
            times[1] = (int)Math.Truncate(times[0] - (times[0] * 0.05));
            times[2] = (int)Math.Truncate(times[1] - (times[1] * 0.045));
            times[3] = (int)Math.Truncate(times[2] - (times[2] * 0.04));
            times[4] = (int)Math.Truncate(times[3] - (times[3] * 0.035));
            times[5] = (int)Math.Truncate(times[4] - (times[4] * 0.03));
            return times;
        }

        private int[] CalcTimesLoop(int time)
        {
            int[] times = new int[6];
            for (int i = 0; i < times.Length; i++)
            {
                int previousTime = i == 0 ? time : times[i - 1];
                times[i] = CalculateImprovedTime(previousTime, i);
            }
            return times;
        }

        private int CalculateImprovedTime(int previousTime, int year)
        {
            double improvementFactor = 0.05 - (0.005 * year);
            double improvement = previousTime * improvementFactor;
            int improvedTime = (int)Math.Truncate(previousTime - improvement);
            return improvedTime;
        }

        private int CalculateMembershipCost(bool isCompetitor, int familyMember)
        {
            int competitorCost = isCompetitor ? 50 : 0;

            int baseCost = 0;
            if (RadioPreminiem.IsChecked == true || RadioMiniem.IsChecked == true)
            {
                baseCost = 150;
            }
            else if (RadioJunior.IsChecked == true || RadioCadet.IsChecked == true)
            {

                baseCost = 170;
            }
            else if (RadioSenior.IsChecked == true)
            {
                baseCost = 200;
            }
            else
            {
                MessageBox.Show("Please select a category");
            }

            int subTotal = baseCost + competitorCost;
            double familyDiscount = subTotal * (0.05 * familyMember - 1);
            int totalCost = (int)Math.Round(subTotal - familyDiscount);
            return totalCost;
        }

        private void InitializeNames()
        {
            ComboBoxNames.ItemsSource = namen;
        }

        private void InitializeGraph(int inputTime = 0)
        {
            int[] times = CalcTimesLoop(inputTime);
            StackPanelGraphList.Children.Clear();
            for (int i = 0; i < prognoseVakken.Length; i++)
            {
                StackPanel panel = new StackPanel();

                Label label = new Label();
                label.Content = i > 0 ? $"Na {i} jaar" : "Nu";

                panel.Children.Add(label);

                TextBox box = new TextBox();
                box.Text = times[i].ToString();
                box.Width = inputTime == 0 ? INITIAL_GRAPH_WIDTH : times[i] / GRAPH_SHRINK_FACTOR;
                box.HorizontalAlignment = HorizontalAlignment.Left;
                box.Background = new SolidColorBrush(Colors.LightGreen);

                panel.Children.Add(box);

                StackPanelGraphList.Children.Add(panel);
            }
        }

        private void Reset()
        {
            InitializeNames();
            InitializeGraph();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeClock();
            Reset();
        }
        private void ButtonCalculate_Click(object sender, RoutedEventArgs e)
        {
            int inputTime;
            bool isInputTimeValid = int.TryParse(TextBoxTime.Text, out inputTime);
            if (!isInputTimeValid)
            {
                MessageBox.Show("Please input a valid time (in seconds)");
                return;
            }
            else
            {
                InitializeGraph(inputTime);
            }

            UserFeedback.Text = CalculateMembershipCost((bool)IsCompetitor.IsChecked, int.Parse(TextFamilyMember.Text)).ToString();
        }

        private void MenuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuErase_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonErase_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
    }
}
