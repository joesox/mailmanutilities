using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailmanUtilities.WordNet;
using System.IO;
//USAGE EXAMPLE FOR GetPopularSubjectWords():
//
//OrderedList Ord = new OrderedList();
//Ord = _EmailList.GetPopularSubjectWords();
//int i = 0;
//foreach (String item in Ord.Keys)
//{
//    tbOutput.AppendText("[" + Convert.ToString(Ord.Values[i]) + "] " + item + "" + Environment.NewLine);
//    i++;
//    if (i == 30)
//        break;
//}

namespace MailmanUtilities
{
    class EmailList : List<Email>
    {
        /// <summary>
        /// Return all Emails in a given subject thread
        /// </summary>
        /// <param name="subjectline">Case-sensitive subject</param>
        /// <returns>a List of Email objects</returns>
        public List<Email> GetThread(String subjectline)
        {
            List<Email> threadlist = new List<Email>();

            foreach (Email email in this)
            {
                if (email.Subject == subjectline)
                {
                    threadlist.Add(email);
                }
            }
            return threadlist;
        }

        /// <summary>
        /// Returns all the unique subject strings
        /// </summary>
        /// <returns>a List of unique subject strings</returns>
        public List<String> GetAllSubjects()
        {
            List<String> subjectlist = new List<String>();

            foreach (Email email in this)
            {
                if (!subjectlist.Contains(email.Subject))
                {
                    subjectlist.Add(email.Subject);
                }
            }
            return subjectlist;
        }

        /// <summary>
        /// Count all the words from the subjects and rank them
        /// (case-sensitive)
        /// </summary>
        /// <returns>OrderedList</returns>
        public OrderedList GetPopularSubjectWords(bool noCommon100)
        {
            OrderedList MyOrderedList = new OrderedList();
            List<string> wordList = new List<string>();
            OrderedDictionary od = new OrderedDictionary();
            System.Collections.SortedList sl = new System.Collections.SortedList();
            foreach (String subjectline in this.GetAllSubjects())
            {
                wordList.AddRange(subjectline.Split(' '));
            }

            foreach (String word in wordList)
            {
                if (od.Contains(word))
                {
                    MyOrderedList.Add(new KeyValuePair<string, int>(word, Convert.ToInt16(od[word]) + 1));
                    //This word is already in dictionary, increase count value
                    od[word] = Convert.ToInt16(od[word]) + 1;
                }
                else
                {
                    //first time word
                    if (noCommon100)
                    {
                        //no common 100 words
                        if (!GrammarUtils.CommonWords100.Contains(word))
                        {
                            MyOrderedList.Add(new KeyValuePair<string, int>(word, 1));
                            od[word] = 1;
                        }
                    }
                    else
                    {
                        //noCommon100 = false
                        MyOrderedList.Add(new KeyValuePair<string, int>(word, 1));
                        od[word] = 1;
                    }
                }
            }

            MyOrderedList.Sort(false);
            return MyOrderedList;
        }

        /// <summary>
        /// Count all the words (Nouns only) from the subjects and rank them
        /// (case-sensitive)
        /// Must have WordNet installed and configured in Config.ini
        /// </summary>
        /// <param name="wordnet_path"></param>
        /// <param name="noCommon100"></param>
        /// <returns></returns>
        public OrderedList GetPopularSubjectNouns(String wordnet_path, bool noCommon100)
        {
            OrderedList MyOrderedList = new OrderedList();
            if (Directory.Exists(wordnet_path))
            {
                Wnlib.WNCommon.path = wordnet_path;
                if (GrammarUtils.AdjDataList.Count == 0)
                    GrammarUtils.LoadDataFiles();
                List<string> wordList = new List<string>();
                OrderedDictionary od = new OrderedDictionary();
                System.Collections.SortedList sl = new System.Collections.SortedList();
                //IsANoun CHECK
                foreach (String subjectline in this.GetAllSubjects())
                {
                    foreach (string subjword in subjectline.Split(' '))
                    {
                        if (GrammarUtils.IsNoun(subjword))
                        {
                            if (noCommon100)
                            {
                                //no common 100 words
                                if (!GrammarUtils.CommonWords100.Contains(subjword))
                                    wordList.Add(subjword);
                            }
                            else
                                wordList.Add(subjword); //noCommon100 = false
                        }
                    }
                }

                foreach (String word in wordList)
                {
                    if (od.Contains(word))
                    {
                        MyOrderedList.Add(new KeyValuePair<string, int>(word, Convert.ToInt16(od[word]) + 1));
                        //This word is already in dictionary, increase count value
                        od[word] = Convert.ToInt16(od[word]) + 1;
                    }
                    else
                    {
                        //first time word
                        MyOrderedList.Add(new KeyValuePair<string, int>(word, 1));
                        od[word] = 1;
                    }
                }

                MyOrderedList.Sort(false);
            }
            else
            {
                //no WordNet path
                Exception ex = new Exception("Bad WordNet Path: " + wordnet_path);
                File.AppendAllText(Common.gLogFile, DateTime.Now.ToString() + ", " + ex.Message + "\r\n");//Log it!
                Console.WriteLine(ex.Message);
            }
            return MyOrderedList;
        }

        /// <summary>
        /// GetTopEmailers
        /// </summary>
        /// <param name="max">maximum number in the list to return</param>
        /// <returns>OrderedList of emails</returns>
        public OrderedList GetTopEmailers(bool bCasesensitive)
        {
            OrderedList MyOrderedList = new OrderedList();
            List<string> emailerList = new List<string>();
            OrderedDictionary od = new OrderedDictionary();
            if (bCasesensitive)
            {
                foreach (Email email in this)
                {
                    if (od.Contains(email.From))
                    {
                        MyOrderedList.Add(new KeyValuePair<string, int>(email.From, Convert.ToInt16(od[email.From]) + 1));
                        //This word is already in dictionary, increase count value
                        od[email.From] = Convert.ToInt16(od[email.From]) + 1;
                    }
                    else
                    {
                        //first time word
                        MyOrderedList.Add(new KeyValuePair<string, int>(email.From, 1));
                        od[email.From] = 1;
                    }
                }
            }
            else
            {
                //all lowercase
                foreach (Email email in this)
                {
                    if (od.Contains(email.From.ToLower()))
                    {
                        MyOrderedList.Add(new KeyValuePair<string, int>(email.From.ToLower(), Convert.ToInt16(od[email.From.ToLower()]) + 1));
                        //This word is already in dictionary, increase count value
                        od[email.From.ToLower()] = Convert.ToInt16(od[email.From.ToLower()]) + 1;
                    }
                    else
                    {
                        //first time word
                        MyOrderedList.Add(new KeyValuePair<string, int>(email.From.ToLower(), 1));
                        od[email.From.ToLower()] = 1;
                    }
                }
            }

            MyOrderedList.Sort(false);
            return MyOrderedList;
        }

        /// <summary>
        /// Get the earliest and latest tamestamps from all the emails in this list
        /// </summary>
        /// <returns>string </returns>
        public String GetDateSpan()
        {
            String daterange = "unknown date range";
            DateTime EarliestDate = new DateTime();
            DateTime LatestestDate = new DateTime();
            int result = 0;
            foreach (Email email in this)
            {
                string s = EarliestDate.Date.ToShortDateString();
                if (EarliestDate.Date.ToShortDateString() == "1/1/0001")
                    EarliestDate = email.DateTime;
                result = DateTime.Compare(email.DateTime, EarliestDate);
                if (result < 0)
                {
                    //"is earlier than";
                    EarliestDate = email.DateTime;
                }
                else if (result == 0)
                {
                }
                else
                {
                    if (email.DateTime.Month == 7)
                        Console.WriteLine("DEBUG");
                    if (DateTime.Compare(email.DateTime, LatestestDate) == 1)
                    {
                        //"is later than";
                        LatestestDate = email.DateTime;
                    }
                }
                daterange = EarliestDate.ToShortDateString() + " " + EarliestDate.ToShortTimeString() + " to " + LatestestDate.ToShortDateString() + " " + LatestestDate.ToShortTimeString();
            }
            return daterange;
        }

        //Find all Emails from an email address

        //GetPopularSubjectThread

        

        //
    }
}
