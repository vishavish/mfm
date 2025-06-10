using Utils;

namespace Dialogs;


/* THIS IS A TEMPORARY *wink wink* APPROACH UNTIL i FIGURE OUT A BETTER WAY */

public class Copy : Dialog
{
	private static TextField? _nameTextField;
	private static Button? _saveButton;
	private static Button? _cancelButton;
	
	private string _originalFilePath = string.Empty;
	private bool _isMove = false;
	
	public Copy(string selectedPath, bool isMove = false)
	{
		_originalFilePath = selectedPath;
		_isMove = isMove;
	
		Title = "COPY";
		X = Pos.Center();
		Y = Pos.Center();
		Width = 50;
		Height = 10;

		ColorScheme = new ColorScheme()
		{
			Normal = Application.Driver!.MakeColor(Color.BrightYellow, Color.DarkGray)	
		};

		_nameTextField = new TextField()
		{
			Text = "",
			Y = 1,
			X = Pos.Center(),
			Width = 35			
		};

		_saveButton = new Button()
		{
			Text = "Save",
			Y = Pos.Bottom(_nameTextField) + 1,
			X = Pos.Percent(25)	
		};

		_cancelButton = new Button()
		{
			Text = "Cancel",
			Y = Pos.Bottom(_nameTextField) + 1,
			X = Pos.Right(_saveButton) + 1,
			IsDefault = true	
		};

		_saveButton.Accept += (_,_) => CopyFile();
		_cancelButton.Accept += (_,_) => CloseDialog();

		Add(_nameTextField, _saveButton, _cancelButton);
	}
	
	private void CloseDialog()
	{
		Application.RequestStop();
	}
	
	private void CopyFile()
	{
		string fileName = _nameTextField!.Text;
		bool isDir = FileHelper.IsDirectory(_originalFilePath);

		if(string.IsNullOrEmpty(fileName))
		{
			MessageBox.ErrorQuery("ERROR", "Invalid filename.", "OK");
			return;
		}
		
		try
		{
			if (!isDir)
			{
				if(!File.Exists(fileName))
				{
					if (_isMove)
						File.Move(_originalFilePath, fileName);
					else
						File.Copy(_originalFilePath, fileName);
				}
			}

			_nameTextField.Text = string.Empty;
			Application.RequestStop();
		}
		catch(Exception e)
		{
			MessageBox.ErrorQuery("ERROR", e.Message, "OK");
		}
	}
}
