using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMoleculeDictionary
{

    public static QuestionMolecule su;
    public static QuestionMolecule metan;
    public static QuestionMolecule karbondioksit;
    public static QuestionMolecule amonyak;
    public static QuestionMolecule dioksijen;
    public static QuestionMolecule diazot;
    public static QuestionMolecule hidrojensulfur;
    public static QuestionMolecule hidrojenflorür;
    public static QuestionMolecule hidrojenklorür;
    public static QuestionMolecule hidrojensiyanür;
    public static QuestionMolecule karbontetraklorür;
    public static QuestionMolecule asetikasit;
    public static QuestionMolecule formikasit;
    public static QuestionMolecule boran;
    public static QuestionMolecule oksijendiflorür;
    public static QuestionMolecule fosfortriflorür;
    public static QuestionMolecule karbondisülfür;

    public static QuestionMolecule methanid;

    public static void initialize()
    {

        List<QuestionAtom> list = new List<QuestionAtom>();

        //Su

        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

        su = new QuestionMolecule("Su",new List<QuestionAtom>(list), 0, 2);
        list.Clear();

        //Metan

        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen}));

        metan = new QuestionMolecule("Metan",new List<QuestionAtom>(list), 0, 4);
        list.Clear();

        //Karbondioksit

        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Oxygen, Atom.AtomType.Oxygen }));

        karbondioksit = new QuestionMolecule("Karbondioksit",new List<QuestionAtom>(list), 0, 4);
        list.Clear();

        //Amonyak 

        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Nitrogen, 7, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

        amonyak = new QuestionMolecule("Amonyak",new List<QuestionAtom>(list), 0, 3);
        list.Clear();

        //Dioksijen

        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));

        dioksijen = new QuestionMolecule("Dioksijen",new List<QuestionAtom>(list), 0, 2);
        list.Clear();

        //Diazot

        list.Add(new QuestionAtom(Atom.AtomType.Nitrogen, 7, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Nitrogen, 7, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Nitrogen }));

        diazot = new QuestionMolecule("Diazot",new List<QuestionAtom>(list), 0, 3);
        list.Clear();

        //Hidrojen sulfur
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Sulfur }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Sulfur }));
        list.Add(new QuestionAtom(Atom.AtomType.Sulfur, 16, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

        hidrojensulfur = new QuestionMolecule("Hidrojen Sülfür",new List<QuestionAtom>(list), 0, 2);
        list.Clear();

        //Hidrojen florür
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Fluorine }));
        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Hydrogen }));

        hidrojenflorür = new QuestionMolecule("Hidrojen Florür", new List<QuestionAtom>(list), 0, 1);
        list.Clear();

        //Hidrojen klorür
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Chlorine }));
        list.Add(new QuestionAtom(Atom.AtomType.Chlorine, 17, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Hydrogen }));

        hidrojenklorür = new QuestionMolecule("Hidrojen Klorür", new List<QuestionAtom>(list), 0, 1);
        list.Clear();

        //Hidrojen siyanür

        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Nitrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Nitrogen, 7, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Carbon }));

        hidrojensiyanür = new QuestionMolecule("Hidrojen Siyanür", new List<QuestionAtom>(list), 0, 4);
        list.Clear();

        //Karbon tetraklorür

        list.Add(new QuestionAtom(Atom.AtomType.Chlorine, 17, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Chlorine, 17, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Chlorine, 17, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Chlorine, 17, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Chlorine, Atom.AtomType.Chlorine, Atom.AtomType.Chlorine, Atom.AtomType.Chlorine }));

        karbontetraklorür = new QuestionMolecule("Karbon Tetraklorür", new List<QuestionAtom>(list), 0, 4);
        list.Clear();

        //Asetik Asit
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Carbon, Atom.AtomType.Oxygen, Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon, Atom.AtomType.Hydrogen }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));

        asetikasit = new QuestionMolecule("Asetik Asit", new List<QuestionAtom>(list), 0, 8);
        asetikasit.setExceptionFormula("CH₃COOH");
        list.Clear();

        //Formik Asit
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Oxygen, Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon, Atom.AtomType.Hydrogen }));

        formikasit = new QuestionMolecule("Formik Asit", new List<QuestionAtom>(list), 0, 5);
        formikasit.setExceptionFormula("HCOOH");
        list.Clear();

        //Boran
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Boron }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Boron }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Boron }));
        list.Add(new QuestionAtom(Atom.AtomType.Boron, 5, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

        boran = new QuestionMolecule("Boran", new List<QuestionAtom>(list), 0, 3);
        list.Clear();

        //Oksijen Diflorür
        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Oxygen }));
        list.Add(new QuestionAtom(Atom.AtomType.Oxygen, 8, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Fluorine, Atom.AtomType.Fluorine }));

        oksijendiflorür = new QuestionMolecule("Oksijen Diflorür", new List<QuestionAtom>(list), 0, 2);
        list.Clear();

        //Fosfor Triflorür

        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Phosphorus }));
        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Phosphorus }));
        list.Add(new QuestionAtom(Atom.AtomType.Fluorine, 9, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Phosphorus }));
        list.Add(new QuestionAtom(Atom.AtomType.Phosphorus, 15, 0, 3, new List<Atom.AtomType> { Atom.AtomType.Fluorine, Atom.AtomType.Fluorine, Atom.AtomType.Fluorine }));

        fosfortriflorür = new QuestionMolecule("Fosfor Triflorür", new List<QuestionAtom>(list), 0, 3);
        list.Clear();

        //Karbon Disülfür
        list.Add(new QuestionAtom(Atom.AtomType.Sulfur, 16, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Sulfur, 16, 0, 2, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 6, 0, 4, new List<Atom.AtomType> { Atom.AtomType.Sulfur, Atom.AtomType.Sulfur }));

        karbondisülfür = new QuestionMolecule("Karbon Disülfür", new List<QuestionAtom>(list), 0, 4);
        list.Clear();

        //Methanid

        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Hydrogen, 1, 0, 1, new List<Atom.AtomType> { Atom.AtomType.Carbon }));
        list.Add(new QuestionAtom(Atom.AtomType.Carbon, 7, -1, 3, new List<Atom.AtomType> { Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen, Atom.AtomType.Hydrogen }));

        methanid = new QuestionMolecule("Methanid",new List<QuestionAtom>(list), -1, 3);
        list.Clear();
        
    }
}
