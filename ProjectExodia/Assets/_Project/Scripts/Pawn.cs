using System;
using System.Collections;
using System.Collections.Generic;
using ProjectExodia;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private float despawnOffset = 10.0f;
    
    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z + -despawnOffset > transform.position.z)
        {
            Destroy(gameObject);
        }
    }
}
