using System;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection.Emit;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Microsoft.VisualBasic;
using SpreadsheetUtilities;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS;

/// <summary>
/// Class that represents a Spreadsheet object
/// </summary>
public class Spreadsheet : AbstractSpreadsheet
{
    /// <summary>
    /// Class that represents a cell
    /// </summary>
    public class Cell
    {
        private object _content = ""; //stores the content of the cell
        private object _value = ""; //stores the value of the cell
        private string stringForm = ""; //stores the stringform of the cell
        private IEnumerable<string> varList; //stores the variable list of a Formula
        private Func<string, double> lookup; //stores the lookup delegate

        [JsonConstructor]
        public Cell(string stringForm)
        {
            this.stringForm = stringForm;
            varList = new List<string>();
            lookup = JsonEvaluator;
        }

        private double JsonEvaluator(string name)
        {
            return 0;
        }

        /// <summary>
        /// Constructor for the cell class
        /// </summary>
        /// <param name="obj">Takes an object as a parameter</param>
        ///

        internal Cell(object obj, Func<string, double> variableEvaluator)
        {
            lookup = variableEvaluator;
            if (obj is Formula f)
            {
                stringForm += "=" + f.ToString();
                _content = f;
                _value = f.Evaluate(lookup);
                varList = f.GetVariables();
            }
            else if (obj is double d)
            {
                stringForm += d.ToString();
                _content = d;
                _value = d;
                varList = new List<string>();
            }
            else
            {
                stringForm += obj.ToString();
                _content = obj;
                _value = obj;
                varList = new List<string>();
            }

        }

        ///// <summary>
        ///// Property of the cell class; getter and setter for cell's stringform
        ///// </summary>
        //public object StringForm
        //{
        //    get { return stringform; }
        //}

        /// <summary>
        /// Property of the cell class; getter and setter for cell's content
        /// </summary>
        internal object Content
        {
            get { return _content; }

            set
            {
                _content = value;

                if (value is Formula f)
                {
                    varList = f.GetVariables();
                    _value = f.Evaluate(lookup);
                    stringForm = "=" + f.ToString();
                }
                else
                {
                    varList = new List<string>();
                    _value = value;
                    if (value is not null)
                    {
                        stringForm = value.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Property of the cell class; getter and setter for cell's value
        /// </summary>
        internal object Value
        {
            get { return _value; }

            set
            {

                if (value is Formula f)
                {
                    //varList = f.GetVariables();
                    _value = f.Evaluate(lookup);
                }
                else
                {
                    //varList = new List<string>();
                    _value = value;
                }
            }
        }

        [JsonPropertyName("StringForm")]
        public string StringForm
        {
            get { return stringForm; }

        }

        /// <summary>
        /// Property of the cell class; gettter and setter for cell's varList
        /// </summary>
        internal IEnumerable<string> VarList
        {
            get
            {
                return varList;
            }
        }
    }

    private Dictionary<string, Cell> cellToName = new Dictionary<string, Cell>(); //meant to track cells and names
    private DependencyGraph cellPairings = new DependencyGraph(); //meant to track cell relationships
    private readonly Func<string, string> normalizer; //stores the normalizer 

    private readonly Func<string, bool> validator; //stores the validator

    private string version;

    private string pathName;

    [JsonPropertyName("Cells")]
    public Dictionary<string, Cell> CellToName
    {
        get { return cellToName; }
    }

    //Creates an empty spreadsheet that imposes no extra validity conditions,
    //normalizes every cell name to itself, and has version "default".
    public Spreadsheet() : base("default")
    {
        this.pathName = "";
        this.normalizer = s => s;
        this.validator = s => true;
        this.version = "default";
    }

    //Creates an empty spreadsheet. However, allows the user to provide a validity delegate (first parameter),
    //a normalization delegate (second parameter), and a version (third parameter).
    public Spreadsheet(Func<string, bool> validator, Func<string, string> normalizer, string version) : base(version)
    {
        this.pathName = "";
        this.normalizer = normalizer;
        this.validator = validator;
        this.version = version;
    }

    [JsonConstructor]
    public Spreadsheet(Dictionary<string, Cell> celltoname, string version) : base(version)
    {
        this.pathName = "";
        this.normalizer = s => s;
        this.validator = s => true;
        this.cellToName = celltoname;
        this.version = version;
    }

    // Allows the user to provide a string representing a path to a file (first parameter), a validity delegate (second parameter),
    // a normalization delegate (third parameter), and a version (fourth parameter).
    // Reads a saved spreadsheet from the file and use it to construct a new spreadsheet.
    // The new spreadsheet should use the provided validity delegate, normalization delegate, and version.
    public Spreadsheet(string pathName, Func<string, bool> validator, Func<string, string> normalizer, string version) : base(version)
    {
        this.pathName = pathName;
        this.normalizer = normalizer;
        this.validator = validator;
        this.version = version;

        Changed = false;

        try
        {
            string fileContent = File.ReadAllText(pathName);

            try
            {
                Spreadsheet? rebuild = JsonSerializer.Deserialize<Spreadsheet>(fileContent);

                if (rebuild != null)
                {
                    Dictionary<string, Cell>? rebuildDictionary = rebuild.CellToName;

                    if (this.version != rebuild.Version)
                    {
                        throw new SpreadsheetReadWriteException("Saved version does not match given version");
                    }

                    foreach (string s in rebuildDictionary.Keys)
                    {
                        if (!IsLegalName(s) | !this.validator(s))
                        {
                            throw new SpreadsheetReadWriteException("Input includes an invalid name");

                        }

                        string newContent = rebuildDictionary[s].StringForm;
                        try
                        {
                            SetContentsOfCell(s, newContent);
                        }
                        catch (CircularException)
                        {
                            throw new SpreadsheetReadWriteException("Input includes a circular dependency or an invalid formula");
                        }
                    }
                }


            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Had an issue with opening the file");

            }
        }
        catch (DirectoryNotFoundException) //FIXED
        {
            throw new SpreadsheetReadWriteException("Had an issue with reading the file");

        }
    }

    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, returns the contents (as opposed to the value) of the named cell.
    /// The return value should be either a string, a double, or a Formula.
    /// </summary>
    public override object GetCellContents(string name)
    {
        name = normalizer(name); //normalizes the name
        if (!IsLegalName(name) | !validator(name)) //if the normalized name is no longer a legal or valid name
        {
            throw new InvalidNameException();
        }

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if the cell has content
        {
            return cell.Content;
        }
        else //if the cell does not have content
        {
            return "";
        }
    }

    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
    /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
    /// </summary>
    public override object GetCellValue(string name)
    {
        name = normalizer(name); //normalizes the name
        if (!IsLegalName(name) | !validator(name)) //if the normalized name is no longer a legal or valid name
        {
            throw new InvalidNameException();
        }

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //checks if the cell is in the dictionary
        {
            if (cell.Content is Formula)
            {
                Formula f = (Formula)cell.Content;
                cell.Value = f.Evaluate(VariableEvaluator);
                return cell.Value;
            } else
            {
                return cell.Value;

            }
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// Enumerates the names of all the non-empty cells in the spreadsheet.
    /// </summary>
    public override IEnumerable<string> GetNamesOfAllNonemptyCells()
    {
        return cellToName.Keys;
    }

    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, if content parses as a double, the contents of the named
    /// cell becomes that double.
    /// 
    /// Otherwise, if content begins with the character '=', an attempt is made
    /// to parse the remainder of content into a Formula f using the Formula
    /// constructor.  There are then three possibilities:
    /// 
    ///   (1) If the remainder of content cannot be parsed into a Formula, a 
    ///       SpreadsheetUtilities.FormulaFormatException is thrown.
    ///       
    ///   (2) Otherwise, if changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown,
    ///       and no change is made to the spreadsheet.
    ///       
    ///   (3) Otherwise, the contents of the named cell becomes f.
    /// 
    /// Otherwise, the contents of the named cell becomes content.
    /// 
    /// If an exception is not thrown, the method returns a list consisting of
    /// name plus the names of all other cells whose value depends, directly
    /// or indirectly, on the named cell. The order of the list should be any
    /// order such that if cells are re-evaluated in that order, their dependencies 
    /// are satisfied by the time they are evaluated.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list {A1, B1, C1} is returned.
    /// </summary>
    public override IList<string> SetContentsOfCell(string name, string content)
    {
        Changed = true; //FIXED
        name = normalizer(name); //normalizes the name

        if (!IsLegalName(name) || !validator(name)) //if the normalized name is no longer a legal or valid name
        {
            throw new InvalidNameException();
        }

        double d;

        if (Double.TryParse(content, out d) && !content.Contains(",")) //if content is a double and an edge case for commas
        {
            Console.WriteLine("is double");
            return SetCellContents(name, d);

        }
        else if (content.StartsWith("=")) //if content is a formula
        {
            try //attempts to read the string into a formula
            {
                Formula f = new Formula(content.Substring(1), normalizer, validator);
                return SetCellContents(name, f);
            }
            catch (FormulaFormatException)
            {
                throw new FormulaFormatException("Content could not be parsed into a Formula");
            }

        }
        else //if content is just a string
        {
            return SetCellContents(name, content);
        }
    }

    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, the contents of the named cell becomes number.  The method returns a
    /// list consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list {A1, B1, C1} is returned.
    /// </summary>
    protected override IList<string> SetCellContents(string name, double number) //there is no variables here
    {

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if cell is already named in the dictionary
        {
            var oldContent = cell.Content;
            var oldValue = cell.Value;

            cell.Content = number; //replace content
            cell.Value = number;
            List<string> varList = cell.VarList.ToList(); //get all the variables of the formula
            cellPairings.ReplaceDependees(name, varList); //replace the dependees
            List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get all the dependents of the cell

            if (dependentList.Count > 0)
            {
                for (int i = 1; i < dependentList.Count; i++)
                {
                    Cell? childCell;

                    if (cellToName.TryGetValue(dependentList[i], out childCell))
                    {
                        string childNewStringForm = childCell.StringForm.Substring(1);
                        string childFinalStringForm = childNewStringForm.Replace(name, number.ToString());
                        var pValue = new Formula(childFinalStringForm).Evaluate(VariableEvaluator);

                        if (pValue is not FormulaError)
                        {
                            childCell.Value = pValue;
                        }
                    }
                }
            }

            return dependentList;

        }
        else
        {
            Cell newCell = new Cell(number, VariableEvaluator); //create a new cell
            cellToName.Add(name, newCell); //add it to the dictionary

            //dependency-checking
            List<string> newVarList = newCell.VarList.ToList(); //get all the variables of the formula
            foreach (string s in newVarList)
            {
                cellPairings.AddDependency(s, name); //add the pairing

            }

            List<string> dependentList = GetDirectIndirectDependents(name, newCell).ToList(); //get all the dependents of the cell

            if (dependentList.Count > 0)
            {
                for (int i = 1; i < dependentList.Count; i++)
                {
                    Cell? childCell;

                    if (cellToName.TryGetValue(dependentList[i], out childCell))
                    {
                        string childNewStringForm = childCell.StringForm.Substring(1);
                        string childFinalStringForm = childNewStringForm.Replace(name, number.ToString());
                        var pValue = new Formula(childFinalStringForm).Evaluate(VariableEvaluator);

                        if (pValue is not FormulaError)
                        {
                            childCell.Value = pValue;
                        }
                    }
                }
            }

            return dependentList;
        }
    }

    /// <summary>
    /// The method returns a hashset consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    private HashSet<string> GetDirectIndirectDependents(string name, Cell cell)
    {
        HashSet<string> allDependents = new HashSet<string>();

        //getting dependents
        IEnumerable<string> indirectDependents = GetCellsToRecalculate(name);

        foreach (string s in indirectDependents)
        {
            allDependents.Add(s);
        }

        return allDependents;
    }


    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, the contents of the named cell becomes text.  The method returns a
    /// list consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list {A1, B1, C1} is returned.
    /// </summary>
    protected override IList<string> SetCellContents(string name, string text)
    {
        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if the cell is already filled
        {
            cell.Content = text; //assign content as param value
            cell.Value = text;
            List<string> varList = cell.VarList.ToList();
            cellPairings.ReplaceDependees(name, varList);

            if (text.Equals("")) //if the cell was deleted
            {
                cellToName.Remove(name);
            }

            List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList();
            return dependentList;
        }
        else //cell doesnt already exist
        {
            Cell newCell = new Cell(text, VariableEvaluator);
            if (!text.Equals(""))
            {
                cellToName.Add(name, newCell);
            }

            List<string> dependentList = GetDirectIndirectDependents(name, newCell).ToList();
            return dependentList;
        }
    }

    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
    /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
    /// 
    /// Otherwise, the contents of the named cell becomes formula.  The method returns a
    /// list consisting of name plus the names of all other cells whose value depends,
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list {A1, B1, C1} is returned.
    /// </summary>
    protected override IList<string> SetCellContents(string name, Formula formula)
    {

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if cell is already named in the dictionary
        {
            var oldContent = cell.Content;
            var oldValue = cell.Value;

            try
            {
                cell.Content = formula; //replace content
                cell.Value = formula.Evaluate(VariableEvaluator);
                List<string> varList = cell.VarList.ToList(); //get all the variables of the formula
                cellPairings.ReplaceDependees(name, varList); //replace the dependees
                List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get all the dependents of the cell

                return dependentList;
            }
            catch (CircularException)
            {
                cell.Content = oldContent;
                cell.Value = oldValue;

                throw new CircularException();
            }

        }
        else
        {
            try
            {
                Cell newCell = new Cell(formula, VariableEvaluator); //create a new cell
                cellToName.Add(name, newCell); //add it to the dictionary

                //dependency-checking
                List<string> newVarList = newCell.VarList.ToList(); //get all the variables of the formula
                foreach (string s in newVarList)
                {
                    cellPairings.AddDependency(s, name); //add the pairing

                }

                List<string> dependentList = GetDirectIndirectDependents(name, newCell).ToList(); //get all the dependents of the cell
                return dependentList;
            }
            catch (CircularException)
            {
                cellToName.Remove(name);
                throw new CircularException();
            }
        }
    }

    /// <summary>
    /// Returns an enumeration, without duplicates, of the names of all cells whose
    /// values depend directly on the value of the named cell.  In other words, returns
    /// an enumeration, without duplicates, of the names of all cells that contain
    /// formulas containing name.
    /// 
    /// For example, suppose that
    /// A1 contains 3
    /// B1 contains the formula A1 * A1
    /// C1 contains the formula B1 + A1
    /// D1 contains the formula B1 - C1
    /// The direct dependents of A1 are B1 and C1
    /// </summary>
    protected override IEnumerable<string> GetDirectDependents(string name)
    {
        return cellPairings.GetDependents(name);

    }

    /// <summary>
    /// Method that checks if the cell names are valid
    /// </summary>
    /// <param name="s">A name</param>
    /// <returns>A boolean</returns>
    private static bool IsLegalName(string s)
    {
        if (string.IsNullOrEmpty(s))
        {
            return false;
        }

        //Variables must consist of a letter or underscore
        if (s[0] == '_' || Char.IsLetter(s[0])) { }
        else
        {
            return false;
        }

        for (int i = 1; i < s.Length; i++)
        {
            if (!(Char.IsLetterOrDigit(s[i]) || s[i] == '_'))
            {
                return false;
            }
        }


        return true;
    }

    /// <summary>
    /// Writes the contents of this spreadsheet to the named file using a JSON format.
    /// The JSON object should have the following fields:
    /// "Version" - the version of the spreadsheet software (a string)
    /// "Cells" - a data structure containing 0 or more cell entries
    ///           Each cell entry has a field (or key) named after the cell itself 
    ///           The value of that field is another object representing the cell's contents
    ///               The contents object has a single field called "StringForm",
    ///               representing the string form of the cell's contents
    ///               - If the contents is a string, the value of StringForm is that string
    ///               - If the contents is a double d, the value of StringForm is d.ToString()
    ///               - If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    /// 
    /// For example, if this spreadsheet has a version of "default" 
    /// and contains a cell "A1" with contents being the double 5.0 
    /// and a cell "B3" with contents being the Formula("A1+2"), 
    /// a JSON string produced by this method would be:
    /// 
    /// {
    ///   "Cells": {
    ///     "A1": {
    ///       "StringForm": "5"
    ///     },
    ///     "B3": {
    ///       "StringForm": "=A1+2"
    ///     }
    ///   },
    ///   "Version": "default"
    /// }
    /// 
    /// If there are any problems opening, writing, or closing the file, the method should throw a
    /// SpreadsheetReadWriteException with an explanatory message.
    /// </summary>
    public override void Save(string filename)
    {
        string spreadsheetContent = JsonSerializer.Serialize(this);
        
        try
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(pathName, filename)))
            {
                outputFile.WriteLine(spreadsheetContent);
                Changed = false;//FIXED
            }
        }
        catch (Exception)
        {
            throw new SpreadsheetReadWriteException("Had an issue with writing to the file");
        }

    }

    /// <summary>
    /// Given a cell name, 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="InvalidNameException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private double VariableEvaluator(string name)
    {
        name = normalizer(name);

        if (!IsLegalName(name) | !validator(name))
        {
            throw new InvalidNameException();
        }
        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if there is such a cell
        {
            double d;
            if (Double.TryParse(cell.Value.ToString(), out d)) //if the content of the cell is a double
            {
                return d;
            }
            else
            {
                throw new ArgumentException();
            }
        }
        else //cell doesn't have a value
        {
            throw new ArgumentException();
        }
    }

}