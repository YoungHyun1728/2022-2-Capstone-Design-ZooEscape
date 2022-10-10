using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class fishController : MonoBehaviour
    {
        Rigidbody2D Rigid;
        // Start is called before the first frame update
        void Start()
        {
            Rigid = GetComponent<Rigidbody2D>();
            Rigid.AddForce(transform.right * -0.5f, ForceMode2D.Impulse);
        }

        // Update is called once per frame
        void Update()
        {
            Destroy(gameObject,3f);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(other.gameObject.tag=="Player")
            {
                Destroy(gameObject);
            }
        }
    }

}

