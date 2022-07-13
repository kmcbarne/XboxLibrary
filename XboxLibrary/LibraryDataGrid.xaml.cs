using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI;
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
    public sealed partial class LibraryDataGrid : Page
    {
        public ObservableCollection<Game> Games { get => DataLibrary.DataStore; }
        //string PrimaryLibraryToken { get; set; }
        int searchResultCount { get; set; }
        int searchResultIndex { get; set; }
        IEnumerable<Game> searchResults { get; set; }
        Game selectedGame { get; set; }
        DataGridRow currentRow { get; set; }
        
        public List<EnumItemsSource<XboxSystem>> XboxSystems
        {
            get => EnumItemsSource<XboxSystem>.ToList();
        }

        public LibraryDataGrid()
        {
            
            this.InitializeComponent();
            //Games = DataLibrary.DataStore;
            searchResultIndex = -1;
            //LoadTestGames();
        }

        /// <summary>
        /// Sorts the ObservableCollection Games in Ascending style by the requested column.
        /// </summary>
        /// <param name="column">The column to sort by.</param>
        private void SortLibraryAscending(DataGridColumn column)
        {
            libraryDisplay.ItemsSource = new ObservableCollection<Game>(from game in DataLibrary.DataStore
                                                                        orderby game.Title ascending
                                                                        select game);

            ObservableCollection<Game> x = DataLibrary.DataStore;

            column.SortDirection = DataGridSortDirection.Ascending;

            foreach (var col in libraryDisplay.Columns)
            {
                if (col.Tag.ToString() != column.Tag.ToString())
                    col.SortDirection = null;
            }
        }

        /// <summary>
        /// Sorts the ObservableCollection Games in Descending style by the requested column.
        /// </summary>
        /// <param name="column">The column to sort by.</param>
        private void SortLibraryDescending(DataGridColumn column)
        {
            libraryDisplay.ItemsSource = new ObservableCollection<Game>(from game in DataLibrary.DataStore
                                                                        orderby game.Title descending
                                                                        select game);
            column.SortDirection = DataGridSortDirection.Descending;

            foreach (var col in libraryDisplay.Columns)
            {
                if (col.Tag.ToString() != column.Tag.ToString())
                    col.SortDirection = null;
            }
        }

        

        private void testButton_Click(object sender, RoutedEventArgs e)
        {            
            //JsonTest();
        }

        private void libraryDisplay_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (e.Column.Tag.ToString() == "Title")
            {
                if (e.Column.SortDirection == null || e.Column.SortDirection == Microsoft.Toolkit.Uwp.UI.Controls.DataGridSortDirection.Descending)
                {
                    SortLibraryAscending(e.Column);
                }
                else
                {
                    SortLibraryDescending(e.Column);
                }
            }
        }

        private void AppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            libraryDisplay.ItemsSource = new ObservableCollection<Game>(from game in DataLibrary.DataStore
                                                                        where game.IsInstalled == true
                                                                        select game);
        }

        private void AppBarToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            libraryDisplay.ItemsSource = new ObservableCollection<Game>(from game in DataLibrary.DataStore
                                                                        select game);
        }

        private void groupByLetter_Click(object sender, RoutedEventArgs e)
        {
            // Add

            //ObservableCollection<GroupInfoCollection<Game>> games = new ObservableCollection<GroupInfoCollection<Game>>();

            //var query = from game in Games
            //            group game by game.Title into g
            //            select new { GroupName = g.Key, Items = g };

            //foreach (var g in query)
            //{
            //    GroupInfoCollection<Game> info = new GroupInfoCollection<Game>();
            //    info.Key = g.GroupName;
            //    foreach (var item in g.Items)
            //        info.Add(item);
            //    games.Add(info);
            //}

            //CollectionViewSource groupedItems = new CollectionViewSource();
            //groupedItems.IsSourceGrouped = true;
            //groupedItems.Source = games;

            //libraryDisplay.ItemsSource = groupedItems.View;
        }

        private void libraryDisplay_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            DataGridColumn col = e.Column;
            DataGridRow row = e.Row;

            
            
        }

        private void TemplateColumn_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox box = sender as TextBox;
            box.SelectAll();
        }

        /// <summary>
        /// Brings a specified item into view within a <c>DataGrid</c> control.
        /// </summary>
        /// <param name="jumpLetter">The letter the Title field begins with for which to search.</param>
        /// <see href="https://social.msdn.microsoft.com/Forums/silverlight/en-US/edcd6796-05b5-4e07-8768-dd564daab4d5/datagrid-scroll-to-last-item-or-to-the-bottom"/>
        /// <seealso cref="operator=="/>
        public void JumpTo(char jumpLetter)
        {
            if (jumpLetter == '#')
                jumpLetter = '3';

            try
            {
                Game found = DataLibrary.DataStore.First(game => game.Title.StartsWith(jumpLetter));
                libraryDisplay.ScrollIntoView(found, null);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Brings a specified item into view within a <c>DataGrid</c> control.
        /// </summary>
        /// <param name="jumpWord">The word in the Title field for which to search.</param>
        /// <see href="https://social.msdn.microsoft.com/Forums/silverlight/en-US/edcd6796-05b5-4e07-8768-dd564daab4d5/datagrid-scroll-to-last-item-or-to-the-bottom"/>
        /// <seealso cref="operator=="/>
        public void JumpTo(string jumpWord)
        {
            try
            {
                Game found = DataLibrary.DataStore.First(game => game.Title.StartsWith(jumpWord));
                libraryDisplay.ScrollIntoView(found, null);
            }
            catch(Exception ex)
            {

            }
        }

        public void JumpTo(char[] jumpLetter)
        {
            bool gameFound = false;
            int i = jumpLetter.Length;
     
                Game found = DataLibrary.DataStore.First(game => game.Title.StartsWith(jumpLetter[i]));
                libraryDisplay.ScrollIntoView(found, null);
        }

        private void CloseJumpFlyout(Button letterButton)
        {
            Grid grid = (Grid)letterButton.Parent;
            FlyoutPresenter presenter = (FlyoutPresenter)grid.Parent;
            Popup popup = (Popup)presenter.Parent;
            popup.IsOpen = false;
        }

        private void jumpToLetter_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void jumpSelection_Click(object sender, RoutedEventArgs e)
        {
            Button letterButton = sender as Button;
            char jumpLetter = char.Parse(letterButton.Content.ToString());

            JumpTo(jumpLetter);
            CloseJumpFlyout(letterButton);            
        }

        private void gameSearch_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !string.IsNullOrWhiteSpace(gameSearch.Text))
            {
                searchResults = DataLibrary.DataStore.Where(game => game.Title.ToLowerInvariant().Contains(gameSearch.Text.ToLowerInvariant()));
            }

            if (searchResults != null)
                searchResultsCount.Text = searchResults.Count().ToString();

            if (searchResults != null && searchResults.Count() > 0)
            {
                searchResultControls.Visibility = Visibility.Visible;
                searchResultIndex = 0;

                libraryDisplay.ScrollIntoView(searchResults.ElementAt(searchResultIndex), null);
            }            
        }

        private void prevSearchResult_Click(object sender, RoutedEventArgs e)
        {
            // Check that searchResultIndex is not disabled [-1] and has not reached the lowest possible index [0]
            if (searchResultIndex > -1 && searchResultIndex != 0)
            {
                searchResultIndex--;
            }
            // If searchResultIndex has reached lowest possible index [0], wrap around to highest index [Count() - 1]
            else if (searchResultIndex == 0)
            {
                searchResultIndex = searchResults.Count() - 1;
            }
            // Display current search results index
            libraryDisplay.ScrollIntoView(searchResults.ElementAt(searchResultIndex), null);
        }

        private void nextSearchResult_Click(object sender, RoutedEventArgs e)
        {
            // Check that searchResultIndex is not disabled [-1] and has not reached the highest possible index [Count() - 1]
            if (searchResultIndex > -1 && searchResultIndex != searchResults.Count() - 1)
            {
                searchResultIndex++;
            }
            // If searchResultIndex has reached highest possible index [Count() - 1], wrap around to 0
            else if (searchResultIndex == searchResults.Count() - 1)
            {
                searchResultIndex = 0;
            }
            // Display current search results index
            libraryDisplay.ScrollIntoView(searchResults.ElementAt(searchResultIndex), null);
        }

        private void Viewbox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Viewbox box = sender as Viewbox;

            //DataGridCell cell = box.Parent as DataGridCell;
            //DataGridCellsPresenter presenter = cell.Parent as DataGridCellsPresenter;
            //DataGridRow row = presenter.Parent as DataGridRow;
            //DataGridFrozenGrid frozen = row.Parent as DataGridFrozenGrid;

            DataLibrary.DataStore.Remove((Game)libraryDisplay.SelectedItem);

            
        }

        private void PathIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void libraryDisplay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Game selected = (Game)libraryDisplay.SelectedItem;

                //if (Games.Any(x => x.Title == selected.Title))
                //{
                //    var selectedGame = Games.FirstOrDefault(x => x.Title == selected.Title);
                //    selectedGame.Title = detailsTitle.Text;
                //}
            }
            catch (Exception ex)
            {
            }            //currentRow = libraryDisplay.
            //    (DataGridRow)grid.SelectedItem;
            //libraryDisplay.SelectedItem = currentRow;
        }

        private void libraryDisplay_Loading(FrameworkElement sender, object args)
        {

        }

        private void updateRecord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            libraryDisplay.InvalidateArrange();
        }

        private void gameSearch_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }
    }    
}