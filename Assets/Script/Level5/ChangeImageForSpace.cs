using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeImageForSpace : MonoBehaviour
{
    [SerializeField] private Sprite emptySpace;

    public void ChangeImage()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySpace;
    }

}
