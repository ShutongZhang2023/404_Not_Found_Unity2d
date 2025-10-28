using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragParent : MonoBehaviour
{
    protected Vector3 offset;
    protected Vector3 originalPosition;
    protected bool isDragging = false;
    protected Camera cam;
    private Material mat;

    protected virtual void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    protected virtual void Start()
    {
        cam = Camera.main;
        originalPosition = transform.position;
    }

    protected virtual void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
    }

    protected virtual void OnMouseDrag()
    {
        if (!isDragging) return;
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
    }

    protected virtual void OnMouseUp()
    {
        isDragging = false;
        OnRelease();
    }

    protected virtual void OnRelease() { }

    public void returnOriginal()
    {
        transform.DOMove(originalPosition, 0.3f);
    }

    public void FadeOutAndDisable()
    {
        DOTween.To(
            () => mat.GetFloat("_DissolveAmount"),
            v => mat.SetFloat("_DissolveAmount", v),
             1f,
            1f
         ).OnComplete(() => gameObject.SetActive(false));
    }

    public void ShakeAndReturn()
    {
        transform.DOShakePosition(0.2f, 0.2f).OnComplete(() =>
        {
            transform.DOMove(originalPosition, 0.3f);
        });
    }
}
