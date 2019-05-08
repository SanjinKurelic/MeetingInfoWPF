using MeetingInfoWPF.Model;
using System;
using System.ComponentModel;

namespace MeetingInfoWPF.ViewModel
{
    class SelectedDateVM : INotifyPropertyChanged
    {

        private SelectedDate selectedDate;
        public event PropertyChangedEventHandler PropertyChanged;

        public SelectedDateVM(SelectedDate selectedDate)
        {
            this.selectedDate = selectedDate;
        }

        private DateTime GetDateTime(int year, int month)
        {
            //return new DateTime(year, month, 1);
            DateTime dateTime = new DateTime(year, month, 1);
            int daysToAdd = ((int)DayOfWeek.Monday - (int)dateTime.DayOfWeek + 7) % 7;
            return dateTime.AddDays(daysToAdd);
        }

        public int DateYear {
            get { return selectedDate.CurrentDate.Year; }
            set {
                if(selectedDate.CurrentDate.Year != value)
                {
                    selectedDate.CurrentDate = GetDateTime(value, selectedDate.CurrentDate.Month);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
                }
            }
        }

        public int DateMonth {
            get { return selectedDate.CurrentDate.Month; }
            set {
                if (selectedDate.CurrentDate.Month != value)
                {
                    selectedDate.CurrentDate = GetDateTime(selectedDate.CurrentDate.Year, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
                }
            }
        }

        public DateTime CurrentDate {
            get { return selectedDate.CurrentDate; }
            set {
                selectedDate.CurrentDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(String.Empty));
            }
        }

    }
}
