using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    private List<Question> questions;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    private void ReadFile()
    {
        string path = "Assets/Files/SDDProjectMCFile.txt"; // can be found in the editor
        StreamReader sR = new StreamReader(path); // setup StreamReader which will read .txt file from path

        string line = sR.ReadLine();
        while (line != null) // check if it is the end of the file
        {
            questions.Add(new Question());
            questions[questions.Count - 1].Setup(); // initialize choices list for Question

            string[] items = line.Split('|'); // split line to multiple strings
            questions[questions.Count - 1].SetQuestion(items[0]); // set items[0] as question
            ShuffleChoices(ref items);

            for (int i = 1; i < items.Length; i++) // set choices to Question
            {
                if (items[i].EndsWith("*")) // the solution
                {
                    questions[questions.Count - 1].SetChoice(items[i].Remove(items[i].Length - 1));
                    questions[questions.Count - 1].SetSolution(i - 1); // i - 1 because items[0] is not included
                }
                else
                    questions[questions.Count - 1].SetChoice(items[i]);
            }

            line = sR.ReadLine();
        }
        sR.Close();

        ShuffleQuestions(ref questions);

        // Debug.Log() - will be removed
        {
            Debug.Log("This is just a test, the questions are not final");
            Debug.Log("questions.Count = " + questions.Count);

            for (int i = 0; i < questions.Count; i++)
            {
                Debug.Log("question " + (i + 1) + " = " + questions[i].GetQuestion());
                Debug.Log("solution " + (i + 1) + " = " + questions[i].GetSolution() + " which is " + questions[i].GetChoiceAt(questions[i].GetSolution()));
            }
        }
    }

    private void ShuffleChoices(ref string[] items) // shuffle choices for randomness
    {
        for (int i = 0; i < items.Length - 1; i++)
        {
            int ran1 = Random.Range(1, items.Length);
            int ran2 = Random.Range(1, items.Length);
            string temp;

            temp = items[ran1];
            items[ran1] = items[ran2];
            items[ran2] = temp;
        }
    }

    private void ShuffleQuestions(ref List<Question> questions) // shuffle questions for randomness
    {
        for (int i = 0; i < questions.Count; i++)
        {
            int ran1 = Random.Range(0, questions.Count);
            int ran2 = Random.Range(0, questions.Count);
            Question temp;

            temp = questions[ran1];
            questions[ran1] = questions[ran2];
            questions[ran2] = temp;
        }
    }

    private void Setup()
    {
        questions = new List<Question>();
        ReadFile();
    }
}
