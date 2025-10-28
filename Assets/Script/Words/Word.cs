using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Word : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    public SpriteRenderer spriteRenderer;
    public char letter;
    public int row, col;
    public bool isLocked;

    private IPuzzleManager GetActiveManager()
    {
        if (WordSearchPuzzle.Instance != null) return WordSearchPuzzle.Instance;
        if (Level3PuzzleManager.Instance != null) return Level3PuzzleManager.Instance;
        return null;
    }

    public void Init(char l, int r, int c, Sprite s)
    {
        letter = l;
        row = r;
        col = c;
        spriteRenderer.sprite = s;
    }

    public void SetColor(Color color) {
        if (isLocked) return;
        spriteRenderer.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetActiveManager()?.BeginSelection(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetActiveManager()?.ContinueSelection(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetActiveManager()?.EndSelection();
    }
}
