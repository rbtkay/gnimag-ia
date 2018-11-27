// GameManagerScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove
{

    public int index;
    public int score;
    public AIMove()
    {
        index = 0;
        score = 0;
    }
}
public class GameManagerScript : MonoBehaviour
{

    public static GameManagerScript instance;
    public int turn;
    public GameObject[] cells;

    int[] cellsState;
    // Use this for initialization
    void Start()
    {
        instance = this;
        turn = 1;


        cellsState = new int[9];
    }

    // Update is called once per frame
    void Update()
    {

        if (turn == 2)
        {
            turn = -1;
            StartCoroutine(WaitForAIMove());
        }

    }

    IEnumerator WaitForAIMove()
    {
        yield return new WaitForSeconds(1);
        print(Time.time);


        for (int i = 0; i < 9; ++i)
        {
            cellsState[i] = cells[i].GetComponent<CellScript>().state;
        }
        PerformAIMove();
    }

    void PerformAIMove()
    {
        AIMove bestMove = GetBestMove(2);

        cells[bestMove.index].GetComponent<MeshRenderer>().material.color = Color.blue;
        cells[bestMove.index].GetComponent<CellScript>().state = 2;
        turn = 1;

    }

    AIMove GetBestMove(int currentTurn)
    {
        AIMove move = new AIMove();

        List<AIMove> moves = new List<AIMove>();


        int val = EvaluteBoardState(currentTurn);
        if (val == 1)
        {
            move.score = 10;
            return move;
        }
        else
        if (val == 2)
        {
            move.score = -10;
            return move;
        }
        else
        if (val == 0)
        {
            move.score = 0;
            return move;
        }

        for (int i = 0; i < 9; ++i)
        {
            if (cellsState[i] == 0)
            {
                cellsState[i] = currentTurn;

                int newTurn = 1;
                if (currentTurn == 1)
                {
                    newTurn = 2;
                }

                AIMove m = new AIMove();
                m.score = GetBestMove(newTurn).score;
                m.index = i;
                moves.Add(m);

                cellsState[i] = 0;

            }
        }

        AIMove bestMove = new AIMove();
        int bestScore = 0;
        int bestIndex = 0;
        if (currentTurn == 1)
        {
            bestScore = -9999;
            for (int i = 0; i < moves.Count; ++i)
            {
                if (moves[i].score > bestScore)
                {
                    bestIndex = i;
                    bestScore = moves[i].score;

                }
            }
            return moves[bestIndex];
        }
        else
        if (currentTurn == 2)
        {
            bestScore = 9999;
            for (int i = 0; i < moves.Count; ++i)
            {
                if (moves[i].score < bestScore)
                {
                    bestIndex = i;
                    bestScore = moves[i].score;
                }
            }

            return moves[bestIndex];
        }

        return move;
    }


    int EvaluteBoardState(int currentTurn)
    {
        int c0, c1, c2;
        for (int i = 0; i < 3; ++i)
        {
            c0 = cellsState[i * 3];
            c1 = cellsState[i * 3 + 1];
            c2 = cellsState[i * 3 + 2];

            if (c0 == c1 && c1 == c2 && c1 != 0)
            {
                return c0;
            }

        }

        for (int i = 0; i < 3; ++i)
        {
            c0 = cellsState[i];
            c1 = cellsState[i + 3];
            c2 = cellsState[i + 6];

            if (c0 == c1 && c1 == c2 && c1 != 0)
            {
                return c0;
            }

        }

        c0 = cellsState[0];
        c1 = cellsState[4];
        c2 = cellsState[8];

        if (c0 == c1 && c1 == c2 && c1 != 0)
        {
            return c0;
        }

        c0 = cellsState[2];
        c1 = cellsState[4];
        c2 = cellsState[6];

        if (c0 == c1 && c1 == c2 && c1 != 0)
        {
            return c0;
        }


        for (int i = 0; i < 9; ++i)
        {
            if (cellsState[i] == 0)
            {
                return -1;
            }
        }

        return 0;
    }


}
