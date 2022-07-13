using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace XboxLibrary
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string valueString = value.ToString();

            return SplitCamelCase(valueString);

            //switch (valueString)
            //{
            //    case "Xbox":
            //        return "Xbox";
            //    case "Xbox360":
            //        return "Xbox 360";
            //    case "XboxOne":
            //        return "Xbox One";
            //    case "XboxSeriesSX":
            //        return "Xbox Series S|X";
            //    case "NotApplicable":
            //        return "Not Applicable";
            //    default:
            //        return valueString;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string valueString = value.ToString();

            switch (valueString)
            {
                case "Xbox":
                    return XboxSystem.Xbox;
                case "Xbox 360":
                    return XboxSystem.Xbox360;
                case "Xbox One":
                    return XboxSystem.XboxOne;
                case "Xbox Series S|X":
                    return XboxSystem.XboxSeriesSX;
                case "Windows":
                    return XboxSystem.Windows;
                default:
                    return XboxSystem.Xbox;
            }
        }

        public string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                    ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
                );
        }
    }
}
