using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using LuaInterface;

namespace CruiserConsole
{
    class DataLoader
    {
       
        public void loadGameData(ref Lua lua, string gameDataFilePath)
        {
            //string lineRead;
            //char[] tokenizer = {':', '\"', '\'' };
            //StreamReader SRead = new StreamReader(gameDataFilePath, System.Text.Encoding.UTF8);
            //while ((lineRead = SRead.ReadLine()) != null)
            //{
            //    readLinetoData(ref lua, lineRead, tokenizer);
            //}
            //SRead.Close();
            try
            {
                XmlDocument xmldata = new XmlDocument();
                xmldata.Load(gameDataFilePath);
                XmlElement root = xmldata.DocumentElement;

                XmlNodeList nodes = root.ChildNodes;

                foreach (XmlNode node in nodes)
                {
                    lua[node.Name] = node.InnerText;
                }
            }
            catch(IOException e)
            {
                Console.WriteLine(e);
            }


            Console.WriteLine("Data Successfully Loaded!");
        }
        public void loadScript(GameData gamedata, string gameScriptPath)
        {
            string lineRead;
            string functionName = null;
            string functionExpressions = null;
            int countLine;
            gamedata.script = new Dictionary<string, string>();
            bool readingFunction = false;
            bool readingComment = false;
            string[] filePaths = Directory.GetFiles(gameScriptPath, "*.cps", SearchOption.AllDirectories);
            foreach (string file in filePaths)
            {
                countLine = 0;
                StreamReader SRead = new StreamReader(file, System.Text.Encoding.UTF8);
                while ((lineRead = SRead.ReadLine()) != null)
                {
                    if (lineRead != null) lineRead = lineRead.TrimStart();
                    countLine++;
                    if (readingFunction)
                    {
                        if (lineRead.StartsWith("#end", true, null) && !readingComment)
                        {
                            readingFunction = false;
                            gamedata.script.Add(functionName, functionExpressions);
                            functionName = null;
                            functionExpressions = null;
                        }
                        else if (lineRead.StartsWith("#function", true, null))
                        {
                            throw new CruiserScriptLoadException("Could not find #end but found new #function at line " + countLine + " of file " + file);
                        }
                        else if (lineRead.StartsWith(@"//", true, null))
                        {
                        }
                        else if (lineRead.StartsWith(@"/*", true, null))
                        {
                            readingComment = true;
                        }
                        else if (lineRead.EndsWith(@"*/", true, null))
                        {
                            readingComment = false;
                        }
                        else if (readingComment)
                        {
                        }
                        else
                        {
                            functionExpressions += (lineRead + "\n");
                        }

                    }
                    else
                    {
                        if (lineRead.StartsWith("#function", true, null))
                        {
                            functionName = lineRead.Replace("#function ", "").ToLower();
                            functionName.Trim();
                            readingFunction = true;
                        }
                    }
                }
                if (readingFunction)
                {
                    throw new CruiserScriptLoadException("Could not find #end but reached EOF at line " + countLine + " of file " + file);
                }
                SRead.Close();
            }
        }

        //public void readLinetoData(ref Lua lua, string lineRead, char[] tokenizer)
        //{
        //    string[] tokens = lineRead.Split(tokenizer);

        //    string name = tokens[0].Trim();
        //    string expression = null;
        //    if (tokens.Length > 1) expression = tokens[1].Trim();

        //    if (name.Equals("")) { }
        //    else { lua[name] = expression; }
        //    else if (name.ToLower().Equals("gamename")) lua["gamename"] = expression;
        //    else if (name.ToLower().Equals("money")) lua["money"] = Convert.ToInt32(expression);
        //    else if (name.ToLower().Equals("byss")) lua["byss"] = Convert.ToInt32(expression);
        //    else if (name.ToLower().Equals("date")) lua["date"] = Convert.ToInt32(expression);
        //    else if (name.ToLower().Equals("phase")) lua["phase"] = Convert.ToInt32(expression);

        //    else if (name.ToUpper().Equals("MAXMONEY")) lua["maxmoney"] = Convert.ToInt32(expression);
        //    else if (name.ToUpper().Equals("MAXBYSS")) lua["maxbyss"] = Convert.ToInt32(expression);
        //    else if (name.ToUpper().Equals("MAXDATE")) lua["maxdate"] = Convert.ToInt32(expression);
        //    else if (name.ToUpper().Equals("MAXPHASE")) lua["maxphase"] = Convert.ToInt32(expression);
        //}
    }
    public class CruiserScriptLoadException : Exception
    {
        public CruiserScriptLoadException()
        {
        }
        public CruiserScriptLoadException(string message)
            : base(message)
        {
        }
        public CruiserScriptLoadException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
