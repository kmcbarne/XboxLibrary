using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Input.Preview.Injection;
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
    public sealed partial class EditSingleItem : Page
    {
        /// <summary>
        /// Acts as a temporary Game object to store edits to until written to json file.
        /// </summary>
        IEnumerable<Game> searchResults { get; set; }

        /// <summary>
        /// Stores the index of the currently displayed Game.
        /// </summary>
        int currentIndex { get; set; }

        private DateTime defaultDate;
        public DateTime DefaultDate
        {
            get
            {
                defaultDate = DateTime.Today;
                return defaultDate;
            }
        }

        public EditSingleItem()
        {
            DataContext = this;
            this.InitializeComponent();
        }

        /// <summary>
        /// Clears the EditSingleItem form and resets the DateAdded field to the current date.
        /// </summary>
        public void ClearForm()
        {
            titleField.Text = "";
            consoleSelect.SelectedIndex = -1;
            gamePassSelect.SelectedIndex = -1;
            formatSelect.SelectedIndex = -1;

            completedToggle.IsSelected = false;
            achievementsToggle.IsSelected = false;
            installedToggle.IsSelected = false;

            currentScoreField.Text = "0";
            maxScoreField.Text = "1000";

            dateAddedSelect.SelectedDate = defaultDate;
            gameSearch.Text = "";
        }

        /// <summary>
        /// Adds a new Game object to the primary ObservableCollection<Game> collection.
        /// </summary>
        public void AddToLibrary()
        {
            Game editing = searchResults.FirstOrDefault();
            editing.Title = titleField.Text;

            switch (consoleSelect.SelectedIndex)
            {
                case 0:
                    editing.ConsoleGeneration = XboxSystem.Windows;
                    break;
                case 1:
                    editing.ConsoleGeneration = XboxSystem.Xbox;
                    break;
                case 2:
                    editing.ConsoleGeneration = XboxSystem.Xbox360;
                    break;
                case 3:
                    editing.ConsoleGeneration = XboxSystem.XboxOne;
                    break;
                case 4:
                    editing.ConsoleGeneration = XboxSystem.XboxSeriesSX;
                    break;
                default:
                    break;
            }

            switch (gamePassSelect.SelectedIndex)
            {
                case 0:
                    editing.GamePassStatus = GamePassStatus.Available;
                    break;
                case 1:
                    editing.GamePassStatus = GamePassStatus.NotApplicable;
                    break;
                case 2:
                    editing.GamePassStatus = GamePassStatus.Removed;
                    break;
                case 3:
                    editing.GamePassStatus = GamePassStatus.Unknown;
                    break;
            }

            switch (formatSelect.SelectedIndex)
            {
                case 0:
                    editing.GameFormat = Format.Physical;
                    break;
                case 1:
                    editing.GameFormat = Format.Digital;
                    break;
                case 2:
                    editing.GameFormat = Format.Unknown;
                    break;
            }

            if (completedToggle.IsSelected == true)
                editing.IsGameCompleted = true;
            else
                editing.IsGameCompleted = false;

            if (achievementsToggle.IsSelected)
                editing.IsAchievementsComplete = true;
            else
                editing.IsAchievementsComplete = false;

            if (installedToggle.IsSelected)
                editing.IsInstalled = true;
            else
                editing.IsInstalled = false;

            if (!string.IsNullOrWhiteSpace(currentScoreField.Text))
                editing.CurrentAchievements = int.Parse(currentScoreField.Text);

            if (!string.IsNullOrWhiteSpace(maxScoreField.Text))
                editing.MaxAchievements = int.Parse(maxScoreField.Text);

            editing.DateAdded = dateAddedSelect.Date.Date;

            // Need to find approach to update currently saved json entry with changes

            //if (!string.IsNullOrWhiteSpace(editing.Title))
            //{
            //    MainPage main = MainPage.Current;
            //    main.AddNewGame(editing);
            //}
        }

        /// <summary>
        /// Checks each field of the currently displayed Game to determine if there are changes from the saved data.
        /// </summary>
        /// <returns>Boolean array</returns>
        private bool[] CheckChangedFields()
        {
            bool[] fieldsChanged = new bool[10];

            if (searchResults.FirstOrDefault().Title != titleField.Text)
                fieldsChanged[0] = true;
            if ((int)searchResults.FirstOrDefault().ConsoleGeneration != consoleSelect.SelectedIndex)
                fieldsChanged[1] = true;
            if ((int)searchResults.FirstOrDefault().GamePassStatus != gamePassSelect.SelectedIndex)
                fieldsChanged[2] = true;
            if ((int)searchResults.FirstOrDefault().GameFormat != formatSelect.SelectedIndex)
                fieldsChanged[3] = true;
            if (searchResults.FirstOrDefault().IsGameCompleted != completedToggle.IsSelected)
                fieldsChanged[4] = true;
            if (searchResults.FirstOrDefault().IsAchievementsComplete != achievementsToggle.IsSelected)
                fieldsChanged[5] = true;
            if (searchResults.FirstOrDefault().IsInstalled != installedToggle.IsSelected)
                fieldsChanged[6] = true;
            if (searchResults.FirstOrDefault().CurrentAchievements.ToString() != currentScoreField.Text)
                fieldsChanged[7] = true;
            if (searchResults.FirstOrDefault().MaxAchievements.ToString() != maxScoreField.Text)
                fieldsChanged[8] = true;
            if (searchResults.FirstOrDefault().DateAdded != dateAddedSelect.Date)
                fieldsChanged[9] = true;

            return fieldsChanged;
        }

        /// <summary>
        /// Saves any edited fields to the json data file.
        /// </summary>
        /// <param name="fieldsChanged">A Boolean array denoting the fields showing changes.</param>
        private void CommitChanges(bool[] fieldsChanged)
        {
            if (fieldsChanged[0])
                searchResults.FirstOrDefault().Title = titleField.Text;
            if (fieldsChanged[1])
                searchResults.FirstOrDefault().ConsoleGeneration = (XboxSystem)consoleSelect.SelectedIndex;
            if (fieldsChanged[2])
                searchResults.FirstOrDefault().GamePassStatus = (GamePassStatus)gamePassSelect.SelectedIndex;
            if (fieldsChanged[3])
                searchResults.FirstOrDefault().GameFormat = (Format)formatSelect.SelectedIndex;
            if (fieldsChanged[4])
                searchResults.FirstOrDefault().IsGameCompleted = completedToggle.IsSelected;
            if (fieldsChanged[5])
                searchResults.FirstOrDefault().IsAchievementsComplete = achievementsToggle.IsSelected;
            if (fieldsChanged[6])
                searchResults.FirstOrDefault().IsInstalled = installedToggle.IsSelected;
            if (fieldsChanged[7])
                searchResults.FirstOrDefault().CurrentAchievements = Int32.Parse(currentScoreField.Text);
            if (fieldsChanged[8])
                searchResults.FirstOrDefault().MaxAchievements = Int32.Parse(maxScoreField.Text);
            if (fieldsChanged[9])
                searchResults.FirstOrDefault().DateAdded = dateAddedSelect.Date.Date;

            JsonUtilities.Write();
        }


        private void clearNewGameEntry_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void saveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Need to fix crash resulting from attemptng to edit the title of an existing game and saving.  When this happens,
            // the <i>unfinishedGames</i> collection below has no games in it, as the new title cannot be found in the original library.
            // Possible solution:  Save off the original title of an existing game when that title starts being edited.
            ObservableCollection<Game> unfinishedGames = new ObservableCollection<Game>(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Equals(titleField.Text.ToLowerInvariant())));

            int targetIndex = DataLibrary.DataStore.IndexOf(unfinishedGames[0]);

            //var changedGame = DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Equals(titleField.Text.ToLowerInvariant()));

            CommitChanges(CheckChangedFields());

            //installedToggle.IsChecked = !installedToggle.IsChecked;

            //AddToLibrary();
            //ClearForm();
        }

        /// <summary>
        /// Gets the previous ten entries after the currently displayed Game for filling the PreviousEntries ListBox.
        /// </summary>
        //private void GetPreviousEntries()
        //{
        //    priorTenEntries.Items.Clear();

        //    if (currentIndex >= DataLibrary.DataStore.Count - 10)
        //    {
        //        for (int i = currentIndex; i <= DataLibrary.DataStore.Count; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex - i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex - i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //priorTenEntries.Items.Add(item);
        //            priorTenEntries.Items.Add(CreateListBoxItem(currentIndex - i));
        //        }
        //    }
        //    else if (currentIndex > 9 && currentIndex <= DataLibrary.DataStore.Count - 10)
        //    {
        //        for (int i = 1; i <= 10; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex - i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex - i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //priorTenEntries.Items.Add(item);
        //            priorTenEntries.Items.Add(CreateListBoxItem(currentIndex - i));
        //        }
        //    }
        //    else if (currentIndex < 10)
        //    {
        //        for (int i = 1; i <= currentIndex; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex - i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex - i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //priorTenEntries.Items.Add(item);
        //            priorTenEntries.Items.Add(CreateListBoxItem(currentIndex - i));

        //        }
        //    }
        //}

        private void GetPreviousEntries()
        {
            // Clears any previously added entries in priorTenEntries ListBox
            if (priorTenEntries.Items.Count > 0)
                priorTenEntries.Items.Clear();

            // Checks for the condition of being one of the first 10 games in the library
            if (currentIndex < 10)
                for (int i = 1; i < currentIndex + 1; i++)
                    priorTenEntries.Items.Add(CreateListBoxItem(currentIndex - i));
            else
                for (int i = 1; i < 11; i++)
                    priorTenEntries.Items.Add(CreateListBoxItem(currentIndex - i));

        }



        /// <summary>
        /// Gets the next ten entries after the currently displayed Game for filling the NextEntries ListBox.
        /// </summary>
        private void GetNextEntries()
        {
            // Clears any previously added entries in nextTenEntries ListBox
            if (nextTenEntries.Items.Count > 0)
                nextTenEntries.Items.Clear();

            // Checks for the condition of being one of the last 10 games in the library
            if (currentIndex > DataLibrary.DataStore.Count - 10)
                for (int i = currentIndex + 1; i < DataLibrary.DataStore.Count; i++)
                    nextTenEntries.Items.Add(CreateListBoxItem(i));
            else
                for (int i = 1; i < 11; i++)
                    nextTenEntries.Items.Add(CreateListBoxItem(currentIndex + i));
        }

        /// <summary>
        /// Gets the next ten entries after the currently displayed Game for filling the NextEntries ListBox.
        /// </summary>
        //private void GetNextEntries()
        //{            
        //    nextTenEntries.Items.Clear();

        //    // Check if currentIndex is one of the last 10 entries
        //    if (currentIndex >= DataLibrary.DataStore.Count - 10)
        //    {
        //        // for (int i = 1; i <= DataLibrary.DataStore.Count - currentIndex; i++)
        //        for (int i = currentIndex; i < DataLibrary.DataStore.Count - 1; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex + i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex + i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //nextTenEntries.Items.Add(item);

        //            // Removed as this would allow 10 "empty" entries to bbe added.
        //            nextTenEntries.Items.Add(CreateListBoxItem(i));
        //        }
        //    }   // Check if currentIndex is one of the middle entries
        //    else if (currentIndex > 9 && currentIndex <= DataLibrary.DataStore.Count - 10)
        //    {
        //        for (int i = 1; i <= 10; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex + i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex + i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //nextTenEntries.Items.Add(item);
        //            nextTenEntries.Items.Add(CreateListBoxItem(currentIndex + i));
        //        }
        //    }   // Check if currentIndex is one of the first 10 entries
        //    else if (currentIndex < 10)
        //    {
        //        for (int i = 1; i <= currentIndex; i++)
        //        {
        //            //ListBoxItem item = new ListBoxItem();

        //            //item.Content = (currentIndex + i + 1).ToString() + ". " + DataLibrary.DataStore[currentIndex + i].Title;
        //            //item.Tapped += Entry_Tapped;
        //            //item.KeyDown += Entry_KeyDown;
        //            //nextTenEntries.Items.Add(item);
        //            nextTenEntries.Items.Add(CreateListBoxItem(currentIndex + i));
        //        }
        //    }
        //}

        /// <summary>
        /// Creates a new ListBoxItem and assigns Content, Tapped, and KeyDown events.
        /// </summary>
        /// <param name="index">The index of the currently displayed Game.</param>
        /// <returns>ListBoxItem</returns>
        private ListBoxItem CreateListBoxItem(int index)
        {
            ListBoxItem item = new ListBoxItem();

            item.Content = (index + 1).ToString() + ". " + DataLibrary.DataStore[index].Title;
            item.Tapped += Entry_Tapped;
            item.KeyDown += Entry_KeyDown;

            return item;
        }

        /// <summary>
        /// Displays the currently selected Game in the EditSingleItem form fields.
        /// </summary>
        /// <param name="game">The currectly selected IEnumerable Game.</param>
        private void DisplayGame(IEnumerable<Game> game)
        {
            if (game != null)
            {
                searchResults = game;
                currentIndex = DataLibrary.DataStore.IndexOf(game.FirstOrDefault());


                titleField.Text = game.FirstOrDefault().Title;
                consoleSelect.SelectedIndex = (int)game.FirstOrDefault().ConsoleGeneration;
                gamePassSelect.SelectedIndex = (int)game.FirstOrDefault().GamePassStatus;
                formatSelect.SelectedIndex = (int)game.FirstOrDefault().GameFormat;
                completedToggle.IsSelected = game.FirstOrDefault().IsGameCompleted;
                achievementsToggle.IsSelected = game.FirstOrDefault().IsAchievementsComplete;
                installedToggle.IsSelected = game.FirstOrDefault().IsInstalled;
                currentScoreField.Text = game.FirstOrDefault().CurrentAchievements.ToString();
                maxScoreField.Text = game.FirstOrDefault().MaxAchievements.ToString();
                dateAddedSelect.Date = game.FirstOrDefault().DateAdded;

                GetPreviousEntries();
                GetNextEntries();

                displayedIndex.Text = (currentIndex + 1).ToString();
                maxIndex.Text = DataLibrary.DataStore.Count.ToString();

                titleField.IsReadOnly = false;
            }
        }

        /// <summary>
        /// Displays the currently selected Game in the EditSingleItem form fields.
        /// </summary>
        /// <param name="game">The currently selected Game.</param>
        private void DisplayGame(Game game)
        {
            currentIndex = DataLibrary.DataStore.IndexOf(game);

            titleField.Text = game.Title;
            consoleSelect.SelectedIndex = (int)game.ConsoleGeneration;
            gamePassSelect.SelectedIndex = (int)game.GamePassStatus;
            formatSelect.SelectedIndex = (int)game.GameFormat;
            completedToggle.IsSelected = game.IsGameCompleted;
            achievementsToggle.IsSelected = game.IsAchievementsComplete;
            installedToggle.IsSelected = game.IsInstalled;
            currentScoreField.Text = game.CurrentAchievements.ToString();
            maxScoreField.Text = game.MaxAchievements.ToString();
            dateAddedSelect.Date = game.DateAdded;

            GetPreviousEntries();
            GetNextEntries();

            displayedIndex.Text = (currentIndex + 1).ToString();
            maxIndex.Text = DataLibrary.DataStore.Count.ToString();

            titleField.IsReadOnly = false;
        }

        /// <summary>
        /// Removes the specified Game from the DataLibrary.DataStore ObservableCollection.
        /// </summary>
        /// <param name="enumerable">The game to be deleted.</param>
        private async void DeleteGame(IEnumerable<Game> enumerable)
        {
            // 1.  Copy current game/gameTitle to temporary storage item.
            Game isolateForDeletion = enumerable.FirstOrDefault();

            // Confirm deletion of game object
            ContentDialog confirmation = new ContentDialog
            {
                Title = "Delete " + isolateForDeletion + " permanently?",
                Content = "If you delete this game data, you won't be able to recover it.  Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult confirmationResult = await confirmation.ShowAsync();

            if (confirmationResult == ContentDialogResult.Primary)
            {
                // 2.  Clear fields of current values OR advance to next game display.
                ClearForm();

                // 3.  Remove indicated game from DataLibrary.DataStore.
                DataLibrary.DataStore.Remove(isolateForDeletion);

                // 4.  Write/save DataLibrary.DataStore to gameLibrary.json file.
                JsonUtilities.Write();
            }
            else
            {

            }
        }

        private void Entry_Tapped(object sender, TappedRoutedEventArgs e)
        {
            priorTenEntries.Items.Clear();
            nextTenEntries.Items.Clear();

            ListBoxItem entry = sender as ListBoxItem;

            if (entry.Parent == nextTenEntries)
                priorTenEntries.SelectedItem = null;
            else if (entry.Parent == priorTenEntries)
                nextTenEntries.SelectedItem = null;

            string title = entry.Content.ToString();
            int cutoff = entry.Content.ToString().IndexOf('.') + 2;

            if (entry != null)
            {
                //DisplayGame(from game in DataLibrary.DataStore
                //            where game.Title == entry.Content.ToString().ToLowerInvariant().Remove(0, cutoff)
                //            select game);
                //DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(entry.Content.ToString().ToLowerInvariant())));
                DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(entry.Content.ToString().ToLowerInvariant().Remove(0, cutoff))));
            }
        }

        /// <summary>
        /// Simulates a keyboard keypress programmatically.
        /// Reference:  https://stackoverflow.com/questions/56636716/how-to-simulate-a-tab-key-press-with-code-in-uwp
        /// </summary>
        /// <param name="key">The VirtualKey representing the key needing to be pressed.</param>
        /// <param name="options">Any options needing to be attached to the simulated keypress.</param>
        private async void InjectKey(VirtualKey key, [Optional] InjectedInputKeyOptions options)
        {
            //InputInjector injector = InputInjector.TryCreate();

            //for (int i = 0; i < 10; i++)
            //{
            //    var info = new InjectedInputKeyboardInfo();
            //    info.VirtualKey = (ushort)key;
            //    info.KeyOptions = options;
            //    injector.InjectKeyboardInput(new[] { info });
            //    await Task.Delay(1000);
            //}
        }

        private async void Entry_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //VirtualKey key = VirtualKey.Enter;            

            //if (e.Key == VirtualKey.Enter)
            //{
            //    InjectKey(VirtualKey.Tab);
            //    InjectKey(VirtualKey.Tab, InjectedInputKeyOptions.KeyUp);
            //}

            priorTenEntries.Items.Clear();
            nextTenEntries.Items.Clear();

            ListBoxItem entry = sender as ListBoxItem;

            if (entry.Parent == nextTenEntries)
                priorTenEntries.SelectedItem = null;
            else if (entry.Parent == priorTenEntries)
                nextTenEntries.SelectedItem = null;

            string title = entry.Content.ToString();
            int cutoff = entry.Content.ToString().IndexOf('.') + 2;

            if (entry != null && (e.Key == Windows.System.VirtualKey.Enter || e.Key == Windows.System.VirtualKey.Space))
            {
                //DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(entry.Content.ToString().ToLowerInvariant())));
                DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(entry.Content.ToString().ToLowerInvariant().Remove(0, cutoff))));
            }
        }

        private void gameSearch_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !string.IsNullOrWhiteSpace(gameSearch.Text))
            {
                searchResults = DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(gameSearch.Text.ToLowerInvariant()));
                currentIndex = DataLibrary.DataStore.IndexOf(searchResults.FirstOrDefault());

                DisplayGame(searchResults);
            }
        }

        private void gameSearch_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.SuggestionChosen)
            {

            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
            {

            }
            else if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(sender.Text.ToLowerInvariant()));
            }
        }

        private void gameSearch_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            Game selectedGame = (Game)args.SelectedItem;
            sender.Text = selectedGame.Title;

            DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(selectedGame.Title.ToLowerInvariant())));
        }

        private void gameSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                Game chosenGame = (Game)args.ChosenSuggestion;
                DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(chosenGame.Title.ToLowerInvariant())));
            }
            else
            {
                DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(args.QueryText.ToLowerInvariant())));
            }
        }

        private void randomGameSelect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Random random = new Random();
            ObservableCollection<Game> unfinishedGames = new ObservableCollection<Game>(DataLibrary.DataStore.Where(game => !game.IsGameCompleted));
            //unfinishedGames = DataLibrary.DataStore.Where(game => !game.IsGameCompleted);

            ObservableCollection<Game> incompleteGames = new ObservableCollection<Game>(from game in DataLibrary.DataStore
                                                                                        where game.IsGameCompleted != true
                                                                                        orderby game.Title ascending
                                                                                        select game);

            int randomIndex = random.Next(0, incompleteGames.Count - 1);

            DisplayGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(incompleteGames[randomIndex].Title.ToLowerInvariant())));
        }

        private void deleteEnable_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (lockedIcon.Visibility == Visibility.Visible)
            {
                lockedIcon.Visibility = Visibility.Collapsed;
                unlockedIcon.Visibility = Visibility.Visible;
                deleteGame.Visibility = Visibility.Visible;
            }
            else if (lockedIcon.Visibility == Visibility.Collapsed)
            {
                lockedIcon.Visibility = Visibility.Visible;
                unlockedIcon.Visibility = Visibility.Collapsed;
                deleteGame.Visibility = Visibility.Collapsed;
            }
        }

        private void deleteGame_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DeleteGame(DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(titleField.Text.ToLowerInvariant())));
        }

        private void selectAllField_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.SelectAll();
        }

        private async void trueAchievementsButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string baseLink = "https://www.trueachievements.com/game/";
            string titleLink = titleField.Text;
            string trailingLink = "/achievements";

            titleLink = titleLink.Replace(' ', '-');
            titleLink = titleLink.Replace("\'", "");

            string taLink = baseLink + titleLink + trailingLink;

            var taUri = new Uri(taLink);
            await Launcher.LaunchUriAsync(taUri);
        }

        private void comboBoxSelect_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (e.Key == VirtualKey.Delete)
                comboBox.SelectedIndex = -1;
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            if (block != null)
            {
                block.FontSize = 20;
            }
        }
    }
}