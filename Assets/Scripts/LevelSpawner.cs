using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public SectionPool pool;
    public GameObject player;
    public Vector3 previousPosition;

    public Vector3 offSet = new Vector3(0, 0, 30);

    GameObject trail;

    private void Start()
    {
        gameManager.OnLoadLevel.AddListener(SpawnLevel);

        GameManager.Instance.OnGameQuit.AddListener(ClearTrail);
    }

    private void SpawnLevel()
    {
        player.SetActive(true);

        //create pool
        pool.ClearPool(); //clear first in case this is a restart...
        pool.GenerateLevelPool();

        //Spawn the ground that will just stay in place
        trail = Instantiate(gameManager.currentlySelectedLevel.environment, Vector3.zero, Quaternion.identity);

        //pool.ActivateARandomSection(new Vector3(0, 0, 0), out previousPosition);

        Instantiate(GameManager.Instance.currentlySelectedLevel.startingSection, Vector3.zero, Quaternion.identity);
        pool.ActivateARandomSection(previousPosition + offSet, out previousPosition);
        pool.ActivateARandomSection(previousPosition + offSet*2, out previousPosition);
        
    }

    private void ClearTrail()
    {
        Destroy(trail);
    }



}
