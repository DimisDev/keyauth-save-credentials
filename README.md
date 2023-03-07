# Keyauth Save Credentials (Username & Password)
Simple C# Code to save user credentials after login and remember them using Json

# How it works
Its really simple, everytime a user logins to your application, if the username & password is correct according to the keyauth api response, the credentials are saved in a json file (named Config.json). Then every time the login form is loaded if the json config exists, the username and password are automatically loaded into the text boxes. The serialization process happens with NewtonsoftJson.

# Installation
1. Load the project you intent in adding the saver and open the package manager console
2. In the console type ```Install-Package Newtonsoft.Json``` , press enter and wait for the package to be installed
3. In the top of your project include Newtonsoft.Json (if its not automatically included) with ```using Newtonsoft.Json;```
4. Replace the Login.cs file or just the parts of code where saver is used ( Credentials Declare, Login_Load, Login Button Click )
5. Build application and you are good to go

                

   ## Credits
- [@DimisSSH](https://github.com/DimisSSH) (Credentials Saver)
- [@KeyAuth](https://keyauth.cc) (Original Example)
