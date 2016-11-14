using System;
using System.Collections.Generic;

public static class Formations
{
    private const int jumpAmount = 10;
    private static List<string> formationList;
    private static Random rnd = new Random();

    private static List<string> generateOneJump(ref List<string> formationPool, int minPointPerJump)
    {
        //Random rnd = new Random();
        List<string> jump = new List<string>();
        int pointsPerJump = 0;
        while(pointsPerJump < minPointPerJump)
        {
            if(formationPool.Count == 0)
            {
                formationPool = new List<string>(formationList);
            }
            int next = rnd.Next(formationPool.Count);
            string selectedFormation = formationPool[next];
            formationPool.RemoveAt(next);
            jump.Add(selectedFormation);
            pointsPerJump += getPointsForFormation(selectedFormation);
        }
        return jump;
    }
    public static List<List<string>> generateJumps(List<string> formations, int rounds, int minPointPerJump)
    {
        formationList = formations;
        List<List<string>> jumpList = new List<List<string>>();
        List<string> formationPool = new List<string>(formationList);
        for(var i = 0;i < rounds; i++)
        {
            List<string> jump = generateOneJump(ref formationPool, minPointPerJump);
            jumpList.Add(jump);
        }
        return jumpList;
    }

    private static int getPointsForFormation(string selectedFormation) {
        int dummyValue;
        if (Int32.TryParse(selectedFormation, out dummyValue))
        {
            return 2;
        }
        return 1;
    }
}
