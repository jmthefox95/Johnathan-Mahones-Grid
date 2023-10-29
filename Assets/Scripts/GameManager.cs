using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToTitleButton()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void GiveUpButton()
    {
        SceneManager.LoadScene("GameOverScreen");
    }
}
