using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZone : MonoBehaviour
{
    public Text displayText;
    public float typingSpeed = 0.05f;
    public List<string> wordsCorrect;
    public bool multipleWord;
    private Coroutine typingCoroutine;
    public bool isLevel2;

    private bool IsWordCorrect(string word)
    {
        bool isCorrect = wordsCorrect.Contains(word);
        return isCorrect;
    }

    public void TryAcceptWord(DraggableWord wordObj)
    {
        if (multipleWord)
        {

        }
        //single word
        else 
        {
            if (IsWordCorrect(wordObj.word))
            {
                wordObj.FadeOutAndDisable();
                ShowTypingEffect(wordObj.word);
            }

            else {
                wordObj.ShakeAndReturn();
            }
        }
    }

    

    private void ShowTypingEffect(string word)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(word));
    }

    private IEnumerator TypeText(string word)
    {
        displayText.text = "";

        for (int i = 0; i < word.Length; i++)
        {
            displayText.text += word[i];
            AudioManager.Instance.PlayTypeSFX();

            yield return new WaitForSeconds(typingSpeed);
        }

        if (!isLevel2) GameManager.Instance.ChangeState(DialogueStage.AfterPuzzle);
        else {
            if (word == "BORING")
            {
                GameManager.Instance.ChangeState(DialogueStage.AfterPuzzle);
            }
            else if (word == "EASY")
            {
                GameManager.Instance.ChangeState(DialogueStage.ChoiceA);
            }
            else if (word == "FUN")
            {
                GameManager.Instance.ChangeState(DialogueStage.ChoiceB);
            }
            else if (word == "CHILDISH")
            {
                GameManager.Instance.ChangeState(DialogueStage.ChoiceC);
            }
        }
    }
}
