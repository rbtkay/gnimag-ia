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
        AIMove bestMove = GetBestMove(2, -99999, 99999);

        cells[bestMove.index].GetComponent<MeshRenderer>().material.color = Color.blue;
        cells[bestMove.index].GetComponent<CellScript>().state = 2;
        turn = 1;

    }

    AIMove GetBestMove(int currentTurn, float alpha, float beta)
    {
        AIMove move = new AIMove();

        int val = EvaluteBoardState();
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

        int bestScore;
        AIMove bestMove = new AIMove();

        if (currentTurn == 1)
        {
            bestScore = -99999;
        }

        else
        {
            bestScore = 99999;
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
                m.score = GetBestMove(newTurn, alpha, beta).score;
                m.index = i;

                float newAlpha = alpha, newBeta = beta;
                if (currentTurn == 1)
                {
                    if (m.score > bestScore)
                    {
                        bestScore = m.score;
                        bestMove = m;
                    }


                    // Beta pruning
                    if (m.score >= newBeta)
                    {
                        cellsState[i] = 0;
                        return bestMove;
                    }

                    if (newAlpha < m.score)
                    {
                        newAlpha = m.score;
                    }
                }

                else
                {
                    if (m.score < bestScore)
                    {
                        bestScore = m.score;
                        bestMove = m;
                    }

                    // Alpha pruning

                    if (m.score <= newAlpha)
                    {
                        cellsState[i] = 0;
                        return bestMove;
                    }

                    if (newBeta > m.score)
                    {
                        newBeta = m.score;
                    }
                }

                m.index = i;

                cellsState[i] = 0;
            }
        }

        return bestMove;
    }


    int EvaluteBoardState()
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
