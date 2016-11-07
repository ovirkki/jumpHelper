using System;
using System.Collections.Generic;

public class Formations
{
    private const int jumpAmount = 10;
    private int minPointPerJump;
    private List<string> formationList;
    private Random rnd = new Random();
    public Formations(List<string> formations, int minPoints, int rounds)
    {
        this.minPointPerJump = minPoints;
        this.formationList = formations; //maybe get array in and convert it to list here?
    }


    private Jump generateOneJump(ref List<string> formationPool)
    {
        //Random rnd = new Random();
        Jump jump = new Jump(this.minPointPerJump);
        int pointsPerJump = 0;
        while(pointsPerJump < this.minPointPerJump)
        {
            if(formationPool.Count == 0)
            {
                //Console.WriteLine("reset pool");
                formationPool = new List<string>(this.formationList);
            }
            int next = this.rnd.Next(formationPool.Count);
            string selectedFormation = formationPool[next];
            formationPool.RemoveAt(next);
            jump.addFormationToJump(selectedFormation);
            pointsPerJump += getPointsForFormation(selectedFormation);
        }
        return jump;
    }
    public List<Jump> generateJumps()
    {
        List<Jump> jumpList = new List<Jump>();
        List<string> formationPool = new List<string>(this.formationList);
        for(var i = 0;i < jumpAmount; i++)
        {
            /*
            Jump jump = new Jump(this.minPointPerJump);
            int pointsPerJump = 0;
            while(pointsPerJump < this.minPointPerJump)
            {
                if(formationPool.Count == 0)
                {
                    formationPool = new List<string>(this.formationList);
                }
                int next = rnd.Next(formationPool.Count);
                string selectedFormation = formationPool[next];
                formationPool.RemoveAt(next);
                jump.addFormationToJump(selectedFormation);
                pointsPerJump += getPointsForFormation(selectedFormation);
            }*/
            Jump jump = generateOneJump(ref formationPool);
            jumpList.Add(jump);
        }
        return jumpList;
    }

    private int getPointsForFormation(string selectedFormation) {
        int dummyValue;
        if (Int32.TryParse(selectedFormation, out dummyValue))
        {
            return 2;
        }
        return 1;
    }
}

public class Jump
{
    private List<string> jumpSequence { get; set; }
    public Jump(int minPointPerJump)
    {
        this.jumpSequence = new List<string>();
    }

    public void addFormationToJump(string formation)
    {
        jumpSequence.Add(formation);
        //Console.WriteLine("formation added: " + formation);
    }

    public List<string> getSequence()
    {
        return this.jumpSequence;
    }

    public string printJump()
    {
        string returnValue = "";
        foreach(string form in jumpSequence)
        {
            returnValue += form + ", ";
        }
        return returnValue;
    }
}