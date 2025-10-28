using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public bool draggable = false;
    private Camera cam;
    private Vector3 offset;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        if (!draggable) return;
        transform.position = GetMouseWorldPos() + offset;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        return pos;
    }
}
