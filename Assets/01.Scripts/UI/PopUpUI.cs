using UnityEngine;

public class PopUpUI : MonoBehaviour
{
    protected string toIntroScreen = "Intro";
    protected string toGameScreen = "RealPlay";

    public bool IsActive { get; private set; }

    public virtual void ShowUI()
    {
        IsActive = true;
        gameObject.SetActive(true);

        GameManager.Instance.BlockPlayerInput();
        UIManager.Instance.ShowCursor();
    }

    public virtual void CloseUI()
    {
        IsActive = false;
        gameObject.SetActive(false);

        GameManager.Instance.RecievePlayerInput();
        UIManager.Instance.LockCursor();
    }
}
