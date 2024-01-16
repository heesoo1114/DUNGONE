using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.OnGameDone();
        }
    }
}
