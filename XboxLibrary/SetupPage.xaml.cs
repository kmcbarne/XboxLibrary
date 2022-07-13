using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace XboxLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetupPage : Page
    {
        public string LibraryToken;

        public SetupPage()
        {
            this.InitializeComponent();
        }

        private async void SelectPrimaryLibrary()
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.List;
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.FileTypeFilter.Add(".json");

            StorageFile file = await filePicker.PickSingleFileAsync();
                        
            LibraryToken = StorageApplicationPermissions.FutureAccessList.Add(file);
            ApplicationData.Current.LocalSettings.Values["libraryToken"] = LibraryToken;     
        }

        private void browseLibrary_Click(object sender, RoutedEventArgs e)
        {
            SelectPrimaryLibrary();
        }
    }
}
