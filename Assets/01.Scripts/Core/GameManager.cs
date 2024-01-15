using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>   
{
    [SerializeField] private PoolingListSO _poolingListSO;

    public PlayerController PlayerController { get; private set; }

    public event Action<bool> OnGameDoneEvent;

    private void Awake()
    {
        PoolManager.Instance = new PoolManager(transform);
        CreatePool();

        UIManager.Instance.SetCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnGameDone();
        }
    }

    // �÷��̾ �׾��� �� or �÷��̾ ������ Ŭ���� �Ͽ��� ��
    // �ش� �Լ��� �����Ű��.
    public void OnGameDone()
    {
        bool isPositive = PlayerController.IsAlive;
        OnGameDoneEvent?.Invoke(isPositive);
    }

    private void CreatePool()
    {
        _poolingListSO.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }

    public void SetPlayerController(PlayerController playerController)
    {
        PlayerController = playerController;
    }

    public void RecievePlayerInput()
    {
        PlayerController?.RecieveInput();
    }

    public void BlockPlayerInput()
    {
        PlayerController?.BlockInput();
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
    }

    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
