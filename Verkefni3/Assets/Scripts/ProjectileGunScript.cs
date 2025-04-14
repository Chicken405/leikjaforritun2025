using Unity.VisualScripting;
using UnityEngine;

/// -------- >>>>>>  ÞETTA ER EKKI MINN KOÐI <<<<<--------


/// Thanks for downloading my projectile gun script! :D
/// Feel free to use it in any project you like!
/// 
/// The code is fully commented but if you still have any questions
/// don't hesitate to write a yt comment
/// or use the #coding-problems channel of my discord server
/// 
/// Dave
public class ProjectileGunTutorial : MonoBehaviour
{
    //bullet 
    public GameObject bullet;

    //bullet force
    public float shootForce, upwardForce;

    //Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;

    //Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    //bools
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;

    //Graphics
    public GameObject muzzleFlash;
    private AmmoUIScript ammoUIScript;

    //bug fixing :D
    public bool allowInvoke = true;
    private AudioSource sound;

    void Start()
    {
        // fær components
        sound = GetComponent<AudioSource>();
        ammoUIScript = AmmoUIScript.Instance;
        ammoUIScript.SetPoints(magazineSize);
    }

    private void Awake()
    {
        //make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        //Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        //Reloading 
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();
        //Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        //Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            //Set bullets shot to 0
            bulletsShot = 0;

            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        sound.Play();

        //Find the exact hit position using a raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //Just a ray through the middle of your current view
        Vector3 targetPoint;
        targetPoint = ray.GetPoint(75); 

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        directionWithoutSpread.Normalize(); // Normalize the direction to avoid scaling the vector

        // Calculate random spread angles
        float spreadAngleX = Random.Range(-spread, spread);
        float spreadAngleY = Random.Range(-spread, spread);
        float spreadAngleZ = Random.Range(-spread, spread);

        // Create a random rotation based on the spread angles
        Quaternion spreadRotation = Quaternion.Euler(spreadAngleX, spreadAngleY, 0); // X and Y spread, Z is 0 for no tilt

        // Apply the spread rotation to the direction
        Vector3 directionWithSpread = spreadRotation * directionWithoutSpread; // Rotate the direction vector

        // Instantiate bullet/projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity); // Store instantiated bullet in currentBullet

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);


        ammoUIScript.DeductPoints(1);
        bulletsLeft--;
        bulletsShot++;

        //Invoke resetShot function (if not already invoked), with your timeBetweenShooting
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;

            //Add recoil to player (should only be called once)
            playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        //if more than one bulletsPerTap make sure to repeat shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        // Allow shooting and invoking again
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        reloading = true;
        ammoUIScript.SetString("Reloading");
        Invoke("ReloadFinished", reloadTime); // Invoke ReloadFinished function with your reloadTime as delay
    }
    private void ReloadFinished()
    {
        // Fill magazine
        bulletsLeft = magazineSize;
        ammoUIScript.SetPoints(magazineSize);
        reloading = false;
    }
}
