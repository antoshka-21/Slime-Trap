using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PixelPerfectTool
{
    public static Vector3 MakePixelPerfect(Vector3 currentPosition, float ppu)
    {
        Vector3 targetPosition = Vector3.zero;

        float pixelInUnits = 1f / ppu;

        targetPosition.x = Mathf.Round(currentPosition.x / pixelInUnits) * pixelInUnits;
        targetPosition.y = Mathf.Round(currentPosition.y / pixelInUnits) * pixelInUnits;

        return targetPosition;
    }
}
