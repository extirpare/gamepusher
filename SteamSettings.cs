using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameUploader
{
	public class SteamSettings : INotifyPropertyChanged
	{
        public static string SavePath { get { return Path.Combine(MetaSettings.ParentFolderPath, "steam-settings.xml"); } }

        //
        // REQUIRED SETTINGS READ/WRITE
        //

        private string m_pathToExe = "";
        public string PathToExe
        {
            get { return m_pathToExe; }
            set { m_pathToExe = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_accountName = "";
        public string AccountName
        {
            get { return m_accountName; }
            set { m_accountName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_accountPassword = "";
        public string AccountPassword
        {
            get { return m_accountPassword; }
            set { m_accountPassword = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_appID = "";
        public string AppID
        {
            get { return m_appID; }
            set { m_appID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_buildDescription = "";
        public string BuildDescription
        {
            get { return m_buildDescription; }
            set { m_buildDescription = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_depotID = "";
        public string DepotID
        {
            get { return m_depotID; }
            set { m_depotID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_buildPath = "";
        public string BuildPath
        {
            get { return m_buildPath; }
            set { m_buildPath = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // READ-ONLY SETTINGS
        //

        public bool IsValid { get { return CalcError() == eErrorCode.Success; } }
        public bool IsInvalid { get { return !IsValid; } }
        public string ErrorStr { get { return Err2Str(CalcError()); } }


        //
        // EVERYTHING ELSE
        //

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            string passwordCopy = m_accountPassword;
            if (!MetaSettings.Instance.SavePasswords)
                m_accountPassword = "";
            
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            using (StreamWriter sw = File.CreateText(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(SteamSettings));
                xmls.Serialize(sw, this);
            }

            m_accountPassword = passwordCopy;
        }

        public static SteamSettings Load()
        {
            if (!File.Exists(SavePath)) return new SteamSettings();
            using (StreamReader sw = new StreamReader(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(SteamSettings));
                return xmls.Deserialize(sw) as SteamSettings;
            }
        }
        
        public enum eErrorCode
        {
            Success,
            NoExe,
            NoAccountName,
            NoAccountPassword,
            NoAppID,
            NoBuildDescription,
            NoDepotID,
            NoBuildPath,
        }

        public eErrorCode CalcError()
        {
            if (string.IsNullOrWhiteSpace(PathToExe))
                return eErrorCode.NoExe;
            if (string.IsNullOrWhiteSpace(AccountName))
                return eErrorCode.NoAccountName;
            if (string.IsNullOrWhiteSpace(AccountPassword))
                return eErrorCode.NoAccountPassword;
            if (string.IsNullOrWhiteSpace(AppID))
                return eErrorCode.NoAppID;
            if (string.IsNullOrWhiteSpace(BuildDescription))
                return eErrorCode.NoBuildDescription;
            if (string.IsNullOrWhiteSpace(DepotID))
                return eErrorCode.NoDepotID;
            if (string.IsNullOrWhiteSpace(BuildPath))
                return eErrorCode.NoBuildPath;

            return eErrorCode.Success;
        }

        public string Err2Str(eErrorCode errorCode)
        {
            switch (errorCode)
            {
                case eErrorCode.Success:
                    return "No error";
                case eErrorCode.NoExe:
                    return "Invalid path to steamcmd.exe";
                case eErrorCode.NoAccountName:
                    return "Invalid Account Name";
                case eErrorCode.NoAccountPassword:
                    return "Invalid Account Password";
                case eErrorCode.NoAppID:
                    return "Invalid AppID";
                case eErrorCode.NoBuildDescription:
                    return "Invalid Build Description";
                case eErrorCode.NoDepotID:
                    return "Invalid Depot ID";
                case eErrorCode.NoBuildPath:
                    return "Invalid Build Path";
                default:
                    return "Unknown Error";
            }
        }
    }
}