using UnityEngine.UI;
using UnityEngine;

public class IntroScreenUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingButton;    
    [SerializeField] private Button exitButton;

    [Header("PopupUIs")]
    [SerializeField] private SettingPopupUI settingPopup;

    private string toPlayScreen = "RealPlay";

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonPress);
        settingButton.onClick.AddListener(OnSettingButtonPress);
        exitButton.onClick.AddListener(OnExitButtonPress);
    }

    private void OnPlayButtonPress()
    {
        ScreenManager.Instance.ChangeScreenWithFade(toPlayScreen);
    }

    private void OnSettingButtonPress()
    {
        settingPopup.ShowUI();
    }

    private void OnExitButtonPress()
    {
        GameManager.Instance.CloseGame();
    }
}
