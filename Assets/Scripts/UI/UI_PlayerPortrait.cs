using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_PlayerPortrait : MonoBehaviour, IPointerDownHandler {
    private Transform cameraTransform;
    private Transform rawImageTransform;
    private Transform nameTransform;
    private Transform healthBarTransform;
    private Transform moveBarTransform;
    private Camera portraitCamera;
    private RawImage rawImage;
    private RenderTexture outputTexture;
    private TextMeshProUGUI textMeshProUGUI;
    private UI_HealthBar healthBar;
    private UI_MoveBar moveBar;

    private Player player;

    private void Awake() {
        
    }

    private void Start() {
        
    }

    private void OnDestroy() {
        if (outputTexture != null) {
            outputTexture.Release();
            Destroy(outputTexture); 
        }
    } 
     
    private void Update() {
        UpdateCameraTransform();
    }

    private void UpdateCameraTransform() {
        if (player != null && cameraTransform != null) {
            Vector3 cameraOffset = new Vector3(player.transform.forward.x, player.transform.forward.y + player.GetHeight() * 0.85f, player.transform.forward.z * 0.9f);
            cameraTransform.position = player.GetWorldPosition() + cameraOffset;

            Vector3 reverseForward = player.GetRotation() * Vector3.back;
            cameraTransform.rotation = Quaternion.LookRotation(reverseForward, Vector3.up);
        }
    }




    private void FindCamera() {
        cameraTransform = transform.Find("PortraitCamera");
        if (cameraTransform != null) {
            portraitCamera = cameraTransform.GetComponent<Camera>();
            if (portraitCamera == null) {
                Debug.LogWarning("Camera component was not found");
            }
        } else {
            Debug.LogWarning("PortraitCamera was not found");
        }
    }

    private void FindRawImage() {
        rawImageTransform = transform.Find("RawImage");
        if (rawImageTransform != null) {
            rawImage = rawImageTransform.GetComponent<RawImage>();
            if (rawImage == null) {
                Debug.LogWarning("RawImage component was not found");
            }
        } else {
            Debug.LogWarning("RawImage was not found");
        } 
    }

    private void FindName() {
        nameTransform = transform.Find("Name");
        if (nameTransform != null) {
            textMeshProUGUI = nameTransform.GetComponent<TextMeshProUGUI>();
            if (textMeshProUGUI == null) {
                Debug.LogWarning("textMeshProUGUI component was not found");
            }
        } else {
            Debug.LogWarning("Name was not found");
        }
    }

    private void FindHealthBar() {
        healthBarTransform = transform.Find("HealthBar");
        if (healthBarTransform != null) {
            healthBar = healthBarTransform.GetComponent<UI_HealthBar>();
            if (healthBar == null) {
                Debug.LogWarning("healthBar component was not found");
            }
        } else {
            Debug.LogWarning("HealthBar was not found");
        }
    }

    private void FindMoveBar() {
        moveBarTransform = transform.Find("MoveBar");
        if (moveBarTransform != null) {
            moveBar = moveBarTransform.GetComponent<UI_MoveBar>();
            if (moveBar == null) {
                Debug.LogWarning("moveBar component was not found");
            }
        } else {
            Debug.LogWarning("MoveBar was not found");
        }
    }


    public void Initialize() {
        FindCamera();
        FindRawImage();
        FindName();
        FindHealthBar();
        FindMoveBar();

        outputTexture = new RenderTexture(256, 256, 16);
        outputTexture.Create();

        portraitCamera.targetTexture = outputTexture;

        rawImage.texture = outputTexture;
    }

    public void SetPlayer(Player player) {
        this.player = player;
        textMeshProUGUI.SetText(this.player.name);
        UpdateCameraTransform();
        healthBar.SetPlayer(this.player);
        moveBar.SetPlayer(this.player);

        player.UpdateBars();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData) {
        PlayerManager.GetInstance().SelectPlayer(player);
    }
}
