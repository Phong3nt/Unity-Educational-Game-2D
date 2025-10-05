using UnityEngine;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI scoreText;
    private int score;

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
        int correctAnswer = numberA + numberB;

        questionText.text = $"{numberA} + {numberB} = ?";

        Debug.Log("New question: " + questionText.text + " - Answer: " + correctAnswer);
    }


    void Update()
    {
        //need for future updates(still unchange)
    }
}