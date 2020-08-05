using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public bool ShouldMove;
    public bool ShouldRotate;
    public float MoveSpeed;
    public float RotateSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldMove)
        {
            transform.position += new Vector3(MoveSpeed, 0f, 0f) * Time.deltaTime;
        }
        if (ShouldRotate) 
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, RotateSpeed * Time.deltaTime, 0f));
        }
    }
}
