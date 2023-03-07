# Keyauth Save Credentials (Username & Password)
Simple C# Code to save user credentials after login and remember them using Json

# How it works
Its really simple, everytime a user logins to your application, if the username & password is correct according to the keyauth api response, the credentials are saved in a json file (named Config.json). Then every time the login form is loaded if the json config exists, the username and password are automatically loaded into the text boxes. The serialization process happens with NewtonsoftJson.

# Installation
1. Load the project you intent in adding the saver and open the package manager console
2. In the console type ```Install-Package Newtonsoft.Json``` , press enter and wait for the package to be installed
3. In the top of your project include Newtonsoft.Json (if its not automatically included) with ```using Newtonsoft.Json;```
4. On the end of the Login_Load void add the following code

 string configpath = "Config.json";
            if (File.Exists(configpath))
            {
                string json = File.ReadAllText(configpath);
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(json);
                string credentialsString = string.Format("Username: {0}, Password: {1}", credentials.Username, credentials.Password);
                username.Text = credentials.Username;
                password.Text = credentials.Password;
            }

          
            5. Define the credentials class 
            

                    public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        6. On the Login Button Click change the code to the following

          KeyAuthApp.login(username.Text,password.Text);
            if (KeyAuthApp.response.success)
            {
                Credentials credentials = new Credentials
                {
                    Username = username.Text,
                    Password = password.Text
                };
                string json = JsonConvert.SerializeObject(credentials);
                string filePath = "Config.json";
                File.WriteAllText(filePath, json);
                Main main = new Main();
                main.Show();
                this.Hide();
            }
            else
                status.Text = "Status: " + KeyAuthApp.response.message;
 
                7. Build the application and you are ready!
                
                ⚠️ I also added a modified version of Login.cs that contains it so you can simply change the whole form to that for faster installation.
                
                
   ## Credits
- [@DimisSSH](https://github.com/DimisSSH)
