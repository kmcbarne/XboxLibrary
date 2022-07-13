using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;

namespace XboxLibrary
{
    public static class Extensions
    {
        public static int CountFieldType(this ObservableCollection<Game> collection, XboxSystem system)
        {
            ObservableCollection<Game> Games = (ObservableCollection<Game>)collection;
            int counted = 0;

            foreach (Game game in Games)
                if (game.ConsoleGeneration == system)
                    counted++;

            return counted;
        }

        public static Color FromHex(this ref Color color, string hexCode)
        {
            if (hexCode.IndexOf("#") != -1)
                hexCode = hexCode.Replace("#", "");

            byte r, g, b = 0;
            r = byte.Parse(hexCode.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            g = byte.Parse(hexCode.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            b = byte.Parse(hexCode.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier);

            color = Color.FromArgb(1, r, g, b);

            return color;
        }
    }
}
