using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ThemeData : ScriptableObject
{
    public int scoreToUnlock;

    public Colors colors = new Colors();

    [System.Serializable]
    public struct Colors
    {
        public Color color1;
        public Color color2;
        public Color color3;
        [Range(0f, 1f)]
        public float constantNoiseAlpha;
    }
}
