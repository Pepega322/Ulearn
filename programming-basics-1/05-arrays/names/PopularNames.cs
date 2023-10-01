using System;
using System.Linq;

namespace Names
{
    internal class PopularNames
    {
        public static string[] GetNameArray(NameData[] dataArray)
        {
            string lineOfNames = "";
            foreach (var name in dataArray)
                if (!lineOfNames.Contains(name.Name)) lineOfNames += name.Name + ";";
            var arrayOfNames = lineOfNames.Split(';');
            Array.Sort(arrayOfNames);
            return arrayOfNames;
        }

        public static int[] GetCountedNameArray(NameData[] dataArray)
        {
            var names = GetNameArray(dataArray);
            var countedNames = new int[names.Length];

            for (var nameIndex = 0; nameIndex < names.Length; nameIndex++)
            {
                foreach (var man in dataArray)
                    if (man.Name == names[nameIndex]) countedNames[nameIndex]++;
            }
            return countedNames;
        }

        //public static string[,] GetTopName(string[,] countedNameArray, int topSize)
        //{

        //}
    }
}
