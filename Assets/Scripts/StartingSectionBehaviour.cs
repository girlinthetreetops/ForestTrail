using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSectionBehaviour : MonoBehaviour
{
    private SectionPool pool;

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
    }

    private void OnEnable()
    {
        movementSpeed = defaultMovementSpeed * GameManager.Instance.GetDifficulty();
    }

    private void Update()
    {
        if (transform.position.z <= -30)
        {
            Vector3 newPosition = transform.localPosition + new Vector3(0, 0, 90);
            pool.ActivateARandomSection(newPosition, out previousPosition);

            Destroy(gameObject);
        }

        if (canMove && !GameManager.Instance.isInCountdown)
        {
            Move();
        }

    }

    private void Move()
    {
        transform.Translate(Vector3.back * Time.deltaTime * movementSpeed);
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
