using MeetingInfoDatabase.Models;
using System.ComponentModel;

namespace MeetingInfoWPF.ViewModel
{
    class MeetingInfoVM : INotifyPropertyChanged
    {

        private Meeting _meeting;

        private Client _client;

        public Meeting Meeting {
            get { return _meeting; }
            set {
                _meeting = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Meeting"));
            }
        }

        public Client Client {
            get { return _client; }
            set {
                _client = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Client"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
