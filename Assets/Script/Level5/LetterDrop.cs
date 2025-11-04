using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDrop : MonoBehaviour
{
    public char letter;
    [SerializeField] private WordTest tester;

    public void updateLetter(char newLetter) { 
        letter = newLetter;
        tester.UpdateWord();
    }
}
