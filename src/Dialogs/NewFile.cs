namespace Dialogs;


public class NewFile : Window
{
	private static TextField? _fileTextField;
	private static Button? _confirmButton;
	private static Button? _closeButton;
	
	public NewFile(string path)
	{
		Title = "NEW";
		X = Pos.Center();
		Y = Pos.Center();
        Width = 40;
        Height = 7;

		ColorScheme = new ColorScheme
		{
			Normal = Application.Driver!.MakeColor(Color.BrightYellow, Color.DarkGray)
		};

        _fileTextField = new TextField
        {
            X = Pos.Center(),
            Y = 1,
            Width = 35
        };

        _confirmButton = new Button()
        {
            Text = "Add",
            X = Pos.Percent(25),
            Y = Pos.Bottom(_fileTextField) + 1,
			IsDefault = true
        };

		_closeButton = new Button()
		{
			Text = "Cancel",
			X = Pos.Right(_confirmButton) + 1,
			Y = Pos.Bottom(_fileTextField) + 1,
		};

        _confirmButton.Accept += (_, _) => AddNew(path, _fileTextField.Text);
		_closeButton.Accept  += (_,_) => CloseDialog();
		
        Add(_fileTextField, _confirmButton, _closeButton);
	}

	private void AddNew(string parentDir, string file)
	{
		string newFile = Path.Combine(parentDir, file);
		if (File.Exists(newFile))
		{
			MessageBox.ErrorQuery("ERROR", "File already exists.", "OK");
			return;
		}

		try
		{
			if(string.IsNullOrWhiteSpace(Path.GetExtension(newFile)))
			{
				Directory.CreateDirectory(newFile);
			}
			else
			{
				File.Create(newFile);
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
