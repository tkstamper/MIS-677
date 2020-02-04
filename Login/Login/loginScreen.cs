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

            int session_attempts = Int32.Parse(dt.Rows[0][2].ToString());
            //convert timer to datetime
            int timer = Int32.Parse(dt.Rows[0][5].ToString());
            int locked = Int32.Parse(dt.Rows[0][6].ToString());
            int accesslevel = Int32.Parse(dt.Rows[0][7].ToString());
            int fails = Int32.Parse(dt.Rows[0][3].ToString());
            int successful_attempts = Int32.Parse(dt.Rows[0][4].ToString());


            //If username doesnt exist
            if (dt.Rows.Count < 1)
            {
                session_attempts = 1;
                timer = 0;
                locked = 0;
                accesslevel = 0;
                fails = 1;
                successful_attempts = 0;
                SqlCommand addUser = new SqlCommand("INSERT INTO attempts (username, password, attempt_per_session, timer, lock, accesslevel, failed_attempts, successful_attempts) VALUES('" + userID + password + session_attempts + timer + locked + accesslevel + successful_attempts + fails + "')", conn);                
            }
            else
            {
                //If no password is found (account doesnt exist)
                if (dt.Rows[0][1].ToString() == "")
                {
                    //If permalocked
                    if (Int32.Parse(dt.Rows[0][6].ToString()) == 1)
                    {
                        MessageBox.Show("Your account is permanently locked. Please contact an administrator to reset your password.");
                    }
                    //If timer isnt 0
                    else if (dt.Rows[0][5].ToString() != "0")
                    {
                        //Display timer
                        MessageBox.Show("Your account is temporarily locked for " + timer + " more seconds. Please try again later.");
                    }
                    else
                    {
                        //Increment failed attempts
                        fails++;

                        //Increment session fails
                        session_attempts++;

                        //Update new fails and session fails in the table
                        SqlCommand updateFails = new SqlCommand("UPDATE attempts (failed_attempts, attempt_per_session) VALUES('" + fails + session_attempts + "') WHERE username = '" + userID + "'", conn);
                        updateFails.ExecuteNonQuery();
                        MessageBox.Show(dt.Rows[0][3].ToString());
                        //If failed attempts = 3
                        if (dt.Rows[0][3].ToString() == "3")
                        {
                            //Lock
                            MessageBox.Show("Your account has been temporarily locked for " + timer + " seconds. Please try again later.");
                        }
                        else if (dt.Rows[0][3].ToString() == "6")
                        {
                            //Permalock
                            locked = 1;
                            MessageBox.Show("Your account has been permanently locked. Please contact an administrator to reset your password.");
                        }
                    }
                }
                //Password is found (account exists)
                else
                {
                    //If permalocked
                    if (Int32.Parse(dt.Rows[0][6].ToString()) == 1)
                    {
                        //Display permalock
                        MessageBox.Show("Your account is permanently locked. Please contact an administrator to reset your password.");
                    }
                    //If timer isnt 0
                    else if (dt.Rows[0][5].ToString() != "0")
                    {
                        //Display timer
                        MessageBox.Show("Your account is temporarily locked for " + timer + " more seconds. Please try again later.");
                    }
                    else
                    {
                        //If password is wrong
                        if (dt.Rows[0][1].ToString() != password)
                        {
                            //Increment failed attempts
                            fails++;
                            //Increment session fails
                            session_attempts++;

                            //If failed attempts = 3
                            if (dt.Rows[0][3].ToString() == "3")
                            {
                                //Lock 3 hours
                                MessageBox.Show("Your account has been locked for " + timer + " seconds. Please try again later.");
                            }
                            else if (dt.Rows[0][3].ToString() == "6")
                            {
                                //Permalock
                                MessageBox.Show("Your account has been permanently locked. Please contact an administrator to reset your password.");
                            }
                            else
                            {
                                //Display password incorrect message and amount of fails
                            }
                        }
                        //If password is right
                        else
                        {
                            //Increment successful attempts


                            //Reset failed attempts


                            //If employee is a worker
                            if (dt.Rows[0][7].ToString() == "Worker")
                            {
                                //Show worker screen
                            }
                            else
                            {
                                //Show manager screen
                            }
                        }
                    }
                }
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {

        }
    }
}
