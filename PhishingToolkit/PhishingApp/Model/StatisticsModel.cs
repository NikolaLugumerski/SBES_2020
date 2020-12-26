using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Model
{
	public class StatisticsModel : INotifyPropertyChanged
	{

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int sentMails;

        public int SentMails
        {
            get { return sentMails; }
            set { 
                sentMails = value;
                OnPropertyChanged("SentMails");
            }
        }

        private int formsFilled;

        public int FormsFilled
        {
            get { return formsFilled; }
            set {
                formsFilled = value;
                OnPropertyChanged("FormsFilled");
            }
        }

        private Dictionary<string,VictimModel> exploitedVictims;

        public Dictionary<string,VictimModel> ExploitedVictims
        {
            get { return exploitedVictims; }
            set {
                exploitedVictims = value;
                OnPropertyChanged("ExploitedVictims");
            }
        }


        public StatisticsModel()
        {
            ExploitedVictims = new Dictionary<string, VictimModel>();
        }



    }
}
