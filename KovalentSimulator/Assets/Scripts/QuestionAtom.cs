using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionAtom
{

    public Atom.AtomType atomType;
    public int electronCount;
    public int formalCharge;
    public int boundElectrons;
    public List<Atom.AtomType> boundTo;

    public QuestionAtom(Atom.AtomType atomType, int electronCount, int formalCharge, int boundElectrons, List<Atom.AtomType> boundTo)
    {
        this.atomType = atomType;
        this.electronCount = electronCount;
        this.formalCharge = formalCharge;
        this.boundElectrons = boundElectrons;
        this.boundTo = boundTo;
    }
}
