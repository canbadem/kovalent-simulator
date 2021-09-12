using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CBTextUtils
{

    public static string getSubscript(string number)
    {
        char[] chars = number.ToCharArray();

        StringBuilder sb = new StringBuilder();

        foreach (char c in chars) { 
            switch (c)
            {
                case '0':
                    sb.Append("₀");
                    break;
                case '1':
                    sb.Append("₁");
                    break;
                case '2':
                    sb.Append("₂");
                    break;
                case '3':
                    sb.Append("₃");
                    break;
                case '4':
                    sb.Append("₄");
                    break;
                case '5':
                    sb.Append("₅");
                    break;
                case '6':
                    sb.Append("₆");
                    break;
                case '7':
                    sb.Append("₇");
                    break;
                case '8':
                    sb.Append("₈");
                    break;
                case '9':
                    sb.Append("₉");
                    break;
          
                default:
                    sb.Append("0");
                    break;
                
            }
        }
        return sb.ToString();
    }

    public static string getChargeString(int charge)
    {
        string s = charge.ToString();

        if(charge > 0)
            s = "+" + s;
        
        return zeroToEmpty(getSuperscript(s));
    }

    public static string zeroToEmpty(string str)
    {
        char[] chars = str.ToCharArray();

        StringBuilder sb = new StringBuilder();

        foreach (char c in chars)
        {
            switch (c)
            {
                case '0':
                    sb.Append("");
                    break;
                case '₀':
                    sb.Append("");
                    break;
                case '⁰':
                    sb.Append("");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        return sb.ToString();
    }

    public static string getSuperscript(string number)
    {
        char[] chars = number.ToCharArray();

        StringBuilder sb = new StringBuilder();

        foreach (char c in chars)
        {
            switch (c)
            {
                case '0':
                    sb.Append("⁰");
                    break;
                case '1':
                    sb.Append("¹");
                    break;
                case '2':
                    sb.Append("²");
                    break;
                case '3':
                    sb.Append("³");
                    break;
                case '4':
                    sb.Append("⁴");
                    break;
                case '5':
                    sb.Append("⁵");
                    break;
                case '6':
                    sb.Append("⁶");
                    break;
                case '7':
                    sb.Append("⁷");
                    break;
                case '8':
                    sb.Append("⁸");
                    break;
                case '9':
                    sb.Append("⁹");
                    break;
                case '-':
                    sb.Append("⁻");
                    break;
                case '+':
                    sb.Append("⁺");
                    break;
                default:
                    sb.Append("⁰");
                    break;
            }
        }
        return sb.ToString();
    }

    public static string getLatinNumberNames(int number)
    {
        switch (number)
        {
            case 1:
                return "mono";
            case 2:
                return "di";
            case 3:
                return "tri";
            case 4:
                return "tetra";
            case 5:
                return "penta";
            case 6:
                return "heksa";
            case 7:
                return "hepta";
            case 8:
                return "okta";
            case 9:
                return "nona";
            case 10:
                return "deka";
        }

        return "";
    }

    public static string getTurkishNumberNames(int number)
    {
        switch (number)
        {
            case 1:
                return "bir";
            case 2:
                return "iki";
            case 3:
                return "üç";
            case 4:
                return "dört";
            case 5:
                return "beş";
            case 6:
                return "altı";
            case 7:
                return "yedi";
            case 8:
                return "sekiz";
            case 9:
                return "dokuz";
            case 10:
                return "on";
        }

        return number.ToString();
    }

    public static string getAnyonNames(Atom.AtomType type)
    {
        switch (type)
        {
            case Atom.AtomType.Oxygen:
                return "oksit";
            case Atom.AtomType.Chlorine:
                return "klorür";
            case Atom.AtomType.Fluorine:
                return "florür";
            case Atom.AtomType.Nitrogen:
                return "nitrür";
            case Atom.AtomType.Hydrogen:
                return "hidrür";
            case Atom.AtomType.Carbon:
                return "karbür";

        }
        return type.ToString();
    }

    public static string fixNamingErrors(string s)
    {
        return firstLetterToCapital(s.Replace("oo", "o").ToLower());
    }

    public static string firstLetterToCapital(string input)
    {
        switch (input)
        {
            case null:
                return "";
            case "":
                return "";
            default: 
                return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

}
