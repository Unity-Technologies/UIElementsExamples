using UnityEngine;

public class CursorSetter : MonoBehaviour
{
    public Texture2D cursor;
    void LateUpdate()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
}
