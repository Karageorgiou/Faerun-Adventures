using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtils {
    public class Mouse3D : MonoBehaviour {
        private static Mouse3D Instance;

        private RaycastHit currentHitInfo;

        [SerializeField] private LayerMask mouseColliderMask = new LayerMask();
        [SerializeField] private Camera activeCamera;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            if (activeCamera != null) {
                activeCamera = Camera.main;
            }
        }

        public void Update() {

            Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit worldHitInfo, 999f, mouseColliderMask)) {
                currentHitInfo = worldHitInfo;
                transform.position = worldHitInfo.point;
            }


        }

        public static Mouse3D GetInstance() {
            return Instance;
        }

        public bool IsMouseOverUI() {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }

        public Vector3 GetMouseWorldPosition() {
            return currentHitInfo.point;
        }

    }
}