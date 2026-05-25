using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
using Yarn.Unity;

public class RespawnBehaviour : MonoBehaviour
{
    public GameObject spawnPoint = null;
    private Transform respawnPoint;

    private Transform teleportationTarget = null;

    public CinemachineVirtualCamera cinemachineCamera;

    public float fallThreshold = -10f;

    void Start()
    {
        if (spawnPoint != null) {
            SetRespawnPoint(spawnPoint);
            Respawn();
        } else {
            var spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            if (spawns.Length > 0)
            {
                var id = Random.Range(0, spawns.Length);
                SetRespawnPoint(spawns[id]);
                Respawn();
            }
        }
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void LateUpdate() 
    {
        if (teleportationTarget != null) {
            TeleportImmediatelyTo(teleportationTarget);
            teleportationTarget = null;
        }
    }

    public void SetRespawnPoint(GameObject spawnPoint)
    {
        respawnPoint = spawnPoint.transform;
    }

    public void Respawn()
    {
        TeleportTo(respawnPoint);
    }

    public void Teleport(GameObject target) {
        TeleportTo(target.transform);
    }

    public void TeleportTo(Transform targetTransform)
    {
        teleportationTarget = targetTransform;
    }

    private void TeleportImmediatelyTo(Transform targetTransform)
  {
      var characterController = GetComponent<CharacterController>();
      var rb = GetComponent<Rigidbody>();

      if (characterController != null) {
          characterController.enabled = false;
      }

      if (rb != null) {
          rb.linearVelocity = Vector3.zero;
          rb.angularVelocity = Vector3.zero;
          rb.position = targetTransform.position;
          rb.rotation = targetTransform.rotation;
      } else {          transform.position = targetTransform.position;
          transform.rotation = targetTransform.rotation;
      }

      if (characterController != null) {
          characterController.enabled = true;
      }
  }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("SpawnPoint")) {
            SetRespawnPoint(hit.gameObject);
        }
        if (hit.collider.CompareTag("RespawnPlayers")) {
            Respawn();
        }
    }
}
