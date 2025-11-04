using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TimeWord : DraggableWord
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite morningSprite;
    [SerializeField] private Sprite eveningSprite;
    private TimeChecker timeChecker;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        timeChecker = FindObjectOfType<TimeChecker>();
        timeChecker.onTimeChange.AddListener(onTimeChange);
        spriteRenderer.sprite = timeChecker.isMorning ? morningSprite : eveningSprite;
        word = timeChecker.isMorning ? "MORNING" : "EVENING";
    }

    public void onTimeChange() 
    {
        spriteRenderer.sprite = timeChecker.isMorning ? morningSprite : eveningSprite;
        word = timeChecker.isMorning ? "MORNING" : "EVENING";
    }
}
