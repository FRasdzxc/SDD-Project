using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class QuizController : MonoBehaviour
{
    private List<Question> questions;
    private int currentQuestion;
    private int score;
    [SerializeField] private Timer timerSlider;
    [SerializeField] private float timer = 30f;
    private bool timerIsActive;
    [SerializeField] private Text questionText;
    [SerializeField] private Button[] choices;
    private Text[] choiceText;
    [SerializeField] private Text scoreText;
    private bool choicesAreActive;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private Text playerName;
    [SerializeField] private Text endScore;

    void Start()
    {
        questions = new List<Question>();
        currentQuestion = 0;
        score = 0;
        timerIsActive = false;
        choicesAreActive = true;
        choiceText = new Text[4];
        for (int i = 0; i < choiceText.Length; i++) // store Text component of buttons
        {
            choiceText[i] = choices[i].transform.Find("Text").GetComponent<Text>(); // get button's child (Text)'s Text component
        }
        ReadFile();
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
            timer = 30f;
            ShowNextQuestion();
        }
    }

    public void ReadFile()
    {
        //string path = "Assets/Files/SDDProjectMCFile.txt"; // test .txt file: can be found in the "Files" folder of the editor
        string path = Application.dataPath + "/StreamingAssets/SDDProjectMCFile.txt"; // build .txt file: can be found in the project file
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
                    questions[questions.Count - 1].SetSolution(i - 1); // i - 1 because items[0] (the question) is not included
                }
                else
                    questions[questions.Count - 1].SetChoice(items[i]);
            }

            line = sR.ReadLine();
        }
        sR.Close();

        ShuffleQuestions(ref questions);
        ShowNextQuestion();

        // Debug.Log()
        {
            Debug.Log(questions.Count + " Questions");

            string soln = "";
            for (int i = 0; i < questions.Count; i++)
            {
                soln += (i + 1);
                soln += ((char)(questions[i].GetSolution() + 65));
                soln += " ";
            }
            Debug.Log(soln);
        }
    }

    public void ShuffleChoices(ref string[] items) // shuffle choices for randomness
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

    public void ShuffleQuestions(ref List<Question> questions) // shuffle questions for randomness
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

    public async void Answer(int n)  // choice button onClick() triggers this function
    {
        if (choicesAreActive) // prevents spamming of score
        {
            if (questions[currentQuestion - 1].CheckAnswer(n))
            {
                score +=  1 * 10; // each question scores 10
                scoreText.text = score.ToString("000");

                choices[n].GetComponent<Image>().color = new Color32(200, 255, 200, 255); // correct answer is marked in green

                timerSlider.pauseTimer();
                timerIsActive = false;
                choicesAreActive = false;
                await Task.Delay(1000); // pause and wait for 1 second to show correct answer
                ShowNextQuestion();
            }
            else
            {
                choices[n].GetComponent<Image>().color = new Color32(255, 200, 200, 255); // wrong answer is marked in red
                choices[questions[currentQuestion - 1].GetSolution()].GetComponent<Image>().color = new Color32(200, 255, 200, 255); // correct answer is marked in green

                timerSlider.pauseTimer();
                timerIsActive = false;
                choicesAreActive = false;
                await Task.Delay(1000); // pause and wait for 1 second to show correct answer
                ShowNextQuestion();
            }
        }
    }

    public void ShowNextQuestion()
    {
        if (currentQuestion < questions.Count) // if there are unanswered questions, show next question
        {
            currentQuestion++;

            questionText.text = currentQuestion + ". " + questions[currentQuestion - 1].GetQuestion();
            for (int i = 0; i < choiceText.Length; i++)
            {
                choices[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255); // reset choice button Image Color to white
                choiceText[i].text = (char)(65 + i) + ". " + questions[currentQuestion - 1].GetChoiceAt(i); // eg A. Banana
            }

            timerSlider.resetTimer();
            timerIsActive = true; // start countdown timer
            choicesAreActive = true;
        }
        else // if all questions are answered
        {
            timerSlider.pauseTimer();
            timerIsActive = false;
            choicesAreActive = false;

            mainPanel.SetActive(false);
            playerName.text = PlayerPrefs.GetString("playerName");
            endScore.text = score + "/" + (questions.Count * 10);
            endPanel.SetActive(true);

            Debug.Log(score + "/" + questions.Count * 10);
        }
    }

    public void Pause() // MainPanel PauseButton triggers this function
    {
        Time.timeScale = 0;
        mainPanel.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void Resume() // MainPanel PauseButton triggers this function
    {
        Time.timeScale = 1;
        mainPanel.SetActive(true);
        pausePanel.SetActive(false);
    }
}
