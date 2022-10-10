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
    public bool isSliding = false;

    public static int hp = 5;
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
        if (Input.GetKeyDown(KeyCode.Z) && jumpCount < 2)
        {
            jumpCount++;
            p_rigid2D.velocity = new Vector2(0, 0);
            p_rigid2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            // 점프애니메이션+
        }
        
        //발판 아래로 내려가는 점프?

        //슬라이딩(좌)
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.X) && isSliding == true)
        {
            StartCoroutine(S_Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            StartCoroutine(IsSliding());  // 슬라이딩 쿨타임
            //슬라이딩 애니메이션+
        }

        //슬라이딩(우)
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.X) && isSliding == true)
        {
            StartCoroutine(S_Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            StartCoroutine(IsSliding());  // 슬라이딩 쿨타임
            //슬라이딩 애니메이션+
        }
        
        //게임오버(체력)
        if(hp == 0)
        {
            isDead = true;
            // 게임오버 애니메이션+
            // 씬전환
        }

        //슬라이딩 쿨타임 2초
        IEnumerator IsSliding()
        {
            isSliding = false;
            yield return new WaitForSeconds(2.0f);
            isSliding = true;
        }

        //슬라이딩 중 무적
        IEnumerator S_Invincible()
        {
            gameObject.layer = 7;
            yield return new WaitForSeconds(1.0f);
            gameObject.layer = 3;
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
        //2단점프 활성화, 슬라이딩 활성화
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
        {
            isSliding = true;
            isGround = true;
            jumpCount = 0;
        }

        //피격 처리
        if (collision.gameObject.tag == "enemy")
        {
            StartCoroutine(H_Invincible());
            Debug.Log("맞음");
            // 피격 애니메이션+
        }

        //피격 후 무적
        IEnumerator H_Invincible()
        {
            Player.hp--;
            gameObject.layer = 7;
            yield return new WaitForSeconds(5.0f);
            gameObject.layer = 3;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //점프 불가
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
        {
            isGround = false;
        }
    }
}
