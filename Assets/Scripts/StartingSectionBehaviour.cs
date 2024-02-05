using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSectionBehaviour : SectionBehaviour

{
   
    protected override void DissapearAndRespawnNewSection()
    {
        if (transform.position.z <= -30)
        {
            Vector3 newPosition = transform.localPosition + GameManager.Instance.sectionOffset * 3;
            pool.ActivateARandomSection(newPosition, out previousPosition);
            Destroy(gameObject);
        }

    }



}
