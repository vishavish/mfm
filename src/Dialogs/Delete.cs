namespace Dialogs;


public class Delete : Window
{
	private static Label? _messageTextField;
	private static Button? _yesButton;
	private static Button? _noButton;

	public Delete(String selectedPath)
	{
		Console.WriteLine($"Path { selectedPath }");
	
		Title = selectedPath;
		X = Pos.Center();
		Y = Pos.Center();
		Width = 40;
		Height = 8;

		ColorScheme =  new ColorScheme()
		{
			Normal = Application.Driver!.MakeColor(Color.BrightYellow, Color.DarkGray)
		};

		_messageTextField = new Label()
		{
			Text = "Are you sure you want\n to delete the file?",
			X = Pos.Center(),
			Y = 1
		};

		_yesButton = new Button()
		{
			Text = "Yes",
			X = Pos.Percent(30),
			Y = Pos.Bottom(_messageTextField) + 1
		};

		_noButton = new Button()
		{
			Text = "No",
			X = Pos.Right(_yesButton) + 1,
			Y = Pos.Bottom(_messageTextField) + 1,
			IsDefault = true
		};

		_noButton.Accept += (_,_) => CloseDialog();
		_yesButton.Accept += (_,_) => ConfirmDialog(selectedPath);
 
		Add(_messageTextField, _yesButton, _noButton);
	}

	private void ConfirmDialog(string path)
	{
		try
		{
			if(Directory.Exists(path))
			{
				// Directory.Delete(path, true);
			}
			else
			{
				// File.Delete(path);
			}

			Application.RequestStop();
		}
		catch(Exception e)
		{
			MessageBox.ErrorQuery("ERROR", e.Message, "OK");
		}
	}

	private void CloseDialog()
	{
		Application.RequestStop();
	}
}
