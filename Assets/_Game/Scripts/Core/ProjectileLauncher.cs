using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour {
    [SerializeField] Transform launchPoint;
    [SerializeField] float launchForce = 15f;
    
    public void LaunchProjectile(Projectile projectile, Color color) {
        projectile.transform.SetPositionAndRotation(launchPoint.position, launchPoint.rotation);
        projectile.Initialize(color, projectile.GetComponent<ProjectilePool>()); // Proper initialization
        projectile.ApplyForce(launchPoint.forward * launchForce);
    }
}
