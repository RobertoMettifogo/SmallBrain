using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivisionOperation : IOperation
{
    private int operand1;
    private int operand2;
    private int result;
    public int divisionBase = 2;

    public (string, string) GiveMeOperation(float currentDifficulty)
    {
        int Avalue;
        if (currentDifficulty.Equals(1f))
        {
            Avalue = 11;
            operand1 = Random.Range(11, Avalue);
            operand1 -= operand1 % divisionBase;
            operand1 = Mathf.Max(operand1, divisionBase);
            operand2 = divisionBase;
        }
        else if (currentDifficulty.Equals(2f))
        {
            Avalue = 21;
            operand1 = Random.Range(11, Avalue);
            operand1 -= operand1 % divisionBase;
            operand1 = Mathf.Max(operand1, divisionBase);
            operand2 = divisionBase;
        }
        else if (currentDifficulty.Equals(3f))
        {
            Avalue = 51;
            operand1 = Random.Range(11, Avalue);
            operand1 -= operand1 % divisionBase;
            operand1 = Mathf.Max(operand1, divisionBase);
            operand2 = divisionBase;
        }
        else
        {
            Debug.Log("Incorrect difficult value");
        }

        result = operand1 / operand2;
        return ($"{operand1} / {operand2}", result.ToString());
    }

    public int GetCorrectAnswer()
    {
        return result;
    }
}