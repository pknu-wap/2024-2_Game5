using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    public Vector3 minBounds;  // 카메라가 이동할 수 있는 최소 좌표
    public Vector3 maxBounds;  // 카메라가 이동할 수 있는 최대 좌표

    public GameObject player;

    private void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);

        this.transform.Translate(moveVector);

        // 카메라의 위치를 최소 및 최대 좌표로 제한
        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(this.transform.position.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(this.transform.position.y, minBounds.y, maxBounds.y),
            this.transform.position.z
        );

        this.transform.position = clampedPosition;
    }
}
