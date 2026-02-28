using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;


    public GameManager gm;

    public TextMeshProUGUI healthText;

    void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString();
    }

    public void HealthD()
    {
        health--;
        healthText.text = health.ToString();
        if(health == 0)
        {
            gm.StopGame();
        }
    }

    

    
}
