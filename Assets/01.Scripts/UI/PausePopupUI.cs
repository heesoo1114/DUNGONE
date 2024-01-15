using UnityEngine.UI;
using UnityEngine;

public class PausePopupUI : PopUpUI
{
    [Header("Buttons")]
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button rejectButton;

    private void Awake()
    {
        acceptButton.onClick.AddListener(OnAcceptButtonPress);
        rejectButton.onClick.AddListener(OnRejectButtonPress);
    }

    public override void ShowUI()
    {
        base.ShowUI();
        GameManager.Instance.SetTimeScale(0f);
    }

    public override void CloseUI()
    {
        GameManager.Instance.SetTimeScale(1f);
        base.CloseUI();
    }

    private void OnAcceptButtonPress()
    {
        ScreenManager.Instance.ChangeScreenWithFade(toIntroScreen);
        CloseUI();
    }

    private void OnRejectButtonPress()
    {
        CloseUI();
    }
}
