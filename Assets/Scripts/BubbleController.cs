using UnityEngine;

public class BubbleController : MonoBehaviour
{
    AudioSource bubblePopSound;

    // Update is called once per frame
    void Start()
    {
        bubblePopSound = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        bubblePopSound.Play();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, bubblePopSound.clip.length);
    }
}
