using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Asteroid))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Asteroid asteroid;

    private bool inputEnabled = true;

    private void Awake()
    {
        if (asteroid == null)
        {
            asteroid = GetComponent<Asteroid>();
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
