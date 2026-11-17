using UnityEngine;

public enum CollectibleType
{
    Coin,
    Magnet,
    Hoverboard,
    Jetpack,
    Shield
}

public class Collectible : MonoBehaviour
{
    public CollectibleType Type;
    public float SpinSpeed = 180f;

    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, SpinSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Collect(Type);
        }

        Destroy(gameObject);
    }
}
