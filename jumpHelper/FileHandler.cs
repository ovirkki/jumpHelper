using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace jumpHelper
{
    public static class FileHandler
    {
        private const string FILE_NAME = "FSNotes.xml";
        //private static string filePath;
        //private static string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        private static string filePath;// = Path.Combine(path, FILE_NAME);

        public static void initialize(string appFilepath)
        {
            filePath = Path.Combine(appFilepath, FILE_NAME);
        }

        public async static Task<Dictionary<string, List<string>>> getCompleteDataAsDictionary()
        {
            Dictionary<string, List<string>> completeData = new Dictionary<string, List<string>>();
            XElement formations = await loadDataAsync();

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

        public async static Task addDataAsync(string formation, string comment)
        {
            Console.WriteLine("add data: " + formation);
            Console.WriteLine("dir: " + filePath);
            XElement fileData = await loadDataAsync();
            XElement formationXElement = await getFormationDataAsync(formation, fileData);
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
            await saveDataAsync(fileData);
            AppEventHandler.emitInfoTextUpdate("Data saved (" + formation + ")");
        }

        public async static Task removeDataAsync(string formation, string commentForRemoval)
        {
            XElement fileData = await loadDataAsync();
            XElement formationData = await getFormationDataAsync(formation, fileData);
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
            await saveDataAsync(fileData);
        }

        public async static void updateData(string formation, string oldComment, string newComment)
        {
            XElement fileData = await loadDataAsync();
            XElement formationData = await getFormationDataAsync(formation, fileData);
            XElement elementToBeUpdated = formationData.Elements("Comment")
                .FirstOrDefault(storedComment => storedComment.Value == oldComment);
            if (elementToBeUpdated != null)
            {
                elementToBeUpdated.ReplaceWith(new XElement("Comment", newComment));
            }
            else
            {
                Console.WriteLine("Tried to update non-existing comment");
            }
            await saveDataAsync(fileData);
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

        private static void saveDataWithFileStream(XElement data)
        {
            using (FileStream fs = File.Create(filePath))
            {
                data.Save(fs);
            }
        }

        private async static Task saveDataAsync(XElement data)
        {
            try
            {

                await Task.Run(() => data.Save(filePath));
            }
            catch (FileNotFoundException e)
            {
                await Task.Run(() => saveDataWithFileStream(data));

            }
            /*using (FileStream fs = File.Create(filePath))
            {
                data.Save(fs);
            }*/
        }

        private async static Task<XElement> loadDataAsync()
        {
            try
            {
                return await Task.Run(() => XElement.Load(filePath));
            }
            catch
            {
                return await Task.Run(() => new XElement("Formations"));
            }
        }

        private static XElement getFormationData(string formation, XElement data)
        {
            return data.Elements("Formation")
                .FirstOrDefault(form => (string)form.Attribute("Name") == formation);
        }

        private async static Task<XElement> getFormationDataAsync(string formation, XElement data)
        {
            return await Task.Run(() =>
            {
                return data.Elements("Formation")
                    .FirstOrDefault(form => (string)form.Attribute("Name") == formation);
            });
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
}