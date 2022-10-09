using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //�̵��ӵ�, ������ ����
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
        // ĳ���� �̵�
        float inputX = Input.GetAxis("Horizontal");
        float fallSpeed = p_rigid2D.velocity.y;
        float speed = walkForce * inputX;

        Vector2 p_velocity = new Vector2(speed, 0);
        p_velocity.y = fallSpeed;
        p_rigid2D.velocity = p_velocity;

        
        //ĳ���� �̵� �ִϸ��̼�+ (�¿��̵� ����)

        
        //����
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount < 2)
        {
            jumpCount++;
            p_rigid2D.velocity = new Vector2(0, 0);
            p_rigid2D.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            // �����ִϸ��̼�+
        }

        //�����̵�(��)
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.Z) && isGround == true)
        {
            StartCoroutine(Invincible()); // ����
            StartCoroutine(Sliding());    // �����̵�
            //�����̵� �ִϸ��̼�+
        }

        //�����̵�(��)
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.Z) && isGround == true)
        {
            StartCoroutine(Invincible()); // ����
            StartCoroutine(Sliding());    // �����̵�
            //�����̵� �ִϸ��̼�+
        }
        
        //���ӿ���+ (�ð�����)
        if(hp == 0)
        {
            isDead = true;
            // ���ӿ��� �ִϸ��̼�+
        }
        
        //�����̵� �� ����
        IEnumerator Invincible()
        {
            gameObject.layer = 10;
            yield return new WaitForSeconds(0.5f);
            gameObject.layer = 0;
        }
        
        //�����̵�
        IEnumerator Sliding()
        {
            walkForce = walkForce * 3;
            yield return new WaitForSeconds(0.15f);
            walkForce = walkForce / 3;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //2������ Ȱ��ȭ
        if (collision.gameObject.tag == "Floor")
        {
            isGround = true;
            jumpCount = 0;
        }

        if (collision.gameObject.tag == "enemy")
        {
            hp--;
            // �ǰ� �ִϸ��̼�+
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //���� �Ұ�
        if (collision.gameObject.tag == "Floor")
        {
            isGround = false;
        }
    }
}
