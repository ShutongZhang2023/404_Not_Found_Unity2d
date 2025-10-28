using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LV1Manager : MonoBehaviour
{
    public LinePrinter linePrinter;

    private void Start()
    {
        StartCoroutine(StartDialogueDelayed());
    }

    private IEnumerator StartDialogueDelayed()
    {
        yield return new WaitForSeconds(2f);
        linePrinter.PlayStage(DialogueStage.BeforePuzzle);
    }
}
