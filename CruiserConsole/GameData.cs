using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLua;

namespace CruiserConsole
{
    class GameData
    {
        public Dictionary<string, string> script;

        public bool isDataLoaded = false;

        public GameData()
        {
            this.script = new Dictionary<string,string>();
        }

        public GameData(string gameDataFilePath, string gameScriptPath, ref Lua lua)
        {
            DataLoader loader = new DataLoader();
            try
            {
                loader.loadGameData(ref lua, gameDataFilePath);
            }
            catch
            {
                Console.WriteLine("{0}을(를) 읽던 도중 오류가 발생했습니다.", gameDataFilePath);
                Console.ReadLine();
                throw new CruiserGameShutException("파일을 읽던 도중 오류가 발생했습니다.");
            }
            try
            {
                loader.loadScript(this, gameScriptPath);
            }
            catch(CruiserScriptLoadException e)
            {
                Console.WriteLine(e);
                throw new CruiserGameShutException("스크립트를 읽던 도중 오류가 발생했습니다.");
            }

        }
    }

    class Hero
    {
    }
    class Member
    {
    }
    class Event
    {
    }
}
