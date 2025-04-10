using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [Header("---------------------------------------------------------------------------------------------------------------------")]

    [Header("Checkpoint Objects:")]
    public GameObject checkpointPoleL;
    public GameObject checkpointPoleR;
    public GameObject checkpointLine;

    [Header("Checkpoint Spawn:")]
    public Transform checkpointSpawn;

    [Header("Pole Materials:")]
    public Material[] poleMat;

    private Renderer poleRenderer;

    private Renderer pole2Renderer;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        poleRenderer = checkpointPoleL.GetComponent<Renderer>();
        pole2Renderer = checkpointPoleR.GetComponent<Renderer>();
        poleRenderer.material = poleMat[0];
        pole2Renderer.material = poleMat[0];
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.playerSpawn.transform.position = checkpointSpawn.transform.position;
            checkpointLine.SetActive(false);
            poleRenderer.material = poleMat[1];
            pole2Renderer.material = poleMat[1];
        }
    }
}
