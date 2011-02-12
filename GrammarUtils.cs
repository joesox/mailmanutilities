// GrammarUtils v1.2.0.0
// by Joe Socoloski
// Copyright 2010. All Rights Reserved
// A class that uses http://wordnetdotnet.googlecode.com
// You must configure config.ini with correct local paths before using.
// Limits: 
/////////////////////////////////////////////////////////
//LICENSE
//BY DOWNLOADING AND USING, YOU AGREE TO THE FOLLOWING TERMS:
//If it is your intent to use this software for non-commercial purposes, 
//such as in academic research, this software is free and is covered under 
//the GNU GPL License, given here: <http://www.gnu.org/licenses/gpl.txt> 
//You agree with 3RDPARTY's Terms Of Service 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Wnlib;
using Inflector.Net;

namespace MailmanUtilities.WordNet
{
    class GrammarUtils
    {
        public static List<String> NounDataList = new List<String>();
        public static List<String> VerbDataList = new List<String>();
        public static List<String> AdjDataList = new List<String>();
        public static List<String> AdvDataList = new List<String>();

        /// <summary>
        /// Most Common English Words (100)
        /// http://www.duboislc.org/EducationWatch/First100Words.html
        /// </summary>
        public static List<String> CommonWords100 = new List<String>(new String[] { "the", "of", "and", "a", "to", "in", "is", "you", "that", "it", "he", "was", "for", "on", "are", "as", "with", "his", "they", "I", "at", "be", "this", "have", "from", "or", "one", "had", "by", "word", "but", "not", "what", "all", "were", "we", "when", "your", "can", "said", "there", "use", "an", "each", "which", "she", "do", "how", "their", "if", "will", "up", "other", "about", "out", "many", "then", "them", "these", "so", "some", "her", "would", "make", "like", "him", "into", "time", "has", "look", "two", "more", "write", "go", "see", "number", "no", "way", "could", "people", "my", "than", "first", "water", "been", "call", "who", "oil", "its", "now", "find", "long", "down", "day", "did", "get", "come", "made", "may", "part" });


        /// <summary>
        /// Load the Wnlib.WNDB files into memory:
        /// NounDataList, VerbDataList, AdjDataList, AdvDataList
        /// NOTE: Must assign Wnlib.WNCommon.path before calling.
        /// </summary>
        public static void LoadDataFiles()
        {
            string[] seperators = new string[] { "\n" };
            NounDataList.AddRange(Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Noun)).ReadToEnd().Split(seperators, StringSplitOptions.None));
            Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Noun)).Close();
            Wnlib.WNDB.reopen(Wnlib.PartOfSpeech.of(PartsOfSpeech.Noun));

            VerbDataList.AddRange(Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Verb)).ReadToEnd().Split(seperators, StringSplitOptions.None));
            Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Verb)).Close();
            Wnlib.WNDB.reopen(Wnlib.PartOfSpeech.of(PartsOfSpeech.Verb));

            AdjDataList.AddRange(Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adj)).ReadToEnd().Split(seperators, StringSplitOptions.None));
            Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adj)).Close();
            Wnlib.WNDB.reopen(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adj));

            AdvDataList.AddRange(Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adv)).ReadToEnd().Split(seperators, StringSplitOptions.None));
            Wnlib.WNDB.data(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adv)).Close();
            Wnlib.WNDB.reopen(Wnlib.PartOfSpeech.of(PartsOfSpeech.Adv));
        }

        /// <summary>
        /// Use if sense of word is not known
        /// </summary>
        /// <param name="word">word to lookup</param>
        /// <returns>List of Winlib.Search</returns>
        public static List<Search> GetWordNetInfo(string word)
        {
            List<Search> SearchList = new List<Search>();

            WnLexicon.WordInfo wordi = new WnLexicon.WordInfo();
            wordi.partOfSpeech = Wnlib.PartsOfSpeech.Unknown;

            // for each part of speech...
            Wnlib.PartsOfSpeech[] enums = (Wnlib.PartsOfSpeech[])Enum.GetValues(typeof(Wnlib.PartsOfSpeech));

            wordi.senseCounts = new int[enums.Length];
            for (int i = 0; i < enums.Length; i++)
            {
                // get a valid part of speech
                Wnlib.PartsOfSpeech pos = enums[i];
                if (pos == Wnlib.PartsOfSpeech.Unknown)
                    continue;

                // get an index to a synset collection
                Wnlib.Index index = Wnlib.Index.lookup(word, Wnlib.PartOfSpeech.of(pos));

                // none found?
                if (index == null)
                    continue;

                //Add
                SearchType ser = new SearchType(false, "OVERVIEW");
                Search s = new Search(word, true, Wnlib.PartOfSpeech.of(pos), ser, 0);
                SearchList.Add(s);
                // does this part of speech have a higher sense count?
                //wordi.senseCounts[i] = index.sense_cnt;
                //if (wordi.senseCounts[i] > maxCount)
                //{
                //    maxCount = wordi.senseCounts[i];
                //    wordi.partOfSpeech = pos;
                //}
            }

            return SearchList;
        }

        /// <summary>
        /// Use if PartOfSpeech is known
        /// </summary>
        /// <param name="word">word to lookup</param>
        /// <param name="pos">PartOfSpeech</param>
        /// <returns>Winlib.Search</returns>
        public static Search GetWordNetInfo(string word, PartOfSpeech pos)
        {
            SearchType ser = new SearchType(false, "OVERVIEW");
            Search s = new Search(word, true, pos, ser, 0);
            return s;
        }

        /// <summary>
        /// Get a random word from the data files and desired PartsOfSpeech
        /// NOTE: Must call LoadDataFiles() before calling.
        /// </summary>
        /// <param name="p">Desired PartsOfSpeech.</param>
        /// <returns>Desired PartsOfSpeech word string from WordNet</returns>
        public static String GetRandomWord(Wnlib.PartsOfSpeech p)
        {
            String word = "";
            try
            {
                Random rand = new Random();
                int r = 0;
                string[] synset_raw = new string[] { "" };
                switch (p)
                {
                    case PartsOfSpeech.Adj:
                        r = rand.Next(AdjDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = AdjDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Adv:
                        r = rand.Next(AdvDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = AdvDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Noun:
                        r = rand.Next(NounDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = NounDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Verb:
                        r = rand.Next(VerbDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = VerbDataList[r].Split(' ');
                        break;
                    default:
                        break;
                }

                word = synset_raw[4];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return word;
        }

        /// <summary>
        /// Needs verb to return a subject
        /// </summary>
        /// <param name="chunked"></param>
        /// <param name="completesubj"></param>
        /// <param name="rtn_chunked"></param>
        /// <returns></returns>
        public static String GetSubject(string chunked, bool completesubj, bool rtn_chunked)
        {
            //the simple subject is always a noun or a pronoun.
            string Subject = "";
            // [PP in/IN ] [NP every/DT country/NN ] ,/, [NP the/DT sun/NN ] [VP rises/VBZ ] [PP in/IN ] [NP the/DT morning/NN ] ./.
            string original_tagged = chunked.Trim();
            string[] seperators = new string[] { "["};
            string[] split_tagged = chunked.Trim().Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            List<String> NPorVPList = new List<string>();
            bool bHasVP = false;
            //Find common tags in a complete subject and note if it has a verb
            foreach (String item in split_tagged)
            {
                if (item.StartsWith("NP") || item.StartsWith("VP") || item.StartsWith("PP"))
                {
                    NPorVPList.Add(item);
                    if (item.StartsWith("VP"))
                        bHasVP = true;
                }
            }

            if (bHasVP)
            {
                bool vp_before = false;
                //there is a verb in the sentence
                for (int i = 0; i < NPorVPList.Count; i++)
                {
                    if (NPorVPList[i].StartsWith("VP"))
                    {
                        vp_before = true;
                    }

                    if(NPorVPList[i].StartsWith("NP"))
                    {
                        int iPeek = i + 1;
                        if (vp_before)
                        {
                            //The verb was before this noun so let's count this as the verb
                            Subject = NPorVPList[i];
                            break;
                        }
                        else
                        {
                            //this is noun so is next a verb?, if yes then this is subject
                            if (iPeek != NPorVPList.Count)
                            {
                                if (NPorVPList[iPeek].StartsWith("VP") || NPorVPList[iPeek].StartsWith("PP"))
                                {
                                    Subject = NPorVPList[i];
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            string rtn = "";
            if (rtn_chunked)
                rtn = "[" + Subject.Replace("]", "") + "]";
            else
            {
                //NP the/DT sun/NN ] 
                //do not chunk
                rtn = Subject.Replace("NNS", "xxS").Replace("NNP", "xxP").Replace("NP", "").Replace("xxS", "NNS").Replace("xxP", "NNP").Trim();// the/DT sun/NN ] 
                string[] phrase = rtn.Split(' ');
                if (!completesubj)
                {
                    //do not want complete subject, just return the simple subject [unchunked]
                    foreach (String word in phrase)
                    {
                        if (word.Contains("/NNP"))
                        {
                            rtn = word.Replace("/NNP", "");
                        }
                        else if (word.Contains("/NNS"))
                        {
                            rtn = word.Replace("/NNS", "");
                        }
                        else if (word.Contains("/NN") )
                        {
                            rtn = word.Replace("/NN", "");
                        }
                    }
                }
                else
                {
                    //Want complete subject, return the complete subject [unchunked]
                    rtn = "";
                    foreach (String word in phrase)
                    {
                        if (word.Contains("/"))
                        {
                            int index = word.IndexOf("/");
                            rtn += word.Remove(index, word.Length - index);
                            rtn += " ";
                        }
                    }
                }

            }

            return rtn.Trim();
        }

        /// <summary>
        /// Needs verb to return a subject
        /// </summary>
        /// <param name="chunked"></param>
        /// <param name="completesubj"></param>
        /// <param name="rtn_chunked"></param>
        /// <param name="verb"></param>
        /// <returns></returns>
        public static String GetSubject(string chunked, bool completesubj, bool rtn_chunked, out string verb)
        {
            //the simple subject is always a noun or a pronoun.
            string Subject = "";
            string Verb = "";
            // [PP in/IN ] [NP every/DT country/NN ] ,/, [NP the/DT sun/NN ] [VP rises/VBZ ] [PP in/IN ] [NP the/DT morning/NN ] ./.
            string original_tagged = chunked.Trim();
            string[] seperators = new string[] { "[", "]" };
            string[] split_tagged = chunked.Trim().Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            List<String> NPorVPList = new List<string>();
            bool bHasVP = false;
            //Find common tags in a complete subject and note if it has a verb
            foreach (String item in split_tagged)
            {
                /*
                if (item.StartsWith("NP") || item.StartsWith("VP") || item.StartsWith("PP"))
                {
                    NPorVPList.Add(item);
                    if (item.StartsWith("VP"))
                    {
                        bHasVP = true;
                        Verb = item;//Verb chunked
                    }
                }
                VB - Verb, base form
                VBD - Verb, past tense
                VBG - Verb, gerund or present participle
                VBN - Verb, past participle
                VBP - Verb, non-3rd person singular present
                VBZ - Verb, 3rd person singular present
                */

                if (item.Contains("/VB") || item.Contains("/VBD") || item.Contains("/VBG") || item.Contains("/VBN")
                    || item.Contains("/VBP") || item.Contains("/VBZ"))
                {
                    bHasVP = true;
                    Verb = item;//Verb chunked
                    if (item.StartsWith("NP") || item.StartsWith("VP") || item.StartsWith("PP"))
                    {
                        NPorVPList.Add(item);
                    }
                }

            }

            if (bHasVP)
            {
                bool vp_before = false;
                //there is a verb in the sentence
                for (int i = 0; i < NPorVPList.Count; i++)
                {
                    if (NPorVPList[i].StartsWith("VP"))
                    {
                        vp_before = true;
                    }

                    if (NPorVPList[i].StartsWith("NP"))
                    {
                        int iPeek = i + 1;
                        if (vp_before)
                        {
                            //The verb was before this noun so let's count this as the verb
                            Subject = NPorVPList[i];
                            break;
                        }
                        else
                        {
                            //this is noun so is next a verb?, if yes then this is subject
                            if (iPeek != NPorVPList.Count)
                            {
                                if (NPorVPList[iPeek].StartsWith("VP") || NPorVPList[iPeek].StartsWith("PP"))
                                {
                                    Subject = NPorVPList[i];
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            string rtn = "";
            if (rtn_chunked)
            {
                rtn = "[" + Subject.Replace("]", "") + "]";
                Verb = "[" + Verb.Replace("]", "") + "]";
            }
            else
            {
                //NP the/DT sun/NN ] 
                //do not chunk
                Verb = Verb.Replace("VP", "").Trim();//VERB
                if(Verb.Length > 0)
                    Verb = Verb.Substring(0, Verb.IndexOf('/'));

                rtn = Subject.Replace("NNS", "xxS").Replace("NNP", "xxP").Replace("NP", "").Replace("xxS", "NNS").Replace("xxP", "NNP").Trim();// the/DT sun/NN ] 
                string[] phrase = rtn.Split(' ');
                if (!completesubj)
                {
                    //do not want complete subject, just return the simple subject [unchunked]
                    foreach (String word in phrase)
                    {
                        if (word.Contains("/NNP"))
                        {
                            rtn = word.Replace("/NNP", "");
                        }
                        else if (word.Contains("/NNS"))
                        {
                            rtn = word.Replace("/NNS", "");
                        }
                        else if (word.Contains("/NN"))
                        {
                            rtn = word.Replace("/NN", "");
                        }
                    }
                }
                else
                {
                    //Want complete subject, return the complete subject [unchunked]
                    rtn = "";
                    foreach (String word in phrase)
                    {
                        if (word.Contains("/"))
                        {
                            int index = word.IndexOf("/");
                            rtn += word.Remove(index, word.Length - index);
                            rtn += " ";
                        }
                    }
                }

            }
            verb = Verb;//VERB??
            return rtn.Trim();
        }

        /// <summary>
        /// Get a related word from the data files and desired PartsOfSpeech
        /// NOTE: Must call LoadDataFiles() before calling.
        /// </summary>
        /// <param name="p">Desired PartsOfSpeech.</param>
        /// <returns>Desired PartsOfSpeech word string from WordNet</returns>
        public static String GetRelatedWord(Wnlib.PartsOfSpeech p)
        {
            String word = "";
            try
            {
                Random rand = new Random();
                int r = 0;
                string[] synset_raw = new string[] { "" };
                switch (p)
                {
                    case PartsOfSpeech.Adj:
                        r = rand.Next(AdjDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = AdjDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Adv:
                        r = rand.Next(AdvDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = AdvDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Noun:
                        r = rand.Next(NounDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = NounDataList[r].Split(' ');
                        break;
                    case PartsOfSpeech.Verb:
                        r = rand.Next(VerbDataList.Count - 29);//do not choose the comments in the first lines
                        synset_raw = VerbDataList[r].Split(' ');
                        break;
                    default:
                        break;
                }

                word = synset_raw[4];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return word;
        }

        /// <summary>
        /// Convert a plural word to its singular form
        /// </summary>
        /// <param name="pluralword">Plural word</param>
        /// <returns>Singular form</returns>
        public static String Singularize(String pluralword)
        {
            return Inflector.Net.Inflector.Singularize(pluralword);
        }

        /// <summary>
        /// Convert a singular word to its plural form
        /// </summary>
        /// <param name="singularword">Singular word</param>
        /// <returns>Plural form</returns>
        public static String Pluralize(String singularword)
        {
            return Inflector.Net.Inflector.Pluralize(singularword);
        }

        /// <summary>
        /// Is the word a noun?
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static bool IsNoun(string word)
        {
            bool bIsANoun = false;

            List<Search> searchList = new List<Search>();
            searchList = GetWordNetInfo(word);
            foreach (Search result in searchList)
            {
                foreach (SynSet synset in result.senses)
                {
                    if(synset.pos.name == "noun")
                        bIsANoun = true;
                    else if (synset.pos.name == "adv")
                    {
                        bIsANoun = false;
                        break;//just quit probably not a good noun
                    }
                }
            }

            return bIsANoun;
        }
    }
}
