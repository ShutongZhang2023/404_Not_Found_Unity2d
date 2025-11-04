using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordTest : MonoBehaviour
{
    public List<LetterDrop> letters = new List<LetterDrop>();
    [SerializeField] private string word;
    private string targetWord = "BREAK";

    public void UpdateWord()
    {
        string currentWord = "";
        foreach (var letterDrop in letters)
        {
            currentWord += letterDrop.letter;
        }
            word = currentWord;
        if (word == targetWord) { 
            GameManager.Instance.ChangeState(DialogueStage.AfterPuzzle);
        }
    }
}
