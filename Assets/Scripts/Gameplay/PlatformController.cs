using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [SerializeField] GameObject platformPrefab;
    [SerializeField] GameObject tokenGroup;
    [SerializeField] GameObject tokenPrefab;
    [SerializeField] GameObject enemyGroup;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float speed = 5f;
    [SerializeField, Range(0, 100)] float tokenPossibility = 10;
    [SerializeField, Range(0, 100)] float enemyPossibility = 10;

    bool generated;
    BoxCollider2D bcollider;
    float rightScreenEdgeX;
    float rightEdgeX, leftEdgeX;

    void Start()
    {
        generated = false;
        bcollider = GetComponentInChildren<BoxCollider2D>();
        rightScreenEdgeX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;
    }

    void FixedUpdate()
    {
        transform.position += Vector3.left * speed * Time.deltaTime; // platform constantly moving left
        for (int i = 0; i < tokenGroup.transform.childCount; i++)
        {
            tokenGroup.transform.GetChild(i).transform.position +=
                Vector3.left * speed / 2 * Time.deltaTime;
        }
        for (int i = 0; i < enemyGroup.transform.childCount; i++)
        {
            enemyGroup.transform.GetChild(i).transform.position +=
                Vector3.left * speed / 2 * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        rightEdgeX = transform.position.x + (transform.localScale.x / 2); // rightEdge.x = center + collider size/2
        leftEdgeX = transform.position.x - (transform.localScale.x / 2);

        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)); // get bottom left corner of screen in world position
        //Vector2 bottomRight = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)); // get bottom left corner of screen in world position

        if (!generated && leftEdgeX < 0)
        {
            //Debug.Log(name + " " + leftEdgeX + " " + bcollider.size.x / 2);
            GenerateGround();
            generated = true;
        }

        if (rightEdgeX < bottomLeft.x)
        { // if pipe off the screen
            Destroy(gameObject);
        }
    }

    void GenerateGround()
    {
        float heightDiff = transform.localScale.y * 0.8f;
        float nextHeight = Random.Range(transform.position.y - heightDiff, transform.position.y + heightDiff);
        Vector2 newPos = new Vector2(rightScreenEdgeX + transform.localScale.x + Random.Range(-1, 3), Mathf.Clamp(nextHeight, -5, 0));
        GameObject platform = Instantiate(platformPrefab, newPos, Quaternion.identity);
        if (Random.Range(0, 100) <= tokenPossibility)
        {
            Vector2 tPos = newPos;
            tPos.y += transform.localScale.y / 2 + 1;
            GameObject token = Instantiate(tokenPrefab, tPos, Quaternion.identity, tokenGroup.transform);
            token.name = "PowerUp";
        }
        if (Random.Range(0, 100) <= enemyPossibility)
        {
            Vector2 ePos = newPos;
            ePos.x += ePos.x * Random.Range(0.1f, 0.8f);
            ePos.y += transform.localScale.y + 0.1f;
            GameObject enemy = Instantiate(enemyPrefab, ePos, Quaternion.identity, enemyGroup.transform);
            enemy.name = "Enemy";
        }
        platform.name = name;
    }
}
