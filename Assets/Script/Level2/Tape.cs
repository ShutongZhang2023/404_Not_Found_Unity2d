using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tape : MonoBehaviour
{
    public float breakDistance = 1.5f; // 撕裂判定的距离
    private Camera cam;
    private Vector3 startDragPos;
    private bool isDragging = false;
    public GameObject draggable;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        startDragPos = GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 currentPos = GetMouseWorldPos();
        float distance = Vector3.Distance(startDragPos, currentPos);

        if (distance >= breakDistance)
        {
            isDragging = false;
            OnTapeRipped();
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    private void OnTapeRipped()
    {
        AudioManager.Instance.PlayTearSFX();
        draggable.GetComponent<Draggable>().draggable = true; // 撕裂后允许拖动
        Destroy(gameObject); // 撕裂后销毁胶带对象
    }
}
