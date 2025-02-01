using System.Collections.ObjectModel;
using Terminal.Gui;
using Utils;


public class App : Window
{
	private static FrameView? _fileFrameView;
	private static ListView? _fileListView;
	private static FrameView? _fileDetailFrameView;
	private static TextView? _fileDetailTextView;
	private static FrameView? _fileContentFrameView;
	private static TextView? _fileContentTextView;
	private static StatusBar? _statusBar;

	public App()
	{
		Title = "mfm";
		ColorScheme = new ColorScheme
		{
			Normal = Application.Driver!.MakeColor(Color.BrightGreen, Color.DarkGray)
		};

		//Frame for the list of files and directories
		_fileFrameView = new FrameView()
		{
			Y = 1,
			X = 1,
			Width = Dim.Percent(25),
			Height = Dim.Fill(),
			BorderStyle = LineStyle.None,
		};

		//View for the fileFrame
		_fileListView = new ListView()
		{
			Width = Dim.Fill(),
			Height = Dim. Fill()	
		};

		//Frame for the text contents of the file
		_fileContentFrameView = new FrameView()
		{
			X = Pos.Right(_fileFrameView),
			// Y = 
			Width = Dim.Percent(75),
			Height = Dim.Percent(75),
		};

		//TextView for the fileTextFrame
		_fileContentTextView = new TextView()
		{
			Width = Dim.Fill(),
			Height = Dim.Fill(),
			ReadOnly = true,
			ColorScheme = new ColorScheme()
			{
				Normal = Application.Driver.MakeColor(Color.Green, Color.Gray)
			}
		};

		//Frame for the selected file details
		_fileDetailFrameView = new FrameView()
		{
			X = Pos.Right(_fileFrameView),
			Y = Pos.Bottom(_fileContentFrameView),
			Width = Dim.Percent(75),
			Height = Dim.Percent(25)		
		};

		//TextView for the file details frame
		_fileDetailTextView = new TextView()
		{
			Width = Dim.Fill(),
			Height = Dim.Fill(),
			ReadOnly = true,
			ColorScheme = new ColorScheme()
			{
				Normal = Application.Driver.MakeColor(Color.Green, Color.Gray)
			}
		};

		_statusBar = new StatusBar(new Shortcut[]
		{
			new(Key.N.WithAlt, "New", () => MessageBox.Query("Dialog", "TODO:: LATER")),
			new(Key.R.WithAlt, "Rename", () => MessageBox.Query("Dialog", "TODO:: LATER")),
			new(Key.D.WithAlt, "Delete", () => DeleteConfirm()),
			new(Key.C.WithAlt, "Copy", () => MessageBox.Query("Dialog", "TODO:: LATER")),
			new(Key.M.WithAlt, "Move", () => MessageBox.Query("Dialog", "TODO:: LATER")),
			new(Application.QuitKey, "Quit", () => Application.RequestStop()),
		});

		_fileListView.SetSource(GetDirectoriesAndFiles());
		_fileListView.KeystrokeNavigator.Collection = Enumerable.Empty<string>().ToList();
		_fileListView.SelectedItemChanged += async (s, e) => await GetSelectedItem();

		_fileFrameView.Add(_fileListView);
		_fileContentFrameView.Add(_fileContentTextView);
		_fileDetailFrameView.Add(_fileDetailTextView);

		Add(_fileFrameView, _fileContentFrameView, _fileDetailFrameView, _statusBar);
	}

	private async Task GetSelectedItem()
	{
		var selectedItem = _fileListView!.SelectedItem;
		var files = GetDirectoriesAndFiles();
		string selectedPath = string.Empty;
		if (selectedItem >= 0 || selectedItem < files.Count())
		{
			selectedPath = files[selectedItem]!;
		}
		try
		{
			var info = new FileInfo(Path.Combine(Environment.CurrentDirectory, selectedPath));
			_fileDetailTextView!.Text = $"Name: {info.Name}\n" +
				// $"Full Path: {info.FullName}\n" +
				$"Size: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "N/A (Directory)" : info.Length + " bytes")}\n" +
				$"Last Modified: {info.LastWriteTime}\n" +
				$"Type: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "Directory" : "File")}";

			if(!info.Attributes.HasFlag(FileAttributes.Directory) && !FileHelper.IsBinary(info.FullName))
				_fileContentTextView!.Text = await FileHelper.ReadFileContents(info.FullName);
			else
				_fileContentTextView!.Text = "\n\n\n\n\t\t\t<Not Supported>\n" +
										$"\t\t\tTemporary until TextAlignment is fixed :)";
		}
		catch (Exception ex)
		{
			_fileDetailTextView!.Text = $"Error: {ex.Message}\n";
		}
	}

	private ObservableCollection<string?> GetDirectoriesAndFiles()
	{
		List<string?> listOfFilesAndDirs = new();

		var homeDir = Environment.CurrentDirectory;
		string?[] directories = Directory.GetDirectories(homeDir)
							.Select(d => new DirectoryInfo(d).Name).ToArray();
		string[] files = Directory.GetFiles(homeDir)
							.Select(f => Path.GetFileName(f)).ToArray();

		listOfFilesAndDirs.AddRange(directories);
		listOfFilesAndDirs.AddRange(files);
			
		return new ObservableCollection<string?>(listOfFilesAndDirs);
	}

	private void DeleteConfirm()
	{
		var prompt = MessageBox.Query(30, 5,
                "Delete?", "Are you sure to delete the file?",
                "Yea", "Nah", "Cancel");

            switch (prompt)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    return;
            }
	}
}
