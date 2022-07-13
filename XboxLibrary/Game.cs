using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace XboxLibrary
{
    public class Game : INotifyCollectionChanged, IComparable
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private string title;
        private XboxSystem consoleGeneration;
        private Format gameFormat;
        private GamePassStatus gamePassStatus;
        private bool isGameCompleted;
        private bool isAchievementsComplete;
        private bool isInstalled;
        private int maxAchievements;
        private int currentAchievements;
        private double percentAchievements;
        private DateTime dateAdded;

        /// <summary>
        /// The name of the Game.
        /// </summary>
        [JsonProperty("Title")]
        public string Title
        {
            get { return title; } 
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// The console the Game was created for.
        /// </summary>
        [JsonProperty("ConsoleGeneration")]
        public XboxSystem ConsoleGeneration
        {
            get { return consoleGeneration; }
            set
            {
                consoleGeneration = value;
                RaisePropertyChanged("ConsoleGeneration");
            }
        }
        
        /// <summary>
        /// Denotes whether the Game is owned physically or digitally.
        /// </summary>
        [JsonProperty("GameFormat")]
        public Format GameFormat
        {
            get { return gameFormat; }
            set
            {
                gameFormat = value;
                RaisePropertyChanged("GameFormat");
            }
        }
        
        /// <summary>
        /// Indicates the Game's status in relation to GamePass.
        /// </summary>
        [JsonProperty("GamePassStatus")]
        public GamePassStatus GamePassStatus
        {
            get { return gamePassStatus; }
            set
            {
                gamePassStatus = value;
                RaisePropertyChanged("GamePassStatus");
            }
        }
        
        /// <summary>
        /// Denotes whether the Game has been beaten, regardless of achievements.
        /// </summary>
        [JsonProperty("IsGameCompleted")]
        public bool IsGameCompleted
        {
            get { return isGameCompleted; }
            set
            {
                isGameCompleted = value;
                RaisePropertyChanged("IsGameCompleted");
            }
        }
        
        /// <summary>
        /// Denotes whether all possible achievements for the Game have been earned.
        /// </summary>
        [JsonProperty("IsAchievementsComplete")]
        public bool IsAchievementsComplete
        {
            get { return isAchievementsComplete; }
            set
            {
                isAchievementsComplete = value;
                RaisePropertyChanged("IsAchievementsComplete");
            }
        }
        
        /// <summary>
        /// Denotes whether the Game is installed on the Xbox console.
        /// </summary>
        [JsonProperty("IsInstalled")]
        public bool IsInstalled
        {
            get { return isInstalled; }
            set
            {
                isInstalled = value;
                RaisePropertyChanged("IsInstalled");
            }
        }
        
        /// <summary>
        /// The maximum schievements score that can be earned for the Game.
        /// </summary>
        [JsonProperty("MaxAchievements")]
        public int MaxAchievements
        {
            get { return maxAchievements; }
            set
            {
                maxAchievements = value;
                RaisePropertyChanged("MaxAchievements");
            }
        }
        
        /// <summary>
        /// The current achievement score earned for the Game.
        /// </summary>
        [JsonProperty("CurrentAchievements")]
        public int CurrentAchievements
        {
            get
            {
                return currentAchievements;
            }
            set
            {
                currentAchievements = value;
                RaisePropertyChanged("CurrentAchievements");
            }
        }

        /// <summary>
        /// The calculated percentage completed of achievements score.
        /// </summary>
        [JsonProperty("PercentAchievements")]
        public string PercentAchievements
        {
            get
            {
                if (CurrentAchievements == 0 || MaxAchievements == 0)
                    return 0.00.ToString("00.00 %");
                else
                {
                    double pct = (double)CurrentAchievements / (double)MaxAchievements;
                    return pct.ToString("00.## %");
                }
            }
        }

        /// <summary>
        /// The date the Game was added to the library.
        /// </summary>
        [JsonProperty("DateAdded")]
        public DateTime DateAdded
        {
            get
            {                
                return dateAdded;
            }
            set
            {
                dateAdded = value;
                RaisePropertyChanged("DateAdded");
            }
        }

        /// <summary>
        /// Creates a new empty Game in the library.
        /// </summary>
        [JsonConstructor]
        public Game()
        {
            
        }

        /// <summary>
        /// Creates a new Game in the library with the provided data, not including achievements.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="consoleGeneration"></param>
        /// <param name="gameFormat"></param>
        /// <param name="gamePassStatus"></param>
        /// <param name="isGameCompleted"></param>
        /// <param name="isAchievementsComplete"></param>
        /// <param name="isInstalled"></param>
        public Game(string title, XboxSystem consoleGeneration, Format gameFormat, GamePassStatus gamePassStatus, bool isGameCompleted, bool isAchievementsComplete, bool isInstalled)
        {
            this.Title = title;
            this.ConsoleGeneration = consoleGeneration;
            this.GameFormat = gameFormat;
            this.GamePassStatus = gamePassStatus;
            this.IsGameCompleted = isGameCompleted;
            this.IsAchievementsComplete = isAchievementsComplete;
            this.IsInstalled = isInstalled;
            this.DateAdded = DateTime.Today;
        }

        /// <summary>
        /// Creates a new Game in the library with the provided data, including achievements.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="consoleGeneration"></param>
        /// <param name="gameFormat"></param>
        /// <param name="gamePassStatus"></param>
        /// <param name="isGameCompleted"></param>
        /// <param name="isAchievementsComplete"></param>
        /// <param name="isInstalled"></param>
        /// <param name="maxAchievements"></param>
        /// <param name="currentAchievements"></param>
        public Game(string title, XboxSystem consoleGeneration, Format gameFormat, GamePassStatus gamePassStatus, bool isGameCompleted, bool isAchievementsComplete, bool isInstalled, int maxAchievements, int currentAchievements)
        {
            this.Title = title;
            this.ConsoleGeneration = consoleGeneration;
            this.GameFormat = gameFormat;
            this.GamePassStatus = gamePassStatus;
            this.IsGameCompleted = isGameCompleted;
            this.IsAchievementsComplete = isAchievementsComplete;
            this.IsInstalled = isInstalled;
            this.MaxAchievements = maxAchievements;
            this.CurrentAchievements = currentAchievements;
            this.DateAdded = DateTime.Today;

            if (CurrentAchievements / MaxAchievements >= 1)
                IsAchievementsComplete = true;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Returns a string containing the Title with any leading articles (<i>A, An, The</i>) moved to the end.
        /// </summary>
        /// <returns></returns>
        public string GetSortableTitle()
        {
            string sortableTitle = "";
            int titleLength = Title.Length;

            if (Title != null)
            {
                if (Title[0] == 'A' || Title[0] == 'a' || Title[0] == 'T' || Title[0] == 't')
                {
                    if (Title[0] == 'A' && Title[1] == ' ')
                    {
                        sortableTitle = Title.Substring(2) + ", " + Title[0];
                    }
                    else if (Title[0] == 'A' && Title[1] == 'n' && Title[2] == ' ')
                    {
                        sortableTitle = Title.Substring(3) + ", " + Title[0] + Title[1];
                    }
                    else if (Title.Substring(0, 4) == "The ")
                    {
                        sortableTitle = Title.Substring(4) + ", " + Title.Substring(0, 3);
                    }
                    else
                        sortableTitle = Title;
                }
                else
                    sortableTitle = Title;
            }
            return sortableTitle;
        }

        public static string ToString(XboxSystem system)
        {
            string generationString = "";

            switch (system)
            {
                case XboxSystem.Xbox:
                    generationString = "XBox";
                    break;
                case XboxSystem.Xbox360:
                    generationString = "Xbox 360";
                    break;
                case XboxSystem.XboxOne:
                    generationString = "Xbox One";
                    break;
                case XboxSystem.XboxSeriesSX:
                    generationString = "Xbox Series S|X";
                    break;
            }
            return generationString;
        }

        public int CompareTo(object obj)
        {
            Game game = obj as Game;
            if (game == null)
            {
                throw new ArgumentException("Object is not a Game");
            }
            return this.GetSortableTitle().CompareTo(game.GetSortableTitle());
            //return this.Title.CompareTo(game.Title);
        }
    }

    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(targetType, value.ToString());
        }
    }

    public enum XboxSystem
    {
        Windows = 0,
        Xbox = 1,
        Xbox360 = 2,
        XboxOne = 3,
        XboxSeriesSX = 4
    }

    public enum Format
    {
        Physical,
        Digital,
        Unknown
    }

    public enum GamePassStatus
    {
        Available,
        NotApplicable,
        Removed,
        Unknown
    }

    public static class ListExtension
    {
        /// <summary>
        /// Sorts the ObservableCollection using a bubble sort approach.
        /// </summary>
        /// <param name="o">The ObservableCollection to be sorted.</param>
        public static void BubbleSort(this IList o)
        {
            bool sorted = true;

            for (int i = o.Count - 1; i >= 0; i--)
            {
                sorted = false;

                for (int j = 1; j <= i; j++)
                {
                    object o1 = o[j - 1];
                    object o2 = o[j];
                    if (((IComparable)o1).CompareTo(o2) > 0)
                    {
                        o.Remove(o1);
                        o.Insert(j, o1);

                        sorted = true;
                    }
                }
            }
        }

        public static int BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0) throw new Exception("Need non-negative number of index.");
            if (count < 0) throw new Exception("Need non-negative number of count.");
            if (source.Count - index < count) throw new Exception("Invalid offset length of count.");
            Contract.Ensures(Contract.Result<int>() <= index + count);
            Contract.EndContractBlock();

            return Array.BinarySearch<T>(source.Cast<T>().ToArray(), index, count, item, comparer);
        }

        public static int BinarySearch<T>(this IList<T> source, T item)
        {
            Contract.Ensures(Contract.Result<int>() <= source.Count);
            return BinarySearch(source, 0, source.Count, item, null);
        }

        public static int BinarySearch<T>(this IList<T> source, T item, IComparer<T> comparer)
        {
            Contract.Ensures(Contract.Result<int>() <= source.Count);
            return BinarySearch(source, 0, source.Count, item, comparer);
        }
    }
}
