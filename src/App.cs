using System.Collections.ObjectModel;
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

	private static string _selectedPath = "";
	private static string _parentDirectory = "";


	public App()
	{
		ColorScheme = new ColorScheme
		{
			Normal = Application.Driver!.MakeColor(Color.BrightYellow, Color.DarkGray)
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
			new(Key.N.WithAlt, "New", () => NewFile()),
			new(Key.R.WithAlt, "Rename", () => Rename()),
			new(Key.D.WithAlt, "Delete", () => Delete()),
			
			// Using CTRL here because it conflicts with my komorebi keybindings
			new(Key.C.WithCtrl, "Copy", () => Copy()), 
			new(Key.M.WithCtrl, "Move", () => Move()), 
			
			new(Application.QuitKey, "Quit", () => Application.RequestStop()),
		});
		
		_parentDirectory = Environment.CurrentDirectory;

		_fileListView.SetSource(GetDirectoriesAndFiles());
		_fileListView.KeystrokeNavigator.Collection = Enumerable.Empty<string>().ToList();
		_fileListView.SelectedItemChanged += async (_, _) => await GetSelectedItem();
		_fileListView.OpenSelectedItem += (_, _)  => Open();

		_fileFrameView.Add(_fileListView);
		_fileContentFrameView.Add(_fileContentTextView);
		_fileDetailFrameView.Add(_fileDetailTextView);

		Add(_fileFrameView, _fileContentFrameView, _fileDetailFrameView, _statusBar);
	}

	private void Open()
	{
		var index = _fileListView!.SelectedItem;
		var selectedFile = _fileListView.Source.ToList()[index]?.ToString();
		_selectedPath = selectedFile!.Equals(".") ?	"../" :	selectedFile;
							
		var path = Path.GetFullPath(Path.Combine(_parentDirectory, _selectedPath));
		if (FileHelper.IsDirectory(path))
		{
			_parentDirectory  = Path.Combine(_parentDirectory, _selectedPath);
			_fileListView.SetSource(GetDirectoriesAndFiles(_parentDirectory));
		}
	}
	
	private async Task GetSelectedItem()
	{
		var index = _fileListView!.SelectedItem;
		var files = _fileListView.Source.ToList();
		string selectedPath = string.Empty;
		if (index >= 0 || index < files.Count)
		{
			selectedPath = files[index]?.ToString() ?? "";
		}
		
		try
		{
			selectedPath = Path.Combine(_parentDirectory, selectedPath);
			_selectedPath= Path.Combine(_parentDirectory, selectedPath);
			var info = new FileInfo(selectedPath);
			_fileDetailTextView!.Text = $"Name: {info.Name}\n" +
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

	private ObservableCollection<string?> GetDirectoriesAndFiles(string dirPath = "")
	{
		List<string?> listOfFilesAndDirs = new();

		var homeDir = string.IsNullOrEmpty(dirPath) ? Environment.CurrentDirectory : dirPath ;
		string?[] directories = Directory.GetDirectories(homeDir)
							.Select(d => new DirectoryInfo(d).Name)
							.ToArray();
		string[] files = Directory.GetFiles(homeDir)
							.Select(f => Path.GetFileName(f))
							.ToArray();

		listOfFilesAndDirs.Add(".");
		listOfFilesAndDirs.AddRange(directories.Order());
		listOfFilesAndDirs.AddRange(files.Order());
			
		return new ObservableCollection<string?>(listOfFilesAndDirs);
	}

	private void Delete()
	{
		string path = string.IsNullOrEmpty(_selectedPath) ? _parentDirectory : _selectedPath;
		Application.Run(new Delete(path)); 
		_fileListView!.SetSource(GetDirectoriesAndFiles(_parentDirectory));
	}

	private void NewFile()
	{
		Application.Run(new NewFile(_parentDirectory));
		_fileListView!.SetSource(GetDirectoriesAndFiles(_parentDirectory));
	}

	private void Rename()
	{
		Application.Run(new Rename(_selectedPath, _parentDirectory));
		_fileListView!.SetSource(GetDirectoriesAndFiles(_parentDirectory));
	}

	private void Copy()
	{
		Application.Run(new Copy(_selectedPath, false));
		_fileListView!.SetSource(GetDirectoriesAndFiles(_parentDirectory));
	}
	
	private void Move()
	{
		Application.Run(new Copy(_selectedPath, true));
		_fileListView!.SetSource(GetDirectoriesAndFiles(_parentDirectory));
	}
}
