using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CsvHelper;
using System.Globalization;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XboxLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;     // https://stackoverflow.com/questions/59207572/how-can-correctly-i-get-mainpage
        public LibraryChangelog Changelog { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            //Current = this;     // https://stackoverflow.com/questions/59207572/how-can-correctly-i-get-mainpage


            //LoadTestGames();

            // IPropertySet roamingProperties = ApplicationData.Current.RoamingSettings.Values;
            // roamingProperties.Remove("HasSelectedLibrary");
            
            DataLibrary.Initialize();
        }


        /// <summary>
        /// Asynchronously reads the contents of a CSV file into a List&lt;Game&gt; object.
        /// </summary>
        /// <param name="file">The StorageFile pointing to the CSV file.</param>
        /// <returns>List&lt;Game&gt;</returns>
        public async Task<List<Game>> ExcelImport(StorageFile file)
        {
            //string csvText = await file.OpenSequentialReadAsync();

            //CsvReader reader = new CsvReader(new StringReader(csvText), System.Globalization.CultureInfo.InvariantCulture);
            
            var records = new List<Game>();

            using (var reader = new StreamReader((Stream)file.OpenReadAsync()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    var record = new Game();
                    // Title
                    record.Title = csv.GetField("Game Title");                    

                    // ConsoleGeneration
                    switch (csv.GetField("System"))
                    {
                        case "ORI":
                            record.ConsoleGeneration = XboxSystem.Xbox;
                            break;
                        case "BS":
                            record.ConsoleGeneration = XboxSystem.Xbox360;
                            break;
                        case "XB1":
                            record.ConsoleGeneration = XboxSystem.XboxOne;
                            break;
                        case "SX":
                            record.ConsoleGeneration = XboxSystem.XboxSeriesSX;
                            break;
                        case "WIN":
                            record.ConsoleGeneration = XboxSystem.Windows;
                            break;
                    }

                    // GameFormat
                    switch (csv.GetField("Format"))
                    {
                        case "Digital":
                        case "Game Pass":
                            record.GameFormat = Format.Digital;
                            break;
                        case "Physical":
                            record.GameFormat = Format.Physical;
                            break;
                        default:
                            record.GameFormat = Format.Unknown;
                            break;
                    }

                    // GamePassStatus
                    switch (csv.GetField("Game Pass Status"))
                    {
                        case "Available":
                            record.GamePassStatus = GamePassStatus.Available;
                            break;
                        case "Not Applicable":
                            record.GamePassStatus = GamePassStatus.NotApplicable;
                            break;
                        case "Removed":
                            record.GamePassStatus = GamePassStatus.Removed;
                            break;
                        default:
                            record.GamePassStatus = GamePassStatus.Unknown;
                            break;
                    }

                    // IsGameCompleted
                    record.IsGameCompleted = csv.GetField<bool>("Complete (Y/N)");

                    // IsAchievementsComplete
                    record.IsAchievementsComplete = csv.GetField<bool>("100%");

                    // IsInstalled
                    record.IsInstalled = csv.GetField<bool>("Installed");
                    string x = "0"; Int32.Parse(x);        
                    records.Add(record);
                }
            }
            return records;
        }

        // Source Reference:  https://stackoverflow.com/questions/67185991/uwp-navigationviewitem-switch-page
        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItemContainer;

            switch (item.Name)
            {
                case "homeSelect":
                    ContentFrame.Navigate(typeof(Home));
                    break;
                case "individualViewSelect":
                    ContentFrame.Navigate(typeof(EditSingleItem));
                    break;
                case "fullLibrarySelect":
                    ContentFrame.Navigate(typeof(LibraryDataGrid));
                    break;
                case "addGameSelect":
                    ContentFrame.Navigate(typeof(AddNewGame));
                    break;
                case "advancedSearchSelect":
                    ContentFrame.Navigate(typeof(AdvancedSearch));
                    break;
                case "importSelect":
                    ContentFrame.Navigate(typeof(Import));
                    break;
                case "exportSelect":
                    ContentFrame.Navigate(typeof(Scratchpad));
                    break;
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {            
            JsonUtilities.Write(DataLibrary.storageFile);
            statusMessage.Text = "Library saved on " + DateTime.Now.ToString("M") + " at " + DateTime.Now.ToString("t");
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(SettingsPage));
        }
    }
}
