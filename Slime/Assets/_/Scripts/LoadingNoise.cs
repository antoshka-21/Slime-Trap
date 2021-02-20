using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingNoise : MonoBehaviour
{
    [SerializeField] private Color targetTextureColor;
    [SerializeField] private float ppu;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private int numberOfSprites;



    private SpriteRenderer _spriteRenderer;

    private Sprite[] noiseSprites;
    private int currentTextureID = 0;

    private int pixWidth;
    private int pixHeight;

    private float pixelChance = 1f;
    private float step;



    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        pixWidth = (int)(width * ppu);
        pixHeight = (int)(height * ppu);

        step = 1f / (numberOfSprites - 1);

        noiseSprites = new Sprite[numberOfSprites];

        for (int i = 0; i < numberOfSprites; i++)
        {
            noiseSprites[i] = CreateSprite();
        }
    }



    private void Start()
    {
        Play(true);
    }



    public void Play(bool fromStart)
    {
        if (fromStart)
        {
            StartCoroutine(PlayFromStart());
        }

        else
        {
            StartCoroutine(PlayFromEnd());
        }
    }



    private Sprite CreateSprite()
    {
        Texture2D noiseTexture = new Texture2D(pixWidth, pixHeight);
        noiseTexture.filterMode = FilterMode.Point;

        Color[] pixels = new Color[pixWidth * pixHeight];

        for (int y = 0; y < pixHeight; y++)
        {
            for (int x = 0; x < pixWidth; x++)
            {
                float alpha = Random.value < pixelChance ? 1f : 0f;
                pixels[y * pixWidth + x] = targetTextureColor.SetAlpha(alpha);
            }
        }

        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();

        Sprite sprite = Sprite.Create(noiseTexture, new Rect(0f, 0f, pixWidth, pixHeight), new Vector2(0.5f, 0.5f), ppu);

        pixelChance -= step;

        return sprite;
    }



    private IEnumerator PlayFromStart()
    {
        for (int i = 0; i < noiseSprites.Length; i++)
        {
            _spriteRenderer.sprite = noiseSprites[i];
            yield return null;
        }
    }



    private IEnumerator PlayFromEnd()
    {
        for (int i = noiseSprites.Length - 1; i >= 0; i--)
        {
            _spriteRenderer.sprite = noiseSprites[i];
            yield return null;
        }
    }
}
