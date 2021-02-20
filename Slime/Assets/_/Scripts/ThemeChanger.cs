using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeChanger : MonoBehaviour
{
    [SerializeField] private ThemeData[] themes;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer[] spriteRenderersWithColor1;
    [SerializeField] private SpriteRenderer[] spriteRenderersWithColor2;
    [SerializeField] private SpriteRenderer[] spriteRenderersWithColor3;
    [SerializeField] private SpriteRenderer constantNoise;

    [Header("Particles")]
    [SerializeField] private ParticleSystem[] particlesWithColor1;
    [SerializeField] private ParticleSystem[] particlesWithColor2;

    [Header("Trails")]
    [SerializeField] private TrailRenderer slimeTrail;

    [Header("Texts")]
    [SerializeField] private Text[] textsWithColor1;
    [SerializeField] private Text[] textsWithColor2;

    [Header("Images")]
    [SerializeField] private Image[] imagesWithColor1;
    [SerializeField] private Image[] imagesWithColor2;
    [SerializeField] private Image[] imagesWithColor3;



    public ThemeData currentTheme { get; private set; }



    private ParticleSystem.MainModule[] partMainsWithColor1;
    private ParticleSystem.MainModule[] partMainsWithColor2;

    private int currentThemeID;



    private void Awake()
    {
        partMainsWithColor1 = new ParticleSystem.MainModule[particlesWithColor1.Length];
        partMainsWithColor2 = new ParticleSystem.MainModule[particlesWithColor2.Length];

        for (int i = 0; i < particlesWithColor1.Length; i++)
        {
            partMainsWithColor1[i] = particlesWithColor1[i].main;
        }

        for (int i = 0; i < particlesWithColor2.Length; i++)
        {
            partMainsWithColor2[i] = particlesWithColor2[i].main;
        }

        currentThemeID = SaveLoad.LoadInt("LastThemeID", 0);
        Change();
    }



    public void SetNextID(int direction)
    {
        currentThemeID += direction;

        if (currentThemeID >= themes.Length)
        {
            currentThemeID = 0;
        }

        SaveLoad.SaveInt("LastThemeID", currentThemeID);
    }



    public void SetID(int id)
    {
        SaveLoad.SaveInt("LastThemeID", id);
    }



    private void Change()
    {
        currentTheme = themes[currentThemeID];

        constantNoise.color = constantNoise.color.SetAlpha(currentTheme.colors.constantNoiseAlpha);
        slimeTrail.startColor = currentTheme.colors.color1;
        slimeTrail.endColor = currentTheme.colors.color1;

        foreach (var sr1 in spriteRenderersWithColor1)
        {
            sr1.color = currentTheme.colors.color1;
        }

        foreach (var sr1 in spriteRenderersWithColor1)
        {
            sr1.color = currentTheme.colors.color1;
        }

        foreach (var sr2 in spriteRenderersWithColor2)
        {
            sr2.color = currentTheme.colors.color2;
        }

        foreach (var sr3 in spriteRenderersWithColor3)
        {
            sr3.color = currentTheme.colors.color3;
        }

        for (int i = 0; i < partMainsWithColor1.Length; i++)
        {
            partMainsWithColor1[i].startColor = currentTheme.colors.color1;
        }

        for (int i = 0; i < partMainsWithColor2.Length; i++)
        {
            partMainsWithColor2[i].startColor = currentTheme.colors.color2;
        }

        foreach (var text in textsWithColor1)
        {
            text.color = currentTheme.colors.color1;
        }

        foreach (var text in textsWithColor2)
        {
            text.color = currentTheme.colors.color2;
        }

        foreach (var image in imagesWithColor1)
        {
            image.color = currentTheme.colors.color1;
        }

        foreach (var image in imagesWithColor2)
        {
            image.color = currentTheme.colors.color2;
        }
        
        foreach (var image in imagesWithColor3)
        {
            image.color = currentTheme.colors.color3;
        }
    }
}
