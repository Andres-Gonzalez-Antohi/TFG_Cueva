using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public Vector3 targetPosition;
    public float speed = 2.0f;

    private GroundManager groundManager;
    private SpawnType spawnType;
    private int variant; 
                public void SetTarget(Vector3 target, GroundManager manager, SpawnType type, int var)
    {
        targetPosition = target;
        groundManager = manager;
        spawnType = type;
        variant = var;
    }

    private void Update()
    {
        if (targetPosition == Vector3.zero) return;

        float distance = Vector3.Distance(transform.position, targetPosition);

                List<Vector2Int> cells = GetGroundPositions();
        foreach (Vector2Int cellPos in cells)
        {
            groundManager.ReportDistance(cellPos, distance);
        }

                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

                if (distance < 0.2f)
        {
            EvaluateNoteHit();
            Destroy(gameObject);
        }
    }

                                                        private void EvaluateNoteHit()
    {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogError("No se encontró un objeto con tag 'Player'.");
            return;
        }
        Vector3 playerPos = playerObj.transform.position;

                        Vector2Int playerTile = new Vector2Int();
        float minDist = Mathf.Infinity;
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                GameObject cell = groundManager.groundCells[i, j];
                if (cell != null)
                {
                    float d = Vector3.Distance(playerPos, cell.transform.position);
                    if (d < minDist)
                    {
                        minDist = d;
                        playerTile = new Vector2Int(i, j);
                    }
                }
            }
        }
                Debug.Log("Jugador en celda: (" + playerTile.x + ", " + playerTile.y + ")");

        bool correct = false;
        int points = 0;
        bool perfect = false;

        switch (spawnType)
        {
            case SpawnType.Front:
                                if (playerTile.y == variant)
                {
                    correct = true;
                                                            if (playerTile.x == 0)
                    {
                        points = 2;
                        perfect = true;
                    }
                    else if (playerTile.x == 1)
                    {
                        points = 1;
                    }
                }
                break;
            case SpawnType.Right:
                                                                                                if (variant == 0)
                {
                    if (playerTile.x == 0)
                    {
                        correct = true;
                        if (playerTile.y == 2)
                        {
                            points = 2;
                            perfect = true;
                        }
                        else
                        {
                            points = 1;
                        }
                    }
                }
                else if (variant == 1)
                {
                    if (playerTile.x == 1)
                    {
                        correct = true;
                        if (playerTile.y == 2)
                        {
                            points = 2;
                            perfect = true;
                        }
                        else
                        {
                            points = 1;
                        }
                    }
                }
                break;
            case SpawnType.Left:
                                                                                                if (variant == 0)
                {
                    if (playerTile.x == 0)
                    {
                        correct = true;
                        if (playerTile.y == 0)
                        {
                            points = 2;
                            perfect = true;
                        }
                        else
                        {
                            points = 1;
                        }
                    }
                }
                else if (variant == 1)
                {
                    if (playerTile.x == 1)
                    {
                        correct = true;
                        if (playerTile.y == 0)
                        {
                            points = 2;
                            perfect = true;
                        }
                        else
                        {
                            points = 1;
                        }
                    }
                }
                break;
        }

        if (correct)
        {
                        ScoreManager_BeatPosition.instance.AddScore(points, perfect);
        }
        else
        {
                        PlayerLivesManager_BeatPosition.instance.LoseLife();
        }
    }

                            private List<Vector2Int> GetGroundPositions()
    {
        List<Vector2Int> positions = new List<Vector2Int>();
        if (spawnType == SpawnType.Front)
        {
            int col = variant;
            positions.Add(new Vector2Int(0, col));
            positions.Add(new Vector2Int(1, col));
        }
        else if (spawnType == SpawnType.Right || spawnType == SpawnType.Left)
        {
            int row = variant;
            for (int col = 0; col < 3; col++)
            {
                positions.Add(new Vector2Int(row, col));
            }
        }
        return positions;
    }
}
