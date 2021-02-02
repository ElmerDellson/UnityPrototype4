using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;
    public float moveForce = 5.0f;
    public float powerupStrength = 10f;
    public float powerupDuration = 5;
    public bool hasPowerup = false;
    public GameObject powerupIndicator;

    private Rigidbody playerRb;
    private MeshRenderer playerMR;
    private GameObject focalPoint;
    private float powerupIndicatorMaxSize = 4.3f;
    private float powerupIndicatorMinSize = 2.3f;
    private float powerupIndicatorAnimationSpeed;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerMR = GetComponent<MeshRenderer>();
        focalPoint = GameObject.Find("FocalPoint");
        powerupIndicatorAnimationSpeed = (powerupIndicatorMaxSize - powerupIndicatorMinSize) / powerupDuration;
    }

    void Update()
    {
        if (gameManager.gameOver)
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition;
            playerMR.enabled = false;
        }
            

        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * Time.deltaTime * moveForce);

        AnimatePowerupIndicator();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine("PowerupCountDownRoutine");
            powerupIndicator.gameObject.SetActive(true);
        }
    }

    IEnumerator PowerupCountDownRoutine()
    {
        yield return new WaitForSeconds(powerupDuration);
        hasPowerup = false;
        powerupIndicator.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position).normalized;

            enemyRb.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
        }
    }

    void AnimatePowerupIndicator()
    {
        powerupIndicator.transform.position = transform.position - new Vector3(0, 0.5f, 0);

        if (hasPowerup)
        {
            Vector3 size = powerupIndicator.transform.localScale;
            powerupIndicator.transform.localScale = size - new Vector3(powerupIndicatorAnimationSpeed, 0, powerupIndicatorAnimationSpeed) * Time.deltaTime;
        } 
        else
        {
            powerupIndicator.transform.localScale = new Vector3(powerupIndicatorMaxSize, 2, powerupIndicatorMaxSize);
        }
    }
}
