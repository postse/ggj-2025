using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private AudioSource popSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        popSound = GetComponent<AudioSource>();
    }

    void OnDestroy() {
        popSound.Play();
    }
}
