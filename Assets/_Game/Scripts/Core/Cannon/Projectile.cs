using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public Color Color { get; private set; }
    private ProjectilePool originPool;
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");

    
    // Initialize both color and pool
    public void Initialize(Color color, ProjectilePool pool) {
        this.Color = color;
        originPool = pool;
        GetComponent<Renderer>().material.SetColor(Color1,color);
    }

    public void ApplyForce(Vector3 force) {
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    public void Recycle()
    {
        gameObject.SetActive(false);
    }
    
    void OnDisable() {
        originPool?.ReturnProjectile(this);
    }
}