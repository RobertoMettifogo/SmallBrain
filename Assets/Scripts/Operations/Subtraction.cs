using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtractionOperation : IOperation
{
    private int operand1;
    private int operand2;
    private int result;

    public (string, string) GiveMeOperation(float currentDifficulty)
    {
        int Avalue;
        if (currentDifficulty.Equals(1f))
        {
            Avalue = 11;
            operand1 = Random.Range(1, Avalue);
            operand2 = Random.Range(1, operand1);
        }
        else if (currentDifficulty.Equals(2f))
        {
            Avalue = 21;
            operand1 = Random.Range(1, Avalue);
            operand2 = Random.Range(1, operand1);
        }
        else if (currentDifficulty.Equals(3f))
        {
            Avalue = 51;
            operand1 = Random.Range(1, Avalue);
            operand2 = Random.Range(1, operand1);
        }
        else
        {
            Debug.Log("Incorrect difficult value");
        }

        result = operand1 - operand2;
        return ($"{operand1} - {operand2}", result.ToString());
    }
    public int GetCorrectAnswer()
    {
        return result;
    }
}
