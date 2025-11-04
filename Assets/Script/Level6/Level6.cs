using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Level6 : Level3PuzzleManager
{
    public List<string> predefinedLines = new List<string>();
    [SerializeField] private TimeChecker timeChecker;

    protected override void Start()
    {
        GenerateGrid();
        ApplySequentialLines();
        decideTime();
        PlaceWordsManually();
    }

    private void ApplySequentialLines() {
        for (int r = 0; r < predefinedLines.Count; r++) {
            string line = predefinedLines[r];
            for (int c = 0; c < cols; c++) {
                char ch = line[c];
                Word tile = grid[r, c];
                tile.Init(ch, r, c, GetDefaultSprite(ch));
            }
        }
    }

    private void decideTime() {
        if (timeChecker.isMorning)
        {
            wordPlacements[0].word = "MORNING";
        }
        else {
            wordPlacements[0].word = "EVENING";
        }
    }

    public void TimeChange()
    {
        var newWord = timeChecker.isMorning ? "MORNING" : "EVENING";
        wordPlacements[0].word = newWord;
        int r = wordPlacements[0].startRow;
        int c = wordPlacements[0].startCol;
        int dx = wordPlacements[0].dx;
        int dy = wordPlacements[0].dy;

        for (int i = 0; i < newWord.Length; i++) { 
            char ch = newWord[i];
            int row = r + dy * i;
            int col = c + dx * i;
            Word tile = grid[row, col];
            if (tile.isLocked)
                tile.Init(ch, row, col, GetActiveSprite(ch));
            else
                tile.Init(ch, row, col, GetDefaultSprite(ch));
        }
    }

}
