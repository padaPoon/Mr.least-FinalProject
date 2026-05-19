using UnityEngine;
using UnityEngine.SceneManagement;
    
public class MainMenu_ui : MonoBehaviour
{
    PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SinglePlayer()
    {
        SceneManager.LoadScene("Bank_Build_Singleplayer");
        player.gameOver = false;
    }

    public void MultiPlayer()
    {
        SceneManager.LoadScene("Bank_Build_Multiplayer");
        player.gameOver = false;
    }

 
}
