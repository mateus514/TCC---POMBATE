using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D crosshairTexture; // arrasta sua mira aqui no inspector
    public Vector2 hotspot = Vector2.zero; // ponto de clique (geralmente centro da imagem)
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        // Centraliza o hotspot no meio da imagem da mira
        Vector2 center = new Vector2(crosshairTexture.width / 2, crosshairTexture.height / 2);

        Cursor.SetCursor(crosshairTexture, center, cursorMode);
    }
}
