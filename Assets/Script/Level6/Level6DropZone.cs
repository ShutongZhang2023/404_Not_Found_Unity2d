using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Level6DropZone : DropZone
{
    [SerializeField] private TimeChecker timeChecker;
    [SerializeField] private TextMeshProUGUI tip;

    private void Start()
    {
        tip.text = timeChecker.isMorning ? "Find EVENING" : "Find MORNING";
        wordsCorrect.Add(timeChecker.isMorning ? "EVENING" : "MORNING");
    }
}
