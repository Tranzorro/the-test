using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private float maxX = 92f;
    private float minX = -92f;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float x = Mathf.Clamp(player.transform.position.x, minX, maxX);
        transform.position = new Vector3(x, player.transform.position.y + offset.y, offset.z);
    }
}
