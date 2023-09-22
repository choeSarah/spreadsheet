using System;
using System.Diagnostics;
using SpreadsheetUtilities;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS;

/// <summary>
/// 
/// </summary>
public class Spreadsheet : AbstractSpreadsheet
{
    /// <summary>
    /// Class that represents a cell
    /// </summary>
    private class Cell
    {
        private object _content = "";
        private IEnumerable<string> varList;

        /// <summary>
        /// Constructor for the cell class
        /// </summary>
        /// <param name="obj">Takes an objectt as a parameter</param>
        internal Cell (object obj)
        {
            if (obj is Formula f) 
            {
                _content = f;
                varList = f. GetVariables();
            } else
            {
                _content = obj;
                varList = new List<string>();
            }
        }

        /// <summary>
        /// Property of the cell class; getter and setter for cell's content
        /// </summary>
        internal object Content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;

                if (value is Formula f)
                {
                    varList = f.GetVariables();
                } else
                {
                    varList = new List<string>();
                }
            }
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


    /// <summary>
    /// If name is invalid, throws an InvalidNameException.
    /// 
    /// Otherwise, returns the contents (as opposed to the value) of the named cell.
    /// The return value should be either a string, a double, or a Formula.
    /// </summary>
    public override object GetCellContents(string name)
    {
        if (IsLegalName(name)) //check if the name is valid
        {
            Cell? cell;

            if (cellToName.TryGetValue(name, out cell)) //if the cell has content
            {
                return cell.Content;
            } else //if the cell does not have content
            {
                return "";
            }

        } else
        {
            throw new InvalidNameException();
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
    /// Otherwise, the contents of the named cell becomes number.  The method returns a
    /// list consisting of name plus the names of all other cells whose value depends, 
    /// directly or indirectly, on the named cell.
    /// 
    /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    /// list {A1, B1, C1} is returned.
    /// </summary>
    public override IList<string> SetCellContents(string name, double number) //there is no variables here
    {
        if (!IsLegalName(name))
        {
            throw new InvalidNameException();
        }

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if cell is already named in the dictionary
        {
            var oldContent = cell.Content;

            cell.Content = number; //replace content
            List<string> varList = cell.VarList.ToList(); //get all the variables of the formula
            cellPairings.ReplaceDependees(name, varList); //replace the dependees
            List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get all the dependents of the cell
            return dependentList;

        }
        else
        {
            Cell newCell = new Cell(number); //create a new cell
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
        //if (IsLegalName(name)) //check if the name is a valid name
        //{
        //    Cell? cell;
        //    if (cellToName.TryGetValue(name, out cell)) //if the cell exists in the dictionary
        //    {
        //        cell.Content = number; //set the content as the param
        //        List<string> varList = cell.VarList.ToList();
        //        cellPairings.ReplaceDependees(name, varList);

        //        List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get the dependents
        //        return dependentList;
        //    } else //cell does not exist in dictionary
        //    {
        //        Cell newCell = new Cell(number); //create a new cell
        //        cellToName.Add(name, newCell); //add the cell to the dictionary
        //        List<string> dependentList = GetDirectIndirectDependents(name, newCell).ToList();//get the dependents
        //        return dependentList;
        //    }
        //} else
        //{
        //    throw new InvalidNameException();
        //}
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
    public override IList<string> SetCellContents(string name, string text)
    {

        if (IsLegalName(name)) //check if the name is valid
        {
            Cell? cell;

            if (cellToName.TryGetValue(name, out cell)) //if the cell is already filled
            {
                cell.Content = text; //assign content as param value
                List<string> varList = cell.VarList.ToList();
                cellPairings.ReplaceDependees(name, varList);

                if (text.Equals("")) //if the cell was deleted
                {
                    cellToName.Remove(name);
                }

                List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList();
                return dependentList;
            } else //cell doesnt already exist
            {
                Cell newCell = new Cell(text);
                if(!text.Equals(""))
                {
                    cellToName.Add(name, newCell);
                }

                List<string> dependentList = GetDirectIndirectDependents(name, newCell).ToList();
                return dependentList;
            }
        } else
        {
            throw new InvalidNameException();
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
    public override IList<string> SetCellContents(string name, Formula formula)
    {

        if (!IsLegalName(name))
        {
            throw new InvalidNameException();
        }

        Cell? cell;

        if (cellToName.TryGetValue(name, out cell)) //if cell is already named in the dictionary
        {
            var oldContent = cell.Content;

            try
            {
                cell.Content = formula; //replace content
                List<string> varList = cell.VarList.ToList(); //get all the variables of the formula
                cellPairings.ReplaceDependees(name, varList); //replace the dependees
                List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get all the dependents of the cell
                return dependentList;
            } catch (CircularException)
            {
                cell.Content = oldContent;
                List<string> dependentList = GetDirectIndirectDependents(name, cell).ToList(); //get all the dependents of the cell
                return dependentList;
            }

        } else
        {
            Cell newCell = new Cell(formula); //create a new cell
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
}

