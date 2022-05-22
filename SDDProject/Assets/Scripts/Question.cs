using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Question
{
    public string question;
    private List<string> choices; // all choices
    private int solution; // the correct answer
    private bool isCorrect;

    public string GetQuestion()
    {
        return question;
    }

    public List<string> GetChoices() // get all choices from this question
    {
        return choices;
    }

    public string GetChoiceAt(int n) // get a single choice from this question
    {
        return choices[n];
    }

    public int GetSolution()
    {
        return solution;
    }

    public void SetQuestion(string q)
    {
        question = q;
    }

    public void SetChoice(string c)
    {
        choices.Add(c);
    }

    public void SetSolution(int n)
    {
        solution = n;
    }

    public bool Answer() // onClick() triggers this function
    {
        // (not finished) add: if button's choice == solution, return true;
        return false;
    }

    public void Setup()
    {
        choices = new List<string>();
        isCorrect = false;
    }
}
