using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler {

    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Canvas canvas;
     
    private void Awake() {
        if (_rectTransform == null) {
            _rectTransform = transform.parent.GetComponent<RectTransform>();
        }

        FindCanvas();
    }

    public void OnPointerDown(PointerEventData eventData) {
        _rectTransform.SetAsLastSibling();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData) {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        //throw new System.NotImplementedException();
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

    
}