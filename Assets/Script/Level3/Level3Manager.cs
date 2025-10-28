using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Manager : MonoBehaviour
{
    [System.Serializable]
    public class SpriteSwapTarget
    {
        public SpriteRenderer renderer;
        public Sprite english;
        public Sprite other;
    }

    [Header("Scene Sprites")]
    public List<SpriteSwapTarget> targets = new List<SpriteSwapTarget>();

    void Awake()
    {
       GameManager.Instance.currentLanguage = Language.Other;
    }
    void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Translator += HandleSpecialLanguage;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Translator -= HandleSpecialLanguage;
    }

    private void HandleSpecialLanguage()
    {
        var lang = GameManager.Instance.currentLanguage;
        foreach (var t in targets)
        {
            if (t == null || t.renderer == null) continue;
            if (lang == Language.English && t.english != null) t.renderer.sprite = t.english;
            if (lang == Language.Other && t.other != null) t.renderer.sprite = t.other;
        }
        if (lang == Language.English)
        {
            Level3PuzzleManager.Instance.ChangeSriteSet(1);
        }
        else {
            Level3PuzzleManager.Instance.ChangeSriteSet(0);
        }
    }
}
