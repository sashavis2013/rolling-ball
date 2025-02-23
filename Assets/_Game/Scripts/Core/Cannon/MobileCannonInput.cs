using UnityEngine;

public class MobileCannonInput : MonoBehaviour, ICannonInput {
    [SerializeField] float maxDragDistance = 100f;
    //
    // public Vector2 GetAimDirection(Vector2 screenTouchPosition) {
    //     if (Input.touchCount == 0) return Vector2.zero;
    //
    //     Touch touch = Input.GetTouch(0);
    //     Vector2 dragDelta = touch.deltaPosition.normalized * 
    //                         Mathf.Clamp(touch.deltaPosition.magnitude / maxDragDistance, -1f, 1f);
    //     return dragDelta; // Remove Mathf.Abs to allow negative Y values
    // }

    
    public Vector2 GetAimDirection(Vector2 screenTouchPosition) {
        if (Input.touchCount == 0) return Vector2.zero;
    
        Touch touch = Input.GetTouch(0);
        Vector2 delta = touch.deltaPosition / Screen.dpi * 0.5f;
        return new Vector2(delta.x, delta.y); // Invert Y for natural drag
    }
}