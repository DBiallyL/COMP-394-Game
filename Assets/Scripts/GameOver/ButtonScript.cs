using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Text text;
    Image sprite;
    public Sprite inactiveSprite;
    public Sprite activeSprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
    * Changes to the inactive button sprite
    */
    public void InactivateButton() {
        sprite.sprite = inactiveSprite;
    }

    /**
    * Changes to the active button sprite
    */
    public void ActivateButton() {
        sprite.sprite = activeSprite;
    }
}
