using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCursorOnStart : MonoBehaviour {
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    private void OnEnable()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
}
