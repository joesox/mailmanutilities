/////////////////////////////////////////////
// MailmanUtilities License                //
// Copyright (C) 2011 JPSIII               //
// http://mailmanutilities.googlecode.com  //
/////////////////////////////////////////////

================
= HOW TO SETUP =
================
!!INSTALLATION PREREQUISITES:
* Microsoft .NET 4.0 (48.1MB)
http://www.microsoft.com/downloads/en/details.aspx?FamilyID=0a391abd-25c1-4fc0-919f-b21f31ab88b7

* OPTIONAL: WordNet 2.1 (for advanced features)
http://wordnet.princeton.edu/wordnet/download/
---

1) Create a folder on your PC at this location 
'C:\mylists\archives'

2) Download and extract your list's archives in its own folder.
For example, for the 
North American Network Operators' Group (NANOG) email list
create the subfolder:
'C:\mylists\archives\nanog' and place the extracted txt files
there

3) After installing and running MailmanUtilities, 
Click 'View' -> 'Config.ini' on the menubar

4) Each of your lists will have its own section.
Name the section exactly like its url name.
For Example, for the 
North American Network Operators' Group (NANOG) email list
http://mailman.nanog.org/pipermail/nanog/
create section:
[nanog]
archives = http://mailman.nanog.org/pipermail/nanog/
archives_local = C:\mylists\archives\nanog

5) Save config.ini (most time you can press CTRL+S)

6) Exit and restart MailmanUtilities and you should
see you lists in the dropdownbox.

===================
= SECTION OPTIONS =
===================
Here are the all the Keys available for a section:
[mylistname]
listinfo = http://lists.mylistwebsite.com/mailman/listinfo/mylistname/
options = http://lists.mylistwebsite.com/mailman/options/mylistname/myemail%40address.com
archives = http://lists.mylistwebsite.com/mailman/private/mylistname/
archives_local = C:\mylists\archives\mylistname

