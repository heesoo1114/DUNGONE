using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Cursor")]
    [SerializeField] private Texture2D cursorSprite;

    #region Cursor

    public void SetCursor()
    {
        Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
        // ShowCursor();
    }

    public void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

}
