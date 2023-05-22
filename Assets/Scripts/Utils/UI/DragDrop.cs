using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler {

    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        FindCanvas();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void FindCanvas() {
        if (canvas == null) {
            Transform canvasTransform = transform.parent;
            while (canvasTransform != null) {
                canvas = canvasTransform.GetComponent<Canvas>();
                if (canvas != null) {
                    break;
                }
                canvasTransform = canvasTransform.parent;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("PointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("BeginDrag");
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        
    }

    public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("EndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData) {
        //Debug.Log("Drag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}

