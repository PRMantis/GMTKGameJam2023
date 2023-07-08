using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Image boosBarImage;
    [SerializeField] private Image healthBarImage;

    private Health playerHealth;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        playerHealth = player.GetComponent<Health>();

        player.OnBoostChange += OnBoostChange;
        playerHealth.OnTakeDamage += OnPlayerTakeDamage;
    }

    private void OnDestroy()
    {
        player.OnBoostChange -= OnBoostChange;
        playerHealth.OnTakeDamage -= OnPlayerTakeDamage;
    }

    private void OnBoostChange(float value)//from 0 to 1
    {
        boosBarImage.fillAmount = value;
    }

    private void OnPlayerTakeDamage(int damage)
    {
        healthBarImage.fillAmount = (float)playerHealth.GetHealth() / (float)playerHealth.GetMaxHealth();
    }
}
