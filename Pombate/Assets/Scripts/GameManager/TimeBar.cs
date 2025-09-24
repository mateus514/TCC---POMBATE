using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    public Image timerFillImage;
    public float totalTime = 5f;

    private float timeLeft;
    private bool isRunning = true;

    void Start()
    {
        timeLeft = totalTime;
    }

    void Update()
    {
        // Não roda se o timer estiver parado ou o diálogo estiver ativo
        if (!isRunning || (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive))
            return;

        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            float fillAmount = timeLeft / totalTime;
            timerFillImage.fillAmount = fillAmount;
        }
        else
        {
            timerFillImage.fillAmount = 0f;
            // ação quando o tempo acaba
        }
    }

    public void ResetTimer()
    {
        timeLeft = totalTime;
        timerFillImage.fillAmount = 1f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
