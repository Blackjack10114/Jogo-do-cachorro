using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Painéis")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;

    private System.Action acaoConfirmada;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!painelPause.activeSelf)
                AbrirPause();
            else
                FecharPause();
        }
    }

    public void AbrirPause()
    {
        Time.timeScale = 0f;
        painelFundoCinza.SetActive(true);
        painelPause.SetActive(true);
        painelConfirmacao.SetActive(false);
    }

    public void FecharPause()
    {
        Time.timeScale = 1f;
        painelFundoCinza.SetActive(false);
        painelPause.SetActive(false);
        painelConfirmacao.SetActive(false);
    }

    public void BotaoContinuar()
    {
        FecharPause();
    }
    
    public void BotaoMenuPrincipal()
    {
        MostrarConfirmacao(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuPrincipal");
        });
    }

    public void BotaoReiniciar()
    {
        MostrarConfirmacao(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    private void MostrarConfirmacao(System.Action acao)
    {
        painelPause.SetActive(false);
        painelConfirmacao.SetActive(true);
        acaoConfirmada = acao;
    }

    public void BotaoConfirmarSim()
    {
        acaoConfirmada?.Invoke();
        acaoConfirmada = null;
    }

    public void BotaoConfirmarNao()
    {
        painelConfirmacao.SetActive(false);
        painelPause.SetActive(true);
    }
}