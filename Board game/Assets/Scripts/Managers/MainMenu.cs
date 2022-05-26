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
        SceneManager.LoadScene("SampleScene");
    }
    
    public void HelpMenu()
    {
        _helpMenu.SetActive(true);
    }
    public void OnBackHelpMenuPressed()
    {
        _helpMenu.SetActive(false);
    }
    
    public void OptionMenu()
    {
        _optionMenu.SetActive(true);
    }
    public void OnBackOptionMenuPressed()
    {
        _optionMenu.SetActive(false);
    }
    
    public void QuitButtonClicked()
    {
        Application.Quit();
    }
}
