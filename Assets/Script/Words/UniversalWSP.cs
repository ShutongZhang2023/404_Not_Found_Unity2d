using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalWSP : MonoBehaviour
{
    public static UniversalWSP Instance;

    [Header("Grid Settings")]
    public int rows = 10;
    public int cols = 10;
    public GameObject tilePrefab;
    public Transform gridParent;
    public float cellSize = 0.3f;

    [Header("Sprites A-Z")]
    public Sprite[] letterSprites;

    [Header("Manual Word Placement")]
    public List<WordPlacement> wordPlacements = new List<WordPlacement>();

    [Header("Color Setting")]
    public Color highlightColor = new Color(1f, 0.5f, 0f); // Orange for selection preview
    public Color correctColor = Color.green; // Green for correct words
    public Color defaultColor = Color.white; // Default color for tiles

    private Dictionary<char, Sprite> spriteDict = new();
    private Word[,] grid;
    private List<Word> currentSelection = new();

    private Vector2Int? startPos = null;
    private Vector2Int? currentMousePos = null;
    private bool isSelecting = false;

    private void Awake()
    {
        Instance = this;
        foreach (Sprite s in letterSprites)
        {
            char c = s.name[0];
            spriteDict[c] = s;
        }
    }

    private void Start()
    {
        GenerateGrid();
        PlaceWordsManually();
        FillRandomLetters();
    }

    void GenerateGrid()
    {
        grid = new Word[rows, cols];
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GameObject go = Instantiate(tilePrefab);
                go.transform.SetParent(gridParent);
                go.transform.localPosition = new Vector3(c * cellSize, -r * cellSize, 0f);
                Word tile = go.GetComponent<Word>();
                tile.Init(' ', r, c, spriteDict['A']);
                grid[r, c] = tile;
            }
        }
    }

    void PlaceWordsManually()
    {
        foreach (var placement in wordPlacements)
        {
            string word = placement.word.ToUpper();
            int r = placement.startRow;
            int c = placement.startCol;
            int dx = placement.dx;
            int dy = placement.dy;

            for (int i = 0; i < word.Length; i++)
            {
                char ch = word[i];
                int row = r + dy * i;
                int col = c + dx * i;

                grid[row, col].Init(ch, row, col, spriteDict[ch]);
            }
        }
    }

    void FillRandomLetters()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Word tile = grid[r, c];
                if (tile.letter == ' ')
                {
                    char rand = (char)('A' + Random.Range(0, 26));
                    tile.Init(rand, r, c, spriteDict[rand]);
                }
            }
        }
    }

    // ------------------ Selection --------------------
    public void BeginSelection(Word w)
    {
        isSelecting = true;
        startPos = new Vector2Int(w.row, w.col);
        currentMousePos = null;
        UpdateSelectionPreview();
    }

    public void ContinueSelection(Word w)
    {
        if (!isSelecting) return;
        currentMousePos = new Vector2Int(w.row, w.col);
        UpdateSelectionPreview();
    }

    void UpdateSelectionPreview()
    {
        if (!isSelecting) return;
        ClearAllHighlights();
        currentSelection.Clear();

        if (startPos == null || currentMousePos == null)
            return;

        Vector2Int start = startPos.Value;
        Vector2Int end = currentMousePos.Value;

        if (!IsInsideGrid(start) || !IsInsideGrid(end))
        {
            Debug.LogWarning("Start or end outside of grid bounds.");
            return;
        }

        Vector2Int delta = end - start;

        Vector2Int direction = GetNearest8Direction(delta);
        if (direction == Vector2Int.zero)
            return;

        float length = delta.magnitude;
        int steps = Mathf.RoundToInt(length); // 向最近整数格取整

        Vector2Int pos = start;
        for (int i = 0; i <= steps; i++)
        {
            if (!IsInsideGrid(pos))
                break;

            Word tile = grid[pos.x, pos.y];
            currentSelection.Add(tile);
            tile.SetColor(highlightColor);

            pos += direction;
        }
    }


    public void EndSelection()
    {
        if (!isSelecting) return;

        if (currentSelection.Count == 0) return;

        string word = "";
        foreach (var w in currentSelection)
            word += w.letter;

        bool matched = false;
        foreach (var placement in wordPlacements)
        {
            if (word == placement.word)
            {
                matched = true;
                break;
            }
        }

        foreach (var w in currentSelection)
        {
            if (matched)
            {
                w.SetColor(correctColor);
                w.isLocked = true;
            }
            else
            {
                w.SetColor(defaultColor);
            }
        }

        startPos = null;
        currentMousePos = null;
        currentSelection.Clear();
        isSelecting = false;
    }

    public void ClearAllHighlights()
    {
        foreach (Word w in currentSelection)
        {
            w.SetColor(defaultColor);
        }
        currentSelection.Clear();
    }

    Vector2Int GetNearest8Direction(Vector2Int delta)
    {
        if (delta == Vector2Int.zero)
            return Vector2Int.zero;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle >= -22.5f && angle < 22.5f)
            return new Vector2Int(1, 0);      // →
        if (angle >= 22.5f && angle < 67.5f)
            return new Vector2Int(1, 1);      // ↘
        if (angle >= 67.5f && angle < 112.5f)
            return new Vector2Int(0, 1);      // ↓
        if (angle >= 112.5f && angle < 157.5f)
            return new Vector2Int(-1, 1);     // ↙
        if (angle >= 157.5f || angle < -157.5f)
            return new Vector2Int(-1, 0);     // ←
        if (angle >= -157.5f && angle < -112.5f)
            return new Vector2Int(-1, -1);    // ↖
        if (angle >= -112.5f && angle < -67.5f)
            return new Vector2Int(0, -1);     // ↑
        if (angle >= -67.5f && angle < -22.5f)
            return new Vector2Int(1, -1);     // ↗

        return Vector2Int.zero;
    }

    bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < cols;
    }
}
