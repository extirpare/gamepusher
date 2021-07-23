using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameUploader
{
    public class ItchSettings : INotifyPropertyChanged
    {
        public static string ButlerCredsPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/itch", "butler_creds"); } }
        public static string SavePath { get { return Path.Combine(MetaSettings.ParentFolderPath, "itch-settings.xml"); } }

        //
        // REQUIRED SETTINGS READ/WRITE
        //

        private string m_pathToExe = "";
        public string PathToExe
        {
            get { return m_pathToExe; }
            set { m_pathToExe = value; PushPropertyChanges(); }
        }

        private string m_pathToUpload = "";
        public string PathToUpload
        {
            get { return m_pathToUpload; }
            set { m_pathToUpload = value; PushPropertyChanges(); }
        }

        private string m_projectUsername = "";
        public string ProjectUsername
        {
            get { return m_projectUsername; }
            set { m_projectUsername  = value; PushPropertyChanges(); }
        }

        private string m_projectName = "";
        public string ProjectName
        {
            get { return m_projectName; }
            set { m_projectName = value; PushPropertyChanges(); }
        }

        private string m_channelName = "";
        public string ChannelName
        {
            get { return m_channelName; }
            set { m_channelName = value; PushPropertyChanges(); }
        }


        //
        // OPTIONAL SETTINGS READ/WRITE + WANTED
        //

        private bool m_wantsOverrideVersionNum;
        private string m_overrideVersionNum = "";
        public bool WantsOverrideVersionNum
        {
            get { return m_wantsOverrideVersionNum; }
            set { m_wantsOverrideVersionNum = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string OverrideVersionNum
        {
            get { return m_overrideVersionNum; }
            set { m_overrideVersionNum = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsIgnorePattern;
        private string m_ignorePattern = "";
        public bool WantsIgnorePattern
        {
            get { return m_wantsIgnorePattern; }
            set { m_wantsIgnorePattern = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string IgnorePattern
        {
            get { return m_ignorePattern; }
            set { m_ignorePattern = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // READ-ONLY SETTINGS
        //

        public bool HasButlerCreds { get { return File.Exists(ButlerCredsPath); } }
        public bool HasNoButlerCreds { get { return !HasButlerCreds; } }
        public string ErrorStr { get { return Err2Str(CalcError()); } }

        public bool IsValid { get { return CalcError() == eErrorCode.Success; } }
        public bool IsInvalid { get { return !IsValid; } }
        public bool ShowCopyCommandButton { get { return IsValid && MetaSettings.Instance.ShowCopyCommandButton; } }

        public string OptionalSettingsCountStr
        {
            get
            {
                int count = 0;
                if (WantsOverrideVersionNum) ++count;
                if (WantsIgnorePattern) ++count;
                if (count == 1)
                    return "1 Optional Setting Applied";
                else
                    return count.ToString() + " Optional Settings Applied";
            }
        }

        //
        // EVERYTHING ELSE
        //

        public event PropertyChangedEventHandler PropertyChanged;
        public void PushPropertyChanges() { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }

        public enum eErrorCode
        {
            Success,
            NoExe,
            NoLoginCreds,
            NoUploadPath,
            NoProjectUsername,
            NoProjectName,
            NoChannelName,
        }

        public eErrorCode CalcError()
        {
            if (string.IsNullOrWhiteSpace(PathToExe))
                return eErrorCode.NoExe;
            if (HasNoButlerCreds)
                return eErrorCode.NoLoginCreds;
            if (string.IsNullOrWhiteSpace(PathToUpload))
                return eErrorCode.NoUploadPath;
            if (string.IsNullOrWhiteSpace(ProjectUsername))
                return eErrorCode.NoProjectUsername;
            if (string.IsNullOrWhiteSpace(ProjectName))
                return eErrorCode.NoProjectName;
            if (string.IsNullOrWhiteSpace(ChannelName))
                return eErrorCode.NoChannelName;

            return eErrorCode.Success;
        }

        public string Err2Str(eErrorCode errorCode)
        {
            switch (errorCode)
            {
                case eErrorCode.Success:
                    return "No error";
                case eErrorCode.NoExe:
                    return "Invalid path to butler.exe";
                case eErrorCode.NoLoginCreds:
                    return "No login credentials";
                case eErrorCode.NoUploadPath:
                    return "Invalid Upload Path";
                case eErrorCode.NoProjectUsername:
                    return "Invalid Project Owner Username";
                case eErrorCode.NoProjectName:
                    return "Invalid Project Name";
                case eErrorCode.NoChannelName:
                    return "Invalid Channel Name";
                default:
                    return "Unknown Error";
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            using (StreamWriter sw = File.CreateText(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(ItchSettings));
                xmls.Serialize(sw, this);
            }
        }

        public static ItchSettings Load()
        {
            if (!File.Exists(SavePath)) return new ItchSettings();
            using (StreamReader sw = new StreamReader(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(ItchSettings));
                return xmls.Deserialize(sw) as ItchSettings;
            }
        }
    }
}