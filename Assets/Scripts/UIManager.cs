using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text usernameText;
    public GameObject usernamePanel; 
	
	public void StartGame() {
        //TODO SAVE USERNAME
        PlayerPrefs.SetString("username", usernameText.text);
		Application.LoadLevel(1);
	}

	public void QuitGame() {
		Application.Quit();
	}

    public void ShowUsernamePanel()
    {
        usernamePanel.SetActive(true);
    }
}
