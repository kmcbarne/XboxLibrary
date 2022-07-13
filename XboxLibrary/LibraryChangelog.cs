using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxLibrary
{
    public class LibraryChangelog
    {
        public string LogPath { get; set; }
        public List<ChangeEvent> Changes { get; set; }

        public LibraryChangelog(string path)
        {
            LogPath = path;
            Changes = new List<ChangeEvent>();
        }

        public void AddEvent(DateTime eventDateTime, EventType eventType, string eventDetail)
        {
            Changes.Add(new ChangeEvent(eventDateTime, eventType, eventDetail));
            WriteToLog();
        }

        public void WriteToLog()
        {
            using (StreamWriter writer = new StreamWriter("ms-appx:///InstanceChangelog.txt"))
            {
                writer.WriteLine(this.ToString());
            }
        }

        public override string ToString()
        {
            string headers = "Timestamp\t\tChangeType\t\tDescription\r\n---------\t\t----------\t\t-----------\r\n";
            string output = "";

            foreach (ChangeEvent change in Changes)
                output += change.ToString();

            return headers + output;
        }
    }

    public class ChangeEvent
    {
        public DateTime ChangeTimeStamp { get; set; } = DateTime.Now;
        public string ChangeDetail { get; set; }
        public EventType ChangeType { get; set; }

        public ChangeEvent()
        {

        }

        public ChangeEvent(EventType changeType, string changeDetail)
        {
            ChangeTimeStamp = DateTime.Now;
            ChangeType = changeType;
            ChangeDetail = changeDetail;
        }

        public ChangeEvent(DateTime changeTimestamp, EventType changeType, string changeDetail)
        {
            ChangeTimeStamp = changeTimestamp;
            ChangeType = changeType;
            ChangeDetail = changeDetail;
        }
        public ChangeEvent(string changeType, string changeDetail)
        {
            ChangeTimeStamp = DateTime.Now;            
            ChangeDetail = changeDetail;

            switch(changeType)
            {
                case "AddGame":
                    ChangeType = EventType.AddGame;
                    break;
                case "RenameGame":
                    ChangeType = EventType.RenameGame;
                    break;
                case "UpdateGame":
                    ChangeType = EventType.UpdateGame;
                    break;
                case "RemoveGame":
                    ChangeType = EventType.RemoveGame;
                    break;
                case "LoadLibrary":
                    ChangeType = EventType.LoadLibrary;
                    break;
            }
        }

        public override string ToString()
        {
            string separator = "\t\t";
            string changeOutput = ChangeTimeStamp.ToFileTime().ToString() + separator + ChangeType.ToString() + separator + ChangeDetail + "\r\n";

            return changeOutput;
        }
    }

    public enum EventType
    {
        AddGame,
        LoadLibrary,
        RemoveGame,
        RenameGame,
        UpdateGame
    }
}
