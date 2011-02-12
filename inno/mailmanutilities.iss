; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{5AAED156-F144-4782-89B9-F0127A65AE2A}
AppName=MailmanUtilities
AppVersion=1.2
;AppVerName=MailmanUtilities 1.2
AppPublisher=JPSIII
AppPublisherURL=http://mailmanutilities.googlecode.com/
AppSupportURL=http://mailmanutilities.googlecode.com/
AppUpdatesURL=http://mailmanutilities.googlecode.com/
DefaultDirName={pf}\MailmanUtilities
DefaultGroupName=MailmanUtilities
AllowNoIcons=yes
LicenseFile=C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\license.txt
InfoBeforeFile=C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\readme.txt
OutputDir=C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\inno
OutputBaseFilename=MailmanUtilitiessetup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "basque"; MessagesFile: "compiler:Languages\Basque.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "catalan"; MessagesFile: "compiler:Languages\Catalan.isl"
Name: "czech"; MessagesFile: "compiler:Languages\Czech.isl"
Name: "danish"; MessagesFile: "compiler:Languages\Danish.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "finnish"; MessagesFile: "compiler:Languages\Finnish.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hebrew"; MessagesFile: "compiler:Languages\Hebrew.isl"
Name: "hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"
Name: "italian"; MessagesFile: "compiler:Languages\Italian.isl"
Name: "japanese"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "norwegian"; MessagesFile: "compiler:Languages\Norwegian.isl"
Name: "polish"; MessagesFile: "compiler:Languages\Polish.isl"
Name: "portuguese"; MessagesFile: "compiler:Languages\Portuguese.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "slovak"; MessagesFile: "compiler:Languages\Slovak.isl"
Name: "slovenian"; MessagesFile: "compiler:Languages\Slovenian.isl"
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags:
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\bin\Release\MailmanUtilities.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\bin\Release\config.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\bin\Release\config-Default.ini"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\bin\Release\WordNetClasses.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\license.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\readme.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Joe\Documents\Visual Studio 2010\Projects\personal\MailmanUtilities\MailmanUtilities\license-wordnetdotnet.txt"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\MailmanUtilities"; Filename: "{app}\MailmanUtilities.exe"
Name: "{group}\{cm:ProgramOnTheWeb,MailmanUtilities}"; Filename: "http://mailmanutilities.googlecode.com/"
Name: "{group}\{cm:UninstallProgram,MailmanUtilities}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\MailmanUtilities"; Filename: "{app}\MailmanUtilities.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\MailmanUtilities"; Filename: "{app}\MailmanUtilities.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\MailmanUtilities.exe"; Description: "{cm:LaunchProgram,MailmanUtilities}"; Flags: nowait postinstall skipifsilent

