using System.Data.SqlClient;

namespace EVOKETASK.Models
{
    public class BackgroundTask : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BackgroundTask(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancel)
        {
            while (!cancel.IsCancellationRequested)
            {
                InsertDataIntoDatabase();
                await Task.Delay(TimeSpan.FromMinutes(1), cancel);
            }
        }
        private void InsertDataIntoDatabase()
        {
            var connectionString = "Data Source=CKOMATI-L-5476\\SQLEXPRESS;Initial Catalog=EVOKETASK;User ID=sa;Password=Welcome2evoke@1234";
            var list = new List<(string NAME, string EMAIL, string PHONENUMBER, int DEPTID, int GID
                )>
        {
                ("Vijju","srija@gmail.com","9014757212",1,1),
                ("Aish","Aish@gmail.com","8106099130",2,2),

        };

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                foreach (var i in list)
                {
                    SqlCommand cmd = new SqlCommand("insert into Employee(NAME, EMAIL, PHONENUMBER, DEPTID, GID)  VALUES (@NAME, @EMAIL,@PHONENUMBER,@DEPTID,@GID)", con);
                    cmd.Parameters.AddWithValue("@NAME", i.NAME);
                    cmd.Parameters.AddWithValue("@EMAIL", i.EMAIL);
                    cmd.Parameters.AddWithValue("@PHONENUMBER", i.PHONENUMBER);
                    cmd.Parameters.AddWithValue("@DEPTID", i.DEPTID);
                    cmd.Parameters.AddWithValue("@GID", i.GID);
                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
