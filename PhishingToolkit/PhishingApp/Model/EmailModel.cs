﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhishingApp.Model
{
	public class EmailModel : INotifyPropertyChanged
	{
        private string emails;

        public string Emails
        {
            get { return emails; }
            set {
                emails = value;
                OnPropertyChanged("Emails");
            }
        }

        private string body;

        public string Body
        {
            get { return body; }
            set { 
                body = value;
                OnPropertyChanged("Body");
            }
        }


        private int myVar;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

     
        public EmailModel()
        {
           
        }
    }

}
