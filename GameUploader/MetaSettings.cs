using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameUploader
{
	public class MetaSettings : INotifyPropertyChanged
	{
        public static string ParentFolderPath { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GamePusher"); } }
        public static string SavePath { get { return Path.Combine(ParentFolderPath, "meta-settings.xml"); } }

        private static MetaSettings k_instance = null;
        public static MetaSettings Instance { get { if (k_instance == null) k_instance = Load(); return k_instance; } }

        //
        // REQUIRED SETTINGS READ/WRITE
        //

        private string m_currPage = "";
        public string CurrPage
        {
            get { return m_currPage; }
            set { m_currPage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_savePasswords = true;
        public bool SavePasswords
        {
            get { return m_savePasswords; }
            set { m_savePasswords = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        private bool m_showCopyCommandButton = false;
        public bool ShowCopyCommandButton
        {
            get { return m_showCopyCommandButton; }
            set { m_showCopyCommandButton = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // READ-ONLY SETTINGS
        //

        public bool CurrPageIsOculus { get { return CurrPage == "Oculus"; } }
        public bool CurrPageIsSteam { get { return CurrPage == "Steam"; } }
        public bool CurrPageIsItch { get { return CurrPage == "Itch"; } }
        public bool CurrPageIsSettings { get { return CurrPage == "Settings"; } }


        //
        // EVERYTHING ELSE
        //

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SavePath));
            using (StreamWriter sw = File.CreateText(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(MetaSettings));
                xmls.Serialize(sw, this);
            }
        }

        public static MetaSettings Load()
        {
            if (!File.Exists(SavePath)) return new MetaSettings();
            using (StreamReader sw = new StreamReader(SavePath))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(MetaSettings));
                return xmls.Deserialize(sw) as MetaSettings;
            }
        }
    }
}