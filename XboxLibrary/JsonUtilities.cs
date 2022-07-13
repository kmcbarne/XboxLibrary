using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace XboxLibrary
{
    public class JsonUtilities
    {
        public JsonUtilities()
        {
            
        }

        /// <summary>
        /// Asynchronously converts the data stored in the JSON Storage File into a string object.
        /// </summary>
        /// <returns>A string converted from a deserialized JSON Storage File.</returns>
        private static async Task<string> DeserializeAsync(StorageFile storageFile)
        {
            try
            {
                return await FileIO.ReadTextAsync(storageFile);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Asynchronously reads the data stored in the JSON Storage File into an ObservableCollection of Games.
        /// </summary>
        /// <returns>An ObservableCollection of Game objects.</returns>
        public static async Task<ObservableCollection<Game>> Read(StorageFile storageFile)
        {
            string jsonString = await DeserializeAsync(storageFile);
            if (jsonString != null)
                return (ObservableCollection<Game>)JsonConvert.DeserializeObject(jsonString, typeof(ObservableCollection<Game>));
            return null;
        }

        public static async void Write()
        {
            if (DataLibrary.storageFile != null)
            {
                DataLibrary.Sort();
                string jsonOutputData = JsonConvert.SerializeObject(DataLibrary.DataStore, Formatting.Indented);
                await FileIO.WriteTextAsync(DataLibrary.storageFile, jsonOutputData);
            }
        }

        /// <summary>
        /// Writes the data in the ObservableCollection of Games to a JSON Storage File.
        /// </summary>
        public static async void Write(StorageFile storageFile)
        {
            if (storageFile != null)
            {
                DataLibrary.Sort();
                string jsonOutputData = JsonConvert.SerializeObject(DataLibrary.DataStore, Formatting.Indented);
                await FileIO.WriteTextAsync(storageFile, jsonOutputData);
            }
        }
    }
}
