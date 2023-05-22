using System.Collections;
using System.Collections.Generic;
using GameUtils;
using TMPro;
using UnityEngine;

public class UI_Tooltip : MonoBehaviour {
    private static UI_Tooltip Instance;


    [SerializeField] private RectTransform canvasRectTransform;

    private RectTransform backgroundRectTransform;
    private TextMeshProUGUI text;
    private RectTransform rectTransform;

    private System.Func<string> getTooltipTextFunc;

    private void Awake() {
        Instance = this;

        backgroundRectTransform = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rectTransform = transform.GetComponent<RectTransform>();

        if (canvasRectTransform == null) {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            if (canvas != null) {
                canvasRectTransform = canvas.GetComponent<RectTransform>();
            } else {
                Debug.LogError("Canvas not found in the scene!");
            }
        }


        HideTooltip();
    }

    void Start() { }

    private void Update() {
        SetText(getTooltipTextFunc());

        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;
        //Debug.Log(anchoredPosition);

        anchoredPosition -= new Vector2(Screen.width * .59f,Screen.height * .59f);

        if (anchoredPosition.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
            // Tooltip left screen on right side
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if (anchoredPosition.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
            // Tooltip left screen on top side
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }


    public static UI_Tooltip GetInstance() {
        return Instance;
    }

    private void SetText(string tooltipText) {
        text.SetText(tooltipText);
        text.ForceMeshUpdate();

        Vector2 textSize = text.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8, 8);

        backgroundRectTransform.sizeDelta = textSize + paddingSize;
    }

    private void HideTooltip() {
        gameObject.SetActive(false);
    }

    private void ShowTooltip(string tooltipText) {
        ShowTooltip(() => tooltipText);
    }

    private void ShowTooltip(System.Func<string> getTooltipTextFunc) {
        this.getTooltipTextFunc = getTooltipTextFunc;
        gameObject.SetActive(true);
        SetText(getTooltipTextFunc());
    }





    public static void ShowTooltip_Static(string tooltipText) {
        Instance.ShowTooltip(tooltipText);
    }

    public static void ShowTooltip_Static(System.Func<string> getTooltipTextFunc) {
        Instance.ShowTooltip(getTooltipTextFunc);
    }

    public static void HideTooltip_Static() {
        Instance.HideTooltip();
    }



}
