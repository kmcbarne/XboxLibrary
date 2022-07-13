using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XboxLibrary
{
    public class EnumItemsSource<T> where T : struct, IConvertible
    {
        public string FullTypeString { get; set; }
        public string Name { get; set; }
        public T Value { get; set; }

        public EnumItemsSource(string name, T value, string fullTypeString)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("EnumItemsSource only accepts Enum type.");

            Name = name;
            Value = value;
            FullTypeString = fullTypeString;
        }

        public static List<EnumItemsSource<T>> ToList()
        {
            var namesList = Enum.GetNames(typeof(T));
            var valuesList = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            var enumItemsSourceList = new List<EnumItemsSource<T>>();
            for (int i = 0; i < namesList.Length; i++)
                enumItemsSourceList.Add(new EnumItemsSource<T>(namesList[i], valuesList[i], $"{typeof(T).Name}.{namesList[i]}"));

            return enumItemsSourceList;
        }
    }
}
