using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{

    public GameObject toDrag;

    public bool dragging;

    public Vector2 beginDragPos;

    private void Update()
    {
        if (dragging)
        {
#if UNITY_EDITOR
            return;
#endif

            toDrag.GetComponent<RectTransform>().offsetMin += new Vector2(0, Input.GetTouch(0).deltaPosition.y);
            toDrag.GetComponent<RectTransform>().offsetMax += new Vector2(0, Input.GetTouch(0).deltaPosition.y);
        }
    }
}
