using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;                  // The damage inflicted by each bullet.
    public float timeBetweenBullets = 0.15f;        // The time between each shot.

    public GameObject bullet;
    public GameObject muzzle;
    public float bulletPower = 100000f;

    float timer;                                    // A timer to determine when to fire.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    AudioSource gunAudio;                           // Reference to the audio source.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");

        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunAudio = GetComponents<AudioSource>()[0];
    }

    void Update()
    {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (Input.GetButton("Fire1") && timer >= timeBetweenBullets)
        {
            // ... shoot the gun.
            Shoot();
        }
    }

    void Shoot()
    {
        // Reset the timer.
        timer = 0f;
        // Play the gun shot audioclip.
        gunAudio.Play();
        /*
                // Stop the particles from playing if they were, then start the particles.
                gunParticles.Stop();
                gunParticles.Play();
        */
        GameObject bulletInstance = GameObject.Instantiate(bullet, muzzle.transform.position, transform.rotation) as GameObject;
        bulletInstance.GetComponent<Rigidbody>().AddForce(transform.forward * bulletPower);
        Destroy(bulletInstance, 2f);
    }

}