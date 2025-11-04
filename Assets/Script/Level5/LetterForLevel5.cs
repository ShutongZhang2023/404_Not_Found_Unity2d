using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LetterForLevel5 : Word
{
    private int hitCount = 0;
    [SerializeField] private GameObject dropableWordPrefab;

    public void addHitCount() { 
        hitCount++;
        CreateDropableWord();
    }

    private void CreateDropableWord() { 
        if (hitCount >= 2)
        {
            GameObject dropableWord = Instantiate(dropableWordPrefab, transform.position, Quaternion.identity);
            dropableWord.GetComponent<SpriteRenderer>().sprite = this.spriteRenderer.sprite;
            dropableWord.GetComponent<DropLetter>().letter = this.letter;
            Destroy(this.gameObject);
        }
    }
}
