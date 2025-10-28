using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterSpriteSet
{
    [Tooltip("用于在 Inspector 中区分不同皮肤")]
    public string setName = "Default";

    [Header("A~Z 的默认外观（索引 0->A, 1->B ... 25->Z）")]
    public Sprite[] defaultSprites = new Sprite[26];

    [Header("A~Z 的激活外观（用于预览高亮 + 匹配锁定）。若为空则回落到 defaultSprites")]
    public Sprite[] highlightSprites = new Sprite[26];
}


public class Level3PuzzleManager : MonoBehaviour, IPuzzleManager
{
    public static Level3PuzzleManager Instance;

    [Header("Grid Settings")]
    public int rows = 10;
    public int cols = 10;
    public GameObject tilePrefab;
    public Transform gridParent;
    public float cellSize = 0.3f;

    [Header("多组字母皮肤")]
    public List<LetterSpriteSet> spriteSets = new List<LetterSpriteSet>();
    [Tooltip("当前生效的皮肤组索引")]
    public int activeSpriteSetIndex = 0;

    [Header("手动放置单词")]
    public List<WordPlacement> wordPlacements = new List<WordPlacement>();

    [Header("匹配成功时生成的物体")]
    public Transform spawnAnchor;
    public float verticalSpacing = 1.0f;

    // 运行态
    private Word[,] grid;
    private List<Word> currentSelection = new();
    private Vector2Int? startPos = null;
    private Vector2Int? currentMousePos = null;
    private bool isSelecting = false;
    private int matchedCount = 0;

    // 活跃皮肤字典
    private Dictionary<char, Sprite> defaultDict = new();
    private Dictionary<char, Sprite> activeDict = new();

    private void Awake()
    {
        Instance = this;
        RebuildSpriteDictionaries();
    }

    private void Start()
    {
        GenerateGrid();
        PlaceWordsManually();
        FillRandomLetters();
    }

    private void RebuildSpriteDictionaries() {
        defaultDict.Clear();
        activeDict.Clear();

        var set = spriteSets[activeSpriteSetIndex];

        for (int i = 0; i < 26; i++)
        {
            char ch = (char)('A' + i);
            var def = set.defaultSprites[i];
            var act = set.highlightSprites[i];

            // 如果激活外观为空，则使用默认外观
            if (act == null) act = def;

            defaultDict[ch] = def;
            activeDict[ch] = act;
        }
    }

    public void ChangeSriteSet(int setNum) { 
        activeSpriteSetIndex = setNum;
        RebuildSpriteDictionaries();
        resetSprite();
    }

    private void resetSprite() {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Word tile = grid[r, c];
                    if (!tile.isLocked)
                        tile.spriteRenderer.sprite = GetDefaultSprite(tile.letter);
                    else
                    {
                        tile.spriteRenderer.sprite = GetActiveSprite(tile.letter);
                    }
                }
        }
    }

    private Sprite GetDefaultSprite(char ch) => defaultDict.TryGetValue(ch, out var s) ? s : null;
    private Sprite GetActiveSprite(char ch) => activeDict.TryGetValue(ch, out var s) ? s : GetDefaultSprite(ch);

    private void GenerateGrid()
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
                tile.Init(' ', r, c, GetDefaultSprite('A'));
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

                grid[row, col].Init(ch, row, col, GetDefaultSprite(ch));
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
                    tile.Init(rand, r, c, GetDefaultSprite(rand));
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

    public void EndSelection() {
        if (!isSelecting) return;

        if (currentSelection.Count == 0) { ResetSelecting(); return; }

        string word = "";
        foreach (var w in currentSelection)
            word += w.letter;

        bool matched = false;

        foreach (var placement in wordPlacements)
        {
            if (word == placement.word)
            {
                matched = true;
                if (placement.prefabToSpawn != null && spawnAnchor != null)
                {
                    Vector3 spawnPos = spawnAnchor.position + spawnAnchor.TransformDirection(Vector3.down) * verticalSpacing * matchedCount;
                    Quaternion spawnRot = spawnAnchor.rotation;
                    Instantiate(placement.prefabToSpawn, spawnPos, spawnRot, spawnAnchor);
                    matchedCount++;
                }
                break;
            }
        }

        foreach (var w in currentSelection)
        {
            char ch = w.letter;
            if (matched)
            {
                w.spriteRenderer.sprite = GetActiveSprite(ch);
                w.isLocked = true;
            }
            else
            {
                if (!w.isLocked)
                    w.spriteRenderer.sprite = GetDefaultSprite(ch);
            }
        }

        ResetSelecting();
    }

    private void UpdateSelectionPreview() {
        if (!isSelecting) return;

        ClearPreviewSprites();
        if (startPos == null || currentMousePos == null) return;

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
        if (direction == Vector2Int.zero) return;

        int steps = Mathf.RoundToInt(delta.magnitude);
        Vector2Int pos = start;

        for (int i = 0; i <= steps; i++)
        {
            if (!IsInsideGrid(pos)) break;

            Word tile = grid[pos.x, pos.y];
            currentSelection.Add(tile);

            if (!tile.isLocked)
            {
                char ch = tile.letter;
                tile.spriteRenderer.sprite = GetActiveSprite(ch);
            }

            pos += direction;
        }
    }

    private void ClearPreviewSprites() {
        foreach (var w in currentSelection)
        {
            if (!w.isLocked)
            {
                char ch = w.letter == ' ' ? 'A' : w.letter;
                w.spriteRenderer.sprite = GetDefaultSprite(ch);
            }
        }
        currentSelection.Clear();
    }

    private void ResetSelecting()
    {
        startPos = null;
        currentMousePos = null;
        currentSelection.Clear();
        isSelecting = false;
    }


    // =============== Helpers ===============
    private Vector2Int GetNearest8Direction(Vector2Int delta)
    {
        if (delta == Vector2Int.zero) return Vector2Int.zero;

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        if (angle >= -22.5f && angle < 22.5f) return new Vector2Int(1, 0);   // →
        if (angle >= 22.5f && angle < 67.5f) return new Vector2Int(1, 1);   // ↘
        if (angle >= 67.5f && angle < 112.5f) return new Vector2Int(0, 1);   // ↓
        if (angle >= 112.5f && angle < 157.5f) return new Vector2Int(-1, 1);  // ↙
        if (angle >= 157.5f || angle < -157.5f) return new Vector2Int(-1, 0); // ←
        if (angle >= -157.5f && angle < -112.5f) return new Vector2Int(-1, -1); // ↖
        if (angle >= -112.5f && angle < -67.5f) return new Vector2Int(0, -1);  // ↑
        if (angle >= -67.5f && angle < -22.5f) return new Vector2Int(1, -1);  // ↗

        return Vector2Int.zero;
    }

    private bool IsInsideGrid(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < cols;
    }
}
