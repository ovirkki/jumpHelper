using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;


public static class FileHandler
{
    private const string FILE_NAME = "FSNotes.xml";
    //private static string filePath;
    private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
    private static string filePath = Path.Combine(path, FILE_NAME);

    /*public FileHandler()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        this.filePath = Path.Combine(path, FILE_NAME);
        XElement data = generateInitData();
        this.saveData(data);
        Console.WriteLine("data saved");
        this.addData("A", "lisätty kommentti");
        this.addData("R", "lisätty r kommentti");
        this.removeData("B", "yksi kommentti");
        this.updateData("A", "yksi kommentti", "korvattu kommentti");
        this.addData("21", "lisätty kommentti 21:lle");
        /*XmlDocument doc = new XmlDocument();
        XmlElement name = doc.CreateElement("Name");
        name.InnerText = "Patrick Hines";
        doc.AppendChild(name);*//*
        //this.streamWriter = new StreamWriter(filePath, true);
        //this.streamReader = new StreamReader(filePath);
    }*/

    public static void initData()
    {
        XElement data = generateInitData();
        saveData(data);
        Console.WriteLine("data saved");
        addData("A", "lisätty kommentti");
        addData("R", "lisätty r kommentti");
        removeData("B", "yksi kommentti");
        updateData("A", "yksi kommentti", "korvattu kommentti");
        addData("21", "lisätty kommentti 21:lle");
    }

    public static Dictionary<string, List<string>> getCompleteDataAsDictionary()
    {
        Dictionary<string, List<string>> completeData = new Dictionary<string, List<string>>();
        XElement formations = XElement.Load(filePath);
        foreach (var formation in formations.Elements())
        {
            string formationName = (string)formation.Attribute("Name");
            List<string> commentList = new List<string>();
            IEnumerable<XElement> comments = formation.Elements();
            foreach (string comment in comments)
            {
                commentList.Add(comment);
            }
            completeData.Add(formationName, commentList);
        }
        return completeData;
    }

    private static XElement generateInitData()
    {
        XElement formations =
            new XElement("Formations",
                new XElement("Formation", new XAttribute("Name", "A"),
                    new XElement("Comment", "yksi kommentti"),
                    new XElement("Comment", "toinen kommentti"),
                    new XElement("Comment", "kolmas kommentti"),
                    new XElement("Comment", "yksia kommentti"),
                    new XElement("Comment", "yksiaa kommentti")),
                new XElement("Formation", new XAttribute("Name", "B"),
                    new XElement("Comment", "yksi kommentti"),
                    new XElement("Comment", "toinen kommentti"),
                    new XElement("Comment", "yksib kommentti"),
                    new XElement("Comment", "yksibb kommentti")));
        return formations;
    }

    public static void addData(string formation, string comment)
    {
        Console.WriteLine("add data: " + formation);
        XElement fileData = XElement.Load(filePath);
        XElement formationXElement = getFormationData(formation, fileData);
        //var formationXElement = fileData.Elements("Formation")
        //.FirstOrDefault(form => (string)form.Attribute("Name") == formation);
        Console.WriteLine("add data, form element: " + formationXElement);
        if (formationXElement == null)
        {
            Console.WriteLine("add data to non-existing formation: " + formation);
            fileData.Add(new XElement("Formation", new XAttribute("Name", formation),
                new XElement("Comment", comment)
            ));
        }
        else
        {
            Console.WriteLine("add data to existing formation: " + formation);
            formationXElement.Add(new XElement("Comment", comment));
        }
        saveData(fileData);

    }

    public static void removeData(string formation, string commentForRemoval)
    {
        XElement fileData = XElement.Load(filePath);
        XElement formationData = getFormationData(formation, fileData);
        XElement elementToBeRemoved = formationData.Elements("Comment")
            .FirstOrDefault(storedComment => storedComment.Value == commentForRemoval);
        if (elementToBeRemoved != null)
        {
            elementToBeRemoved.Remove();
        }
        else
        {
            Console.WriteLine("Tried to remove non-existing comment");
        }
        saveData(fileData);
    }

    public static void updateData(string formation, string oldComment, string newComment)
    {
        XElement fileData = XElement.Load(filePath);
        XElement formationData = getFormationData(formation, fileData);
        XElement elementToBeUpdated = formationData.Elements("Comment")
            .FirstOrDefault(storedComment => storedComment.Value == oldComment);
        if (elementToBeUpdated != null)
        {
            elementToBeUpdated.ReplaceWith(new XElement("Comment", newComment));
        }
        else
        {
            Console.WriteLine("Tried to remove non-existing comment");
        }
        saveData(fileData);
    }

    /*public void saveCompleteData(Dictionary<string, List<string>> completeData)
    {
        string[] keys = completeData.Keys.ToArray();
        foreach (string key in keys)
        {
            Console.WriteLine("key: " + key);
            List<string> comments = notes[key];
            comments.ForEach(delegate (string comment)
            {
                Console.WriteLine(comment);
            });
        }
    }*/

    private static void saveData(XElement data)
    {
        using (FileStream fs = File.Create(filePath))
        {
            data.Save(fs);
        }
    }

    private static XElement getFormationData(string formation, XElement data)
    {
        return data.Elements("Formation")
            .FirstOrDefault(form => (string)form.Attribute("Name") == formation);
    }

    public static string getData()
    {
        return "foobar";
    }

    public static void writeComment(string comment)
    {
        using (FileStream fs = File.Create(filePath))
        //using (StringWriter stringwriter = new StringWriter())
        //using (StreamWriter sr = new StreamWriter(this.filePath))
        {
            AddText(fs, comment);
        }
    }
    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }
}