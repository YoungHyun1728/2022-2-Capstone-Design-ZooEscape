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

    private static int hp = 5;
    public static bool isDead = false;

    Rigidbody2D p_rigid2D;
    SpriteRenderer spriteRenderer;
    Animator anim;

    void Start()
    {
        p_rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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


        //좌우 플립, 걷는 애니메이션
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetBool("isWalk", true);
            spriteRenderer.flipX = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("isWalk", true);
            spriteRenderer.flipX = false;
        }

        // 멈춤
        if(p_velocity.x == 0)
            anim.SetBool("isWalk", false);

        //점프
        if (Input.GetKeyDown(KeyCode.Z) && jumpCount < 2)
        {
            jumpCount++;
            p_rigid2D.velocity = new Vector2(0, 0);
            p_rigid2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);   
        }

        //점프 애니메이션 --
        if(isGround == false)
            anim.SetBool("isJump", true);
        else
            anim.SetBool("isJump", false);

        //슬라이딩(좌)
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.X) && isSliding == true)
        {
            StartCoroutine(S_Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            StartCoroutine(IsSliding());  // 슬라이딩 쿨타임
        }

        //슬라이딩(우)
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.X) && isSliding == true)
        {
            StartCoroutine(S_Invincible()); // 무적
            StartCoroutine(Sliding());    // 슬라이딩
            StartCoroutine(IsSliding());  // 슬라이딩 쿨타임
        }
        
        //게임오버(체력)
        if(hp == 0)
        {
            isDead = true;
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
            anim.SetBool("isSlide", true);
            walkForce = walkForce * 3;
            yield return new WaitForSeconds(0.15f);
            anim.SetBool("isSlide", false);
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
        }

        //피격 후 무적 + 깜빡임
        IEnumerator H_Invincible()
        {
            Player.hp--;
            gameObject.layer = 7;
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            p_rigid2D.AddForce(new Vector2(-8, 1) * 7, ForceMode2D.Impulse);
            yield return new WaitForSeconds(3.0f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
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

    public int getHP()
    {
        return hp;
    }
}
