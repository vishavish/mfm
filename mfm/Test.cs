// using System.Collections.ObjectModel;
// using Terminal.Gui;

// Application.Init();
// var top = Application.Top;

// // Create a new window
// var win = new Window()
// {
//     Width = Dim.Fill(),
//     Height = Dim.Fill(),
// 	ColorScheme = new ColorScheme
// 	{
// 	    Normal = Application.Driver!.MakeColor(Color.Green, Color.Black) // Text color
// 	}
// };

// // Create a frame view for the file list
// var fileListFrame = new FrameView()
// {
//     Width = Dim.Percent(50),
//     Height = Dim.Fill()
// };

// // Create a list view for files and directories
// var fileList = new ListView
// {
//     Width = Dim.Fill(),
//     Height = Dim.Fill(),
// };

// // Create a frame view for the details
// var detailFrame = new FrameView()
// {
//     X = Pos.Right(fileListFrame),
//     Width = Dim.Percent(50),
//     Height = Dim.Fill()
// };

// // Create a text view for displaying details
// var detailView = new TextView
// {
// 	Width = Dim.Fill(),
// 	Height = Dim.Fill(),
// 	ReadOnly = true,
// 	ColorScheme = new ColorScheme
// 	{
// 	    Normal = Application.Driver.MakeColor(Color.White, Color.Blue), // Text color
// 	    Focus = Application.Driver.MakeColor(Color.Black, Color.Cyan)  // Focused text color
// 	}
// };

// string currentDirectory = Environment.CurrentDirectory;
// string[] files = Directory.GetFileSystemEntries(currentDirectory);
// ObservableCollection<string> obsFiles = new ObservableCollection<string>(files);
// // Load the current directory files
// fileList.SetSource(obsFiles);


// // Handle selection change
// fileList.SelectedItemChanged += (s,e) =>
// {
//     var selectedItem = fileList.SelectedItem;
//     if (selectedItem >= 0)
//     {
//         var selectedPath = files[selectedItem];
//         try
//         {
//             var info = new FileInfo(selectedPath);
//             detailView.Text = $"Name: {info.Name}\n" +
//                               $"Full Path: {info.FullName}\n" +
//                               $"Size: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "N/A (Directory)" : info.Length + " bytes")}\n" +
//                               $"Last Modified: {info.LastWriteTime}\n" +
//                               $"Type: {(info.Attributes.HasFlag(FileAttributes.Directory) ? "Directory" : "File")}";
//         }
//         catch (Exception ex)
//         {
//             detailView.Text = $"Error: {ex.Message}";
//         }
//     }
// };


// // Add views to frames
// fileListFrame.Add(fileList);
// detailFrame.Add(detailView);

// // Add frames to the main window
// win.Add(fileListFrame, detailFrame);
// top!.Add(win);

// // Run the application
// Application.Run();


