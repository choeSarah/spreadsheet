using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Foundation;
using JavaScriptCore;
using SpreadsheetUtilities;
using SS;
using UIKit;

namespace SpreadsheetGUI;

/// <summary>
/// Example of using a SpreadsheetGUI object
/// </summary>
public partial class MainPage : ContentPage
{
    public static Spreadsheet ss = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

    /// <summary>
    /// Constructor for the demo
    /// </summary>
    public MainPage()
    {
        InitializeComponent();

        // This an example of registering a method so that it is notified when
        // an event happens.  The SelectionChanged event is declared with a
        // delegate that specifies that all methods that register with it must
        // take a SpreadsheetGrid as its parameter and return nothing.  So we
        // register the displaySelection method below.
        spreadsheetGrid.SelectionChanged += displaySelection;
        spreadsheetGrid.SetSelection(0, 0);
        contentBox.Text = "";
        nameBox.Text = "A1";
        valueBox.Text = "";

    }


    /// <summary>
    /// Converts a col and row pairing into an cell name
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private string crToCellName (int c, int r)
    {
        r = r + 1;
        Dictionary<int, char> alphabet = new Dictionary<int, char>();
        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        int index = 0;

        foreach (char a in alpha)
        {
            alphabet.Add(index, a);
            index++;
        }

        Char col;
        String cellName;
        if (alphabet.TryGetValue(c, out col))
        {
            cellName = col + r.ToString();
        } else
        {
            cellName = c.ToString() + r.ToString();
        }

        return cellName;

    }

    private void displaySelection(ISpreadsheetGrid grid)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.GetValue(col, row, out string value);

        String cellName = crToCellName(col, row);

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();


        if (value == "")
        {
            contentBox.Text = "";
            nameBox.Text = cellName;
            valueBox.Text = value;

        }
        else
        {
            contentBox.Text = ss.GetCellContents(cellName).ToString();
            nameBox.Text = cellName;
            valueBox.Text = value;
            Console.WriteLine("Cell: " + cellName + "; Value: " + value);
        }
    }

    private void ContentEntryCompleted(object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        String cellName = crToCellName(col, row);

        string enteredText = contentBox.Text;

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();

        try //try to set content of cell
        {
            ss.SetContentsOfCell(cellName, enteredText);
            string cellValue = ss.GetCellValue(cellName).ToString();
            Console.WriteLine("Changed: " + ss.Changed);


            if (ss.GetCellValue(cellName) is FormulaError)
            {

                DisplayAlert("ERROR", "Invalid formula: FormulaError", "OK");
                contentBox.Text = oldContent;
                nameBox.Text = cellName;
                valueBox.Text = oldValue;
            }
            else
            {
                spreadsheetGrid.SetValue(col, row, cellValue);
                contentBox.Text = enteredText;
                nameBox.Text = cellName;
                valueBox.Text = "" + ss.GetCellValue(cellName);


            }

        } catch (FormulaFormatException)
        {
            DisplayAlert("ERROR", "Invalid formula: FormulaFormatException", "OK");
            contentBox.Text = oldContent; 

        } catch (CircularException)
        {
            DisplayAlert("ERROR", "Circular Dependency Detected", "OK");
            contentBox.Text = oldContent;
        }
    }

    private void OnButtonClicked(Object sender, EventArgs e)
    {
        string enteredInput = sortBox.Text;
        try
        {
            int selectRow = int.Parse(enteredInput) - 1;
            string[] rowValues = new string[26];

            for (int i=0; i< 26; i++)
            {
                if (spreadsheetGrid.GetValue(i, selectRow, out rowValues[i]))
                {}
            }


        } catch (Exception) {
            DisplayAlert("ERROR", "Please input a valid row", "OK");

        }

    }

    private void NewClicked(Object sender, EventArgs e)
    {
        spreadsheetGrid.Clear();
    }

    private async void SaveClicked (Object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Save to File", "Enter Filename");
        ss.Save(result);
    }

    private async void HelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Information", "select cell to modify \nmodify cell by using the top textbar.", "Ok");

    }

    private async void CloseClicked(Object sender, EventArgs e)
    {
        bool userInput = await DisplayAlert("Confirmation", "You're about to close without saving. Do you want to continue?", "Yes", "No");

        if (userInput)
        {
            System.Environment.Exit(0);
            Application.Current.Quit();
        } else
        {

        }
    }

    /// <summary>
    /// Opens any file as text and prints its contents.
    /// Note the use of async and await, concepts we will learn more about
    /// later this semester.
    /// </summary>
    private async void OpenClicked(Object sender, EventArgs e)
    {
        if (ss.Changed == true)
        {
            bool answer = await DisplayAlert("Question?", "You're about to open without saving", "Yes", "No");

            if (answer == true)
            {
                System.Environment.Exit(0);
            }
            else
            {
                try
                {
                    FileResult fileResult = await FilePicker.Default.PickAsync();
                    if (fileResult != null)
                    {
                        Console.WriteLine("Successfully chose file: " + fileResult.FileName);

                        string fileContents = File.ReadAllText(fileResult.FullPath);
                        Console.WriteLine("First 100 file chars:\n" + fileContents.Substring(0, 100));

                        ss = new Spreadsheet(fileResult.FullPath, s => true, s => s, "ps6");
                        foreach (string s in ss.CellToName.Keys)
                        {
                            int start = (int)'A';
                            int c = (int)s[0] - start;
                            int r = int.Parse(s.Substring(1)) - 1;

                            spreadsheetGrid.SetValue(c, r, ss.CellToName[s].StringForm);
                        }
                    }
                    else
                    {
                        await DisplayAlert("ERROR", "No File Selected", "OK");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("ERROR", "Error opening file", "OK");
                }
            }
        }
        else //if file has not changed
        {
            try
            {
                FileResult fileResult = await FilePicker.Default.PickAsync();
                if (fileResult != null)
                {
                    Console.WriteLine("Successfully chose file: " + fileResult.FileName);

                    string fileContents = File.ReadAllText(fileResult.FullPath);
                    Console.WriteLine("First 100 file chars:\n" + fileContents.Substring(0, 100));

                    ss = new Spreadsheet(fileResult.FullPath, s => true, s => s, "ps6");
                    foreach (string s in ss.CellToName.Keys)
                    {
                        int start = (int)'A';
                        int c = (int)s[0] - start;
                        int r = int.Parse(s.Substring(1)) - 1;

                        spreadsheetGrid.SetValue(c, r, ss.CellToName[s].StringForm);
                    }
                }
                else
                {
                    await DisplayAlert("ERROR", "No File Selected", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("ERROR", "Error opening file", "OK");
            }
        }
    }
}
