using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector2 _touchStart;
    private BallController _ballController;

    private void Start()
    {
        _ballController = GetComponent<BallController>();
    }

    void Update()
    {
        // Keyboard
        if(Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2Int.left);
        if(Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2Int.right);
        if(Input.GetKeyDown(KeyCode.UpArrow)) TryMove(Vector2Int.up);
        if(Input.GetKeyDown(KeyCode.DownArrow)) TryMove(Vector2Int.down);
        if(Input.GetKeyDown(KeyCode.Backspace)) _ballController.UndoLastMove();

        // Touch
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if(touch.phase == TouchPhase.Began)
                _touchStart = touch.position;
            
            if(touch.phase == TouchPhase.Ended)
            {
                Vector2 delta = touch.position - _touchStart;
                Vector2Int direction = GetSwipeDirection(delta);
                TryMove(direction);
            }
        }
    }

    private Vector2Int GetSwipeDirection(Vector2 delta)
    {
        if(delta.magnitude < 20) return Vector2Int.zero;
        
        float angle = Vector2.SignedAngle(Vector2.up, delta.normalized);
        
        if(angle > -45 && angle <= 45) return Vector2Int.up;
        if(angle > 45 && angle <= 135) return Vector2Int.left;
        if(angle > 135 || angle <= -135) return Vector2Int.down;
        return Vector2Int.right;
    }

    private void TryMove(Vector2Int direction)
    {
        Debug.Log($"Trying to move in direction {direction}");
        if(direction != Vector2Int.zero)
            _ballController.TryMove(direction);
    }
}