using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Vector3 movingVector;
    public float speed;
    public float jumpPower;
    public float dashPower;
    [SerializeField] private float DashCoolDown = 0;
    [SerializeField] private bool canMove3D = false;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private Animator animator;

    void Awake(){
        rigid = GetComponent<Rigidbody>();
    }
    void Update(){
        Move();
        Jump();
        Animation();
        Dash();
    }

    void Move(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float maxSpeed = speed * 12;

        movingVector = new Vector3(horizontal,0, canMove3D? vertical : 0);

        rigid.AddForce(movingVector * speed * Time.deltaTime * 10, ForceMode.Impulse);

        if(Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector3(rigid.velocity.x * 0.1f , rigid.velocity.y,rigid.velocity.z);
        }
        if(Input.GetButtonUp("Vertical")){
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y,rigid.velocity.z * 0.1f);
        }

        rigid.velocity = new Vector3(
            Mathf.Abs(rigid.velocity.x) >= maxSpeed? maxSpeed * Mathf.Sign(rigid.velocity.x): rigid.velocity.x , 
            rigid.velocity.y ,
            Mathf.Abs(rigid.velocity.z) >= maxSpeed? maxSpeed * Mathf.Sign(rigid.velocity.z) : rigid.velocity.z);
    }
    void Jump(){
        bool isJump = Input.GetButtonDown("Jump");
        if (isJump && !animator.GetBool("isJumping")){
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            animator.SetTrigger("doJump");
            animator.SetBool("isJumping", true);
        }
    }

    void Dash(){

        if (DashCoolDown <= 0)
        {
            bool isDash = Input.GetButtonDown("Dash");
            if (isDash) {
                rigid.AddForce(transform.forward * dashPower, ForceMode.Impulse);
                DashCoolDown = 3.0f;
            }
        } else {
            DashCoolDown -= Time.deltaTime;
        }
    }

    void Animation(){
        animator.SetBool("isMovingX", Input.GetAxisRaw("Horizontal") != 0);
        if(canMove3D == false && Mathf.Abs(rigid.velocity.x) > 0){
            transform.rotation = Quaternion.Euler(
                transform.rotation.x, 
                rigid.velocity.x == 0? transform.rotation.y : Mathf.Sign(rigid.velocity.x) * 90, 
                transform.rotation.z);
        }
    }
    

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.CompareTag("Floor")){
            if (animator.GetBool("isJumping")) animator.SetBool("isJumping",false);
            rigid.velocity = new Vector3(rigid.velocity.x * 0.1f , rigid.velocity.y,rigid.velocity.z * 0.1f);
        }
    }
}
