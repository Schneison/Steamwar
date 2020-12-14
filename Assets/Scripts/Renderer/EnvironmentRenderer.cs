using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Steamwar.Renderer
{

    public class EnvironmentRenderer : MonoBehaviour
    {
        public GameObject cloadPrefab;
        public GameObject sky;
        public GameObject skyFirst;
        public GameObject skySecond;
        public GameObject skyThird;
        public SpriteRenderer sector;

        internal int timer;
        internal int nextSpawn;
        internal GameObject cloadContainer;
        internal List<SpriteRenderer> cloads = new List<SpriteRenderer>();
        internal Queue<SpriteRenderer> skies = new Queue<SpriteRenderer>();
        internal SpriteRenderer lastSky;

        void Start()
        {
            cloadContainer = new GameObject();
            cloadContainer.transform.name = "Board/EnvironmentContainer";
            sky.GetComponent<Rigidbody2D>().velocity = new Vector2(0.75F, 0);
            skies.Enqueue(skyThird.GetComponent<SpriteRenderer>());
            skies.Enqueue(skySecond.GetComponent<SpriteRenderer>());
            lastSky = skyFirst.GetComponent<SpriteRenderer>();
            skies.Enqueue(lastSky);
        }

        void Update()
        {
            if(timer > nextSpawn)
            {
                GameObject cload = Instantiate(cloadPrefab, cloadContainer.transform);
                cload.transform.position = SessionManager.session.activeSector.RandomCloudPosition();
                if(Random.value >= 0.5)
                {
                    cload.GetComponent<SpriteRenderer>().flipX = true;
                }
                nextSpawn = 1160 + Mathf.FloorToInt(Random.value * 580);
                cload.GetComponent<Rigidbody2D>().velocity = new Vector2(1.5F, 0);
                cloads.Add(cload.GetComponent<SpriteRenderer>());
                timer = 0;
                cloads.RemoveAll((SpriteRenderer renderer) =>
                {
                    if (renderer.bounds.min.x > SessionManager.session.activeSector.bounds.z)
                    {
                        Destroy(renderer.gameObject);
                        return true;
                    }
                    return false;
                });
            }
            SpriteRenderer rightSky = skies.Peek();
            if (rightSky.bounds.min.x > sector.bounds.max.x)
            {
                rightSky.transform.position = new Vector2(lastSky.bounds.min.x - rightSky.bounds.size.x/2, rightSky.transform.position.y);
                lastSky = skies.Dequeue();
                skies.Enqueue(lastSky);
            }
            timer++;
        }
    }
}
