using System;
using System.Windows;
using System.Windows.Controls;

namespace MeetingInfoWPF.View.Components
{

    public partial class CustomCalendar : UserControl
    {

        public static readonly DependencyProperty SelectedDateDependency = DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(CustomCalendar),
            new FrameworkPropertyMetadata(DateTime.Now, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedDateChange)));
        private bool dateLock = false;

        private void SelectWeek()
        {
            dateLock = true;
            DateTime firstDay = SelectedDate.AddDays(DayOfWeek.Monday - SelectedDate.DayOfWeek);

            calendar.SelectedDates.Clear();
            for (int i = 0; i <= 6; i++)
            {
                calendar.SelectedDates.Add(firstDay.AddDays(i));
            }
            dateLock = false;
        }

        public DateTime SelectedDate {
            get {
                return (DateTime)GetValue(SelectedDateDependency);
            }
            set {
                calendar.SelectedDate = value;
                calendar.DisplayDate = value;
                SelectWeek();
                SetValue(SelectedDateDependency, value);
            }
        }

        public CustomCalendar()
        {
            InitializeComponent();
        }

        private static void OnSelectedDateChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomCalendar)d).SelectedDate = (DateTime)e.NewValue;
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dateLock)
            {
                return;
            }
            SelectedDate = calendar.SelectedDates[0];
        }
    }



}
