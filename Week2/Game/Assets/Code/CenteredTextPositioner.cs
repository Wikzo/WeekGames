using UnityEngine;

public class CenteredTextPositioner : IFloatingTextPositioner
{
    private readonly float speed;
    private float textPosition;

    public CenteredTextPositioner(float speed)
    {
        this.speed = speed;
    }
    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 sizeOfTextInPixels)
    {
        textPosition += Time.deltaTime * speed;

        if (textPosition > 1)
            return false;

        position = new Vector2(Screen.width / 2f - sizeOfTextInPixels.x / 2f,
            Mathf.Lerp(Screen.height / 2f + sizeOfTextInPixels.y, 0, textPosition));
        return true;
    }
}
