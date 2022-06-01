using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] internal GameObject _helpMenu;
    [SerializeField] internal GameObject _optionMenu;

    public void Play()
    {
        if (!isAnyMenuOpen())
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void HelpMenu()
    {
        if (!isAnyMenuOpen()) _helpMenu.SetActive(true);
    }

    public void OnBackHelpMenuPressed()
    {
        _helpMenu.SetActive(false);
    }

    public void OptionMenu()
    {
        if (!isAnyMenuOpen()) _optionMenu.SetActive(true);
    }


    public void OnBackOptionMenuPressed()
    {
        _optionMenu.SetActive(false);
    }

    public bool isAnyMenuOpen() => _optionMenu.activeSelf || _helpMenu.activeSelf;


    public void QuitButtonClicked()
    {
        if (!isAnyMenuOpen())
        {
            Application.Quit();
        }
    }
}