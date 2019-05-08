using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingInfoWPF.ViewModel
{
    class MeetingVM
    {

        public int MeetingId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int RowIndex { get; set; }

        public int ColumnIndex { get; set; }

    }
}
