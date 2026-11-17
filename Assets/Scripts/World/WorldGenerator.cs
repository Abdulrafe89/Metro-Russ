using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public float SegmentLength = 35f;
    public float LaneOffset = 3.5f;
    public int InitialSegments = 10;

    private readonly List<GameObject> segments = new List<GameObject>();
    private float nextSegmentZ;

    private void Start()
    {
        BuildStaticEnvironment();
        SpawnInitialSegments();
    }

    private void Update()
    {
        if (GameManager.Instance == null || GameManager.Instance.CurrentState != GameManager.GameState.Running)
        {
            return;
        }

        float scroll = GameManager.Instance.CurrentRunSpeed * Time.deltaTime;
        for (int i = 0; i < segments.Count; i++)
        {
            var segment = segments[i];
            segment.transform.position = new Vector3(segment.transform.position.x, segment.transform.position.y, segment.transform.position.z - scroll);

            if (segment.transform.position.z < -30f)
            {
                RecycleSegment(segment);
            }
        }
    }

    private void BuildStaticEnvironment()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.tag = "Ground";
        ground.transform.localScale = new Vector3(18f, 1f, 250f);
        ground.transform.position = new Vector3(0f, -0.5f, 20f);

        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = new Color(0.55f, 0.57f, 0.62f);
        ground.GetComponent<Renderer>().material = mat;

        for (int i = 0; i < 8; i++)
        {
            var building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.name = "Building";
            building.transform.position = new Vector3(-14f + (i * 4.2f), 4f, 25f + (i * 18f));
            building.transform.localScale = new Vector3(4f, 8f + ((i % 3) * 3), 5f);
            building.GetComponent<Renderer>().material.color = new Color(0.9f, 0.7f, 0.55f);
        }

        for (int i = 0; i < 8; i++)
        {
            var building = GameObject.CreatePrimitive(PrimitiveType.Cube);
            building.name = "Building";
            building.transform.position = new Vector3(14f - (i * 4.2f), 4f, 30f + (i * 17f));
            building.transform.localScale = new Vector3(4f, 10f + ((i % 4) * 2), 5f);
            building.GetComponent<Renderer>().material.color = new Color(0.6f, 0.72f, 0.86f);
        }
    }

    private void SpawnInitialSegments()
    {
        nextSegmentZ = 0f;

        for (int i = 0; i < InitialSegments; i++)
        {
            GameObject segment = CreateSegment();
            segment.transform.position = new Vector3(0f, 0f, nextSegmentZ);
            segments.Add(segment);
            nextSegmentZ += SegmentLength;
        }
    }

    private GameObject CreateSegment()
    {
        GameObject segment = new GameObject("TrackSegment");
        segment.transform.SetParent(transform);

        for (int lane = 0; lane < 3; lane++)
        {
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = "Rail";
            rail.transform.SetParent(segment.transform);
            rail.transform.localPosition = new Vector3((lane - 1) * LaneOffset, 0.05f, 0f);
            rail.transform.localScale = new Vector3(0.3f, 0.1f, SegmentLength);
            rail.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }

        int barrierLane = Random.Range(0, 3);
        int coinCount = 5;

        for (int i = 0; i < coinCount; i++)
        {
            int lane = Random.Range(0, 3);
            float z = Random.Range(-12f, 12f);
            SpawnCollectible(segment, lane, z, CollectibleType.Coin);
        }

        for (int i = 0; i < 2; i++)
        {
            int lane = Random.Range(0, 3);
            float z = Random.Range(-8f, 10f);
            if (lane == barrierLane && i == 0)
            {
                lane = (lane + 1) % 3;
            }

            SpawnObstacle(segment, lane, z);
        }

        int powerupLane = Random.Range(0, 3);
        float powerupZ = Random.Range(-10f, 10f);
        if (Random.value > 0.65f)
        {
            SpawnCollectible(segment, powerupLane, powerupZ, (CollectibleType)Random.Range(1, 5));
        }

        return segment;
    }

    private void RecycleSegment(GameObject segment)
    {
        segment.transform.position = new Vector3(0f, 0f, nextSegmentZ);
        nextSegmentZ += SegmentLength;

        for (int i = segment.transform.childCount - 1; i >= 0; i--)
        {
            var child = segment.transform.GetChild(i);
            if (child.gameObject.name.Contains("Rail") || child.gameObject.name.Contains("Train") || child.gameObject.name.Contains("Obstacle") || child.gameObject.name.Contains("Coin") || child.gameObject.name.Contains("PowerUp") || child.gameObject.name.Contains("Collectible"))
            {
                Destroy(child.gameObject);
            }
        }

        for (int lane = 0; lane < 3; lane++)
        {
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = "Rail";
            rail.transform.SetParent(segment.transform);
            rail.transform.localPosition = new Vector3((lane - 1) * LaneOffset, 0.05f, 0f);
            rail.transform.localScale = new Vector3(0.3f, 0.1f, SegmentLength);
            rail.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);
        }

        int barrierLane = Random.Range(0, 3);
        for (int i = 0; i < 2; i++)
        {
            int lane = Random.Range(0, 3);
            float z = Random.Range(-8f, 10f);
            if (lane == barrierLane && i == 0)
            {
                lane = (lane + 1) % 3;
            }
            SpawnObstacle(segment, lane, z);
        }

        int coinCount = 5;
        for (int i = 0; i < coinCount; i++)
        {
            int lane = Random.Range(0, 3);
            float z = Random.Range(-12f, 12f);
            SpawnCollectible(segment, lane, z, CollectibleType.Coin);
        }

        if (Random.value > 0.65f)
        {
            SpawnCollectible(segment, Random.Range(0, 3), Random.Range(-8f, 8f), (CollectibleType)Random.Range(1, 5));
        }
    }

    private void SpawnObstacle(GameObject parent, int lane, float z)
    {
        var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obstacle.name = "Barrier";
        obstacle.transform.SetParent(parent.transform);
        obstacle.transform.localPosition = new Vector3((lane - 1) * LaneOffset, 1.25f, z);
        obstacle.transform.localScale = new Vector3(2.2f, 2.2f, 2.8f);
        obstacle.tag = "Obstacle";

        var renderer = obstacle.GetComponent<Renderer>();
        renderer.material.color = new Color(0.88f, 0.25f, 0.25f);
    }

    private void SpawnCollectible(GameObject parent, int lane, float z, CollectibleType type)
    {
        GameObject collectible = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        collectible.name = type.ToString() + "Collectible";
        collectible.transform.SetParent(parent.transform);
        collectible.transform.localPosition = new Vector3((lane - 1) * LaneOffset, 1.6f, z);
        collectible.transform.localScale = new Vector3(0.5f, 0.08f, 0.5f);
        collectible.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        var collider = collectible.GetComponent<CapsuleCollider>();
        collider.isTrigger = true;

        var collectibleComponent = collectible.AddComponent<Collectible>();
        collectibleComponent.Type = type;
        collectibleComponent.SpinSpeed = 220f;
        collectible.tag = "Collectible";
    }
}
