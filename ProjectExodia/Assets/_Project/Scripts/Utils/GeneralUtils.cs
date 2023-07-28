using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExodia
{
    public static class GeneralUtils 
    {
        public static Vector3 ScreenToWorld(Camera camera, Vector2 screenPosition) => 
            camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, camera.nearClipPlane));
    }
}
