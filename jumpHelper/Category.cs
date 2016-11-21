using System.Collections.Generic;
using System.Threading.Tasks;

public class MakeJumpsController
{
    public Category category { get; set; }
    public MakeJumpsController(string category)
    {
        switch(category)
        {
            case "R":
                this.category = new Rookie();
                break;
            case "A":
                this.category = new Intermediate();
                break;
            case "AA":
                this.category = new DoubleA();
                break;
            case "AAA":
                this.category = new Open();
                break;
            default:
                this.category  = new Rookie();
                break;
        }
    }
    public List<List<string>> getJumps()
    {
        return category.getDraw();
    }
}

public class Category
{
    //protected string[] formations;
    protected List<string> formationsList;
    protected string name, shortName;
    protected string[] randoms = {"A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "O", "P", "Q"};
    protected string[] aBlocks = {"2", "4", "6", "7", "8", "9", "19", "21"};
    protected string[] aaBlocks = {"14", "15", "22"};
    protected string[] aaaBlocks = {"3", "17", "18"};
    protected int minPointPerJump;
    protected int rounds;

    public Category()
    {
        //this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
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
        //this.formations = new Formations(this.formations);
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