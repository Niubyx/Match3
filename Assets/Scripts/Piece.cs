using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Piece : MonoBehaviour
{
    public int x;
    public int y;
    public Board board;
    public GameObject boards;
    public Piece leftPiece;
    public Piece rightPiece;
    public Piece topPiece;
    public Piece bottomPiece;
    public bool ismatched = false;

    private void Start()
    {        
        boards = GameObject.FindWithTag("board");
        MatchCheck();
        PieceCheck();

    }

    private void Update()
    {
        MatchCheck();
        PieceCheck();


    }

    public enum type
    {
        elephant,
        giraffe,
        hippo,
        monkey,
        panda,
        parrot,
        penguin,
        pig,
        rabbit,
        snake
    };

    public type pieceType;

    public void Setup(int x_, int y_, Board board_)
    {
        this.x = x_;
        this.y = y_;
        this.board = board_;
    }

    public void Move(int desx, int desy)
    {
        transform.DOMove(new Vector3(desx, desy, -5f), 0.25f).SetEase(Ease.InOutCubic).onComplete = () =>
        {
            x = desx;
            y = desy;
        };
    }

    [ContextMenu("Test Move")]
    public void MoveTest()
    {
        Move(0, 0);
    }

    public void PieceCheck()
    {
        if (x > 0)
        {
            leftPiece = boards.GetComponent<Board>().Pieces[x - 1, y];
            
        }
        else
        {
            leftPiece = null;
        }


        if (x < board.width - 1)
        {
            rightPiece = boards.GetComponent<Board>().Pieces[x + 1, y];

        }
        else
        {
            rightPiece = null;
        }

        if (y > 0)
        {
            topPiece = boards.GetComponent<Board>().Pieces[x, y - 1];

        }
        else
        {
            topPiece = null;
        }

        if (y < board.height - 1)
        {
            bottomPiece = boards.GetComponent<Board>().Pieces[x, y + 1];

        }
        else
        {
            bottomPiece = null;
        }


    }

    public void MatchCheck()
    {      

        if (leftPiece != null && rightPiece != null)
        {
            if (leftPiece.tag == gameObject.tag && rightPiece.tag == gameObject.tag)
            {
                leftPiece.GetComponent<Piece>().ismatched = true;
                rightPiece.GetComponent<Piece>().ismatched = true;
                ismatched = true;


            }
        }

        if (topPiece != null && bottomPiece != null)
        {
            if (topPiece.tag == gameObject.tag && bottomPiece.tag == gameObject.tag)
            {
                topPiece.GetComponent<Piece>().ismatched = true;
                bottomPiece.GetComponent<Piece>().ismatched = true;
                ismatched = true;

            }
        }

    }

}
