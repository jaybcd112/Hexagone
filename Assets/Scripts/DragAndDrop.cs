using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform tform;
    private CanvasGroup cgroup;
    private void Awake()
    {
        tform = GetComponent<Transform>();
        cgroup = GetComponent<CanvasGroup>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cgroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        tform.position += new Vector3(eventData.delta.x / 80, eventData.delta.y / 80);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cgroup.blocksRaycasts = true;
    }
}
