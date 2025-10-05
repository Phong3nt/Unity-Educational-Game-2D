using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    public OperationType currentOperation = OperationType.Addition;

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
        StartCoroutine(StartGameRoutine());
        GenerateNewQuestion();
    }

    IEnumerator StartGameRoutine()
    {
        yield return null;

        GenerateNewQuestion();

        Debug.Log(" StartGameRoutine run, success create first question!");
    }

    void GenerateNewQuestion()
    {
        Debug.Log($"Start create new question - Type: {currentOperation}");

        int numberA = 0;
        int numberB = 0;
        correctAnswer = 0;

        switch (currentOperation)
        {
            case OperationType.Addition:
                numberA = Random.Range(1, 11);
                numberB = Random.Range(1, 11);
                correctAnswer = numberA + numberB;
                questionText.text = $"{numberA} + {numberB} = ?";
                break;

            case OperationType.Subtraction:
                Debug.Log("Start create subtraction");

                numberA = Random.Range(10, 26);
                numberB = Random.Range(1, numberA + 1);

                correctAnswer = numberA - numberB;
                questionText.text = $"{numberA} - {numberB} = ?";

                Debug.Log($"Question: {numberA} - {numberB} = {correctAnswer}");
                break;

            // Wait for update (Multiplication, Division, Comparison)
            case OperationType.Multiplication:
                // Wait for update
                break;

            case OperationType.Division:
                // Wait for update
                break;

            case OperationType.Comparison:
                // Wait for update
                break;

            default:
                Debug.LogError("Undetected math type or not yet implemented");
                return;
        }
        Debug.Log("New question: " + questionText.text + " - Answer: " + correctAnswer);

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
            Debug.Log($" Answer {answers[i]} in button {i + 1}.");
        }
            Debug.Log("New question generated successfully!");
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

    public void AnswerButtonClicked(TextMeshProUGUI buttonText)
    {
        int answerSubmitted;
        if (int.TryParse(buttonText.text, out answerSubmitted))
        {
            Debug.Log($"User choose answer: {answerSubmitted}");
            CheckAnswer(answerSubmitted);
        }
        else
        {
            Debug.LogError("Something wrong with this button's number");
        }

        SetButtonsInteractable(false);
    }

    void CheckAnswer(int submittedAnswer)
    {
        if (submittedAnswer == correctAnswer)
        {
            score++;
            scoreText.text = "Score: " + score;
            Debug.Log("Correct! Your score: " + score);
        }
        else
        {
            // (wait for update)
            Debug.Log($"Wrong! Answer: {correctAnswer}");
        }

        StartCoroutine(NextQuestionRoutine());
    }

    IEnumerator NextQuestionRoutine()
    {
        yield return new WaitForSeconds(1f);

        SetButtonsInteractable(true);
        GenerateNewQuestion();
    }

    void SetButtonsInteractable(bool isInteractable)
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = isInteractable;
        }
    }


    void Update()
    {
        //need for future updates(still unchange)
    }
}

public enum OperationType
{
    Addition,       
    Subtraction,    
    Multiplication, 
    Division,       
    Comparison      
}
