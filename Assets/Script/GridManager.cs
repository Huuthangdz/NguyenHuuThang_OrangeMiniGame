using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private enum TitleType
    {   
        Empty,
        Block,
        Piece
    }

    private class Tile
    {
        public TitleType Type;
        public GameObject GameObject;

        public Tile(TitleType titleType,GameObject gameObject)
        {
            this.Type = titleType;
            this.GameObject = gameObject;
        }
    }

    private Tile[,] grid;

    [SerializeField] private int width, hight;

    [SerializeField] private GameObject[] piecePrefab;
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private Transform Parent;
    [SerializeField] private float spacing;
    [SerializeField] private Camera mainCam;
    [SerializeField] private int blockCount;

    void Start()
    {
        GenerateGrid();
        PlaceBlocksAndPieces();
    }

    private void GenerateGrid()
    {
        for ( int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                Vector3 position = new Vector3(i * spacing, j * spacing, 0);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, Parent);
                cell.name = $"Cell {i} {j}";
            }
        }
        mainCam.transform.position = new Vector3((width - 1) * spacing / 2, (hight - 1) * spacing / 2, -10);
    }

    private void PlaceBlocksAndPieces()
    {
        grid = new Tile[width, hight];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                grid[i, j] = new Tile(TitleType.Empty,null);
            }
        }

        List<Vector2Int> allPositions = new List<Vector2Int>();

        for ( int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                allPositions.Add(new Vector2Int(i, j));
            }
        }

        for ( int i = 0; i < allPositions.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, allPositions.Count);
            (allPositions[i], allPositions[randomIndex]) = (allPositions[randomIndex], allPositions[i]);
        }

        PlaceBlock(allPositions);
        PlacePiece(allPositions);

    }
    private void PlaceBlock(List<Vector2Int> allPositions)
    {
        for (int i = 0; i < Mathf.Min(blockCount, allPositions.Count); i++)
        {
            Vector2Int position = allPositions[i];
            GameObject block = Instantiate(blockPrefab, new Vector3(position.x * spacing, position.y * spacing, 0), Quaternion.identity, Parent);
            block.name = $"Block {position.x} {position.y}";
            grid[position.x,position.y] = new Tile(TitleType.Block,block);
        }
    }

    private void PlacePiece(List<Vector2Int> allPositions)
    {
        for (int i = 0; i < piecePrefab.Length; i++)
        {
            int index = i + blockCount;
            if (index >= allPositions.Count) break;

            Vector2Int position = allPositions[index];
            GameObject piece = Instantiate(piecePrefab[i], new Vector3(position.x * spacing, position.y * spacing, 0), Quaternion.identity, Parent);
            piece.name = $"Piece {position.x} {position.y}";
            grid[position.x, position.y] = new Tile(TitleType.Piece, piece);

            PiecePuzzle piecePuzzle = piece.GetComponent<PiecePuzzle>();
            piecePuzzle.PieceIndex = i;
        }
    }

    public void Swipe(Vector2Int direction)
    {
        int startX = direction.x > 0 ? width - 1 : 0;
        int endX = direction.x > 0 ? -1 : width;
        int stepX = (int)(direction.x != 0 ? -Mathf.Sign(direction.x) : 1);

        int startY = direction.y > 0 ? hight - 1 : 0;
        int endY = direction.y > 0 ? -1 : hight;
        int stepY = (int)(direction.y != 0 ? -Mathf.Sign(direction.y) : 1);

        for (int y = startY; y != endY; y += stepY)
        {
            for (int x = startX; x != endX; x += stepX)
            {
                Tile current = grid[x, y];
                if (current.Type == TitleType.Piece)
                {
                    int newX = x + direction.x;
                    int newY = y + direction.y;

                    if (IsInBounds(newX, newY) && grid[newX, newY].Type == TitleType.Empty)
                    {
                        StartCoroutine(MovePiece(current.GameObject, new Vector3(newX * spacing, newY * spacing, 0)));
                        grid[newX, newY] = new Tile(TitleType.Piece, current.GameObject);
                        grid[x, y] = new Tile(TitleType.Empty, null);
                    }
                }
            }
        }
    }
    private IEnumerator MovePiece(GameObject piece, Vector3 targetPosition)
    {
        float duration = 0.2f; // Thời gian di chuyển (càng nhỏ càng nhanh)
        float elapsedTime = 0f;

        Vector3 startPosition = piece.transform.position;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Lerp để di chuyển từ vị trí bắt đầu đến vị trí đích
            piece.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null; // Đợi đến frame tiếp theo
        }

        // Đảm bảo vị trí cuối cùng chính xác
        piece.transform.position = targetPosition;
    }


    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < hight;
    }

    public bool CheckWin()
    {
        for ( int i = 0; i < width - 1; i++)
        {
            for( int j = 0; j < hight - 1; j++)
            {
                Tile topLeft = grid[i, j];
                Tile topRight = grid[i + 1, j];
                Tile bottomLeft = grid[i, j + 1];
                Tile bottomRight = grid[i + 1,j + 1];

                if (topLeft.Type == TitleType.Piece &&
               topRight.Type == TitleType.Piece &&
               bottomLeft.Type == TitleType.Piece &&
               bottomRight.Type == TitleType.Piece)
                {
                    int id0 = topLeft.GameObject.GetComponent<PiecePuzzle>().PieceIndex;
                    int id1 = topRight.GameObject.GetComponent<PiecePuzzle>().PieceIndex;
                    int id2 = bottomLeft.GameObject.GetComponent<PiecePuzzle>().PieceIndex;
                    int id3 = bottomRight.GameObject.GetComponent<PiecePuzzle>().PieceIndex;

                    // You can add more win patterns here if needed
                    if (id0 == 0 && id1 == 1 && id2 == 2 && id3 == 3)
                    {
                        return true;
                    }
                }
            }
        }
        return false;   
    }
}
