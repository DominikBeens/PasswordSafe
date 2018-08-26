using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
