using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPool : MonoBehaviour
{
    public static SectionPool SharedInstance;
    public GameManager gameManager;

    public List<GameObject> PooledSections;
    public List<GameObject> ActiveSections;
    public int amountOfEachSectionToPool = 3;

    void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        gameManager.OnGameQuit.AddListener(ClearPool);
    }

    public void GenerateLevelPool()
    {
        int numberOfSectionsInLevel = gameManager.GetCurrentlySelectedLevel().sectionPrefabs.Count;

        GameObject tmp;

        for (int i = 0; i < numberOfSectionsInLevel; i++)
        {
            GameObject objectToPool = gameManager.GetCurrentlySelectedLevel().sectionPrefabs[i];
            for (int j = 0; j < amountOfEachSectionToPool; j++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                PooledSections.Add(tmp);
            }
        }
    }

    public void ActivateARandomSection(Vector3 position, out Vector3 previousPosition)
    {
        if (PooledSections != null && PooledSections.Count > 0)
        {
            //get one random from the pool
            int randomIndex = Random.Range(0, PooledSections.Count);
            GameObject sectionToActivate = PooledSections[randomIndex];

            //set 'spawn' position and make it active
            sectionToActivate.transform.localPosition = position;
            sectionToActivate.SetActive(true);
            previousPosition = sectionToActivate.transform.localPosition;

            //activate all coins (if previously picked up)
            ActivateAllSectionChildren(sectionToActivate);
            ActiveSections.Add(sectionToActivate);

            //remove it from pool
            PooledSections.RemoveAt(randomIndex);
        }
        previousPosition = Vector3.zero;
        
    }


    public void RemoveAnActiveSection(GameObject sectionToPlaceInPool)
    {
        if (ActiveSections.Count > 0)
        {
            sectionToPlaceInPool.SetActive(false);

            PooledSections.Add(sectionToPlaceInPool);

            ActiveSections.Remove(sectionToPlaceInPool);
        }
    }

    public void ActivateAllSectionChildren(GameObject section)
    {
        if (section != null)
        {
            foreach (Transform child in section.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void ClearPool()
    {
        foreach (var section in PooledSections)
        {
            Destroy(section);
        }

        // Clear the list to remove references
        PooledSections.Clear();

        foreach (var activeSection in ActiveSections)
        {
            Destroy(activeSection);
        }

        ActiveSections.Clear();
    }
}
