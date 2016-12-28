using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class Category
{
    protected List<string> formationsList;
    protected string name, shortName;
    protected string[] randoms = {"A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "O", "P", "Q"};
    protected string[] aBlocks = {"2", "4", "6", "7", "8", "9", "19", "21"};
    protected string[] aaBlocks = {"1", "11", "13", "14", "15", "18", "20", "22"};
    protected string[] aaaBlocks = {"3", "5", "10", "12", "16", "17"};
    protected int minPointPerJump;
    protected int rounds;

    public Category()
    {
    }

    public string getName() {
        return this.name;
    }

    public async Task<string> getShortName()
    {
        return await Task.FromResult<string>(shortName);
    }

    public string ShortName
    {
        get { return this.shortName; }
    }

    public List<string> FormationList
    {
        get { return this.formationsList; }
    }

    public List<List<string>> getDraw()
    {
        List<List<string>> draw = Formations.generateJumps(this.formationsList, rounds, this.minPointPerJump);
        return Formations.generateJumps(this.formationsList, rounds, this.minPointPerJump);
    }
}

public class Rookie : Category
{
    public Rookie()
    {
        this.name = "Rookie";
        this.shortName = "R";
        this.minPointPerJump = 3;
        this.rounds = 10;
        this.formationsList = new List<string>(randoms);
    }
}

public class Intermediate : Rookie
{
    public Intermediate()
    {
        this.name = "Intermediate";
        this.shortName = "A";
        this.minPointPerJump = 3;
        this.rounds = 10;
        this.formationsList.AddRange(new List<string>(aBlocks));
    }
}

public class DoubleA : Intermediate
{
    public DoubleA()
    {
        this.name = "DoubleA";
        this.shortName = "AA";
        this.minPointPerJump = 4;
        this.rounds = 10;
        this.formationsList.AddRange(new List<string>(aaBlocks));
    }
}

public class Open : DoubleA
{
    public Open()
    {
        this.name = "Open";
        this.shortName = "AAA";
        this.minPointPerJump = 5;
        this.rounds = 10;
        this.formationsList.AddRange(new List<string>(aaaBlocks));
    }
}