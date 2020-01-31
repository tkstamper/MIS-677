using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace Login
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            SqlConnection conn = null;
            SqlDataReader reader = null;

        }

        private void Login_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            conn = new SqlConnection(@"Data Source = 10.135.85.184; User ID = group3; Password = Grp3s2117");
            conn.Open();



        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            conn = new SqlConnection(@"Data Source = 10.135.85.184; User ID = group3; Password = Grp3s2117");
            conn.Open();
            string userID = textBox1.Text;
            string password = textBox2.Text;


            SqlCommand check = new SqlCommand("SELECT * FROM attempts WHERE username = '" + userID + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(check);
            DataTable dt = new DataTable();
            da.Fill(dt);




            if (dt.Rows.Count < 1)
            {
                int attempt_per_session = 1;
                int timer = 0;
                int locked = 0;
                int accesslevel = 0;
                int failed_attempts = 1;
                int succesful_attempts = 0;
                SqlCommand addUser = new SqlCommand("INSERT INTO attempts (username, password, attempt_per_session, timer, lock, accesslevel, failed_attempts, successful_attempts) VALUES('" + userID + password + attempt_per_session + timer + locked + accesslevel + succesful_attempts + failed_attempts + "')", conn);
            }
            else
            {
                if (dt.Rows[0][4].ToString() != "4")
                {
                    MessageBox.Show(dt.Rows[0][4].ToString());
                }

            }
        }

      
    }
}
