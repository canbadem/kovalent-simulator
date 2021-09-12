using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question
{
    public enum QuestionType
    {
        Molecule,
        Double,
        String
    }

    public int time;
    private int points;

    public QuestionType qt;

    public QuestionMolecule qm;
    public string question;
    public double answerDouble;
    public string answerString;

    public Question(int time, int points, string question, string answerString)
    {
        this.time = time;
        this.points = points;
        this.question = question;
        this.answerString = answerString;
        qt = QuestionType.String;
    }

    public Question(int time, int points, string question, double answerDouble)
    {
        this.time = time;
        this.points = points;
        this.question = question;
        this.answerDouble = answerDouble;
        qt = QuestionType.Double;
    }

    public Question(int time, int points, QuestionMolecule qm)
    {
        this.time = time;
        this.points = points;
        this.qm = qm;
        qt = QuestionType.Molecule;
    }

    public int calculatePoints(float timeLeft)
    {
        return Mathf.RoundToInt(timeLeft / time * points);
    }

}
