using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MailmanUtilities
{
    class Common
    {
        public static String gConfigIni = Application.StartupPath + "\\config.ini";
        public static String gLogFile = Application.StartupPath + "\\Log.txt";
        public static String sxAdminGuide = Application.StartupPath + "\\AdminGuide.pdf";
        public static String ReadMe = Application.StartupPath + "\\ReadMe.txt";
        public static String InstallReadMe = Application.StartupPath + "\\INSTALLATION README.txt";

        public static String SeperatorArchive = "From "; //this starts the begining of each new message
    }
}
