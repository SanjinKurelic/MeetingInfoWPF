using MeetingInfoWPF.Model;
using MeetingInfoWPF.ViewModel;
using System;

namespace MeetingInfoWPF.Service
{
    class SelectedDateService
    {

        private static SelectedDateVM selectedDateVM;

        public static SelectedDateVM GetSelectedDateVM()
        {
            if (selectedDateVM == null)
            {
                selectedDateVM = new SelectedDateVM(new SelectedDate
                {
                    CurrentDate = DateTime.Now
                });
            }
            return selectedDateVM;
        }

    }
}
