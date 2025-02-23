using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour, IProjectileFactory {
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] int initialPoolSize = 10;
    
    Queue<Projectile> availableProjectiles = new Queue<Projectile>();
    
    void Awake() => WarmPool();
    
    void WarmPool() {
        for (int i = 0; i < initialPoolSize; i++) {
            ReturnProjectile(CreateNewProjectile());
        }
    }
    
    public Projectile GetProjectile() {
        Projectile p = availableProjectiles.Count > 0 ? 
            availableProjectiles.Dequeue() : 
            CreateNewProjectile();
        
        p.gameObject.SetActive(true);
        return p;
    }
    
    public void ReturnProjectile(Projectile projectile) {
        projectile.gameObject.SetActive(false);
        availableProjectiles.Enqueue(projectile);
    }
    
    
    Projectile CreateNewProjectile() {
        Projectile p = Instantiate(projectilePrefab);
        p.gameObject.SetActive(false);
        p.Initialize(new Color(194,57,57,255), this);
        return p;
    }
}