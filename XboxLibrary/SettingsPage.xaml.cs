using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class SettingsPage : Page
    {


        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void resetFinal_Click(object sender, RoutedEventArgs e)
        {
            if (resetConfirm.Text == "RESET")
            {
                IPropertySet roamingProperties = ApplicationData.Current.RoamingSettings.Values;
                roamingProperties.Remove("HasSelectedLibrary");
            }
        }

        private void ThemeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;

            LibraryDataGrid grid = new LibraryDataGrid();

            switch (box.SelectedValue)
            {
                case "Gothic":
                    //grid.AlternateRowColor = new SolidColorBrush(Color.FromArgb(1, 48, 48, 48));
                    //grid.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                    //grid.FontColor = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
                    break;
                case "Cloudy Day":
                    //grid.AlternateRowColor = new SolidColorBrush(Color.FromArgb(1, 144, 175, 197));
                    //grid.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                    //grid.FontColor = new SolidColorBrush(Color.FromArgb(1, 255, 255, 255));
                    break;
            }            
        }
    }
}
