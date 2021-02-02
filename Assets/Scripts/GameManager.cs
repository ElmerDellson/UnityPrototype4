using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public bool gameWasOver = false;

    private GameObject player;
    
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine("CheckGameOverRoutine");

        if (Input.GetKey(KeyCode.Space))
        {
            RestartGame();
        }
    }

    IEnumerator CheckGameOverRoutine()
    {
        yield return new WaitForSeconds(1f);

        if (player.transform.position.y < -30)
            gameOver = true;
    }

    private void RestartGame()
    {
        gameOver = false;

        ResetPlayer();

        DestroyOthers();

        //Reset waveNumber
        GameObject.Find("SpawnManager").GetComponent<SpawnManager>().waveNumber = 1;
    }

    void ResetPlayer()
    {
        //Reset position and rotation
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;

        //Reset Rigidbody
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        playerRb.constraints = RigidbodyConstraints.None;
        playerRb.angularVelocity = Vector3.zero;
        playerRb.velocity = Vector3.zero;

        //Reset visibility
        player.GetComponent<MeshRenderer>().enabled = true;

        //Reset powerup
        if (player.GetComponent<PlayerController>().hasPowerup)
            GameObject.Find("PowerupIndicator").gameObject.SetActive(false);
        
        player.GetComponent<PlayerController>().hasPowerup = false;
        
    }

    void DestroyOthers()
    {
        //Destroy all powerups
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        for (int i = 0; i < powerups.Length; i++)
        {
            Destroy(powerups[i]);
        }

        //Destroy all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
    }
}
