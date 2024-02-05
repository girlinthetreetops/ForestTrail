using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionBehaviour : MonoBehaviour
{
    protected SectionPool pool;

    private bool canMove = true;
    private float movementSpeed = 40f;
    private float defaultMovementSpeed = 40f;

    public Vector3 previousPosition;

    private void Start()
    {
        pool = FindObjectOfType<SectionPool>();

        GameManager.Instance.OnGameOver.AddListener(StopMoving);
        GameManager.Instance.OnLoadLevel.AddListener(StopMoving);
        GameManager.Instance.OnGameStart.AddListener(StartMoving);

        movementSpeed = defaultMovementSpeed * GameManager.Instance.GetDifficulty();
    }

    private void OnEnable()
    {
        movementSpeed = defaultMovementSpeed * GameManager.Instance.GetDifficulty();
    }

    private void Update()
    {
        DissapearAndRespawnNewSection();
        Move();
        
    }

    protected virtual void DissapearAndRespawnNewSection()
    {
        if (transform.position.z <= -30)
        {
            Vector3 newPosition = transform.localPosition + GameManager.Instance.sectionOffset * 3;
            pool.ActivateARandomSection(newPosition, out previousPosition);
            pool.RemoveAnActiveSection(gameObject);
        }
    }

    private void Move()
    {
    if (canMove && !GameManager.Instance.isInCountdown)
    {
        transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
    }
    }

    private void StopMoving()
    {
        canMove = false;
    }

    private void StartMoving()
    {
        canMove = true;
    }
}
