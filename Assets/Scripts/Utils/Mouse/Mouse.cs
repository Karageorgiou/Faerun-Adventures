using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 
    --------------------------------------------------
    from:      unitycodemonkey.com
    --------------------------------------------------
*/

namespace GameUtils {

    public static class Mouse {

        public static Vector3 GetMouseWorldPosition() {
            Vector3 mousePos = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            mousePos.y = 0f;
            return mousePos;
        }
        public static Vector3 GetMouseWorldPositionWithZ() {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }

}
