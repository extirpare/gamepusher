using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameUploader
{
    public class OculusUploaderGUISettings : INotifyPropertyChanged
    {
        public static string DefaultPath
        { get {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "OculusUploaderGUI", "settings.xml");
        } }

        public enum eCredentialsSource { UserToken, AppSecret };
        public enum ePlatform{ Quest, Rift };

        //
        // REQUIRED SETTINGS READ/WRITE
        //

        private string m_pathToOculusExe = "";
        public string PathToOculusExe 
        { 
            get { return m_pathToOculusExe; } 
            set { m_pathToOculusExe = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); } 
        }

        private string m_appID = "";
        public string AppID
        {
            get { return m_appID; }
            set { m_appID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        public ePlatform Platform;
        [XmlIgnore] public bool PlatformIsQuest 
        { 
            get { return Platform == ePlatform.Quest; } 
            set { Platform = value ? ePlatform.Quest : ePlatform.Rift; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        [XmlIgnore] public bool PlatformIsRift 
        { 
            get { return Platform == ePlatform.Rift; } 
            set { Platform = value ? ePlatform.Rift : ePlatform.Quest; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }


        public eCredentialsSource CredentialsSource;
        [XmlIgnore] public bool CredentialsSourceIsUserToken 
        { 
            get { return CredentialsSource == eCredentialsSource.UserToken; } 
            set { CredentialsSource = value ? eCredentialsSource.UserToken : eCredentialsSource.AppSecret; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        [XmlIgnore] public bool CredentialsSourceIsAppSecret 
        { 
            get { return CredentialsSource == eCredentialsSource.AppSecret; } 
            set { CredentialsSource = value ? eCredentialsSource.AppSecret : eCredentialsSource.UserToken; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_userToken = "";
        public string UserToken
        {
            get { return m_userToken; }
            set { m_userToken = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_appSecret = "";
        public string AppSecret
        {
            get { return m_appSecret; }
            set { m_appSecret = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private string m_pathToAPK = "";
        public string PathToAPK 
        { 
            get { return m_pathToAPK; }
            set { m_pathToAPK = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); } 
        }

        private string m_releaseChannel = "ALPHA";
        public string ReleaseChannel
        {
            get { return m_releaseChannel; }
            set { m_releaseChannel = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // OPTIONAL SETTINGS READ/WRITE + WANTED
        //

        private bool m_wantsAssetsDir;
        private string m_assetsDir = "";
        public bool WantsAssetsDir
        {
            get { return m_wantsAssetsDir; }
            set { m_wantsAssetsDir = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string AssetsDir
        {
            get { return m_assetsDir; }
            set { m_assetsDir = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsAssetFilesConfig;
        private string m_assetFilesConfig = "";
        public bool WantsAssetFilesConfig
        {
            get { return m_wantsAssetFilesConfig; }
            set { m_wantsAssetFilesConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string AssetFilesConfig
        {
            get { return m_assetFilesConfig; }
            set { m_assetFilesConfig = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsOBB;
        private string m_pathToOBB = "";
        public bool WantsOBB
        {
            get { return m_wantsOBB; }
            set { m_wantsOBB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string PathToOBB
        {
            get { return m_pathToOBB; }
            set { m_pathToOBB = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsReleaseNotes;
        private string m_releaseNotes = "";
        public bool WantsReleaseNotes
        {
            get { return m_wantsReleaseNotes; }
            set { m_wantsReleaseNotes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string ReleaseNotes
        {
            get { return m_releaseNotes; }
            set { m_releaseNotes = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsLanguagePacks;
        private string m_languagePacksDir = "";
        public bool WantsLanguagePacks
        {
            get { return m_wantsLanguagePacks; }
            set { m_wantsLanguagePacks = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string LanguagePacksDir
        {
            get { return m_languagePacksDir; }
            set { m_languagePacksDir = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsDebugSymbols;
        private string m_debugSymbolsDir = "";
        public bool WantsDebugSymbols
        {
            get { return m_wantsDebugSymbols; }
            set { m_wantsDebugSymbols = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string DebugSymbolsDir
        {
            get { return m_debugSymbolsDir; }
            set { m_debugSymbolsDir = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_wantsDebugSymbolsPattern;
        private string m_debugSymbolsPattern = "";
        public bool WantsDebugSymbolsPattern
        {
            get { return m_wantsDebugSymbolsPattern && WantsDebugSymbols; }
            set { m_wantsDebugSymbolsPattern = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }
        public string DebugSymbolsPattern
        {
            get { return m_debugSymbolsPattern; }
            set { m_debugSymbolsPattern = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // READ-ONLY SETTINGS
        //

        public bool IsValid { get { return CalcError() == eErrorCode.Success; } }
        public bool IsInvalid { get { return !IsValid; } }
        public string ErrorStr { get { return Err2Str(CalcError()); } }
        
        public string OptionalSettingsCountStr
        { get {
                int count = 0;
                if (WantsAssetFilesConfig) ++count;
                if (WantsAssetsDir) ++count;
                if (WantsDebugSymbols) ++count;
                if (WantsDebugSymbolsPattern) ++count;
                if (WantsLanguagePacks) ++count;
                if (WantsOBB) ++count;
                if (WantsReleaseNotes) ++count;
                if (count == 1)
                    return "1 Optional Setting Applied";
                else
                    return count.ToString() + " Optional Settings Applied";
            } }
        //
        // EVERYTHING ELSE
        //
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void Save(string filename)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (StreamWriter sw = File.CreateText(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(OculusUploaderGUISettings));
                xmls.Serialize(sw, this);
            }
        }

        public static OculusUploaderGUISettings Load(string filename)
        {
            if (!File.Exists(filename))
                return new OculusUploaderGUISettings();

            using (StreamReader sw = new StreamReader(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(OculusUploaderGUISettings));
                return xmls.Deserialize(sw) as OculusUploaderGUISettings;
            }
        }

        public enum eErrorCode
        {
            Success,
            NoOvrExe,
            NoAppID,
            NoUserToken,
            NoAppSecret,
            NoApk,
            NoReleaseChannel,
            NoAssetsDir,
            NoAssetFilesConfig,
            NoOBB,
            NoReleaseNotes,
            NoLanguagePacks,
            NoDebugSymbols,
            NoDebugSymbolsPattern,
        }

        public eErrorCode CalcError()
        {
            if (string.IsNullOrWhiteSpace(PathToOculusExe))
                return eErrorCode.NoOvrExe;
            if (string.IsNullOrWhiteSpace(AppID))
                return eErrorCode.NoAppID;
            if (string.IsNullOrWhiteSpace(UserToken) && CredentialsSourceIsUserToken)
                return eErrorCode.NoUserToken;
            if (string.IsNullOrWhiteSpace(AppSecret) && CredentialsSourceIsAppSecret)
                return eErrorCode.NoAppSecret;
            if (string.IsNullOrWhiteSpace(PathToAPK))
                return eErrorCode.NoApk;
            if (string.IsNullOrWhiteSpace(ReleaseChannel))
                return eErrorCode.NoReleaseChannel;
            if (WantsAssetsDir && string.IsNullOrWhiteSpace(AssetsDir))
                return eErrorCode.NoAssetsDir;
            if (WantsAssetFilesConfig && string.IsNullOrWhiteSpace(AssetFilesConfig))
                return eErrorCode.NoAssetFilesConfig;
            if (WantsOBB && string.IsNullOrWhiteSpace(PathToOBB))
                return eErrorCode.NoOBB;
            if (WantsReleaseNotes && string.IsNullOrWhiteSpace(ReleaseNotes))
                return eErrorCode.NoReleaseNotes;
            if (WantsLanguagePacks && string.IsNullOrWhiteSpace(LanguagePacksDir))
                return eErrorCode.NoLanguagePacks;
            if (WantsDebugSymbols && string.IsNullOrWhiteSpace(DebugSymbolsDir))
                return eErrorCode.NoDebugSymbols;
            if (WantsDebugSymbolsPattern && string.IsNullOrWhiteSpace(DebugSymbolsPattern))
                return eErrorCode.NoDebugSymbolsPattern;

            return eErrorCode.Success;
        }

        public string Err2Str(eErrorCode errorCode)
        {
            switch (errorCode)
            {
                case eErrorCode.Success:
                    return "No error";
                case eErrorCode.NoOvrExe:
                    return "Invalid path to ovr-platform-util.exe";
                case eErrorCode.NoAppID:
                    return "Invalid App ID";
                case eErrorCode.NoUserToken:
                    return "Invalid User Token";
                case eErrorCode.NoAppSecret:
                    return "Invalid App Secret";
                case eErrorCode.NoApk:
                    return "Invalid APK";
                case eErrorCode.NoReleaseChannel:
                    return "Invalid Release Channel";
                case eErrorCode.NoAssetsDir:
                    return "Invalid path to Assets Dir";
                case eErrorCode.NoAssetFilesConfig:
                    return "Invalid Asset Files Config";
                case eErrorCode.NoOBB:
                    return "Invalid path to OBB";
                case eErrorCode.NoReleaseNotes:
                    return "Invalid Release Notes";
                case eErrorCode.NoLanguagePacks:
                    return "Invalid path to Language Packs";
                case eErrorCode.NoDebugSymbols:
                    return "Invalid path to Debug Symbols";
                case eErrorCode.NoDebugSymbolsPattern:
                    return "Invalid Debug Symbols Pattern";
                default:
                    return "Unknown Error";
            }
        }
    }
}
