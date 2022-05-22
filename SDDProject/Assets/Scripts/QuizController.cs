using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class QuizController : MonoBehaviour
{
    private List<Question> questions;
    private int currentQuestion;
    private int score;
    [SerializeField] private float timer = 20f;
    private bool timerIsActive;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        if (timerIsActive) // if timer is Active, countdown happens
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0f) // if timer reaches 0, timer will be reset and next question will be shown
        {
            timerIsActive = false;
            timer = 20f;
            ShowNextQuestion();
        }
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
                if (items[i].EndsWith("*")) // if the item ends with "*", it is the solution
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
        ShowNextQuestion();

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

    private void Answer()  // choice button onClick() triggers this function
    {
        if (questions[currentQuestion - 1].CheckAnswer())
        {
            score++;
            // (not finished) add: ui showing "correct"
        }
        else
        {
            // (not finished) add: ui showing "incorrect" and the solution
        }

        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
        if (currentQuestion <= questions.Count) // if there are unanswered questions, show next question
        {
            currentQuestion++;
            timerIsActive = true; // start countdown timer
            // (not finished) add: ui showing next question
        }
        else // if all questions are answered
        {
            // (not finished) add: ui showing "game ended" + score screen maybe
        }
    }

    private void Setup() // play again button onClick() triggers this function (if applicable)
    {
        questions = new List<Question>();
        currentQuestion = 0;
        score = 0;
        timerIsActive = false;
        ReadFile();
    }
}
