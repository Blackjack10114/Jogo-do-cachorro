using UnityEngine;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LocaleDebugger : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("Iniciando carregamento de idiomas...");

        yield return LocalizationSettings.InitializationOperation;

        Debug.Log("Idiomas carregados com sucesso.");

        var locales = LocalizationSettings.AvailableLocales.Locales;

        Debug.Log("Quantidade de idiomas disponíveis: " + locales.Count);

        foreach (var locale in locales)
        {
            Debug.Log("Idioma disponível: " + locale.Identifier.Code);
        }

        Debug.Log("Idioma atual: " + LocalizationSettings.SelectedLocale?.Identifier.Code);
    }
}