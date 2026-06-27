using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Language_Preference : MonoBehaviour
{
    private bool active = false;

    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        Debug.Log("Idioma salvo: " + ID);
        ChangeLocale(ID);
    }

    public void ChangeLocale(int localeID)
    {
        if (active == true) return;
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;

        var locales = LocalizationSettings.AvailableLocales.Locales;
        Debug.Log("Quantidade de idiomas: " + locales.Count);

        if (_localeID < locales.Count)
        {
            LocalizationSettings.SelectedLocale = locales[_localeID];
            Debug.Log("Idioma trocado para: " + locales[_localeID].Identifier.Code);
            PlayerPrefs.SetInt("LocaleKey", _localeID);
        }
        else
        {
            Debug.LogWarning("Locale ID inválido: " + _localeID);
        }

        active = false;
    }
}
