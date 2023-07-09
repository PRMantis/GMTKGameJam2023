using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Asteroid))]
[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;
    [SerializeField] private Player player;

    private bool inputEnabled = true;

    private void Awake()
    {
        if (asteroid == null)
        {
            asteroid = GetComponent<Asteroid>();
        }
        if (player == null)
        {
            player = GetComponent<Player>();
        }
    }

    void Update()
    {
        if (inputEnabled)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 direction = new Vector2(h, v);
            direction.Normalize();
            asteroid.ApplyForce(direction);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.TryBoost(direction);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.TryPauseGame();
            }
        }
    }

    public void SetInputState(bool isEnabled)
    {
        inputEnabled = isEnabled;
    }

    public bool IsInputEnabled()
    {
        return inputEnabled;
    }
}
