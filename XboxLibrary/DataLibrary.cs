using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace XboxLibrary
{
    public static class DataLibrary
    {
        #region Properties/Variables
        public static StorageFile storageFile;

        public static ApplicationDataContainer LocalSettings { get => ApplicationData.Current.LocalSettings; }

        public static ObservableCollection<Game> DataStore { get; set; }

        public static string PrimaryLibraryToken { get; set; }
        #endregion Properties\Variables

        static DataLibrary()
        {
            
        }

        /// Methods handling the initial setup of the ObservaleCOllection<Games>.
        #region Library Setup Methods
        /// <summary>
        /// Readies the application to use the DataLibrary class.
        /// </summary>
        public async static void Initialize()
        {
            if (!HasAccess())
                await SelectDataFile();

            DataStore = await JsonUtilities.Read(storageFile);
        }

        /// <summary>
        /// Checks the Future Access List to see if it contains the JSON Storage File.
        /// </summary>
        /// <returns><em>True</em> if the Future Access List contains the JSON Storage File, otherwise <em>false</em>.</returns>
        private static bool HasAccess()
        {
            bool isLibraryConfigured = false;
            object libraryAccessToken = LocalSettings.Values["libraryToken"];

            if (libraryAccessToken != null)
            {
                if (libraryAccessToken.ToString() != "uninitialized")
                {
                    PrimaryLibraryToken = LocalSettings.Values["libraryToken"].ToString();
                    GetStorageFileAsync();
                    isLibraryConfigured = true;
                }
            }
            else
            {
                LocalSettings.Values["libraryToken"] = "uninitialized";
                isLibraryConfigured = false;
            }
            return isLibraryConfigured;
        }

        /// <summary>
        /// Displays a FileOpenPicker that allows the user to create or select a JSON file to be used to store Game Library data.
        /// </summary>
        public static async Task SelectDataFile()
        {
            if (storageFile.IsEqual(null))
            {
                var filePicker = new FileOpenPicker();
                filePicker.ViewMode = PickerViewMode.List;
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.FileTypeFilter.Add("*.json");
                filePicker.CommitButtonText = "Select Data File";
                filePicker.SettingsIdentifier = "DataFile";

                StorageFile file = await filePicker.PickSingleFileAsync();

                if (PrimaryLibraryToken.Equals(null) && !StorageApplicationPermissions.FutureAccessList.ContainsItem("libraryToken"))
                {
                    PrimaryLibraryToken = StorageApplicationPermissions.FutureAccessList.Add(file);
                    LocalSettings.Values["libraryToken"] = PrimaryLibraryToken;
                }
            }
        }

        /// <summary>
        /// Retrieves the StorageFile used to store data from the Future Access List.
        /// </summary>
        private static async void GetStorageFileAsync()
        {
            storageFile = Task.Run(async() => await StorageApplicationPermissions.FutureAccessList.GetFileAsync(PrimaryLibraryToken)).Result;
        }
        #endregion Library Setup Methods

        /// Methods that affect ObservableCollection<Game> contents.
        #region Data Manipulation Methods        
        /// <summary>
        /// Adds an entry to the ObservableCollection of Games.
        /// </summary>
        /// <param name="addGame">The Game to add to the ObservableCollection.</param>
        public static void Add(Game addGame)
        {
            DataStore.Add(addGame);
            DataStore.BubbleSort();
        }

        /// <summary>
        /// Removes the Game object stored at the spe3cified index from the ObservableCollection.
        /// </summary>
        /// <param name="removeIndex">The index of the Game to remove.</param>
        public static void RemoveAt(int removeIndex)
        {
            DataStore.RemoveAt(removeIndex);
        }

        /// <summary>
        /// Removes the Game object matching the spe3cified Game from the ObservableCollection.
        /// </summary>
        /// <param name="removeGame">The Game to remove.</param>
        public static void Remove(Game removeGame)
        {
            DataStore.Remove(removeGame);
        }

        /// <summary>
        /// Removes the Game object with the spe3cified title from the ObservableCollection.
        /// </summary>
        /// <param name="removeTitle">The title of the Game to remove.</param>
        public static void Remove(string removeTitle)
        {
            DataStore.Remove((Game)DataStore.Where(game => game.Title.Equals(removeTitle)));
        }

        /// <summary>
        /// Removes a Game object matching the specified Game from the ObservableCollection and inserts a different Game object in it's place.
        /// </summary>
        /// <param name="removeGame">The Game to be replaced.</param>
        /// /// <param name="addGame">The Game to be inserted.</param>
        public static void Replace(Game removeGame, Game addGame)
        {
            DataStore.Remove(removeGame);
            DataStore.Remove(addGame);
        }

        /// <summary>
        /// Removes a Game object stored at the specified index from the ObservableCollection and inserts a different Game object in it's place.
        /// </summary>
        /// <param name="removeIndex">The index of then Game to remove.></param>
        /// <param name="addGame">The Game to be inserted at the specified index.</param>
        public static void Replace(int removeIndex, Game addGame)
        {
            DataStore[removeIndex] = addGame;
        }

        /// <summary>
        /// Sorts the ObservableCollection alphabetically.
        /// </summary>
        public static void Sort()
        {
            DataStore.BubbleSort();
        }

        /// <summary>
        /// Repaces the data stored in the specified Game in favor of the new information.
        /// </summary>
        /// <param name="updateIndex">The index where the game to be modified is stored.</param>
        /// <param name="updateData">The Game to pull data from in </param>
        public static void Update(int updateIndex, Game updateData)
        {
            DataStore[updateIndex].Title = updateData.Title;
            DataStore[updateIndex].ConsoleGeneration = updateData.ConsoleGeneration;
        }
        #endregion Data Manipulation Methods
    }
}
