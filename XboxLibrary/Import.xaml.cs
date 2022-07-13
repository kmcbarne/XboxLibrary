using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
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
    public sealed partial class Import : Page
    {
        StorageFile importFile;

        public Import()
        {
            this.InitializeComponent();
        }

        public async Task<List<Game>> ExcelImport(StorageFile file)
        {
            //string csvText = await file.OpenSequentialReadAsync();

            //CsvReader reader = new CsvReader(new StringReader(csvText), System.Globalization.CultureInfo.InvariantCulture);

            var records = new List<Game>();

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
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
                    if (csv.GetField("Complete (Y/N)") == "Yes")
                        record.IsGameCompleted = true;
                    else
                        record.IsGameCompleted = false;

                    // IsAchievementsComplete
                    if (csv.GetField("100%") == "Yes")
                        record.IsAchievementsComplete = true;
                    else
                        record.IsAchievementsComplete = false;

                    // IsInstalled
                    if (csv.GetField("Installed") == "Yes")
                        record.IsInstalled = true;
                    else
                        record.IsInstalled = false;

                    if (csv.GetField("DateAdded") == null)
                        record.DateAdded = DateTime.Today;
                    else
                        record.DateAdded = DateTime.Parse(csv.GetField("DateAdded"));

                    records.Add(record);
                }
            }
            return records;
        }

        public async void BeginImport()
        {            
            List<Game> importedGames = await ExcelImport(importFile);

            foreach (Game game in importedGames)
            {
                DataLibrary.DataStore.Add(game);

                //if (DataLibrary.DataStore.Count >= 30)
                //    return;
            }
        }
        
        private async void launchPicker_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.List;
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.FileTypeFilter.Add(".csv");

            importFile = await filePicker.PickSingleFileAsync();

            importPath.Text = importFile.Path;
        }

        private void startImport_Click(object sender, RoutedEventArgs args)
        {
            //TextBlock textBlock = new TextBlock();
            //textBlock.Text = "Begin importing " + importFile.Path + "?";

            //Flyout flyout = new Flyout();
            //flyout.Content = textBlock;
            //flyout.ShowAt(this);

            string importConfirmation = "Begin importing " + importFile.Path + "?";

            object[] button_one = { "Yes", new UICommandInvokedHandler((e) =>
            {
                BeginImport();
            }) };

            object[] button_two = { "No", new UICommandInvokedHandler((e) =>
            {
                return;
            })};

            object[][] buttons = new object[][]
            {
                button_one,
                button_two
            };

            ShowMessage("Title", importConfirmation, buttons);

            //await ExcelImport(importFile);
        }

        /// <summary>
        /// Displays a MessageBox.
        /// Code Source:  https://stackoverflow.com/questions/46565499/message-box-in-uwp-not-working/46571099#46571099
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="buttons"></param>
        private async void ShowMessage(string title, string content, [Optional]object[][] buttons)
        {
            MessageDialog dialog = new MessageDialog(content, title);
            
            dialog.CancelCommandIndex = 0;
            dialog.DefaultCommandIndex = 0;

            if (buttons != null)
            {
                if (buttons.Length > 1)
                {
                    for (Int32 i = 0; i < buttons.Length; i++)
                    {
                        string text = (string)buttons[i][0];

                        UICommandInvokedHandler handler = (UICommandInvokedHandler)buttons[i][1];

                        if (handler.GetType().Equals(typeof(UICommandInvokedHandler)) && text.GetType().Equals(typeof(string)))
                        {
                            UICommand button = new UICommand(text, handler);
                            dialog.Commands.Add(button);
                        }
                        else return;
                    }
                }
                else
                {
                    string text = (string)buttons[0][0];

                    UICommandInvokedHandler handler = (UICommandInvokedHandler)buttons[0][1];

                    if (handler.GetType().Equals(typeof(UICommandInvokedHandler)) && text.GetType().Equals(typeof(string)))
                    {
                        UICommand button = new UICommand(text, handler);

                        dialog.Commands.Add(button);
                    }
                    else return;
                }

                dialog.DefaultCommandIndex = (UInt32)buttons.Length;
            }
            await dialog.ShowAsync();
        }
    }
}
