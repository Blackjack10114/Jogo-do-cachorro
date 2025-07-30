using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Language_Preference : MonoBehaviour
{
    private void Start()
    {
        int ID = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(ID);
    }
    private bool active = false;
    public void ChangeLocale(int localeID)
    {
        if (active == true) return;
        StartCoroutine(SetLocale(localeID));
    }
    IEnumerator SetLocale(int _localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        PlayerPrefs.SetInt("LocaleKey", _localeID);
        active = false;
    }
}
