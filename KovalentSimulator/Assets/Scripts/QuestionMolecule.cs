using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestionMolecule
{
    public List<QuestionAtom> atomList;

    public int formalCharge;
    public int boundElectronPairs;

    public string name;
    public string exceptionFormula = "";

    public QuestionMolecule(string name, List<QuestionAtom> atomList, int formalCharge, int boundElectronPairs)
    {
        this.atomList = atomList;
        this.formalCharge = formalCharge;
        this.boundElectronPairs = boundElectronPairs;
        this.name = name;
    }

    public void setExceptionFormula(string exceptionFormula)
    {
        this.exceptionFormula = exceptionFormula;
    }

    public string getFormula()
    {

        if (exceptionFormula.Length != 0)
            return exceptionFormula;

        string name = "";

        Dictionary<Atom.AtomType, List<Atom.AtomType>> dic = new Dictionary<Atom.AtomType, List<Atom.AtomType>>();

        foreach (QuestionAtom a in this.atomList)
        {
            if (!dic.ContainsKey(a.atomType))
            {
                dic.Add(a.atomType, new List<Atom.AtomType>() { a.atomType });
            }
            else
            {
                List<Atom.AtomType> l = dic[a.atomType];

                l.Add(a.atomType);

                dic[a.atomType] = l;

            }
        }

        List<Atom.AtomType> list = dic.Keys.ToList();

        list.Sort(SortMolecules);

        foreach (Atom.AtomType key in list)
        {
            List<Atom.AtomType> value = dic[key];

            if (value.Count.Equals(1))
                name += (Atom.GetInfo(key).atomSymbol);
            else
                name += (Atom.GetInfo(key).atomSymbol + CBTextUtils.getSubscript(value.Count.ToString()));

        }

        return name + CBTextUtils.getChargeString(this.formalCharge);
    }

    private int SortMolecules(Atom.AtomType x, Atom.AtomType y)
    {


        Atom.AtomInfo ix = Atom.GetInfo(x); //4
        Atom.AtomInfo iy = Atom.GetInfo(y); //1

        if (ix.atomSymbol.Equals("C"))
            return -1;

        int toReturn = ix.electroNegativity.CompareTo(iy.electroNegativity);
        //    Debug.Log(ix.atomSymbol + "("+ix.electroNegativity+") ," + iy.atomSymbol + "("+iy.electroNegativity+") | " + toReturn);
        return toReturn;
    }

}
