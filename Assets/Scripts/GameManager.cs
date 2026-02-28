using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SpikeSpawner ss1;
    public SpikeSpawner ss2;

    public GameObject StartText;
    public GameObject EndText;

    

    public bool GameRunning = false;
    public bool GameRestart = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f;
        ss1.spawn = false;
        ss2.spawn = false;
        //StartGame();

    }

    

    public void StartGame()
    {
        Time.timeScale = 1f;
        ss1.spawn = true;
        ss2.spawn = true;
        ss1.StartG();
        ss2.StartG();
        StartText.SetActive(false);
    }

    public void StopGame()
    {
        Time.timeScale = 0f;
        ss1.spawn = false;
        ss2.spawn = false;
        GameRestart = true;
        EndText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameRunning == false && Input.GetKeyUp(KeyCode.Tab))
        {
            StartGame();
            GameRunning = true;
        }
        if(GameRestart == true && Input.GetKeyUp(KeyCode.Tab))
        {
            StartText.SetActive(true);
            EndText.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
