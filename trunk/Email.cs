using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MailmanUtilities
{
    /// <summary>
    /// Summary description for EmailClass.
    /// </summary>
    /// <summary>
    /// Summary description for Email Class
    /// </summary>
    public class Email
    {
        #region Variables
        /// <summary>
        /// The date and time of email
        /// </summary>
        public string DateTimeStamp
        {
            get { return _DateTimeStamp; }
            set { _DateTimeStamp = value; }
        }
        string _DateTimeStamp = "";

        /// <summary>
        /// The date and time of email in DateTime Object
        /// </summary>
        public DateTime DateTime
        {
            get { return _DateTime; }
            set { _DateTime = value; }
        }
        DateTime _DateTime = new DateTime();

        /// <summary>
        /// A valid email address for the party sending the email
        /// </summary>
        public string From
        {
            get { return _From; }
            set { _From = value; }
        }
        string _From = "";

        /// <summary>
        /// A semi-colon (";") separted list of email recipients
        /// </summary>
        public string To
        {
            get { return _To; }
            set { _To = value; }
        }
        string _To = "";

        /// <summary>
        /// A semi-colon(";") separated list of email courtesy copies
        /// </summary>
        public string Cc
        {
            get { return _Cc; }
            set { _Cc = value; }
        }
        string _Cc = "";

        /// <summary>
        /// The subject of the email
        /// </summary>
        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        string _Subject = "";

        /// <summary>
        /// The message body for the email
        /// </summary>
        public string Body
        {
            get { return _Body; }
            set { _Body = value; }
        }
        string _Body = "";

        #endregion Variables
        public Email()
        { }
        /// <summary>
        /// Constructor for the email object
        /// </summary>
        /// <param name="strEmailFrom"></param>
        /// <param name="strEmailTo"></param>
        /// <param name="strEmailCc"></param>
        /// <param name="strEmailSubject"></param>
        /// <param name="strEmailMessageBody"></param>
        /// <param name="strSmtpServer"></param>
        public Email(string strEmailFrom, string strEmailTo, string strEmailCc,string strDateTimeStamp, System.DateTime dt, string strEmailSubject,
          string strEmailMessageBody)
        {
            _DateTimeStamp = strDateTimeStamp;
            CultureInfo enUS = new CultureInfo("en-US");
            //_DateTime = System.DateTime.ParseExact(strDateTimeStamp, "ddd MMM  d hh:mm:ss yyyy", enUS);//Sun Aug  1 00:02:46 2010
            _DateTime = dt;
            _From = strEmailFrom;
            _To = strEmailTo;
            _Cc = strEmailCc;
            _Subject = strEmailSubject;
            _Body = strEmailMessageBody;
        }

        public void Clear()
        {
            _DateTimeStamp = "";
            _DateTime = new System.DateTime();
            _From = "";
            _To = "";
            _Cc = "";
            _Subject = "";
            _Body = "";
        }

        public Email Clone()
        {
            return new Email(_From, _To, _Cc, _DateTimeStamp, _DateTime, _Subject, _Body);
        }

        public static String GetFromEmail(String line, bool emailonly)
        {
            //eg "From: billbob at hotmail.com (Bill B)"
            String emailaddress = "";
            try
            {
                //Make sure not a false From
                if (!line.Contains(">") || !line.Contains("<") || !line.Contains("&lt;"))
                {
                    if (emailonly)
                    {
                        String workingline = line.Replace(" at ", "@").Replace("From: ", "").Trim();
                        string[] splitline = workingline.Split(' ');
                        int atIndex = 0;
                        foreach (string item in splitline)
                        {
                            if (item.Contains("@"))
                            {
                                    emailaddress = item;
                                    break;
                            }
                            atIndex++;
                        }
                        //We know the atIndex so get front and back
                        //emailaddress = splitline[atIndex - 1] + splitline[atIndex] + splitline[atIndex + 1];
                    }
                    else
                        emailaddress = line.Replace(" at ", "@").Replace("From: ", "").Trim(); //Return full line
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }


            return emailaddress;
        }
    }

}
