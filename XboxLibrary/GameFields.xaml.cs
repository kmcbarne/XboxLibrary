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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XboxLibrary
{
    public sealed partial class GameFields : UserControl
    {
        public string TitleField
        {
            get { return (string)GetValue(TitleFieldProperty); }
            set { SetValue(TitleFieldProperty, value); }
        }

        public int ConsoleSelect
        {
            get { return (int)GetValue(ConsoleSelectProperty); }
            set { SetValue(ConsoleSelectProperty, value); }
        }

        public int GamePassSelect
        {
            get { return (int)GetValue(GamePassSelectProperty); }
            set { SetValue(GamePassSelectProperty, value); }
        }

        public int FormatSelect
        {
            get { return (int)GetValue(FormatSelectProperty); }
            set { SetValue(FormatSelectProperty, value); }
        }

        public bool CompletedFlag
        {
            get { return (bool)GetValue(CompletedFlagProperty); }
            set { SetValue(CompletedFlagProperty, value); }
        }

        public bool AchievementsFlag
        {
            get { return (bool)GetValue(AchievementsFlagProperty); }
            set { SetValue(AchievementsFlagProperty, value); }
        }

        public bool InstalledFlag
        {
            get { return (bool)GetValue(InstalledFlagProperty); }
            set { SetValue(InstalledFlagProperty, value); }
        }

        public string CurrentScoreField
        {
            get { return (string)GetValue(CurrentScoreFieldProperty); }
            set { SetValue(CurrentScoreFieldProperty, value); }
        }

        public string MaxScoreField
        {
            get { return (string)GetValue(MaxScoreFieldProperty); }
            set { SetValue(MaxScoreFieldProperty, value); }
        }

        public DateTime DateAddedField
        {
            get { return (DateTime)GetValue(DateAddedFieldProperty); }
            set { SetValue(DateAddedFieldProperty, value); }
        }

        public static readonly DependencyProperty TitleFieldProperty =
            DependencyProperty.Register("TitleField", typeof(string), typeof(GameFields), new PropertyMetadata(null));

        public static readonly DependencyProperty ConsoleSelectProperty =
            DependencyProperty.Register("ConsoleSelect", typeof(int), typeof(GameFields), new PropertyMetadata(-1));

        public static readonly DependencyProperty GamePassSelectProperty =
            DependencyProperty.Register("GamePassSelect", typeof(int), typeof(GameFields), new PropertyMetadata(-1));

        public static readonly DependencyProperty FormatSelectProperty =
            DependencyProperty.Register("FormatSelect", typeof(int), typeof(GameFields), new PropertyMetadata(-1));

        public static readonly DependencyProperty CompletedFlagProperty =
            DependencyProperty.Register("CompletedFlag", typeof(bool), typeof(GameFields), new PropertyMetadata(false));

        public static readonly DependencyProperty AchievementsFlagProperty =
            DependencyProperty.Register("AchievementsFlag", typeof(bool), typeof(GameFields), new PropertyMetadata(false));

        public static readonly DependencyProperty InstalledFlagProperty =
            DependencyProperty.Register("InstalledFlag", typeof(bool), typeof(GameFields), new PropertyMetadata(false));

        public static readonly DependencyProperty CurrentScoreFieldProperty =
            DependencyProperty.Register("CurrentScoreField", typeof(string), typeof(GameFields), new PropertyMetadata("0"));
        
        public static readonly DependencyProperty MaxScoreFieldProperty =
            DependencyProperty.Register("MaxScoreField", typeof(string), typeof(GameFields), new PropertyMetadata("1000"));

        public static readonly DependencyProperty DateAddedFieldProperty =
            DependencyProperty.Register("DateAddedField", typeof(DateTime), typeof(GameFields), new PropertyMetadata(DateTime.Today));



        public DatePicker DateSelector
        {
            get { return (DatePicker)GetValue(DateSelectProperty); }
            set { SetValue(DateSelectProperty, value); }
        }

        public static readonly DependencyProperty DateSelectProperty =
            DependencyProperty.Register("DateSelect", typeof(DatePicker), typeof(GameFields), new PropertyMetadata(null));



        /// <summary>
        /// Initializes an instance of the GameFields user control with default values.
        /// </summary>
        public GameFields()
        {
            DateSelector = dateAddedSelect;

            this.InitializeComponent();
        }

        /// <summary>
        /// Initializes an instance of the GameFields user control with specified values.
        /// </summary>
        /// <param name="gameTitle"></param>
        /// <param name="consoleIndex"></param>
        /// <param name="gamePassIndex"></param>
        /// <param name="formatIndex"></param>
        /// <param name="completionState"></param>
        /// <param name="achievementState"></param>
        /// <param name="installationState"></param>
        /// <param name="currentScoreValue"></param>
        /// <param name="maxScoreValue"></param>
        /// <param name="addedDate"></param>
        public GameFields(string gameTitle, int consoleIndex, int gamePassIndex, int formatIndex, bool completionState, bool achievementState, bool installationState, string currentScoreValue, string maxScoreValue, DateTime addedDate)
        {
            TitleField = gameTitle;
            ConsoleSelect = consoleIndex;
            GamePassSelect = gamePassIndex;
            FormatSelect = formatIndex;
            CompletedFlag = completionState;
            AchievementsFlag = achievementState;
            InstalledFlag = installationState;
            CurrentScoreField = currentScoreValue;
            MaxScoreField = maxScoreValue;
            DateAddedField = addedDate;

            this.InitializeComponent();
        }

        private void clearNewGameEntry_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            //installedToggle.IsChecked = !installedToggle.IsChecked;

            //AddToLibrary();
            //ClearForm();
        }

        private void deleteGame_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
