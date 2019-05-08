using MeetingInfoDatabase.Models;
using MeetingInfoWPF.Service;
using MeetingInfoWPF.ViewModel;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace MeetingInfoWPF.View.Forms
{
    public partial class MainWindow : Window
    {

        private SelectedDateVM SelectedDate;
        private List<CultureInfo> languagesList;

        public MainWindow()
        {
            languagesList = new List<CultureInfo>();
            languagesList.Add(new CultureInfo("hr"));
            languagesList.Add(new CultureInfo("en"));

            // Center Window
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            // Set language
            CultureInfo currentLanguage = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            System.Threading.Thread.CurrentThread.CurrentUICulture = currentLanguage;
            System.Threading.Thread.CurrentThread.CurrentCulture = currentLanguage;

            // Initialize components
            InitializeComponent();            
        }

        private void Button_Year_Up_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate.DateYear++;
        }

        private void Button_Year_Down_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate.DateYear--;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedDate = SelectedDateService.GetSelectedDateVM();
            DataContext = SelectedDate;
        }

        private void Month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDate.DateMonth = currentMonth.SelectedIndex;
        }

        private void Add_Meeting_Click(object sender, RoutedEventArgs e)
        {
            MeetingInfo meeting = new MeetingInfo();
            meeting.MeetingType = MeetingInfo.TYPE.ADD_MEETING;

            Meeting m = new Meeting();
            m.Date = SelectedDate.CurrentDate;
            meeting.Meeting = m;

            if (meeting.ShowDialog() == true)
            {
                if (App.Repository.GetMeetingsTable().AddMeeting(meeting.Meeting))
                {
                    SelectedDate.CurrentDate = meeting.Meeting.Date;
                    MessageBox.Show(Properties.Resources.SuccessDescription, Properties.Resources.SuccessTitle, MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show(Properties.Resources.ErrorDescription, Properties.Resources.ErrorTitle, MessageBoxButton.OK);
                }
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(weekPlaner, Title);
            }
        }

        private void ChangeLanguage()
        {
            CultureInfo currentLanguage = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            CultureInfo newLanguage = languagesList.Find(language => language.TwoLetterISOLanguageName != currentLanguage.TwoLetterISOLanguageName);

            System.Threading.Thread.CurrentThread.CurrentUICulture = newLanguage;
            System.Threading.Thread.CurrentThread.CurrentCulture = newLanguage;

            MainWindow mainWindow = new MainWindow();
            Close();
            mainWindow.Show();
        }

        private void Language_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage();
        }
    }
}
