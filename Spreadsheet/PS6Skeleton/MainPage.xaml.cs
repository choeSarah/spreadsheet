using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
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
    public static Spreadsheet ss = new Spreadsheet(s => new Regex("^[A-Za-z][0-9]{0,2}$").IsMatch(s), s => s.ToUpper(), "ps6");

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
        nameBox.Text = "Cell: A1";
        valueBox.Text = "Value: " + ss.GetCellValue("A1");

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

    /// <summary>
    /// Method that is in charge of what is being displayed onto the grid
    /// </summary>
    /// <param name="grid"></param>
    private void displaySelection(ISpreadsheetGrid grid)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        spreadsheetGrid.GetValue(col, row, out string value);

        String cellName = crToCellName(col, row);

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();


        if (value == "") //if the cell is empty:
        {
            contentBox.Text = "";
            nameBox.Text = "Cell: " + cellName;
            valueBox.Text = "Value: " + value;

        }
        else //if the cell is not empty:
        {
            contentBox.Text = ss.GetCellContents(cellName).ToString();
            nameBox.Text = "Cell: " + cellName;

            if (ss.GetCellValue(cellName) is FormulaError)
            {
                valueBox.Text = "Value: #ERROR";

            }
            else
            {
                valueBox.Text = "Value: " + value;

            }
        }
    }

    /// <summary>
    /// Method that handles what to do with the content input from user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ContentEntryCompleted(object sender, EventArgs e)
    {
        spreadsheetGrid.GetSelection(out int col, out int row);
        String cellName = crToCellName(col, row);

        string enteredText = contentBox.Text;

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();

        try //try to set content of cell
        {
            SetCellContents(cellName, enteredText, row, col);

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

    private void SetCellContents(string cellName, string enteredText, int row, int col)
    {

        string oldContent = ss.GetCellContents(cellName).ToString();
        string oldValue = ss.GetCellValue(cellName).ToString();

        IList<string> dependees = ss.SetContentsOfCell(cellName, enteredText);
        string cellValue = ss.GetCellValue(cellName).ToString();

        //updating grid based on the dependencies
        foreach (string s in dependees)
        {
            string dependeeCellValue = ss.GetCellValue(s).ToString();

            int c = (int)s[0] - (int)'A';
            int r = int.Parse(s.Substring(1)) - 1;

            if (ss.GetCellValue(s) is FormulaError)
            {
                spreadsheetGrid.SetValue(c, r, "#ERROR");

            }
            else
            {
                spreadsheetGrid.SetValue(c, r, dependeeCellValue);

            }
        }


        if (ss.GetCellValue(cellName) is FormulaError) //if the value of the cell is a formulaError
        {
            spreadsheetGrid.SetValue(col, row, "#ERROR");
            contentBox.Text = enteredText;
            nameBox.Text = "Cell: " + cellName;
            valueBox.Text = "#ERROR";
        }
        else //if not
        {
            spreadsheetGrid.SetValue(col, row, cellValue);
            contentBox.Text = enteredText;
            nameBox.Text = "Cell: " + cellName;
            valueBox.Text = "Value: " + ss.GetCellValue(cellName);


        }
    }

    /// <summary>
    /// Method to handle clicking to open a new file
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void NewClicked(Object sender, EventArgs e)
    {
        if (ss.Changed == true) //if the spreadsheet has been changed
        {
            bool answer = await DisplayAlert("Are you sure?", "You're about to open a new file without saving", "Yes", "No");

            if (answer == true) //if user really wants to open a new file without saving
            {
                spreadsheetGrid.Clear();
                ss = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

                contentBox.Text = "";
                valueBox.Text = "Value: ";
            }
   
        } else //if spreadhsheet has not been changed
        {
            spreadsheetGrid.Clear();
            ss = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
        }
    }


    private async void SaveClicked(Object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Save to File", "Enter Filename");
        FileResult fileResult = await FilePicker.Default.PickAsync();
        string filePath = fileResult.FullPath;
        int IntToPath = filePath.LastIndexOf("\\");
        string filePathToSave = filePath.Substring(0, IntToPath + 1);
        filePathToSave = Path.Combine(filePathToSave, result);
        ss.Save(filePathToSave + ".sprd");
    }

    private async void HelpClicked(Object sender, EventArgs e)
    {
        await DisplayAlert("Help", "At the top of the spreadsheet, the cell name and value are displayed and cannot be changed.\n" +
            "Underneath them is the textbox for changing a cell’s content. \n\n" +
            "To select a cell, click on its placement in the grid.\n" +
            "To modify a cell, type the desired value into the content text box at the top.\n\n" +
            "To clear a Spreadsheet, click on “New” in the File menu.\n" +
            "To open an existing Spreadsheet, click on “Open” in the File menu.\n" +
            "To save a Spreadsheet, click on “Save” in the File menu and type in a name.\n\n" +
            "To split cell values across columns, select the cell and click on “Split”. \n" +
            "Based on how the cell values are initially combined, you may split them on commas, semicolons, period, or spaces. "
            , "Ok");

    }

    private void SplitC_Clicked(Object sender, EventArgs e)
    {
        Split_Clicked(0);
    }

    private void SplitS_Clicked(Object sender, EventArgs e)
    {
        Split_Clicked(1);
    }

    private void SplitP_Clicked(Object sender, EventArgs e)
    {
        Split_Clicked(2);
    }

    private void SplitSp_Clicked(Object sender, EventArgs e)
    {
        Split_Clicked(3);
    }

    /// <summary>
    /// Main method to handle the functionality split. Split allows the user to automatically split a cell's content across
    /// neighboring columns, but the same row. This splitting can follow one of 4 rules: splitting by a comma, semicolon, period, or a space.
    /// </summary>
    /// <param name="cOrS"></param>
    private void Split_Clicked(int cOrS)
    {
        spreadsheetGrid.GetSelection(out int col, out int row); //getting the cell
        string cellName = crToCellName(col, row); //getting the name of cell
        string cellContent = ss.GetCellContents(cellName).ToString(); //getting the content of the cell

        //if the cell content is not empty
        if (cellContent.Length > 0)
        {
            //split the content into tokens based on different rules:
            string[] splitString;
            if (cOrS == 0)
            {
                splitString = cellContent.Split(",");   
            } else if (cOrS == 1)
            {
                splitString = cellContent.Split(";");

            } else if (cOrS == 2) {
                splitString = cellContent.Split(".");

            } else
            {
                splitString = cellContent.Split(" ");

            }

            string newCellCol = cellName;


            //for each token
            foreach (string str in splitString)
            {
                try //set the neighborhood cells
                {
                    SetCellContents(newCellCol, str, row, col++);
                    newCellCol = ((char)(((int)newCellCol[0]) + 1)).ToString() + (row + 1);
                }
                catch (FormulaFormatException)
                {
                    DisplayAlert("ERROR", "Invalid formula: FormulaFormatException", "OK");
                    contentBox.Text = "";
                }
            }

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
            bool answer = await DisplayAlert("Are you sure?", "You're about to open without saving", "Yes", "No");

            if (answer == true)
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

                        spreadsheetGrid.SelectionChanged += displaySelection;
                        spreadsheetGrid.SetSelection(0, 0);
                        contentBox.Text = ss.GetCellContents("A1").ToString();
                        nameBox.Text = "Cell: A1";
                        valueBox.Text = "Value: " + ss.GetCellValue("A1").ToString();
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

                    spreadsheetGrid.SelectionChanged += displaySelection;
                    spreadsheetGrid.SetSelection(0, 0);
                    contentBox.Text = ss.GetCellContents("A1").ToString();
                    nameBox.Text = "Cell: A1";
                    valueBox.Text = "Value: " + ss.GetCellValue("A1").ToString();
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
