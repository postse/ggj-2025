using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Rigidbody player;

    [SerializeField]
    private float cameraParallaxMultiplier = 2.0f;
    private static float cameraShakeIntensity = 0.1f;

    private static bool isShaking = false;

    private static CameraController _instance;

    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject(typeof(CameraController).ToString());
                    _instance = singleton.AddComponent<CameraController>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 cameraPosition = new Vector3(player.transform.position.x / cameraParallaxMultiplier, player.transform.position.y / cameraParallaxMultiplier, transform.position.z);

        if (isShaking)
        {
            cameraPosition.x += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
            cameraPosition.y += Random.Range(-1f * cameraShakeIntensity, cameraShakeIntensity);
        }

        transform.position = cameraPosition;
        transform.LookAt(player.transform.position / (cameraParallaxMultiplier * 2));
    }

    public static void ShakeCamera(float seconds, float intensity)
    {
        cameraShakeIntensity = intensity;
        isShaking = true;
        if (Instance != null)
        {
            Instance.StartCoroutine(Instance.StopShakeAfterDelay(seconds));
        }
    }

    private IEnumerator StopShakeAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isShaking = false;
    }
}
