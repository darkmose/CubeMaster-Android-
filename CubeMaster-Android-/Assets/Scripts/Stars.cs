using UnityEngine;

public class Stars
{
    [HideInInspector]
    public int countAll;
    [HideInInspector]
    public int countCur;

    public Stars()
    {
        ComputeAll();
    }

    void ComputeAll()
    {
        countAll = GameObject.FindGameObjectsWithTag("Platform").Length;
    }

    public int GetResult()
    {
        int res = (int)((float)countCur / (float)countAll*70);

        if (res >= 70)
        {
            return 1;
        }
        else if (res < 70 && res >= 45)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    public void ComputeCurr()
    {
        countCur++;
    }
}
