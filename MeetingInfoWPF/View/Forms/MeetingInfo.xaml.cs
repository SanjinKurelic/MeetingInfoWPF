using MeetingInfoDatabase.Models;
using MeetingInfoWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MeetingInfoWPF;

namespace MeetingInfoWPF.View.Forms
{
    /// <summary>
    /// Interaction logic for MeetingInfo.xaml
    /// </summary>
    public partial class MeetingInfo : Window
    {

        private MeetingInfoVM _infoVM;
        private List<Client> _clients;

        public enum TYPE { ADD_MEETING, EDIT_MEETING, VIEW_MEETING };

        public TYPE MeetingType { get; set; } = TYPE.VIEW_MEETING;

        public Meeting Meeting {
            get { return _infoVM.Meeting; }
            set {
                _infoVM.Meeting = value;
                clients.SelectedIndex = value.ClientID - 1;
            }
        }

        public MeetingInfo()
        {
            InitializeComponent();
            _infoVM = new MeetingInfoVM();
            _clients = App.Repository.GetClientTable().GetClients();

            // Center Window
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            DataContext = _infoVM;
            clients.SelectedItem = _clients[0];
        }

        private void ShowButtons()
        {
            switch (MeetingType)
            {
                case TYPE.ADD_MEETING:
                case TYPE.EDIT_MEETING:
                    btnClose.Visibility = Visibility.Collapsed;                    
                    break;
                case TYPE.VIEW_MEETING:
                    btnSave.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    DisableInputFields();
                    break;
            }
        }

        private void DisableInputFields()
        {
            UIElementCollection elements = ((Panel)Content).Children;
            IEnumerable<Control> elementList = elements.Cast<FrameworkElement>().ToList().OfType<Control>();

            foreach(Control control in elementList)
            {
                if(control is TextBox || control is ComboBox || control is DatePicker)
                {
                    control.IsEnabled = false;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            clients.ItemsSource = _clients;
            ShowButtons();
        }

        private void clients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _infoVM.Client = (Client)clients.SelectedItem;
            _infoVM.Meeting.ClientID = _infoVM.Client.IDClient;
        }

        private bool RequiredFieldsEmpty()
        {
            return 
                string.IsNullOrWhiteSpace(_infoVM.Meeting.Title) || 
                string.IsNullOrWhiteSpace(_infoVM.Meeting.Place) || 
                string.IsNullOrWhiteSpace(_infoVM.Meeting.Description);
        }

        private bool IsDateReserved()
        {
            foreach(Meeting meeting in App.Repository.GetMeetingsTable().GetMeetings(_infoVM.Meeting.Date))
            {
                if (_infoVM.Meeting.Date == meeting.Date && _infoVM.Meeting.IDMeeting != meeting.IDMeeting)
                {
                    return true;
                }
            }
            return false;
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if(RequiredFieldsEmpty())
            {
                MessageBox.Show(Properties.Resources.RequiredFields, Properties.Resources.ErrorTitle, MessageBoxButton.OK);
                return;
            }
            if(IsDateReserved())
            {
                MessageBox.Show(Properties.Resources.DateReserved, Properties.Resources.ErrorTitle, MessageBoxButton.OK);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void hour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox hours = sender as ComboBox;
            if(hours == null || hours.SelectedValue == null)
            {
                return;
            }
            _infoVM.Meeting.Date = new DateTime(_infoVM.Meeting.Date.Year, _infoVM.Meeting.Date.Month, _infoVM.Meeting.Date.Day, int.Parse(hours.SelectedValue.ToString()), 0, 0);
        }
    }
}