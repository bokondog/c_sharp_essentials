using System;
using System.Collections.Generic;
using System.Data;
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

namespace Vault
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        const int AMOUNT_OF_TRIES = 3;
        const int COMBINATION_LENGTH = 6;

        Random random = new Random();
        int remainingAttempts = AMOUNT_OF_TRIES;

        int[] combination;
        int[] userInput;
        string userInputDisplay;

        int inputIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            Reset();
        }

        private void Reset()
        {
            inputIndex = 0;
            remainingAttempts = AMOUNT_OF_TRIES;

            combination = new int[COMBINATION_LENGTH];
            
            ResetUserInput();

            GenerateCombination();
        }

        private void GenerateCombination()
        {
            string combinationLabel = "";

            for (int i = 0; i < COMBINATION_LENGTH; i++)
            {
                int randomNumber = random.Next(0, 9);
                combination[i] = randomNumber;

                combinationLabel += randomNumber.ToString();
            }

            LblCombination.Content = combinationLabel;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int numberClicked;
            bool isValid = int.TryParse(btn.Content.ToString(), out numberClicked);

            if (!isValid)
            {
                MessageBox.Show("Error!");
            }
            else
            {
                AddUserInput(inputIndex, numberClicked);

                if (inputIndex == COMBINATION_LENGTH - 1)
                {
                    Evaluate();
                }
                else
                {
                    inputIndex++;
                }
            }
        }

        private void DrawUserInput()
        {
            LblInput.Content = userInputDisplay;
        }

        private void AddUserInput(int index, int number)
        {
            userInputDisplay += number.ToString();
            userInput[index] = number;
            DrawUserInput();
        }

        private void ResetUserInput()
        {
            inputIndex = 0;
            userInput = new int[COMBINATION_LENGTH];
            userInputDisplay = "";
            DrawUserInput();
        }

        private void Evaluate()
        {
            int correct = 0;
            for(int i = 0; i < COMBINATION_LENGTH; i++)
            {
                if (userInput[i] == combination[i])
                {
                    correct++;
                } 
            }

            if(correct == COMBINATION_LENGTH)
            {
                int attemptsNeeded = AMOUNT_OF_TRIES - remainingAttempts + 1;
                MessageBox.Show($"Vault open! It took you {attemptsNeeded} attempt(s). Resetting...");
                Reset();
            } 
            else
            {
                remainingAttempts--;
                MessageBox.Show($"Wrong, try again! {remainingAttempts} attempts remaining, you had {correct} correct numbers.");
                ResetUserInput();
            }

            if(remainingAttempts <= 0)
            {
                MessageBox.Show("Maximum attempts reached! Resetting...");
                Reset();
            }
        }

        private void HackTheSafe()
        {

        }

        private void CheatMode()
        {

        }
    }
}
