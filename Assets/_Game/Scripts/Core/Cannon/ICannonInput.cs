using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICannonInput {
    Vector2 GetAimDirection(Vector2 screenTouchPosition);
}
