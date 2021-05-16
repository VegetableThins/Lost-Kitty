using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject playerMovePoint;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //experimental camera setting
        //BoardGenerator board = FindObjectOfType<BoardGenerator>();
        //transform.position = new Vector3(((float)board.width / 2) - 0.5f, ((float)board.height / 2) - 0.5f, gameObject.transform.position.z);
    }
}