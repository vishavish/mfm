using System.Collections.ObjectModel;
using Terminal.Gui;


public class App : Window
{
	public App()
	{
		Title = $"mfm - { Application.QuitKey } to quit";
		ColorScheme = new ColorScheme
		{
			Normal = Application.Driver!.MakeColor(Color.BrightGreen, Color.DarkGray)
		};

		var fileFrameView = new FrameView()
		{
			Y = 1,
			X = 1,
			Width = Dim.Percent(25),
			Height = Dim.Fill(),
			BorderStyle = LineStyle.None,
		};

		var fileListView = new ListView()
		{
			Width = Dim.Fill(),
			Height = Dim. Fill()	
		};

		var detailFrameView = new FrameView()
		{
			X = Pos.Right(fileFrameView),
			Width = Dim.Percent(75),
			Height = Dim.Fill()		
		};

		var detailView = new TextView()
		{
			Width = Dim.Fill(),
			Height = Dim.Fill(),
			ReadOnly = true,
			ColorScheme = new ColorScheme()
			{
				// Focus = Application.Driver.MakeColor(Color.Green, Color.DarkGray),
				Normal = Application.Driver.MakeColor(Color.Green, Color.Gray)
			}
		};

		fileListView.SetSource(GetDirectoriesAndFiles());
		fileListView.SelectedItemChanged += (s, e) => GetSelectedItem(fileListView, detailView);

		fileFrameView.Add(fileListView);
		detailFrameView.Add(detailView);

		Add(fileFrameView, detailFrameView);
	}

	private void GetSelectedItem(ListView fileList, TextView detailListView)
	{
		var selectedItem = fileList.SelectedItem;
		var files = GetDirectoriesAndFiles();
		string selectedPath = string.Empty;
		if (selectedItem >= 0 || selectedItem < files.Count())
		{
			selectedPath = files[selectedItem]!;
		}
		try
		{
			var info = new FileInfo(Path.Combine(Environment.CurrentDirectory, selectedPath));
			detailListView.Text = $"Name: {info.Name}\n" +
				$"Full Path: {info.FullName}\n" +
				$"Size: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "N/A (Directory)" : info.Length + " bytes")}\n" +
				$"Last Modified: {info.LastWriteTime}\n" +
				$"Type: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "Directory" : "File")}";
		}
		catch (Exception ex)
		{
			detailListView.Text = $"Error: {ex.Message}";
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
}
