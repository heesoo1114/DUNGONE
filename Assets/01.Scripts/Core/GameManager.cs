using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 체크포인트 만들기 (짧게 짧게)

    public static GameManager Instance = null;

    [SerializeField] private PoolingListSO _poolingList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Multiple Gamemanager is running");
        }
        Instance = this;

        PoolManager.Instance = new PoolManager(transform);
        CreatePool();

        LockCursor();
    }

    private void CreatePool()
    {
        _poolingList.PoolList.ForEach(p =>
        {
            PoolManager.Instance.CreatePool(p.Prefab, p.Count);
        });
    }

    private void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
