using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeChecker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    public bool isMorning;
    private bool lastIsMorning;
    public UnityEvent onTimeChange;

    private void Awake()
    {
        System.DateTime now = System.DateTime.Now;
        isMorning = now.Hour < 12;
        lastIsMorning = isMorning;
    }

    private void Update()
    {
        System.DateTime now = System.DateTime.Now;
        isMorning = now.Hour < 12;
        string period = isMorning ? "AM" : "PM";
        timeText.text = now.ToString("hh:mm ") + period;

        if (isMorning != lastIsMorning)
        {
            lastIsMorning = isMorning;
            onTimeChange?.Invoke();
        }
    }
}
