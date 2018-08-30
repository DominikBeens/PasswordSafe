﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// Old class. Not in use right now.
public class Draggable : MonoBehaviour
{

    private Drag drag;

    public enum Type
    {
        Folder,
        InfoBlock
    }
    public Type type;

    private void Start()
    {
        if (type == Type.Folder)
        {
            drag = GameObject.FindWithTag("HomePanel").GetComponent<Drag>();
        }
        else if (type == Type.InfoBlock)
        {
            drag = GameObject.FindWithTag("InfoBlockPanel").GetComponent<Drag>();
        }
    }

    public void BeginDrag()
    {
        drag.dragging = true;

#if UNITY_EDITOR
        return;
#endif

        drag.beginDragPos = Input.GetTouch(0).position;
    }

    public void EndDrag()
    {
        drag.dragging = false;
    }

    //public void Test()
    //{
    //    UnityEngine.UI.ScrollRect test = GetComponentInParent<UnityEngine.UI.ScrollRect>();
    //    test.verticalNormalizedPosition += Input.GetTouch(0).deltaPosition.magnitude;
    //}
}
