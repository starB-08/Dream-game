using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector3 movingVector;
    public float speed;
    public float jumpPower;
    [SerializeField] private bool canMove3D = false;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Animator animator;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
    }
    void Update(){
        Move();
        Jump();
    }

    void Move(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float maxSpeed = speed * 12;

        movingVector = new Vector3(horizontal,0, canMove3D? vertical : 0);

        rigid.AddForce(movingVector * speed * Time.deltaTime * 10, ForceMode.Impulse);

        if(horizontal == 0){
            rigid.velocity = new Vector3(rigid.velocity.x * 0.1f , rigid.velocity.y,rigid.velocity.z);
        }
        if(vertical == 0){
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y,rigid.velocity.z * 0.1f);
        }

        rigid.velocity = new Vector3(
            Mathf.Abs(rigid.velocity.x) >= maxSpeed? maxSpeed * Mathf.Sign(rigid.velocity.x): rigid.velocity.x , 
            rigid.velocity.y ,
            Mathf.Abs(rigid.velocity.z) >= maxSpeed? maxSpeed * Mathf.Sign(rigid.velocity.z) : rigid.velocity.z);
    }
    void Jump(){
        bool isJump = Input.GetButtonDown("Jump");
        if (isJump){
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }
}
