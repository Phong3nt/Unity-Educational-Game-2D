using UnityEngine;

[CreateAssetMenu(fileName = "New QuestionData", menuName = "Quiz/Question Data")]
public class QuestionData : ScriptableObject
{
    public int numberA;
    public int numberB;
    public int correctAnswer;
}