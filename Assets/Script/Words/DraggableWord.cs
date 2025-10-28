using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableWord : MonoBehaviour
{
    public string word;
    public LayerMask dropZoneLayer;

    private Vector3 offset;
    private Vector3 originalPosition;
    private bool isDragging = false;
    private Camera cam;
    private Material mat;

    private void Awake()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Start()
    {
        cam = Camera.main;
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        transform.position = cam.ScreenToWorldPoint(Input.mousePosition) + offset;
    }

    private void OnMouseUp()
    {
        isDragging = false;

        Collider2D hit = Physics2D.OverlapPoint(transform.position, dropZoneLayer);
        if (hit != null)
        {
            DropZone zone = hit.GetComponent<DropZone>();
            zone.TryAcceptWord(this);
            return;
        }

        returnOriginal();

    }

    public void ShakeAndReturn()
    {
        transform.DOShakePosition(0.2f, 0.2f).OnComplete(() =>
        {
            transform.DOMove(originalPosition, 0.3f);
        });
    }

    public void returnOriginal() {
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
}
