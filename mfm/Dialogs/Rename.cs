using Utils;

namespace Dialogs;


public class Rename : Window
{
	private static TextField? _nameTextField;
	private static Button? _saveButton;
	private static Button? _cancelButton;

	private string _originalFileName = string.Empty;
	private string _originalFilePath = string.Empty;

	public Rename(string selectedPath)
	{
		Title = "Rename";
		X = Pos.Center();
		Y = Pos.Center();
		Width = 40;
		Height = 7;

		_originalFilePath =selectedPath;
		_originalFileName = Path.GetFileNameWithoutExtension(selectedPath);

		ColorScheme = new ColorScheme()
		{
			Normal = Application.Driver!.MakeColor(Color.BrightGreen, Color.DarkGray)	
		};

		_nameTextField = new TextField()
		{
			Text = Path.GetFileNameWithoutExtension(selectedPath),
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

		_saveButton.Accept += (_,_) => RenameFile();
		_cancelButton.Accept += (_,_) => CloseDialog();

		Add(_nameTextField, _saveButton, _cancelButton);
	}

	private void RenameFile()
	{
		string fileName = _nameTextField!.Text;
		bool isDir = FileHelper.IsDirectory(_originalFilePath);

		if(string.IsNullOrEmpty(fileName))
		{
			MessageBox.ErrorQuery("ERROR", "Invalid filename.", "OK");
			return;
		}

		// if(_originalFileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
		// {
		// 	MessageBox.Query("WARNING", "NO changes were made.", "OK");
		// 	return;
		// }

		try
		{
			if (isDir)
			{
				if(!Directory.Exists(fileName))
				{
					Directory.Move(_originalFilePath, Path.Combine(Environment.CurrentDirectory, fileName));
				}
			}
			else
			{
				if(!File.Exists(fileName))
				{
					File.Move(_originalFilePath, Path.Combine(Environment.CurrentDirectory, fileName + Path.GetExtension(_originalFilePath)));				
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

	private void CloseDialog()
	{
		Application.RequestStop();
	}
}
