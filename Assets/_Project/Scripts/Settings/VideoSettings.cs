using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettings : MonoBehaviour
{
    public TMP_Dropdown dropdownResolution; // Componente Dropdown com as opções de resolução
    public Toggle toggleFullScreen;     // Toggle para ativar/desativar tela cheia

    private Resolution[] availableResolutions; // Lista de resoluções disponíveis
    private int actualResolutionIndex = 0;     // Índice da resolução atual

    void Awake()
    {
        // Pega todas as resoluções que o monitor suporta
        availableResolutions = Screen.resolutions;

        // Carrega as preferências salvas do jogador
        actualResolutionIndex = PlayerPrefs.GetInt("indexResolution", GetCurrentResolutionIndex());
        bool isFullScreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;

        // Aplica a resolução e o modo de tela cheia salvos
        ApplyResolution(actualResolutionIndex, isFullScreen);
    }

    void Start()
    {
        // Limpa opções anteriores do Dropdown
        dropdownResolution.ClearOptions();
        var options = new System.Collections.Generic.List<string>();

        // Cria uma lista de strings com resoluções no formato "Largura x Altura"
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + " x " + availableResolutions[i].height;
            options.Add(option);
        }

        // Adiciona as opções no Dropdown
        dropdownResolution.AddOptions(options);

        // Seleciona a resolução atual como padrão no Dropdown
        dropdownResolution.value = actualResolutionIndex;
        dropdownResolution.RefreshShownValue();

        // Define o estado do toggle com base no modo atual da tela
        toggleFullScreen.isOn = Screen.fullScreen;

        // Conecta os eventos da interface com as funções
        dropdownResolution.onValueChanged.AddListener(ChangeResolution);
        toggleFullScreen.onValueChanged.AddListener(ChangeFullScreen);
    }

    // Chamada quando o jogador escolhe outra resolução
    public void ChangeResolution(int index)
    {
        actualResolutionIndex = index;
        ApplyResolution(index, Screen.fullScreen);
    }

    // Chamada quando o jogador ativa ou desativa o modo tela cheia
    public void ChangeFullScreen(bool isFullScreen)
    {
        ApplyResolution(actualResolutionIndex, isFullScreen);
    }

    // Aplica a resolução e o modo tela cheia escolhidos
    private void ApplyResolution(int index, bool isFullScreen)
    {
        Resolution res = availableResolutions[index];
        Screen.SetResolution(res.width, res.height, isFullScreen);
    }

    // Retorna o índice da resolução atual do sistema
    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }
        return 0; // Padrão caso não encontre
    }

    // Salva as escolhas do jogador quando o objeto for desativado
    void OnDisable()
    {
        PlayerPrefs.SetInt("indexResolution", actualResolutionIndex);
        PlayerPrefs.SetInt("fullscreen", Screen.fullScreen ? 1 : 0);
    }
}
