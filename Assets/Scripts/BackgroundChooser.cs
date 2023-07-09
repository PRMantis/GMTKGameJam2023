using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChooser : MonoBehaviour
{
    public List<Sprite> ListOfAllBackgrounds;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ChooseSprite();
    }


    private void ChooseSprite()
    {
        var randomIndex = Random.Range(0, ListOfAllBackgrounds.Count - 1);

        spriteRenderer.sprite = ListOfAllBackgrounds[randomIndex];
    }
}
