using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeBar : MonoBehaviour
{
    public Image timerFillImage;
    public float totalTime = 5f;

    private float timeLeft;
    private bool isRunning = true;

    // Evento para notificar quando o tempo acabar
    public event Action OnTimerEnd;

    void Start()
    {
        timeLeft = totalTime;
    }

    void Update()
    {
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
            if (isRunning)
            {
                isRunning = false;
                OnTimerEnd?.Invoke(); // dispara evento
            }
        }
    }

    public void ResetTimer()
    {
        Debug.Log("ResetTimer chamado");
        timeLeft = totalTime;
        timerFillImage.fillAmount = 1f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}