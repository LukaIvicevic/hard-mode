using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private TextMeshProUGUI ammoDisplay;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip audioClip;

    [Header("Bullet Stats")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootForce;

    [Header("Gun States")]
    [SerializeField]
    private float spread, reloadTime, fireRate;
    [SerializeField]
    private int magazineSize, bulletsPerTap;
    [SerializeField]
    private bool allowButtonHold;

    private int bulletsLeft, bulletsShot;
    private bool isShooting, isReadyToShoot, isReloading;

    [Header("Debug")]
    private bool allowInvoke = true;

    private Animator weaponAnimator;

    private void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        isReadyToShoot = true;

        // Get weapon animator
        weaponAnimator = GetComponentInChildren<Animator>();

        // Set reload animation time
        // This assumes 1 second long default Reload animation
        weaponAnimator.SetFloat("reloadTime", 1f/reloadTime);
    }

    void Update()
    {
        HandleInput();

        // Setup GUI
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    private void HandleInput()
    {
        // Check if automatic or single fire
        if (allowButtonHold)
        {
            isShooting = Input.GetMouseButton(0);
        }
        else
        {
            isShooting = Input.GetMouseButtonDown(0);
        }

        // Update shooting animation
        if (weaponAnimator != null)
        {
            weaponAnimator.SetBool("isFiring", isShooting && !isReloading);
        }


        // Check if we should reload
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize)
        {
            Reload();
        }

        if (isReadyToShoot && isShooting && !isReloading && bulletsLeft <= 0)
        {
            Reload();
        }

        if (isReadyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        isReadyToShoot = false;

        // Find where to shoot
        var ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out var hit))
        {
            targetPoint = hit.point;
        } else
        {
            targetPoint = ray.GetPoint(100);
        }

        // Get direction from bulletSpawner to targetPoint
        var directionWithoutSpread = targetPoint - bulletSpawnPoint.position;

        // Calculate random spread
        var x = Random.Range(-spread, spread);
        var y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        var directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet
        var currentBullet = Instantiate(bullet, bulletSpawnPoint.position, Quaternion.identity);
        currentBullet.transform.forward = directionWithSpread.normalized;
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);

        // Debug
        Logger.Instance.DrawRay(bulletSpawnPoint.position, currentBullet.transform.forward * 20, Color.white, 2);

        // Play muzzle flash
        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, bulletSpawnPoint.position, Quaternion.identity);
        }

        // Play gun shot sound
        if (audioSource != null)
        {
            SoundManager.Instance.PlayOneShot(audioSource, audioClip);
        }

        bulletsLeft--;
        bulletsShot++;

        // Invoke ResetShot if not already invoked
        if (allowInvoke)
        {
            Invoke("ResetShot", fireRate);
        }

        // If more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", fireRate);
        }
    }

    private void ResetShot()
    {
        isReadyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        isReloading = true;
        weaponAnimator.SetBool("isReloading", true);
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
        weaponAnimator.SetBool("isReloading", false);
    }
}
