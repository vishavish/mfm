using System.Collections.ObjectModel;
using Terminal.Gui;


public class App : Window
{
	public App()
	{
		Title = $"Minimalist File Manager - { Application.QuitKey } to quit";
		// DefaultBorderStyle = LineStyle.None;
		ColorScheme = new ColorScheme
		{
			Normal = Application.Driver!.MakeColor(Color.BrightGreen, Color.DarkGray)
		};

		var fileFrameView = new FrameView()
		{
			Y = 1,
			Title = "Test",
			Width = Dim.Percent(50),
			Height = Dim.Fill(),
		};

		var fileListView = new ListView()
		{
			Width = Dim.Fill(),
			Height = Dim. Fill()	
		};

		var detailFrameView = new FrameView()
		{
			Title = "Test1",
			X = Pos.Right(fileFrameView),
			Y = 1,
			Width = Dim.Percent(50),
			Height = Dim.Fill()	
		};

		var detailView = new ListView()
		{
			Width = Dim.Fill(),
			Height = Dim.Fill()	
		};


		var homeDir = Environment.CurrentDirectory;
		string?[] directories = Directory.GetDirectories(homeDir)
							.Select(d => new DirectoryInfo(d).Name).ToArray();
		string[] files = Directory.GetFiles(homeDir)
							.Select(f => Path.GetFileName(f)).ToArray();

		fileListView.SetSource(GetDirectoriesAndFiles(directories, files));

		fileFrameView.Add(fileListView);
		detailFrameView.Add(detailView);

		Add(fileFrameView, detailFrameView);
	}

	private ObservableCollection<string?> GetDirectoriesAndFiles(
		string?[] dirs, string[] files)
	{
		List<string?> listOfFilesAndDirs = new();

		listOfFilesAndDirs.AddRange(dirs);
		listOfFilesAndDirs.AddRange(files);
			
		return new ObservableCollection<string?>(listOfFilesAndDirs);
	}
}
