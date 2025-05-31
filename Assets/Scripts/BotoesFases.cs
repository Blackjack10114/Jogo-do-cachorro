using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;   // ⬅️  ADICIONE ESTA LINHA

public class BotaoFase : MonoBehaviour
{
    [Header("Dados da fase")]
    public int numeroFase;        // 1, 2, 3…
    public string nomeCena;        // "Fase_TatuMafioso_01" etc.

    [Header("Sprites")]
    public Sprite spriteLiberado;   // botão normal
    public Sprite spriteBloqueado;  // botão com cadeado

    private void Start()
    {
        bool liberado = GerenciadorDeJogo.Instance.FaseEstaDesbloqueada(numeroFase);

        Button btn = GetComponent<Button>();
        Image img = GetComponent<Image>();
        TextMeshProUGUI t = GetComponentInChildren<TextMeshProUGUI>();

        if (img) img.sprite = liberado ? spriteLiberado : spriteBloqueado;
        if (t) t.text = liberado ? numeroFase.ToString() : "";   

        btn.interactable = liberado;

        if (liberado)
            btn.onClick.AddListener(() => SceneManager.LoadScene(nomeCena));
    }
}
