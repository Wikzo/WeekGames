using UnityEngine;
using System.Collections;

public class FromWorldPointTextPositioner : IFloatingTextPositioner
{
    private readonly Camera cam;
    private readonly Vector3 worldPosition;
    private float timeToLive;
    private readonly float speed;
    private float yOffset;

    public FromWorldPointTextPositioner(Camera mainCam, Vector3 worldPos, float timeToLive, float speed)
    {
        this.cam = mainCam;
        this.worldPosition = worldPos;
        this.timeToLive = timeToLive;
        this.speed = speed;
    }

    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 sizeOfTextInPixels)
    {
        if ((timeToLive -= Time.deltaTime) <= 0)
            return false; // destroy

        // center text
        var screenPos = cam.WorldToScreenPoint(worldPosition);
        position.x = screenPos.x - (sizeOfTextInPixels.x / 2);
        position.y = Screen.height - screenPos.y - yOffset;

        yOffset += Time.deltaTime * speed;

        return true;
    }
}
