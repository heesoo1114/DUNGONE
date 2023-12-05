using UnityEngine;

public abstract class PoolableMono : MonoBehaviour
{
    public string id;
    public abstract void Init();
}