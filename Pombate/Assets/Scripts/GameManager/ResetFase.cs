using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ResetFase : MonoBehaviour
{
    [Header("Referências do Jogo")]
    public Player player;
    public BulletController bulletCtrl;
    public Tilemap tilemap;

    [Header("Scripts de Estado")]
    public GameOverManager gameOverScript;
    public PauseManager pauseManagerScript;
    public DialogueManager dialogueScript;

    [Header("Configurações")]
    public KeyCode resetKey = KeyCode.R;

    private Vector3 playerStartPos;
    private Quaternion playerStartRot;
    private Dictionary<Vector3Int, TileBase> initialTiles;

    void Start()
    {
        // Salva posição inicial do player
        playerStartPos = player.transform.position;
        playerStartRot = player.transform.rotation;

        // Salva os tiles originais do tilemap
        initialTiles = new Dictionary<Vector3Int, TileBase>();
        BoundsInt bounds = tilemap.cellBounds;
        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(pos);
            if (tile != null)
                initialTiles[pos] = tile;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(resetKey) && PodeResetar())
        {
            ResetarCena();
        }
    }

    private bool PodeResetar()
    {
        bool gameOverAtivo = gameOverScript != null && gameOverScript.isGameOver;
        bool pauseAtivo = pauseManagerScript != null && pauseManagerScript.IsPaused();
        bool dialogueAtivo = dialogueScript != null && dialogueScript.isDialogueActive;

        return !gameOverAtivo && !pauseAtivo && !dialogueAtivo;
    }

    private void ResetarCena()
    {
        // Reset player
        player.transform.position = playerStartPos;
        player.transform.rotation = playerStartRot;
        player.ResetarEstado();
        

        // Reset balas
        bulletCtrl.ResetBalas();

        // Reset tilemap
        tilemap.ClearAllTiles();
        foreach (var kvp in initialTiles)
        {
            tilemap.SetTile(kvp.Key, kvp.Value);
        }
    }
}
