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
    public class GameUploaderSettings : INotifyPropertyChanged
    {
        public enum eService
		{
            Oculus,
            Steam,
		}

        private eService m_currService;
        public eService CurrService 
        { 
            get { return m_currService; } 
            set { m_currService = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null)); } 
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void Save(string filename)
        {
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
    }
}
