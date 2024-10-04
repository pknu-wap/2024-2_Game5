using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform cam_transform;
    Transform player_tr;
    // Start is called before the first frame update
    void Start()
    {
        cam_transform = GetComponent<Transform>();
        player_tr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        cam_transform.position = new Vector3(player_tr.transform.position.x + 5, 6, -15);
    }
}