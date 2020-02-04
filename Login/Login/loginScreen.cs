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

            int attempt_per_session = Int32.Parse(dt.Rows[0][2].ToString());
            int timer = Int32.Parse(dt.Rows[0][5].ToString());
            int locked = Int32.Parse(dt.Rows[0][6].ToString());
            int accesslevel = Int32.Parse(dt.Rows[0][7].ToString());
            int failed_attempts = Int32.Parse(dt.Rows[0][3].ToString());
            int successful_attempts = Int32.Parse(dt.Rows[0][4].ToString());


            //If username doesnt exist
            if (dt.Rows.Count < 1)
            {
                attempt_per_session = 1;
                timer = 0;
                locked = 0;
                accesslevel = 0;
                failed_attempts = 1;
                successful_attempts = 0;
                SqlCommand addUser = new SqlCommand("INSERT INTO attempts (username, password, attempt_per_session, timer, lock, accesslevel, failed_attempts, successful_attempts) VALUES('" + userID + password + attempt_per_session + timer + locked + accesslevel + successful_attempts + failed_attempts + "')", conn);
            }
            else
            {
                //If no password is found (account doesnt exist)
                if (dt.Rows[0][1].ToString() == null)
                {
                    //Increment failed attempts
                    failed_attempts++; 

                    //If permalocked
                    if (dt.Rows[0][6].ToString() == "1")
                    {
                        //Display permalock
                    }
                    //If failed attempts = 3
                    if (dt.Rows[0][3].ToString() == "3")
                    {
                        //Lock
                        locked = 1; 
                    }
                    else if (dt.Rows[0][3].ToString() == "6")
                    {
                        //Permalock
                        locked = 1;
                    }
                }
                else
                {
                    //Increment fail counter
                    failed_attempts++;

                    //If permalocked
                    if (dt.Rows[0][6].ToString() == "1")
                    {
                        //Display timer

                    }
                    //If timer isnt 0
                    else if (dt.Rows[0][5].ToString() != "0")
                    {
                        //Display timer

                    }
                    else
                    {
                        //If password is wrong
                        if (dt.Rows[0][1].ToString() != password)
                        {
                            //If failed attempts = 3
                            if (dt.Rows[0][3].ToString() == "3")
                            {
                                //Lock 3 hours


                            }
                            else if (dt.Rows[0][3].ToString() == "6")
                            {
                                //Permalock
                                locked = 1;
                            }
                            else
                            {
                                //Display password incorrect message and amount of fails
                                LockMessage Form2 = new LockMessage();
                                Form2.ShowDialog();

                            }
                        }
                        //If password is right
                        else
                        {
                            //Increment successful attempts
                            successful_attempts++;


                            //Reset failed attempts
                            attempt_per_session = 0;
                          


                            //If employee is a worker
                            if (dt.Rows[0][7].ToString() == "Worker")
                            {
                                //Show worker screen
                                workerView Form3 = new workerView();
                                Form3.ShowDialog();
                            }
                            else
                            {
                                //Show manager screen
                                managerView Form4 = new managerView();
                                Form4.ShowDialog();
                            }
                        }
                    }
                }
                MessageBox.Show(dt.Rows[0][0].ToString());

            }
        }

      
    }
}
