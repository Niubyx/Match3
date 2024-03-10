using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public int width = 6;
    public int height = 10;
    public GameObject tileObject;

    public float cameraSizeOffset;
    public float cameraVerticalOffset;

    public GameObject[] availablePieces;

    public Tiles[,] Tiles;
    public Piece[,] Pieces;

    Tiles startTile;
    Tiles endTile;

    public bool isAlive;


    void Start()
    {
        Tiles = new Tiles[width, height];
        Pieces = new Piece[width, height];
        SetupBoard();
        PositionCamera();
        SetupPieces();
    }

    private void Update()
    {
        EliminarMatchCheck();
    }


    private void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
                int operations = 0;

                while (boardMatched(x, y, selectedPiece) && operations < 100)
                {
                    selectedPiece = availablePieces[UnityEngine.Random.Range(0, availablePieces.Length)];
                    operations++;


                }

                operations = 0;
                var o = Instantiate(selectedPiece, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                Pieces[x, y] = o.GetComponent<Piece>();
                Pieces[x, y]?.Setup(x, y, this);
            }

        }
    }
    private void PositionCamera()
    {
        float newPosX = (float)width / 2f;
        float newPosY = (float)height / 2f;
        Camera.main.transform.position = new Vector3(newPosX - 0.5f, newPosY - 0.5f + cameraVerticalOffset, -10f);

        float horizontal = width + 1;
        float vertical = (height / 2) + 1;

        Camera.main.orthographicSize = horizontal > vertical ? horizontal + cameraSizeOffset : vertical;
    }

    private void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity);
                o.transform.parent = transform;
                Tiles[x, y] = o.GetComponent<Tiles>();
                Tiles[x, y]?.Setup(x, y, this);
            }

        }
    }


    public void TileDown(Tiles tile_)
    {
        startTile = tile_;

    }

    public void TileOver(Tiles tile_)
    {
        endTile = tile_;

    }

    public void TileUp(Tiles tile_)
    {
        if (startTile != null && endTile != null && IsCloseTo(startTile, endTile))
        {
            SwapTiles();
                      
        }     

    }

    private void SwapTiles()
    {
        var StartPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        if (StartPiece != null && EndPiece != null)
        {
            StartPiece.Move(endTile.x, endTile.y);
            EndPiece.Move(startTile.x, startTile.y);

            Pieces[startTile.x, startTile.y] = EndPiece;
            Pieces[endTile.x, endTile.y] = StartPiece;
        }

    }

    public bool IsCloseTo(Tiles start, Tiles end)
    {
        if (Math.Abs(start.x - end.x) == 1 && start.y == end.y)
        {
            return true;

        }

        if (Math.Abs(start.y - end.y) == 1 && start.x == end.x)
        {
            return true;

        }

        return false;
    }

    private bool boardMatched(int columna, int fila, GameObject pieza)
    {
        if (columna > 1 && fila > 1)
        {
            if (Pieces[columna - 1, fila].tag == pieza.tag && Pieces[columna - 2, fila].tag == pieza.tag)
            {
                return true;
            }

            if (Pieces[columna, fila - 1].tag == pieza.tag && Pieces[columna, fila - 2].tag == pieza.tag)
            {
                return true;
            }
        }
        else if (columna <= 1 || fila <= 1)
        {
            if (fila > 1)
            {
                if (Pieces[columna, fila - 1].tag == pieza.tag && Pieces[columna, fila - 2].tag == pieza.tag)
                {
                    return true;
                }
            }
            if (columna > 1)
            {
                if (Pieces[columna - 1, fila].tag == pieza.tag && Pieces[columna - 2, fila].tag == pieza.tag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void LowerPieces()
    {
        int espacios = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Pieces[x, y] == null)
                {
                    espacios++;
                }
                else if (espacios > 0)
                {
                    Pieces[x, y].GetComponent<Transform>().position = new Vector3(x, y - espacios);
                }
            }
            espacios = 0;
        }
        
    }

    private void EliminarMatch(int columna, int fila)
    {
        
        if (Pieces[columna, fila].GetComponent<Piece>().ismatched)
        {
            Destroy(Pieces[columna, fila].gameObject, 0.4f);
            Invoke("LowerPieces", 1.2f);
            Pieces[columna, fila] = null;
        }
    }

    public void EliminarMatchCheck()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (Pieces[i, j] != null)
                {
                    EliminarMatch(i, j);                    
                }
            }
        }

    }
}
