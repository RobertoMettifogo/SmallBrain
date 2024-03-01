using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicationOperation : IOperation
{
    private int operand1;
    private int operand2;
    private int result;

    public (string, string) GiveMeOperation(float currentDifficulty)
    {
        int Bvalue;
        if (currentDifficulty.Equals(1f))
        {
            Bvalue = 2;
            operand1 = Random.Range(1, 21);
            operand2 = Random.Range(1, Bvalue);
        }
        else if (currentDifficulty.Equals(2f))
        {
            Bvalue = 3;
            operand1 = Random.Range(1, 21);
            operand2 = Random.Range(2, Bvalue);
        }
        else if (currentDifficulty.Equals(3f))
        {
            Bvalue = 4;
            operand1 = Random.Range(1, 31);
            operand2 = Random.Range(2, Bvalue);
        }
        else
        {
            Debug.LogError("Incorrect difficulty value");
        }

        result = operand1 * operand2;
        return ($"{operand1} * {operand2}", result.ToString());
    }
    public int GetCorrectAnswer()
    {
        return result;
    }
}
