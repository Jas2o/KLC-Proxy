using LibKaseya;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLC_Proxy {
    public class MainData /*: INotifyPropertyChanged*/ {

        public ObservableCollection<Agent> ListAgent { get; set; }

        public MainData() {
            ListAgent = new ObservableCollection<Agent>();
            //ListAgent.CollectionChanged += ListAgent_CollectionChanged;
        }

        /*
        private void ListAgent_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            if (e.OldItems != null) {
                foreach (INotifyPropertyChanged item in e.OldItems) {
                    item.PropertyChanged -= NotifyPropertyChanged;
                }
            }
            if (e.NewItems != null) {
                foreach (INotifyPropertyChanged item in e.NewItems) {
                    item.PropertyChanged += NotifyPropertyChanged;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs("ListAgent"));
                PropertyChanged(this, new PropertyChangedEventArgs("Label"));
            }
        }
        */
    }
}
