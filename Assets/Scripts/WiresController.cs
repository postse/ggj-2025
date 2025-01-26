using UnityEngine;

public class WiresController : MonoBehaviour
{
    public float wireSize = 5.0f;

    void Start()
    {
        GameObject particles = GetComponentInChildren<ParticleSystem>().gameObject;
        particles.transform.localScale = particles.transform.localScale * wireSize;
        transform.localScale = transform.localScale * wireSize;
    }
}
