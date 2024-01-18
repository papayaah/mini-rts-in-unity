using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
	public Sprite[] sprites; // Array to hold your sprites
	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SwitchSprite(int index)
	{
		if (index >= 0 && index < sprites.Length)
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprites[index];
		}
	}

    // Update is called once per frame
    void Update()
    {
	
    }
}
