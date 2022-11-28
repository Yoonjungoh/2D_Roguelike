using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler
{
    public Action<PointerEventData> OnPointerClickHandler = null;
    public Action<PointerEventData> OnBeginDragHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    public void OnPointerClick(PointerEventData eventData)  // Ŭ��
    {
        if (OnPointerClickHandler != null)
            OnPointerClickHandler.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ����
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)  // �巡�� ����
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke(eventData);
    }

}
