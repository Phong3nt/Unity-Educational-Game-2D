using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections.Generic; 


public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;

    public Button[] answerButtons;


    private int score;
    private int correctAnswer;
    public QuestionData[] questions;

    void Start()
    {
        Debug.Log("QuizManager is ready!");
        score = 0;
        scoreText.text = "Score: " + score;

        GenerateNewQuestion();
    }

    void GenerateNewQuestion()
    {
        int numberA = Random.Range(1, 11);
        int numberB = Random.Range(1, 11);
        correctAnswer = numberA + numberB; 

        questionText.text = $"{numberA} + {numberB} = ?";

        List<int> answers = new List<int>();
        answers.Add(correctAnswer);

        int incorrectAnswerCount = 0;
        while (incorrectAnswerCount < 3)
        {
            int randomOffset = Random.Range(-5, 6);
            int incorrectAnswer = correctAnswer + randomOffset;

            if (incorrectAnswer > 0 && !answers.Contains(incorrectAnswer))
            {
                answers.Add(incorrectAnswer);
                incorrectAnswerCount++;
            }
        }

        Shuffle(answers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = answers[i].ToString();
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    void Update()
    {
        //need for future updates(still unchange)
    }
}