using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLetter : DragParent
{
    public char letter;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            AudioManager.Instance.PlayHitGroundSFX();
        }

        if (collision.gameObject.CompareTag("WordBlock"))
        {
            AudioManager.Instance.PlayHitEachOtherSFX();
        }
    }

    protected override void OnRelease()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("DropZone"));
        if (hit != null && hit.CompareTag("DropZone"))
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.DOMove(hit.transform.position, 0.2f);
            hit.gameObject.GetComponent<LetterDrop>().updateLetter(letter);
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
