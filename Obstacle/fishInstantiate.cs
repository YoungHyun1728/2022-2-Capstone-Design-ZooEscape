using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class fishInstantiate : MonoBehaviour
    {
        public GameObject fishPrefeb;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                GameObject fish = Instantiate(fishPrefeb);
                fish.transform.position = new Vector2(transform.position.x + 10f, 6f);
                Destroy(gameObject);
            }
        }
    }

}

