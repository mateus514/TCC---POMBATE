using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [Header("Painel de Opções")]
    public GameObject painelOpcoes;     // Painel com sliders e botão Voltar

    [Header("Componentes de Áudio")]
    public AudioMixer mixer;            // Mixer com parâmetros expostos: VolumeMusica e VolumeSons
    public Slider sliderMusica;         // Slider para o volume da música
    public Slider sliderSons;           // Slider para o volume dos sons

    void Start()
    {
        // Garante que o painel de opções comece escondido
        painelOpcoes.SetActive(false);

        // Carrega valores salvos (0.0001 até 1)
        float volMusica = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        float volSons = PlayerPrefs.GetFloat("VolumeSons", 1f);

        // Atualiza os sliders
        sliderMusica.value = volMusica;
        sliderSons.value = volSons;

        // Aplica o volume no Mixer
        AlterarVolumeMusica(volMusica);
        AlterarVolumeSons(volSons);

        // Eventos dos sliders
        sliderMusica.onValueChanged.AddListener(AlterarVolumeMusica);
        sliderSons.onValueChanged.AddListener(AlterarVolumeSons);
    }

    // ===============================
    // 🔘 BOTÕES DO MENU PRINCIPAL
    // ===============================
    public void JogarTutorial()
    {
        SceneManager.LoadScene("Fase1");
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Sair do jogo chamado!");
    }

    // ===============================
    // ⚙ PAINEL DE OPÇÕES
    // ===============================
    public void AbrirOpcoes()
    {
        painelOpcoes.SetActive(true);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
    }

    // ===============================
    // 🎵 CONTROLE DE VOLUME (com conversão para dB)
    // ===============================
    public void AlterarVolumeMusica(float valor)
    {
        float volumeDB = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1)) * 20;
        mixer.SetFloat("VolumeMusica", volumeDB);
        PlayerPrefs.SetFloat("VolumeMusica", valor);
    }

    public void AlterarVolumeSons(float valor)
    {
        float volumeDB = Mathf.Log10(Mathf.Clamp(valor, 0.0001f, 1)) * 20;
        mixer.SetFloat("VolumeSom", volumeDB);
        PlayerPrefs.SetFloat("VolumeSom", valor);
    }
}
