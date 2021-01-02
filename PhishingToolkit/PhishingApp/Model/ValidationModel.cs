using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Model
{
	public class ValidationModel : INotifyPropertyChanged
	{

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public ValidationModel()
        {
            Text = "";
        }
    }
}
