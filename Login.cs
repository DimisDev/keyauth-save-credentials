using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json; // Including NewtonsoftJson for serialising credentials into config

namespace KeyAuth
{
    public partial class Login : Form
    {

        /*
        * 
        * WATCH THIS VIDEO TO SETUP APPLICATION: https://youtube.com/watch?v=RfDTdiBq4_o
        * 
        */

        /*
        Optional Functions:
        
        KeyAuthApp.webhook("7kR0UedlVI", "&ip=1.1.1.1&hwid=abc");
        // send secure request to webhook which is impossible to crack into. the base link set on the website is https://keyauth.win/api/seller/?sellerkey=sellerkeyhere&type=black, which nobody except you can see, so the final request is https://keyauth.win/api/seller/?sellerkey=sellerkeyhere&type=black&ip=1.1.1.1&hwid=abc
        
        // byte[] result = KeyAuthApp.download("902901"); // downloads application file
        // File.WriteAllBytes("C:\\Users\\mak\\Downloads\\KeyAuth-CSHARP-Example-main (5)\\KeyAuth-CSHARP-Example-main\\ConsoleExample\\bin\\Debug\\countkeys.txt", result);
        
        MessageBox.Show(KeyAuthApp.var("123456")); // retrieve application variable
        */

        // KeyAuthApp.register("username", "password", "key");
        //KeyAuthApp.login("username", "password"); 

        public static api KeyAuthApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: "1.0"
        );

         public class Credentials // Save Credentials - github.com/DimisSSH/keyauth-save-credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public Login()
        {
            InitializeComponent();
        }

        private void siticoneControlBox1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
        
        public static bool SubExist(string name)
        {
            if(KeyAuthApp.user_data.subscriptions.Exists(x => x.subscription == name))
                return true;
            return false;
        }
        
        private void Login_Load(object sender, EventArgs e)
        {
            KeyAuthApp.init();

            if (KeyAuthApp.response.message == "invalidver")
            {
                if (!string.IsNullOrEmpty(KeyAuthApp.app_data.downloadLink))
                {
                    DialogResult dialogResult = MessageBox.Show("Yes to open file in browser\nNo to download file automatically", "Auto update", MessageBoxButtons.YesNo);
                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            Process.Start(KeyAuthApp.app_data.downloadLink);
                            Environment.Exit(0);
                            break;
                        case DialogResult.No:
                            WebClient webClient = new WebClient();
                            string destFile = Application.ExecutablePath;

                            string rand = random_string();

                            destFile = destFile.Replace(".exe", $"-{rand}.exe");
                            webClient.DownloadFile(KeyAuthApp.app_data.downloadLink, destFile);

                            Process.Start(destFile);
                            Process.Start(new ProcessStartInfo()
                            {
                                Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\"",
                                WindowStyle = ProcessWindowStyle.Hidden,
                                CreateNoWindow = true,
                                FileName = "cmd.exe"
                            });
                            Environment.Exit(0);

                            break;
                        default:
                            MessageBox.Show("Invalid option");
                            Environment.Exit(0);
                            break;
                    }
                }
                MessageBox.Show("Version of this program does not match the one online. Furthermore, the download link online isn't set. You will need to manually obtain the download link from the developer");
                Environment.Exit(0);
            }
            
            if (!KeyAuthApp.response.success)
            {
                MessageBox.Show(KeyAuthApp.response.message);
                Environment.Exit(0);
            }
            // if(KeyAuthApp.checkblack())
            // {
            //     MessageBox.Show("user is blacklisted");
            // }
            // else
            // {
            //     MessageBox.Show("user is not blacklisted");
            // }
            // check if subscription exists
            // if(SubExist("default"))
            // {
            //     MessageBox.Show("default subscription exists");
            // }
            KeyAuthApp.check();
            siticoneLabel1.Text = $"Current Session Validation Status: {KeyAuthApp.response.success}";
            // Save Credentials - github.com/DimisSSH/keyauth-save-credentials
             string configpath = "Config.json";
            if (File.Exists(configpath)) // Checks if the config file exists
            {
                string json = File.ReadAllText(configpath);
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(json);
                string credentialsString = string.Format("Username: {0}, Password: {1}", credentials.Username, credentials.Password);
                username.Text = credentials.Username;
                password.Text = credentials.Password;
            }
        }

        static string random_string()
        {
            string str = null;

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                str += Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))).ToString();
            }
            return str;

        }

        private void UpgradeBtn_Click(object sender, EventArgs e)
        {
            KeyAuthApp.upgrade(username.Text, key.Text); // success is set to false so people can't press upgrade then press login and skip logging in. it doesn't matter, since you shouldn't take any action on succesfull upgrade anyways. the only thing that needs to be done is the user needs to see the message from upgrade function
            status.Text = "Status: " + KeyAuthApp.response.message;
            // don't login, because they haven't authenticated. this is just to extend expiry of user with new key.
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            KeyAuthApp.login(username.Text,password.Text);
            if (KeyAuthApp.response.success)
            {
            // Save Credentials - github.com/DimisSSH/keyauth-save-credentials
                Credentials credentials = new Credentials
                {
                    Username = username.Text,
                    Password = password.Text
                };
                string json = JsonConvert.SerializeObject(credentials);
                string filePath = "Config.json";
                File.WriteAllText(filePath, json); // Saves the credentials to the json file
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + KeyAuthApp.response.message;
        }

        private void RgstrBtn_Click(object sender, EventArgs e)
        {
            string email = this.email.Text;
            if(email == "Email (leave blank if none)") { // default value
                email = null;
            }

            KeyAuthApp.register(username.Text, password.Text, key.Text, email);
            if (KeyAuthApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + KeyAuthApp.response.message;
        }

        private void LicBtn_Click(object sender, EventArgs e)
        {
            KeyAuthApp.license(key.Text);
            if (KeyAuthApp.response.success)
            {
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + KeyAuthApp.response.message;
        }

        private void siticoneRoundedButton1_Click(object sender, EventArgs e)
        {
            KeyAuthApp.forgot(username.Text, email.Text);
            status.Text = "Status: " + KeyAuthApp.response.message;
        }
    }
}