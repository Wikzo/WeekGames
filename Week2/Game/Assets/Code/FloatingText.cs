using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    private static readonly GUISkin Skin = Resources.Load<GUISkin>("GameSkin");

    public static FloatingText Show(string text, string style, IFloatingTextPositioner positioner)
    {
        var go = new GameObject("FloatingText");
        var floatingText = go.AddComponent<FloatingText>();
        floatingText.Style = Skin.GetStyle(style);
        floatingText.positioner = positioner;
        floatingText.content = new GUIContent(text);

        return floatingText; // not really used...
    }

    private GUIContent content;
    private IFloatingTextPositioner positioner;

    public string Text { get { return content.text; } set { content.text = value; } }
    public GUIStyle Style { get; set; }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(content);

        if (!positioner.GetPosition(ref position, content, contentSize))
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, contentSize.x, contentSize.y), content, Style);
    }
}
