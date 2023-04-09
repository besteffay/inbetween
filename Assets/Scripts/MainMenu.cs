using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    Player player;

    public AudioSource clickSound;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void PlayGame()
    {
        clickSound.Play();
        StartCoroutine(player.ChangeScene());
    }

    public void QuitGame()
    {
        clickSound.Play();
        Application.Quit();
    }

}
