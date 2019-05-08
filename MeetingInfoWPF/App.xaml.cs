using MeetingInfoDatabase;
using System.Configuration;
using System.Windows;

namespace MeetingInfoWPF
{
    public partial class App : Application
    {

        public static Repository Repository { get; private set; }
        private static readonly string cs = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public App()
        {
            Repository = new Repository(cs, MeetingInfoDatabase.DAO.DatabaseType.SqlHelper);
        }

    }
}
