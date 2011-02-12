using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailmanUtilities
{
    class OrderedList
    {
        public List<String> Keys = new List<String>();
        public List<int> Values = new List<int>();

        /// <summary>
        /// Removes all the elements from Keys and Values
        /// </summary>
        public void Clear()
        {
            Keys.Clear();
            Values.Clear();
        }

        /// <summary>
        /// If new adds KeyValuePair, if exists it increments its count
        /// </summary>
        /// <param name="keyvaluepair">String, int</param>
        public void Add(KeyValuePair<String, int> keyvaluepair)
        {
            //Was this word/key previously found?
            if (Keys.Contains(keyvaluepair.Key))
            {
                //then we need to add one to its value
                int index = Array.IndexOf(Keys.ToArray(), keyvaluepair.Key);
                Values[index] = Values[index] + 1;
            }
            else
            {
                Keys.Add(keyvaluepair.Key);
                Values.Add(1);
            }
        }

        /// <summary>
        /// Sort the Keys and Values
        /// </summary>
        /// <param name="ascending">If true sorts from Lowest to Highest</param>
        public void Sort(bool ascending)
        {
            //"-"=37
            string[] keys = new string[Keys.Count];
            Keys.CopyTo(keys, 0);
            int[] values = new int[Values.Count];
            Values.CopyTo(values, 0);
            Array.Sort(values, keys);

            Values.Clear();
            Keys.Clear();
            if (!ascending)
            {
                //provide Highest to Lowest
                Values.AddRange(values.Reverse<int>());
                Keys.AddRange(keys.Reverse<String>());
            }
            else
            {
                //Lowest to Highest
                Values.AddRange(values);
                Keys.AddRange(keys);
            }
        }
    
    }
}
