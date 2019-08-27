using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float xBoundaryPadding = 10f;
    [SerializeField] float yBoundaryPadding = 10f;
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileSpriteCorrectionRotation;
    [SerializeField] Vector3 projectileOffset;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    [Header("Effects")]
    [SerializeField] float explosionDuration;
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float volumeSFX; // TODO make this retrieve value from general SFX volume setting

    Coroutine firingCoroutine;

    float xMin;
    float xMax;
    float yMin;
    float yMax;

    Level currentLevel;
    
    void Start()
    {
        SetUpMoveBoundaries();
        currentLevel = FindObjectOfType<Level>();
    }

    void Update()
    {
        Move();
        Fire();
    }

    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
            
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessDamage(damageDealer);
    }

    private void ProcessDamage(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, volumeSFX);
        Destroy(explosion, explosionDuration);
        currentLevel.LoadGameOver();
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            // TODO make cooldown independent from the holding down button part
            // so that slow auto-fire speed cannot be loopholed through clicking fast instead

            GameObject laser = Instantiate(
                    laserPrefab,
                    transform.position + projectileOffset,
                    Quaternion.Euler(new Vector3(Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z + projectileSpriteCorrectionRotation))) as GameObject; // Fixing the rotation of the sprite
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, volumeSFX);
            yield return new WaitForSeconds(projectileFiringPeriod);

        }
    }

    private void Move()
    {
        // TODO tweak movement to feel better
        // More floaty? More sharp? Faster? Slower?
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var rotation = new Quaternion(0f, Input.GetAxis("Horizontal")*0.25f, 0f, 1f);
        transform.rotation = rotation;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);        
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
       
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xBoundaryPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xBoundaryPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yBoundaryPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y - yBoundaryPadding; // should this be limited further? Instead of whole screen, bottom half? two thirds?
    }
}
