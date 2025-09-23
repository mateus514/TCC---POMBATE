using UnityEngine;

public class AimManager : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if (mainCam == null) return;

        // Pega posição do mouse na tela e converte para mundo
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // Faz a mira seguir o mouse
        transform.position = mouseWorldPos;
    }
}