using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueStage
{
    BeforePuzzle,
    AfterPuzzle,
    ChoiceA,
    ChoiceB,
    ChoiceC,
}

[System.Serializable]
public class StageLines
{
    public DialogueStage stage;
    public List<string> lines;
}

public class LinePrinter : MonoBehaviour
{
    [Header("UI Reference")]
    public Text textField;
    public GameObject bubbleRoot;
    public GameObject bubbleContent;

    [Header("Dialogue Data")]
    public List<StageLines> dialogueData;

    [Header("Player Setting")]
    public float typingSpeed = 0.05f;
    public float pauseAfterLine = 1f;

    private Coroutine dialogueRoutine;
    private Dictionary<DialogueStage, List<string>> dialogueMap;

    public event Action<DialogueStage> OnStageFinished;
    private DialogueStage currentStage;

    private void Awake()
    {
        dialogueMap = dialogueData.ToDictionary(entry => entry.stage, entry => entry.lines);
    }

    public void PlayStage(DialogueStage stage)
    {
        if (!dialogueMap.ContainsKey(stage))
        {
            Debug.LogWarning("No dialogue lines for stage: " + stage);
            return;
        }

        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        currentStage = stage;
        bubbleRoot.SetActive(true);
        dialogueRoutine = StartCoroutine(PlayStageSequence(dialogueMap[stage]));
    }

    private IEnumerator PlayStageSequence(List<string> lines)
    {
        bubbleRoot.SetActive(false);
        bubbleContent.transform.localScale = Vector3.zero;

        yield return new WaitForSeconds(1f);
        bubbleRoot.SetActive(true);

        yield return null;

        bubbleContent.transform.DOScale(Vector3.one * 2.2f, 1f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(1f + 0.3f);
        yield return StartCoroutine(PlayDialogue(lines));

        bubbleContent.transform.DOScale(Vector3.zero, 1f)
            .SetEase(Ease.InBack)
            .OnComplete(() => bubbleRoot.SetActive(false));

        yield return null;
        OnStageFinished?.Invoke(currentStage);
    }

    private IEnumerator PlayDialogue(List<string> lines)
    {
        foreach (var line in lines)
        {
            textField.text = "";

            int count = 0;
            string fullText = line;
            int lastPlayedChar = -1;

            Tween typing = DOTween.To(() => count, x => 
            {
                count = x;
                textField.text = fullText.Substring(0, count);

                if (count > 0 && count != lastPlayedChar)
                {
                    AudioManager.Instance.PlayTypeSFX();
                    lastPlayedChar = count;
                }
            }, fullText.Length, fullText.Length * typingSpeed);

            yield return typing.WaitForCompletion();
            yield return new WaitForSeconds(pauseAfterLine);
        }

        textField.text = ""; // Clear text after finishing the dialogue
    }
}
