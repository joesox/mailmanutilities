// MailmanUtilities v1.2.0.0
// by Joe Socoloski
// Copyright 2011. All Rights Reserved
// To Do: 
// Limits: 
/////////////////////////////////////////////////////////
//LICENSE 
//BY DOWNLOADING AND USING, YOU AGREE TO THE FOLLOWING TERMS: 
//If it is your intent to use this software for non-commercial purposes,  
//such as in academic research, this software is free and is covered under  
//the GNU GPL License, given here: <http://www.gnu.org/licenses/gpl.txt>  
//You agree with 3RDPARTY's Terms Of Service 
//given here: <http://3RDPARTY.com> 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MailmanUtilities.WordNet;
using System.Globalization;

namespace MailmanUtilities
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CreateLogFile()
        {
            //Create log file if not there
            if (!File.Exists(Common.gLogFile))
            {
                FileStream fs = File.Create(Common.gLogFile);
                fs.Close();
            }
        }

        private void Log(string logline)
        {
            File.AppendAllText(Common.gLogFile, DateTime.Now.ToString() + ", " + logline + "\r\n");
        }

        IniFile ini = new IniFile();
        EmailList _EmailList = new EmailList();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Update the Title bar text
            this.Text = Application.ProductName + " " + Application.ProductVersion;

            ReadSettings();
            CreateLogFile();
        }

        #region Settings Read and Save
        /// <summary>
        /// Load the info in the .ini and populate the related Executable listbox
        /// </summary>
        private void InitINI()
        {
            try
            {
                //ini.GetSection("SharpNLP").GetKey("mModelPath").Value
                if (ini.Sections.Count > 0)
                {
                    foreach (IniFile.IniSection section in ini.Sections)
                    {
                        //Add each section name (which should part of the list's url)
                        if(section.Name.ToLower() != "wordnet")
                            cboxMailManLists.Items.Add(section.Name);
                    }
                    cboxMailManLists.Enabled = true;
                    cboxMailManLists.SelectedIndex = 0;
                    Log("InitINI(): Loaded ini settings.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReadSettings()
        {
            //Read the Config file and display in the textboxes
            if (Properties.Settings.Default.AppPath == "")
            {
                tBoxAppPath.Text = Application.StartupPath;
            }
            else
                tBoxAppPath.Text = Properties.Settings.Default.AppPath;

            //read ini settings
            // Load the first ini into the object
            string configFile = Application.StartupPath + "\\config.ini";
            if (File.Exists(configFile))
                ini.Load(Application.StartupPath + "\\config.ini");
            else
            {
                FileStream fs = File.Create(configFile);
                fs.Close();
            }

            //Load the info in the .ini and populate the related controls
            this.InitINI();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.AppPath = tBoxAppPath.Text;

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion Settings Read and Save

        #region MenuStrip Items
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Common.gLogFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.Show();
        }
        
        private void configiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Common.gConfigIni);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion MenuStrip Items

        #region Events
        private void cboxMailManLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tbOutput.Clear();
                string cur = cboxMailManLists.Items[cboxMailManLists.SelectedIndex].ToString();
                IniFile.IniSection section = ini.GetSection(cur);
                foreach (IniFile.IniSection.IniKey iniKey in section.Keys)
                {
                    tbOutput.AppendText(iniKey.Name + " = " + iniKey.GetValue() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion Events

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog ofd = new FolderBrowserDialog();
                ofd.ShowNewFolderButton = true;
                ofd.Description = "Browse to Folder to Clean-up";
                string archives_local = ini.GetSection(cboxMailManLists.Items[cboxMailManLists.SelectedIndex].ToString()).GetKey("archives_local").GetValue();
                if (Directory.Exists(archives_local))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ofd.SelectedPath = archives_local;
                    //ofd.RootFolder = Environment.SpecialFolder.DesktopDirectory;
                    //ofd.ShowDialog();

                    if (ofd.SelectedPath != string.Empty)
                    {
                        _EmailList.Clear();
                        string[] DaysOfWeek = new string[] { "Sun ","Mon ", "Tue ", "Wed ", "Thu ", "Fri ", "Sat " };
                        //Get all the Files in the Directory
                        string[] FolderPath = Directory.GetFiles(ofd.SelectedPath);
                        foreach (string file in FolderPath)
                        {
                            //Examine the file,
                            System.IO.FileInfo theFileInfo = new System.IO.FileInfo(file);

                            List<String> list = new List<String>();
                            list.AddRange(File.ReadAllLines(theFileInfo.FullName));

                            bool ReadingEmailBody = false;
                            Email email = new Email();
                            int index = 0;
                            foreach (string line in list)
                            {
                                if (line.StartsWith("From: "))
                                {
                                    if (list[index - 1].StartsWith("From "))
                                    {
                                        //this is the start of an email
                                        email.From = Email.GetFromEmail(line, true);

                                        //Let's get the date since we know the line it is in
                                        int iDay = 0;
                                        foreach (string day in DaysOfWeek)
                                        {
                                            if(list[index - 1].IndexOf(day) != -1)
                                            {
                                                iDay = list[index - 1].IndexOf(day);
                                                break;
                                            }
                                        }
                                        string[] seperators = new string[] { " ", "(", ")" };
                                        string[] betterdate = list[index - 1].Substring(iDay, list[index - 1].Length - iDay).Trim().Split(seperators, StringSplitOptions.RemoveEmptyEntries);
                                        //should be four parts
                                        if (betterdate.Length == 5)
                                        {
                                            //Looking at this line does not provide any GMT or CDT time stamps.
                                            //CultureInfo enUS = new CultureInfo("en-US");
                                            //email.DateTime = System.DateTime.ParseExact(betterdate, "ddd MMM  d hh:mm:ss yyyy", enUS);//Mailman format: Sun Aug  1 00:02:46 2010
                                            //Reformat to: Thu, 01 May 2008 07:34:42
                                            string parsestring = betterdate[0] + ", " + betterdate[2] + " " + betterdate[1] + " " + betterdate[4] + " " + betterdate[3];
                                            System.DateTime dt = new DateTime();
                                            if (System.DateTime.TryParse(parsestring, out dt))
                                                email.DateTime = dt;
                                            else
                                                MessageBox.Show("Could not create DateTime object for email:" + Environment.NewLine + Environment.NewLine + list[index + 2] + Environment.NewLine + parsestring + Environment.NewLine + Environment.NewLine + "Check the archive formatting", "System.DateTime.TryParse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else if (line.StartsWith("Date: "))
                                {
                                    email.DateTimeStamp = line.Replace("Date: ", "").Trim().Replace("(", "").Replace(")", "");
                                }
                                else if (line.StartsWith("Subject: "))
                                {
                                    email.Subject = line.Replace("Subject: ", "").Trim();
                                }
                                else if (line.StartsWith("Cc: "))
                                {
                                    email.Cc = line.Replace(" at ", "@").Replace("Cc: ", "").Trim();
                                }
                                else if (line.StartsWith("Message-ID: "))
                                {
                                    //the next line is the message Body
                                    ReadingEmailBody = true;
                                }
                                else if (ReadingEmailBody)
                                {
                                    if (line.StartsWith("From "))
                                    {
                                        //we are in a new email...bailout
                                        ReadingEmailBody = false;
                                        _EmailList.Add(email.Clone());
                                        email.Clear();
                                    }
                                    else
                                        email.Body += line + Environment.NewLine;
                                }
                                index++;
                            }
                        }
                    }
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);
                    tbOutput.AppendText("TOTAL EMAILS:  " + Convert.ToString(_EmailList.Count) + Environment.NewLine);
                    tbOutput.AppendText("FROM: " + _EmailList.GetDateSpan() + Environment.NewLine);
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);

                    //find subject test
                    List<Email> thread = new List<Email>();
                    thread = _EmailList.GetThread("wikileaks unreachable");
                    foreach (Email threademail in thread)
                    {
                        tbOutput.AppendText(threademail.Subject + " [" + threademail.DateTimeStamp + "]" + Environment.NewLine);
                    }
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);

                    OrderedList Ord = new OrderedList();
                    //Get TopEmailers
                    Ord = _EmailList.GetTopEmailers(false);
                    tbOutput.AppendText("**************************************" + Environment.NewLine);
                    tbOutput.AppendText("** GetTopEmailers(case-insensitive) **" + Environment.NewLine);
                    tbOutput.AppendText("**************************************" + Environment.NewLine);
                    int y = 0;
                    foreach (String item in Ord.Keys)
                    {
                        tbOutput.AppendText("[" + Convert.ToString(Ord.Values[y]) + "] " + item + "" + Environment.NewLine);
                        y++;
                    }
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);

                    //Get PopularSubjectWords
                    Ord = _EmailList.GetPopularSubjectWords(true);
                    tbOutput.AppendText("****************************" + Environment.NewLine);
                    tbOutput.AppendText("**   PopularSubjectWords  **" + Environment.NewLine);
                    tbOutput.AppendText("****************************" + Environment.NewLine);
                    int i = 0;
                    foreach (String item in Ord.Keys)
                    {
                        tbOutput.AppendText("[" + Convert.ToString(Ord.Values[i]) + "] " + item + "" + Environment.NewLine);
                        i++;
                        if (i == 30)
                            break;
                    }
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);

                    //GetPopularSubjectNouns if WordNet Patch is valid
                    if (Directory.Exists(ini.GetSection("WordNet").GetKey("WordNetPath").Value))
                    {
                        Ord = _EmailList.GetPopularSubjectNouns(ini.GetSection("WordNet").GetKey("WordNetPath").Value, true);
                        tbOutput.AppendText("****************************" + Environment.NewLine);
                        tbOutput.AppendText("** GetPopularSubjectNouns **" + Environment.NewLine);
                        tbOutput.AppendText("****************************" + Environment.NewLine);
                        int x = 0;
                        foreach (String item in Ord.Keys)
                        {
                            tbOutput.AppendText("[" + Convert.ToString(Ord.Values[x]) + "] " + item + "" + Environment.NewLine);
                            x++;
                            if (x == 30)
                                break;
                        }
                        tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);
                    }
                    else
                        MessageBox.Show("WordNet Path is invalid; skipping advanced features. If installed, correct 'WordNetPath =' value in '[WordNet]' of Config.ini" + Environment.NewLine + ini.GetSection("WordNet").GetKey("WordNetPath").Value);

                    //Get all subjects test
                    List<String> subjectlist = new List<String>();
                    subjectlist = _EmailList.GetAllSubjects();
                    subjectlist.Sort();//looks better when displayed
                    tbOutput.AppendText("****************************" + Environment.NewLine);
                    tbOutput.AppendText("**     GetAllSubjects     **" + Environment.NewLine);
                    tbOutput.AppendText("****************************" + Environment.NewLine);
                    foreach (String Subjectline in subjectlist)
                    {
                        tbOutput.AppendText(Subjectline + Environment.NewLine);
                    }
                    tbOutput.AppendText("--------------------------------------------------------------" + Environment.NewLine);
                    //

                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    MessageBox.Show(ini.GetSection(cboxMailManLists.Items[cboxMailManLists.SelectedIndex].ToString()).GetKey("archives_local").GetValue() + " does not exist. Please fix your ini file in section 'archives_local'");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}