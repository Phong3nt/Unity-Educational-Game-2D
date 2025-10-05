using UnityEngine;
using TMPro;
using UnityEngine.UI; 
using System.Collections.Generic;
using System.Collections;

public class QuizManager : MonoBehaviour
{
    public OperationType currentOperation = OperationType.Addition;
    public QuestionMode currentMode = QuestionMode.FindResult;

    private string correctOperator;

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

        switch (currentOperation)
        {
            case OperationType.Addition:

            case OperationType.Subtraction:
                Debug.Log("Start create subtraction");

                GenerateBasicMathQuestion();
                return;
            
            case OperationType.Comparison:
                GenerateComparisonQuestion();
                return;
// Wait for update (Multiplication, Division)
            case OperationType.Division:
                // Wait for update
                break;

            case OperationType.Multiplication:
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

    void GenerateBasicMathQuestion()
    {
        Debug.Log($"Current mode: {currentMode}");

        int numberA = Random.Range(10, 26);
        int numberB = Random.Range(1, 16);
        int resultC;
        string mathOperator = "";

        if (currentOperation == OperationType.Subtraction)
        {
            if (numberA < numberB) 
            {
                int temp = numberA;
                numberA = numberB;
                numberB = temp;
            }
            resultC = numberA - numberB;
            mathOperator = "-";
        }
        else 
        {
            resultC = numberA + numberB;
            mathOperator = "+";
        }

        int missingValuePosition = Random.Range(0, 3); 
        string questionFormat = "";

        if (currentMode == QuestionMode.FindResult) 
        {
            questionFormat = $"{numberA} {mathOperator} {numberB} = ?";
            this.correctAnswer = resultC;
        }
        else 
        {
            switch (missingValuePosition)
            {
                case 0: 
                    questionFormat = $"? {mathOperator} {numberB} = {resultC}";
                    this.correctAnswer = numberA;
                    break;
                case 1: 
                    questionFormat = $"{numberA} {mathOperator} ? = {resultC}";
                    this.correctAnswer = numberB;
                    break;
                case 2: 
                    questionFormat = $"{numberA} {mathOperator} {numberB} = ?";
                    this.correctAnswer = resultC;
                    break;
            }
        }

        questionText.text = questionFormat;
        Debug.Log($"Question generated: {questionFormat} - Correct answer: {this.correctAnswer}");

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

        Debug.Log("The question has been generated!");
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
        SetButtonsInteractable(false); 
        if (currentOperation == OperationType.Comparison)
        {
            CheckComparisonAnswer(buttonText.text);
        }
        else
        {
            int answerSubmitted;
            if (int.TryParse(buttonText.text, out answerSubmitted))
            {
                Debug.Log($"User choose answer: {answerSubmitted}");
                CheckAnswer(answerSubmitted); 
            }
            else
            {
                Debug.LogError("Undetected answer");
                SetButtonsInteractable(true); 
            }
        }
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

    void GenerateComparisonQuestion()
    {
        Debug.Log("Start create Comparison question");

        int num1 = Random.Range(1, 21);
        int num2 = Random.Range(1, 21);

        if (num1 > num2)
        {
            correctOperator = ">";
        }
        else if (num1 < num2)
        {
            correctOperator = "<";
        }
        else
        {
            correctOperator = "=";
        }

        questionText.text = $"{num1} ? {num2}";
        Debug.Log($"Question: {num1} ? {num2} - Correct Answer: {correctOperator}");

        List<string> operatorAnswers = new List<string> { ">", "<", "=" };

        ShuffleOperators(operatorAnswers);

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i < operatorAnswers.Count)
            {
                buttonText.text = operatorAnswers[i];
                answerButtons[i].interactable = true; 
                Debug.Log($"Sign answer {operatorAnswers[i]} to button {i + 1}.");
            }
            else 
            {
                buttonText.text = ""; 
                answerButtons[i].interactable = false; 
                Debug.Log("Inactive button 4");
            }
        }
    }

    void ShuffleOperators(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void CheckComparisonAnswer(string submittedOperator)
    {
        if (submittedOperator == correctOperator)
        {
            score++;
            scoreText.text = "Score: " + score;
            Debug.Log("Correct! ");
        }
        else
        {
            Debug.Log($"Wrong. Correct Answer: {correctOperator}");
        }

        StartCoroutine(NextQuestionRoutine());
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

public enum QuestionMode
{
    FindResult,        
    FillInTheBlank   
}