// Skeleton implementation by: Joe Zachary, Daniel Kopta, Travis Martin for CS 3500
// Last updated: August 2023 (small tweak to API)

using System;
using System.Collections;
using System.Collections.Generic;

namespace SpreadsheetUtilities;

/// <summary>
/// (s1,t1) is an ordered pair of strings
/// t1 depends on s1; s1 must be evaluated before t1
/// 
/// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
/// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
/// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
/// set, and the element is already in the set, the set remains unchanged.
/// 
/// Given a DependencyGraph DG:
/// 
///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///        (The set of things that depend on s)    
///        
///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///        (The set of things that s depends on) 
//
// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
//     dependents("a") = {"b", "c"}
//     dependents("b") = {"d"}
//     dependents("c") = {}
//     dependents("d") = {"d"}
//     dependees("a") = {}
//     dependees("b") = {"a"}
//     dependees("c") = {"a"}
//     dependees("d") = {"b", "d"}
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// Given the pairing (s, t), this class contains all the dependent values of s
    /// using a Hashset. 
    /// </summary>
    internal class DGNode
    {
        private HashSet<String> _dependentValues;

        /// <summary>
        /// Creates an empty DGNode
        /// </summary>
        internal DGNode ()
        {
            _dependentValues = new HashSet<string>();
        }

        /// <summary>
        /// Creates an empty DGNode and adds the given parameter to the Node. 
        /// </summary>
        /// <param name="t"></param>
        internal DGNode(string t)
        {
            _dependentValues = new HashSet<string>();
            _dependentValues.Add(t);
        }

        /// <summary>
        /// Returns the list of dependents for the dependee
        /// </summary>
        internal HashSet<String> DependentValues
        {
            get { return _dependentValues; }
        }
    }

    private Dictionary<string, DGNode> pairings;

    /// <summary>
    /// Creates an empty DependencyGraph.
    /// </summary>
    public DependencyGraph()
    {
        pairings = new Dictionary<string, DGNode>();
    }


    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// This is an example of a property.
    /// </summary>
    public int NumDependencies
    {
        get {
            int count = 0;
            foreach (String temp in pairings.Keys)
            {
                count += pairings[temp].DependentValues.Count;
            }

            return count;
        }

    }


    /// <summary>
    /// Returns the size of dependees(s),
    /// that is, the number of things that s depends on.
    /// </summary>
    public int NumDependees(string s)
    {
        int _numDependees = 0;
        foreach (DGNode tempNode in pairings.Values)
        {
            if (tempNode.DependentValues.Contains(s))
            {
                _numDependees++;
            }
        }

        return _numDependees;
    }


    /// <summary>
    /// Reports whether dependents(s) is non-empty.
    /// </summary>
    public bool HasDependents(string s)
    {
        if (pairings.Keys.Contains(s) && pairings[s].DependentValues.Count!=0) //If DGNode is not empty
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// Reports whether dependees(s) is non-empty.
    /// </summary>
    public bool HasDependees(string s)
    {
        foreach (string tempStr in pairings.Keys)
        {
            if (pairings[tempStr].DependentValues.Contains(s))
            {
                return true;
            }
        }

        return false;
    }


    /// <summary>
    /// Enumerates dependents(s).
    /// </summary>
    public IEnumerable<string> GetDependents(string s)
    {
        List<string>? dependents = new List<string>();

        if (HasDependents(s))
        {
            foreach (String temp in pairings[s].DependentValues)
            {
                dependents.Add(temp);
            }

        }

        return dependents;
    }


    /// <summary>
    /// Enumerates dependees(s).
    /// </summary>
    public IEnumerable<string> GetDependees(string s)
    {
        List<string>? dependents = new List<string>();

        if(HasDependees(s))
        {
            foreach (string tempStr in pairings.Keys)
            {
                if (pairings[tempStr].DependentValues.Contains(s))
                {
                    dependents.Add(tempStr);
                }
            }
        }

        return dependents;
    }


    /// <summary>
    /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
    /// 
    /// <para>This should be thought of as:</para>   
    /// 
    ///   t depends on s
    ///
    /// </summary>
    /// <param name="s"> s must be evaluated first. T depends on S</param>
    /// <param name="t"> t cannot be evaluated until s is</param>
    public void AddDependency(string s, string t)
    {

        if (pairings.ContainsKey(s)) //if the key exists
        {
            if (pairings[s].DependentValues.Contains(t)) //if the pairing exists
            {
            } else //add the dependent to the key
            {
                pairings[s].DependentValues.Add(t);
            }

        } else //if the key doesn't exist, just add it
        {
            pairings.Add(s, new DGNode(t));
        }
    }


    /// <summary>
    /// Removes the ordered pair (s,t), if it exists
    /// </summary>
    /// <param name="s"></param>
    /// <param name="t"></param>
    public void RemoveDependency(string s, string t)
    {
        DGNode? valueOutcome;
        if (pairings.TryGetValue(s, out valueOutcome)) //if the ordered pair does exist
        {
            pairings[s].DependentValues.Remove(t);
        }
    }


    /// <summary>
    /// Removes all existing ordered pairs of the form (s,r).  Then, for each
    /// t in newDependents, adds the ordered pair (s,t).
    /// </summary>
    public void ReplaceDependents(string s, IEnumerable<string> newDependents)
    {
        if (HasDependents(s))
        {
            pairings[s].DependentValues.Clear();

            foreach (String temp in newDependents)
            {
                pairings[s].DependentValues.Add(temp);
            }

        } else
        {

            foreach (String temp in newDependents)
            {
                AddDependency(s, temp);
            }
        }
    }


    /// <summary>
    /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
    /// t in newDependees, adds the ordered pair (t,s).
    /// </summary>
    public void ReplaceDependees(string s, IEnumerable<string> newDependees)
    {
        foreach (String temp in pairings.Keys) {
            if (pairings[temp].DependentValues.Contains(s))
            {
                pairings[temp].DependentValues.Remove(s);
            }
        }

        foreach (String temp in newDependees)
        {
            AddDependency(temp, s);
        }


    }
}