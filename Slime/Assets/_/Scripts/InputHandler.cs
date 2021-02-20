using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameStateController stateController;



    public event System.Action PushDown;
    public event System.Action PushUp;



    private void OnMouseDown()
    {
        if (stateController.CurrentState == GameState.NotStarted ||
            stateController.CurrentState == GameState.Started)
        {
            PushDown?.Invoke();
        }
    }



    private void OnMouseUp()
    {
        if (stateController.CurrentState == GameState.NotStarted ||
            stateController.CurrentState == GameState.Started)
        {
            PushUp?.Invoke();
        }
    }



#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Debug.Break();
    }

#endif
}
