using UnityEngine.UI;
using UnityEngine;

public class InGameScreenUI : MonoBehaviour
{
    [Header("PopupUIs")]
    [SerializeField] private PausePopupUI pausePopupUI;
    [SerializeField] private GameDonePopupUI gameDonePopupUI;

    private void Awake()
    {
        GameManager.Instance.OnGameDoneEvent += GameDonePopup;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (false == pausePopupUI?.IsActive)
            {
                pausePopupUI.ShowUI();
            }
            else
            {
                pausePopupUI.CloseUI();
            }
        }
    }

    public void GameDonePopup(bool isPositive)
    {
        gameDonePopupUI.SetMessageText(isPositive);
        gameDonePopupUI.ShowUI();
    }
}
