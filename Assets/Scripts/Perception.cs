using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    public string tagName;
    public float maxDistance;
    public float maxAngel;

    public abstract GameObject[] GetGameObjects();
}
