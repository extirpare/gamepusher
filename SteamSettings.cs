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
        public static string DefaultPath
        { get {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameUploader", "steam-settings.xml");
        } }

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

        public void Save(string filename)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (StreamWriter sw = File.CreateText(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(SteamSettings));
                xmls.Serialize(sw, this);
            }
        }

        public static SteamSettings Load(string filename)
        {
            if (!File.Exists(filename))
                return new SteamSettings();

            using (StreamReader sw = new StreamReader(filename))
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
        }

        public eErrorCode CalcError()
        {
            if (string.IsNullOrWhiteSpace(PathToExe))
                return eErrorCode.NoExe;
            if (string.IsNullOrWhiteSpace(AccountName))
                return eErrorCode.NoAccountName;

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
                default:
                    return "Unknown Error";
            }
        }
    }
}