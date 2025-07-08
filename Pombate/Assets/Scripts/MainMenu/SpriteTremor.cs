using UnityEngine;

public class SpriteTremor : MonoBehaviour
{
    public float tremorDuration = 0.2f;  // Duração do tremor (em segundos)
    public float tremorMagnitude = 0.05f;  // Intensidade da tremedeira
    public float tremorInterval = 0.5f;  // Intervalo entre cada tremor

    private Vector3 originalPosition;
    private float timer = 0f;
    private bool isShaking = false;
    private float shakeTimer = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!isShaking && timer >= tremorInterval)
        {
            isShaking = true;
            shakeTimer = 0f;
            timer = 0f;
        }

        if (isShaking)
        {
            shakeTimer += Time.deltaTime;

            if (shakeTimer <= tremorDuration)
            {
                float offsetX = Random.Range(-1f, 1f) * tremorMagnitude;
                float offsetY = Random.Range(-1f, 1f) * tremorMagnitude;
                transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);
            }
            else
            {
                isShaking = false;
                transform.localPosition = originalPosition;
            }
        }
    }
}
