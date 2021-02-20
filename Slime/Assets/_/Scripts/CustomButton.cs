using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CustomButton : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip down;
    [SerializeField] private AudioClip up;

    private BoxCollider2D _boxCollider;

    private void Awake()
    {
        var button = GetComponent<RectTransform>();

        _boxCollider = GetComponent<BoxCollider2D>();
        _boxCollider.isTrigger = true;
        _boxCollider.size = new Vector2(button.rect.width, button.rect.height);
    }


    private void OnMouseDown()
    {
        audioSource.PlayOneShot(down);
    }

    private void OnMouseUp()
    {
        audioSource.PlayOneShot(up);
    }
}
