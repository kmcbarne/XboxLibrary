using Microsoft.Toolkit.Uwp.UI.Controls;
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
    public sealed partial class AdvancedSearch : Page
    {
        private bool[] sectionsSet;

        public AdvancedSearch()
        {
            this.InitializeComponent();
        }

        private void Search()
        {
            IEnumerable<Game> results = DataLibrary.DataStore;

            /*
             * 0 - Title        4 - Completion
             * 1 - Console      5 - Achievements
             * 2 - GamePass     6 - Installation
             * 3 - Format       7 - DateTime
             * 
             */

            // Check if Installation checkboxes are checked
            if (sectionsSet[6])
            {
                if (installationYes.IsChecked == true)
                    results = results.Where(game => game.IsInstalled);
                else if (installationNo.IsChecked == true)
                    results = results.Where(game => !game.IsInstalled);
            }

            if (sectionsSet[4])
            {
                if (completionYes.IsChecked == true)
                    results = results.Where(game => game.IsGameCompleted);
                if (completionNo.IsChecked == true)
                    results = results.Where(game => !game.IsGameCompleted);
            }

            if (sectionsSet[7])
            {
                if (dateStart.SelectedDate != null && dateEnd.SelectedDate != null)
                    results = results.Where(game => ((dateStart.SelectedDate.Value.Date <= game.DateAdded) == true) && (game.DateAdded <= dateEnd.SelectedDate.Value.Date));
                else if (dateStart.SelectedDate != null && dateEnd.SelectedDate == null)
                    results = results.Where(game => (dateStart.SelectedDate.Value.Date <= game.DateAdded) == true);
                else if (dateEnd.SelectedDate != null && dateStart.SelectedDate == null)
                    results = results.Where(game => game.DateAdded <= dateEnd.SelectedDate.Value.Date);
            }

            if (sectionsSet[5])
            {
                if (achievementsYes.IsChecked == true)
                    results = results.Where(game => game.IsAchievementsComplete);

                if (achievementsNo.IsChecked == true)
                    results = results.Where(game => game.CurrentAchievements <= 0);
                
                if (achievementsScore.Value > 0)
                    results = results.Where(game => game.CurrentAchievements >= 0);
                if (achievementsScore.Value > 0 && achievementsScore.Value <= achievementsScore.Maximum)
                    results = results.Where(game => game.CurrentAchievements == achievementsScore.Value);
                if (achievementsScore.Value == achievementsScore.Maximum)
                    results = results.Where(game => game.IsAchievementsComplete == true || (game.CurrentAchievements / game.MaxAchievements == 1.0));


            }
        }

        private void Expander_LostFocus(object sender, RoutedEventArgs e)
        {
            Expander expander = (Expander)sender;
            sectionsSet = new bool[8];

            switch (expander.Header)
            {
                case "Title":
                case "Title Set":
                    if (titleFieldA.Text != "" || titleFieldB.Text != "" || titleFieldC.Text != "" || titleFieldD.Text != "")
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[0] = true;
                        }
                    }
                    else if (titleFieldA.Text == "" && titleFieldB.Text == "" && titleFieldC.Text == "" && titleFieldD.Text == "")
                    {
                        expander.Header = "Title";
                        sectionsSet[0] = true;
                    }
                    break;
                case "Console":
                case "Console Set":
                    if (consoleWin.IsChecked == true || consoleOri.IsChecked == true || console360.IsChecked == true || consoleOne.IsChecked == true || consoleSX.IsChecked == true)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[1] = true;
                        }
                    }
                    else if (consoleWin.IsChecked == false && consoleOri.IsChecked == false && console360.IsChecked == false && consoleOne.IsChecked == false && consoleSX.IsChecked == false)
                    {
                        expander.Header = "Console";
                        sectionsSet[1] = true;
                    }
                    break;
                case "GamePass":
                case "GamePass Set":
                    if (gamePassAvailable.IsChecked == true || gamePassNA.IsChecked == true || gamePassRemove.IsChecked == true || gamePassUnk.IsChecked == true)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[2] = true;
                        }
                    }
                    else if (gamePassAvailable.IsChecked == false && gamePassNA.IsChecked == false && gamePassRemove.IsChecked == false && gamePassUnk.IsChecked == false)
                    {
                        expander.Header = "GamePass";
                        sectionsSet[2] = true;
                    }
                    break;
                case "Format":
                case "Format Set":
                    if (formatDigital.IsChecked == true || formatPhysical.IsChecked == true)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[3] = true;
                        }
                    }
                    else if (formatDigital.IsChecked == false && formatPhysical.IsChecked == false)
                    {
                        expander.Header = "Format";
                        sectionsSet[3] = true;
                    }
                    break;
                case "Completion":
                case "Completion Set":
                    if (completionYes.IsChecked == true || completionNo.IsChecked == true)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[4] = true;
                        }
                    }
                    else if (completionYes.IsChecked == false && completionNo.IsChecked == false)
                    {
                        expander.Header = "Completion";
                        sectionsSet[4] = true;
                    }
                    break;
                case "Achievements":
                case "Achievements Set":
                    if (achievementsYes.IsChecked == true || achievementsNo.IsChecked == true || achievementsScore.Value != 0 || achievementsFieldD.Value != 0)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[5] = true;
                        }
                    }
                    else if (achievementsYes.IsChecked == false && achievementsNo.IsChecked == false && achievementsScore.Value == 0 && achievementsFieldD.Value == 0)
                    {
                        expander.Header = "Achievements";
                        sectionsSet[5] = true;
                    }
                    break;
                case "Installation":
                case "Installation Set":
                    if (installationYes.IsChecked == true || installationNo.IsChecked == true)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[6] = true;
                        }
                    }
                    else if (installationYes.IsChecked == false && installationNo.IsChecked == false)
                    {
                        expander.Header = "Installation";
                        sectionsSet[6] = true;
                    }
                    break;
                case "DateTime":
                case "DateTime Set":
                    if (dateStart.SelectedDate != null || dateEnd.SelectedDate != null)
                    {
                        if (expander.Header.ToString().Contains(" Set"))
                            break;
                        else
                        {
                            expander.Header += " Set";
                            sectionsSet[7] = true;
                        }
                    }
                    else if (dateStart.SelectedDate == null && dateEnd.SelectedDate == null)
                    {
                        expander.Header = "DateTime";
                        sectionsSet[7] = true;
                    }
                    break;
            }
        }

        private void advancedSearchButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
