using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameDonePopupUI : PopUpUI
{
    [Header("Buttons")]
    [SerializeField] private Button continueButton;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI messageText;

    private string positiveMessage = "Game Clear!!!";
    private string negativeMessage = "Game Over...";

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinueButtonPress);
    }

    public void SetMessageText(bool isPositive)
    {
        if (isPositive)
        {
            messageText.text = positiveMessage;
        }
        else
        {
            messageText.text = negativeMessage;
        }
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

    private void OnContinueButtonPress()
    {
        ScreenManager.Instance.ChangeScreenWithFade(toIntroScreen);
        CloseUI();
    }
}
