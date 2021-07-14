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
        public static string DefaultPath
        { get {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GameUploader", "meta-settings.xml");
        } }

        private static MetaSettings k_instance = null;
        public static MetaSettings Instance { get { if (k_instance == null) k_instance = Load(DefaultPath); return k_instance; } }

        //
        // REQUIRED SETTINGS READ/WRITE
        //

        private string m_currPage = "";
        public string CurrPage
        {
            get { return m_currPage; }
            set { m_currPage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); }
        }

        //
        // READ-ONLY SETTINGS
        //

        public bool CurrPageIsOculus { get { return CurrPage == "Oculus"; } }
        public bool CurrPageIsSteam { get { return CurrPage == "Steam"; } }
        public bool CurrPageIsSettings { get { return CurrPage == "Settings"; } }


        //
        // EVERYTHING ELSE
        //

        public event PropertyChangedEventHandler PropertyChanged;

        public void Save(string filename)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filename));
            using (StreamWriter sw = File.CreateText(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(MetaSettings));
                xmls.Serialize(sw, this);
            }
        }

        public static MetaSettings Load(string filename)
        {
            if (!File.Exists(filename))
                return new MetaSettings();

            using (StreamReader sw = new StreamReader(filename))
            {
                XmlSerializer xmls = new XmlSerializer(typeof(MetaSettings));
                return xmls.Deserialize(sw) as MetaSettings;
            }
        }
    }
}