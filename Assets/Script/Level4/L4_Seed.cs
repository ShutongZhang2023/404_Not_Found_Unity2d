using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L4_Seed : DragParent
{
    public string word;
    public LayerMask dropZoneLayer;

    protected override void OnRelease()
    {
        Collider2D hit = Physics2D.OverlapPoint(transform.position, dropZoneLayer);
        if (hit != null)
        {
            L4_TreeZone zone = hit.GetComponent<L4_TreeZone>();
            zone.TryAcceptWord(this);
            return;
        }

        returnOriginal();
    }
}
