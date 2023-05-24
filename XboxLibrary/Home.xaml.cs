using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
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
    public sealed partial class Home : Page
    {
        public ObservableCollection<Game> Games { get => DataLibrary.DataStore; }
        public ObservableCollection<Game> Xbox => new ObservableCollection<Game>(DataLibrary.DataStore.Where(Game => Game.ConsoleGeneration == XboxSystem.Xbox));
        public ObservableCollection<Game> Xbox360 => new ObservableCollection<Game>(DataLibrary.DataStore.Where(Game => Game.ConsoleGeneration == XboxSystem.Xbox360));
        public ObservableCollection<Game> XboxOne => new ObservableCollection<Game>(DataLibrary.DataStore.Where(Game => Game.ConsoleGeneration == XboxSystem.XboxOne));
        public ObservableCollection<Game> XboxSeries => new ObservableCollection<Game>(DataLibrary.DataStore.Where(Game => Game.ConsoleGeneration == XboxSystem.XboxSeriesSX));
        public ObservableCollection<Game> Windows => new ObservableCollection<Game>(DataLibrary.DataStore.Where(Game => Game.ConsoleGeneration == XboxSystem.Windows));
        public StorageFile storageFile { get => DataLibrary.storageFile; }
        public BasicProperties FileProperties => GetFileProperties().Result;

        public Home()
        {
            this.InitializeComponent();       
        }

        private async Task<BasicProperties> GetFileProperties()
        {
            try
            {
                if (storageFile != null)
                {
                    BasicProperties properties = await storageFile.GetBasicPropertiesAsync();
                    return properties;
                }
                else return null;
            }catch (Exception ex) { }
            return null;
        }
    }
}
