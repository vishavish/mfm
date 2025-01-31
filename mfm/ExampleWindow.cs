using Terminal.Gui;

public class ExampleWindow : Window
{
    public ExampleWindow()
    {
        Title = $"Example App - ({Application.QuitKey} to quit)";
        ColorScheme = new ColorScheme
        {
            Normal = Application.Driver!.MakeColor(Color.Green, Color.Black)
        };

        var usernameLabel = new Label { Text = "Username" };
        var usernameText = new TextField
        {
            X = Pos.Right(usernameLabel) + 1,
            Width = Dim.Fill(),
            BorderStyle = LineStyle.Single
        };

        var passwordLabel = new Label
        {
            Text = "Password",
            X = Pos.Left(usernameLabel),
            Y = Pos.Bottom(usernameLabel) + 1
        };

        var passwordText = new TextField
        {
            Secret = true,
            X = Pos.Left(usernameText),
            Y = Pos.Top(passwordLabel),
            Width = Dim.Fill(),          
            BorderStyle = LineStyle.Single,
        };

        var btnLogin = new Button
        {
            Text = "Login",
            Y = Pos.Bottom(passwordLabel) + 3,
            X = Pos.Center(),
            IsDefault = true  
        };

        btnLogin.Accept += (s,e) => Login(s, e, usernameText, passwordText);

        Add (usernameLabel, usernameText, passwordLabel, passwordText, btnLogin);
    }

    private void Login(object sender, EventArgs e, TextField username, TextField pwd)
    {
        if(username.Text == "admin" && pwd.Text == "password")
        {
            MessageBox.Query ("Logging In", "Login Successful", "Ok");
            Application.RequestStop ();
       }
       else
       {
           MessageBox.ErrorQuery ("Logging In", "Incorrect username or password", "Ok");
       }       
    }    
}
