using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //이동속도, 점프력 조절
    public float walkForce = 8f;
    public float jumpForce = 50f;

    public int jumpCount = 0;
    public bool isGround = false;

    public int hp = 5;
    public static bool isDead = false;

    Rigidbody2D p_rigid2D;

    void Start()
    {
        p_rigid2D = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        // 캐릭터 이동
        float inputX = Input.GetAxis("Horizontal");
        float fallSpeed = p_rigid2D.velocity.y;
        float speed = walkForce * inputX;

        Vector2 p_velocity = new Vector2(speed, 0);
        p_velocity.y = fallSpeed;
        p_rigid2D.velocity = p_velocity;

        
        //캐릭터 이동 애니메이션+ (좌우이동 구분)

        
        //점프
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2)
        {
            jumpCount++;
            p_rigid2D.velocity = new Vector2(0, 0);
            p_rigid2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            // 점프애니메이션+
        }

        //슬라이딩(좌)
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.Z) && isGround == true)
        {
            StartCoroutine(Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            //슬라이딩 애니메이션+
        }

        //슬라이딩(우)
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.Z) && isGround == true)
        {
            StartCoroutine(Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            //슬라이딩 애니메이션+
        }
        
        //게임오버+ (시간제한)
        if(hp == 0)
        {
            isDead = true;
            // 게임오버 애니메이션+
        }
        
        //슬라이딩 중 무적
        IEnumerator Invincible()
        {
            gameObject.layer = 10;
            yield return new WaitForSeconds(0.5f);
            gameObject.layer = 0;
        }
        
        //슬라이딩
        IEnumerator Sliding()
        {
            walkForce = walkForce * 3;
            yield return new WaitForSeconds(0.15f);
            walkForce = walkForce / 3;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //2단점프 활성화
        if (collision.gameObject.tag == "Floor")
        {
            isGround = true;
            jumpCount = 0;
        }

        if (collision.gameObject.tag == "enemy")
        {
            hp--;
            // 피격 애니메이션+
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //점프 불가
        if (collision.gameObject.tag == "Floor")
        {
            isGround = false;
        }
    }
}
