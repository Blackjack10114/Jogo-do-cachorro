using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelecionadorDeFase : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] botoesFasesDesbloqueadas;
    public GameObject prefabFaseBloqueada;

    [Header("Cenas das Fases")]
    public string[] nomesCenas = {
        "Fase_TatuMafioso_01",
        "Fase_Alien_02",
        "Fase_Dino_03"
    };

    [Header("Pai dos Botões")]
    public Transform container;

    void Start()
    {
        GerarBotoesDeFase();
    }

    void GerarBotoesDeFase()
    {
        for (int i = 0; i < nomesCenas.Length; i++)
        {
            int numeroFase = i + 1;
            bool desbloqueada = GerenciadorDeJogo.Instance.FaseEstaDesbloqueada(numeroFase);
            Debug.Log($"Fase {numeroFase} desbloqueada? {desbloqueada}");

            GameObject botaoGO;

            if (desbloqueada && i < botoesFasesDesbloqueadas.Length)
            {
                botaoGO = Instantiate(botoesFasesDesbloqueadas[i], container);
                Debug.Log($"Instanciado botão da fase {numeroFase}");

                string cenaParaCarregar = nomesCenas[i];
                Button btn = botaoGO.GetComponent<Button>();
                if (btn != null)
                    btn.onClick.AddListener(() => SceneManager.LoadScene(cenaParaCarregar));
            }
            else
            {
                botaoGO = Instantiate(prefabFaseBloqueada, container);
                Debug.Log($"Instanciado botão BLOQUEADO da fase {numeroFase}");
            }
        }
    }
}
