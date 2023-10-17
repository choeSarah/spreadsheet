using SpreadsheetUtilities;
using SS;

namespace SpreadsheetGUI;

/// <summary>
/// Example of using a SpreadsheetGUI object
/// </summary>
public partial class MainPage : ContentPage
{
    private Spreadsheet ss = new Spreadsheet();

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
        //spreadsheetGrid.SetSelection(2, 3);
    }

    protected void OnSleep()
    {
        
    }

    /// <summary>
    /// Converts a col and row pairing into an cell name
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private string cellNameConverter (int c, int r)
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

        String cellName = cellNameConverter(col, row);

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();


        if (value == "")
        {
            contentBox.Text = "";
            nameValueBox.Text = "Cell: " + cellName + "; Value: " + value;
        }
        else
        {
            contentBox.Text = ss.GetCellContents(cellName).ToString();
            nameValueBox.Text = "Cell: " + cellName + "; Value: " + value;

        }
    }

    private void ContentEntryCompleted(object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        String cellName = cellNameConverter(col, row);

        string enteredText = contentBox.Text;

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();

        try //try to set content of cell
        {
            ss.SetContentsOfCell(cellName, enteredText);
            string cellValue = ss.GetCellValue(cellName).ToString();


            if (ss.GetCellValue(cellName) is FormulaError)
            {

                DisplayAlert("ERROR", "Invalid formula: FormulaError", "OK");
                contentBox.Text = oldContent;
                nameValueBox.Text = "Cell: " + cellName + "; Value: " + oldValue;

            }
            else
            {
                spreadsheetGrid.SetValue(col, row, cellValue);
                contentBox.Text = enteredText;
                nameValueBox.Text = "Cell: " + cellName + "; Value: " + ss.GetCellValue(cellName);

            }

        } catch (FormulaFormatException)
        {
            DisplayAlert("ERROR", "Invalid formula: FormulaFormatException", "OK");
            contentBox.Text = ""; 

        }
    }

    private void NewClicked(Object sender, EventArgs e)
    {
        spreadsheetGrid.Clear();
    }

    /// <summary>
    /// Opens any file as text and prints its contents.
    /// Note the use of async and await, concepts we will learn more about
    /// later this semester.
    /// </summary>
    private async void OpenClicked(Object sender, EventArgs e)
    {
        try
        {
            FileResult fileResult = await FilePicker.Default.PickAsync();
            if (fileResult != null)
            {
        Console.WriteLine( "Successfully chose file: " + fileResult.FileName );
        // for windows, replace Console.WriteLine statements with:
        //System.Diagnostics.Debug.WriteLine( ... );

        string fileContents = File.ReadAllText(fileResult.FullPath);
                Console.WriteLine("First 100 file chars:\n" + fileContents.Substring(0, 100));
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error opening file:");
            Console.WriteLine(ex);
        }
    }
}
