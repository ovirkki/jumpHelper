using System.Collections.Generic;

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
        List<Jump> draw = this.category.getDraw();
        List<List<string>> returnList = new List<List<string>>();
        draw.ForEach(delegate (Jump jump)
        {
            returnList.Add(jump.getSequence());
        });
        return returnList;
    }

    public string[] getJumpsArray()
    {
        List<List<string>> jumpList = getJumps();
        List<string> converted = new List<string>();
        jumpList.ForEach(delegate (List<string> formList)
        {
            string jump = string.Join(",", formList);
            converted.Add(jump);
        });
        return converted.ToArray();
    }
}

public class Category
{
    //protected string[] formations;
    protected List<string> formationsList;
    protected List<Jump> draw;
    protected string name;
    protected Formations formations;
    protected string[] randoms = {"A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "O", "P", "Q"};
    protected string[] aBlocks = {"2", "4", "6", "7", "8", "9", "19", "21"};
    protected string[] aaBlocks = {"14", "15", "22"};
    protected string[] aaaBlocks = {"3", "17", "18"};
    protected int minPointPerJump;

    public Category()
    {
        //this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
    }

    public string getName() {
        return this.name;
    }

    public List<string> getFormations()
    {
        return this.formationsList;
        //return new List<string>(this.formations);
    }
    public List<Jump> getDraw()
    {
        return this.formations.generateJumps();
    }
}

public class Rookie : Category
{
    public Rookie()
    {
        this.name = "Rookie";
        this.minPointPerJump = 3;
        this.formationsList = new List<string>(randoms);
        this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
        //this.formations = new Formations(this.formations);
    }

}

public class Intermediate : Rookie
{
    public Intermediate()
    {
        this.name = "Intermediate";
        this.minPointPerJump = 3;
        this.formationsList.AddRange(new List<string>(aBlocks));
        this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
    }
}

public class DoubleA : Intermediate
{
    public DoubleA()
    {
        this.name = "DoubleA";
        this.minPointPerJump = 4;
        this.formationsList.AddRange(new List<string>(aaBlocks));
        this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
    }
}

public class Open : DoubleA
{
    public Open()
    {
        this.name = "Open";
        this.minPointPerJump = 5;
        this.formationsList.AddRange(new List<string>(aaaBlocks));
        this.formations = new Formations(this.formationsList, this.minPointPerJump, 0);
    }
}