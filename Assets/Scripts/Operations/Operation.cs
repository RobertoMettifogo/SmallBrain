using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
    public interface IOperation
    {
        (string operationText, string operationResult) GiveMeOperation(float currentDifficulty);

        int GetCorrectAnswer();
    }
