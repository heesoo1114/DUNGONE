using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class VolumeToggleData
{
    public bool bgmToggleIsOn = false;
    public bool sfxToggleIsOn = false;
}

public class SettingPopupUI : PopUpUI
{
    private VolumeToggleData volumeToggleSaveData = new VolumeToggleData();

    [Header("Buttons")]
    [SerializeField] private Button closeButton;

    [Header("AudioVolume")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Toggle bgmMusicToggle;
    [SerializeField] private Toggle sfxMusicToggle;

    private float bgmDefaultVolume = -10f;
    private float sfxDefaultVolume = -2f;
    private float muteDefaultVomue = -80f;

    private string bgmMixerGroupName = "bgmVolume";
    private string sfxMixerGroupName = "sfxVolume";

    private string dataSavePath = Application.dataPath + "/99.Others/SaveData/";
    private string saveFileName = "volumeSaveData.json";

    private void Awake()
    {
        closeButton.onClick.AddListener(CloseUI);

        bgmMusicToggle.onValueChanged.AddListener(SetBgmSoundMute);
        sfxMusicToggle.onValueChanged.AddListener(SetSfxSoundMute);
    }

    private void Start()
    {
        LoadVolumeData();
    }

    public override void ShowUI()
    {
        base.ShowUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
        UIManager.Instance.ShowCursor();
        SaveVolumeData();
    }

    private void SaveVolumeData()
    {
        volumeToggleSaveData.bgmToggleIsOn = bgmMusicToggle.isOn;
        volumeToggleSaveData.sfxToggleIsOn = sfxMusicToggle.isOn;

        string data = JsonUtility.ToJson(volumeToggleSaveData);
        File.WriteAllText(dataSavePath + saveFileName, data);
    }

    private void LoadVolumeData()
    {
        if (File.Exists(dataSavePath + saveFileName))
        {
            string data = File.ReadAllText(dataSavePath + saveFileName);
            volumeToggleSaveData = JsonUtility.FromJson<VolumeToggleData>(data);

            bgmMusicToggle.isOn = volumeToggleSaveData.bgmToggleIsOn;
            sfxMusicToggle.isOn = volumeToggleSaveData.sfxToggleIsOn;
        }
        else
        {
            Debug.Log("파일이 존재하지 않습니다.");
        }
    }

    private void SetBgmSoundMute(bool isOn)
    {
        if (isOn)
        {
            // mute off
            _audioMixer.SetFloat(bgmMixerGroupName, bgmDefaultVolume);
        }
        else
        {
            // mute on
            _audioMixer.SetFloat(bgmMixerGroupName, muteDefaultVomue);
        }
    }

    private void SetSfxSoundMute(bool isOn)
    {
        if (isOn)
        {
            // mute off
            _audioMixer.SetFloat(sfxMixerGroupName, sfxDefaultVolume);
        }
        else
        {
            // mute on
            _audioMixer.SetFloat(sfxMixerGroupName, muteDefaultVomue);
        }
    }
}
