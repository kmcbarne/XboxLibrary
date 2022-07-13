using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XboxLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddNewGame : Page
    {
        private DateTime defaultDate;
        public DateTime DefaultDate
        {
            get
            {
                defaultDate = DateTime.Today;
                return defaultDate;
            }
        }

        public AddNewGame()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        public void ClearForm()
        {
            titleField.Text = "";
            consoleSelect.SelectedIndex = -1;
            gamePassSelect.SelectedIndex = -1;
            formatSelect.SelectedIndex = -1;
            //digitalSelect.IsChecked = false;
            //physicalSelect.IsChecked = false;
            //gamePassAvailableSelect.IsChecked = false;
            //gamePassNotApplicableSelect.IsChecked = false;
            //gamePassRemovedSelect.IsChecked = false;
            
            completedToggle.IsSelected = false;
            achievementsToggle.IsSelected = false;
            installedToggle.IsSelected = false;

            currentScoreField.Text = "0";
            maxScoreField.Text = "1000";

            dateAddedSelect.SelectedDate = defaultDate;
            //VisualStateManager.GoToState(completedToggle, "Unchecked", false);
            //VisualStateManager.GoToState(achievementsToggle, "Unchecked", false);
            //VisualStateManager.GoToState(installedToggle, "Unchecked", false);
        }

        public void AddToLibrary()
        {
            Game added = new Game();
            added.Title = titleField.Text;
            
            switch (consoleSelect.SelectedIndex)
            {
                case 0:
                    added.ConsoleGeneration = XboxSystem.Windows;
                    break;
                case 1:
                    added.ConsoleGeneration = XboxSystem.Xbox;
                    break;
                case 2:
                    added.ConsoleGeneration = XboxSystem.Xbox360;
                    break;
                case 3:
                    added.ConsoleGeneration = XboxSystem.XboxOne;
                    break;
                case 4:
                    added.ConsoleGeneration = XboxSystem.XboxSeriesSX;
                    break;
                default:
                    break;
            }

            switch (gamePassSelect.SelectedIndex)
            {
                case 0:
                    added.GamePassStatus = GamePassStatus.Available;
                    break;
                case 1:
                    added.GamePassStatus = GamePassStatus.NotApplicable;
                    break;
                case 2:
                    added.GamePassStatus = GamePassStatus.Removed;
                    break;
                case 3:
                    added.GamePassStatus = GamePassStatus.Unknown;
                    break;
            }

            switch (formatSelect.SelectedIndex)
            {
                case 0:
                    added.GameFormat = Format.Physical;
                    break;
                case 1:
                    added.GameFormat = Format.Digital;
                    break;
                case 2:
                    added.GameFormat = Format.Unknown;
                    break;
            }

            //if (digitalSelect.IsChecked == true)
            //    added.GameFormat = Format.Digital;
            //else
            //    added.GameFormat = Format.Physical;

            //if (gamePassAvailableSelect.IsChecked == true)
            //    added.GamePassStatus = GamePassStatus.Available;
            //else if (gamePassRemovedSelect.IsChecked == true)
            //    added.GamePassStatus = GamePassStatus.Removed;
            //else if (gamePassNotApplicableSelect.IsChecked == true)
            //    added.GamePassStatus = GamePassStatus.NotApplicable;

            if (completedToggle.IsSelected == true)
                added.IsGameCompleted = true;
            else
                added.IsGameCompleted = false;

            if (achievementsToggle.IsSelected)
                added.IsAchievementsComplete = true;
            else
                added.IsAchievementsComplete = false;

            if (installedToggle.IsSelected)
                added.IsInstalled = true;
            else
                added.IsInstalled = false;

            if (!string.IsNullOrWhiteSpace(currentScoreField.Text))
                added.CurrentAchievements = int.Parse(currentScoreField.Text);

            if (!string.IsNullOrWhiteSpace(maxScoreField.Text))
                added.MaxAchievements = int.Parse(maxScoreField.Text);

            added.DateAdded = dateAddedSelect.Date.Date;

            if (!string.IsNullOrWhiteSpace(added.Title))
            {
                DataLibrary.Add(added);
            }
        }

        private void clearNewGameEntry_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void addNewGame_Click(object sender, RoutedEventArgs e)
        {
            //installedToggle.IsChecked = !installedToggle.IsChecked;

            AddToLibrary();
            ClearForm();
        }

        private void selectAllField_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = (TextBox)sender;

            box.SelectAll();
        }
    }
}
