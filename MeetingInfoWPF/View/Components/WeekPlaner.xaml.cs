using MeetingInfoDatabase.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MeetingInfoWPF.ViewModel;
using MeetingInfoWPF.View.Forms;

namespace MeetingInfoWPF.View.Components
{
    public partial class WeekPlaner : UserControl
    {

        private const int HOUR_PADDING = 8;
        private const int DAY_PADDING = 1;

        public static readonly DependencyProperty CurrentDateDependency = DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(WeekPlaner),
            new FrameworkPropertyMetadata(DateTime.Now, new PropertyChangedCallback(OnCurrentDateChange)));
        private DateTime _currentTime = DateTime.Now;

        public WeekPlaner()
        {
            InitializeComponent();    
        }

        private int GetRowIndex(DateTime date)
        {
            return date.TimeOfDay.Hours - HOUR_PADDING;
        }

        private int GetColumnIndex(DateTime date)
        {
            return (int)date.DayOfWeek - DAY_PADDING;
        }

        private void RefillMeetings()
        {
            ObservableCollection<MeetingVM> meetings = new ObservableCollection<MeetingVM>();
            foreach(Meeting meeting in App.Repository.GetMeetingsTable().GetMeetings(CurrentDate))
            {
                meetings.Add(new MeetingVM()
                {
                    Title = meeting.Title,
                    Description = meeting.Description,
                    RowIndex = GetRowIndex(meeting.Date),
                    ColumnIndex = GetColumnIndex(meeting.Date),
                    MeetingId = meeting.IDMeeting
                });
            }
            weekList.ItemsSource = meetings;
        }

        public DateTime CurrentDate {
            get { return _currentTime; }
            set { _currentTime = (DateTime)GetValue(CurrentDateDependency); RefillMeetings(); }
        }

        private static void OnCurrentDateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((WeekPlaner)d).CurrentDate = (DateTime)e.NewValue;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefillMeetings();
        }

        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if(menuItem == null)
            {
                return;
            }

            StackPanel meeting = menuItem.CommandTarget as StackPanel;
            if(meeting == null)
            {
                return;
            }

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Meeting = App.Repository.GetMeetingsTable().GetMeeting((int)meeting.Tag);
            meetingInfo.MeetingType = MeetingInfo.TYPE.EDIT_MEETING;

            if(meetingInfo.ShowDialog() == true)
            {
                if(App.Repository.GetMeetingsTable().ChangeMeeting(meetingInfo.Meeting))
                {
                    RefillMeetings();
                    MessageBox.Show(Properties.Resources.SuccessDescription, Properties.Resources.SuccessTitle, MessageBoxButton.OK);
                } else
                {
                    MessageBox.Show(Properties.Resources.ErrorDescription, Properties.Resources.ErrorTitle, MessageBoxButton.OK);
                }
            }
        }

        private void RemoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null)
            {
                return;
            }

            StackPanel meeting = menuItem.CommandTarget as StackPanel;
            if (meeting == null)
            {
                return;
            }

            if (MessageBox.Show(Properties.Resources.ConfirmDescription, Properties.Resources.ConfirmTitle, MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                if(App.Repository.GetMeetingsTable().RemoveMeeting((int)meeting.Tag))
                {
                    RefillMeetings();
                } else
                {
                    MessageBox.Show(Properties.Resources.ErrorDescription, Properties.Resources.ErrorTitle, MessageBoxButton.OK);
                }
            }

           
        }

        private void ContentItem_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel item = sender as StackPanel;
            if(item == null)
            {
                return;
            }
            int meetingId = (int)item.Tag;

            MeetingInfo meetingInfo = new MeetingInfo();
            meetingInfo.Meeting = App.Repository.GetMeetingsTable().GetMeeting(meetingId);
            meetingInfo.MeetingType = MeetingInfo.TYPE.VIEW_MEETING;
            meetingInfo.ShowDialog();
        }
    }
}
