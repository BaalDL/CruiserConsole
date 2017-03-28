using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NLua;
using NLua.Exceptions;
using Myevan;

namespace CruiserConsole
{
    class CruiserGame
    {
        public GameData gameData;
        public bool isGameShut = false;
        public static Lua lua;
        public static Hashtable luaFunctions = null;
        public static string foreground;
        public static string background;
        public enum colorCodes 
        {
            K, B, G, C, R, M, Y, w, k, b, g, c, r, m, y, W
        };
        public static string[] colorTokens = {"/fK","/fB","/fG","/fC","/fR","/fM","/fY","/fw","/fk","/fb","/fg","/fc","/fr","/fm","/fy","/fW",
                                  "/bK","/bB","/bG","/bC","/bR","/bM","/bY","/bw","/bk","/bb","/bg","/bc","/br","/bm","/by","/bW",
                                  "/fx","/fX","/bx","/bX"};
        public static string colorPattern = "(/[fb][KBGCRMYwkbgcrmyWxX]|/x|/X)";
        public void initialize()
        {
            lua = new Lua();
            luaFunctions = new Hashtable();
            registerLuaFunctions(this);
            try
            {
                Console.OutputEncoding = Encoding.GetEncoding(949);
                Console.WriteLine("Loading Games with " + lua["_VERSION"] + "...");
                gameData = new GameData("scripts\\gamedata.xml", "scripts\\", ref lua);
            }
            catch (CruiserGameShutException e)
            {
                Console.WriteLine(e);
                isGameShut = true;
            }
        }
        public void startGame()
        {
            if (!isGameShut)
            {
                //lua["Csharp1"] = "String 문자열" as string;
                //lua.DoString(System.Text.Encoding.GetEncoding("euc-kr").GetBytes("Csharp2 = 'String 문자열 abc'"));
                ////lua.DoString("Csharp2 = 'String 문자열 abc'");
                //string text = "String 문자열";
                //Console.WriteLine("Csharp1: Assignment");
                //Console.WriteLine("Csharp2: DoString");
                //Console.WriteLine("C# Csharp1: String 문자열 == " + lua["Csharp1"] as string + " -> " + (text.Equals(lua["Csharp1"] as string)).ToString()); // True
                //Console.WriteLine("C# Csharp2: String 문자열 == " + lua["Csharp2"] as string + " -> " + (text.Equals(lua["Csharp2"] as string)).ToString()); // False

                //Console.WriteLine(Console.InputEncoding);
                //Console.WriteLine(Console.OutputEncoding);
                ////Console.WriteLine(System.Text.Encoding.UTF8.GetString(System.Text.Encoding.GetEncoding("euc-kr").GetBytes(lua["Csharp2"] as string))); // False
                //Console.WriteLine(lua["Csharp2"] as string); // False

                //lua.DoString("print('Lua Csharp1 = ' .. Csharp1)");
                //lua.DoString("print('Lua Csharp2 = ' .. Csharp2)");
                executeFunction("startgame");
            }
        }

        [AttrLuaFunc("quit", "Exit the Program.")]
        public void quit()
        {
            isGameShut = true;
        }
        [AttrLuaFunc("execute", "Executing Functions", "name")]
        public void executeFunction(String name)
        {
            try
            {
                lua.DoString(System.Text.Encoding.GetEncoding(949).GetBytes(gameData.script[name].ToString()));
                //lua.DoString(System.Text.Encoding.UTF8.GetBytes(gameData.script[name].ToString()));
                //lua.DoString(gameData.script[name].ToString());
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine(e);
                isGameShut = true;
            }
            catch (LuaException e)
            {
                Console.WriteLine("An unhandled exception occured at string chunk " + name + ". Please check the script.");
                Console.WriteLine(e);
                isGameShut = true;
            };
        }
        [AttrLuaFunc("printl", "Print string and New Line", "string")]
        public void printLine(String message)
        {
            CruiserGame.WriteLine(message);
        }
        [AttrLuaFunc("_print", "Print string", "string")]
        public void print(String message)
        {
            CruiserGame.Write(message);
        }
        [AttrLuaFunc("printlw", "Print string and New Line and Wait", "string")]
        public void printLineWait(String message)
        {
            printLine(message);
            Console.ReadKey(true);
        }
        [AttrLuaFunc("printw", "Print string and Wait", "string")]
        public void printWait(String message)
        {
            print(message);
            Console.ReadKey(true);
        }
        [AttrLuaFunc("say_depricate", "Print character's dialogue", "string", "string")]
        public void characterSay(String actor, String dialogue)
        {
            printLine("<" + actor + "> \"" + dialogue + "\"");
        }
        [AttrLuaFunc("sayw_depricate", "Print character's dialogue", "string", "string")]
        public void characterSayWait(String actor, String dialogue)
        {
            printLineWait("<" + actor + "> \"" + dialogue + "\"");
        }
        [AttrLuaFunc("inputl", "Get input", "string")]
        public void inputLine(string varName)
        {
            print("> ");
            lua[varName] = Console.ReadLine();
        }
        [AttrLuaFunc("inputq", "Print string and get answer", "string", "string")]
        public void inputQuestion(string message, string varName)
        {
            printLine(message);
            inputLine(varName);
        }
        [AttrLuaFunc("moveto", "set the position of the cursor", "column number", "row number")]
        public void moveCursor(int row, int column)
        {
            Console.SetCursorPosition(column, row);
        }
        [AttrLuaFunc("movev", "change the vertical position of the cursor", "row number")]
        public void moveCursorVertical(int row)
        {
            Console.SetCursorPosition(Console.CursorLeft, row);
        }
        [AttrLuaFunc("moveh", "change the horizontal position of the cursor", "column number")]
        public void moveCursorHorizontal(int column)
        {
            Console.SetCursorPosition(column, Console.CursorTop);
        }
        [AttrLuaFunc("moveu", "change the vertical position of the cursor", "row number")]
        public void moveCursorUp(int row)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - row);
        }
        [AttrLuaFunc("moved", "change the vertical position of the cursor","row number")]
        public void moveCursorDown(int row)
        {
            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + row);
        }
        [AttrLuaFunc("movel", "change the horizontal position of the cursor", "column number")]
        public void moveCursorLeft(int column)
        {
            Console.SetCursorPosition(Console.CursorLeft - column, Console.CursorTop);
        }
        [AttrLuaFunc("mover", "change the horizontal position of the cursor", "column number")]
        public void moveCursorRight(int column)
        {
            Console.SetCursorPosition(Console.CursorLeft + column, Console.CursorTop);
        }
        [AttrLuaFunc("getcl", "get current left position of the cursor")]
        public int getCursorLeft()
        {
            return Console.CursorLeft;
        }
        [AttrLuaFunc("getct", "get current left position of the cursor")]
        public int getCursorTop()
        {
            return Console.CursorTop;
        }
        [AttrLuaFunc("_drawLine", "Draw string and get cursor back to original position", "string", "row number")]
        public void drawLine(string line, int row)
        {
            int r = getCursorTop();
            int c = getCursorLeft();
            moveCursor(row, 0);
            print(line);
            moveCursor(r, c);
        }
        [AttrLuaFunc("_draw", "Draw string and get cursor back to original position", "string", "row number", "column number")]
        public void draw(string line, int row, int column)
        {
            int r = getCursorTop();
            int c = getCursorLeft();
            moveCursor(row, column);
            print(line);
            moveCursor(r, c);
        }
        [AttrLuaFunc("_setwindowsize", "set the size of console window", "width", "height")]
        public void setWindowSize(int width, int height)
        {
            Console.SetWindowSize(width, height);
        }
        [AttrLuaFunc("_setwindowposition", "set the size of console window", "width", "height")]
        public void setWindowPosition(int top, int left)
        {
            Console.SetWindowPosition(left, top);
        }
        [AttrLuaFunc("_setbuffersize", "set the size of buffer", "width", "height")]
        public void setConsolebufferSize(int width, int height)
        {
            Console.SetBufferSize(width, height);
        }
        [AttrLuaFunc("_setdefaultcolor", "set default fg/bg color", "foreground", "background")]
        public void setDefaultColor(string foreground, string background)
        {
            CruiserGame.foreground = foreground;
            CruiserGame.background = background;
        }

        [AttrLuaFunc("setwindowname", "set the name of window", "name")]
        public void setWindowName(string name)
        {
            Console.Title = name;
        }


        public static void Write(String message)
        {
            String[] words = Regex.Split(message, CruiserGame.colorPattern);
            foreach (String word in words) {
                if (Regex.IsMatch(word, CruiserGame.colorPattern))
                {
                    if (word.StartsWith("/f"))
                    {
                        if (word.EndsWith("x") || word.EndsWith("X"))
                        {
                            Console.ForegroundColor = (System.ConsoleColor)Enum.Parse(typeof(colorCodes), foreground);
                        }

                        Console.ForegroundColor = (System.ConsoleColor)(int)Enum.Parse(typeof(colorCodes), word.Substring(2));
                    }
                    else if (word.StartsWith("/b"))
                    {
                        if (word.EndsWith("x") || word.EndsWith("X"))
                        {
                            Console.BackgroundColor = (System.ConsoleColor)Enum.Parse(typeof(colorCodes), background);
                        }
                        Console.BackgroundColor = (System.ConsoleColor)(int)Enum.Parse(typeof(colorCodes), word.Substring(2));
                    }
                    else
                    {
                        Console.ForegroundColor = (System.ConsoleColor)Enum.Parse(typeof(colorCodes), foreground);
                        Console.BackgroundColor = (System.ConsoleColor)Enum.Parse(typeof(colorCodes), background);
                    }
                }
                else
                {
                    Console.Write(Korean.ReplaceJosa(word));
                }
            }
        }

        public static void WriteLine(String message)
        {
            Write(message + "\n");
        }

        public static void registerLuaFunctions(Object pTarget)
        {
            // Sanity checks
            if (lua == null || luaFunctions == null)
                return;

            // Get the target type
            Type targetType = pTarget.GetType();

            // ... and simply iterate through all it's methods
            foreach (MethodInfo mInfo in targetType.GetMethods())
            {
                // ... then through all this method's attributes
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo))
                {
                    // and if they happen to be one of our AttrLuaFunc attributes
                    if (attr.GetType() == typeof(AttrLuaFunc))
                    {
                        AttrLuaFunc pAttr = (AttrLuaFunc)attr;
                        Hashtable pParams = new Hashtable();

                        // Get the desired function name and doc string, along with parameter info
                        String strFName = pAttr.getFuncName();
                        String strFDoc = pAttr.getFuncDoc();
                        String[] pPrmDocs = pAttr.getFuncParams();

                        // Now get the expected parameters from the MethodInfo object
                        ParameterInfo[] pPrmInfo = mInfo.GetParameters();

                        // If they don't match, someone forgot to add some documentation to the
                        // attribute, complain and go to the next method
                        if (pPrmDocs != null && (pPrmInfo.Length != pPrmDocs.Length))
                        {
                            Console.WriteLine("Function " + mInfo.Name + " (exported as " +
                                              strFName + ") argument number mismatch. Declared " +
                                              pPrmDocs.Length + " but requires " +
                                              pPrmInfo.Length + ".");
                            break;
                        }

                        // Build a parameter <-> parameter doc hashtable
                        for (int i = 0; i < pPrmInfo.Length; i++)
                        {
                            pParams.Add(pPrmInfo[i].Name, pPrmDocs[i]);
                        }

                        // Get a new function descriptor from this information
                        LuaFuncDescriptor pDesc = new LuaFuncDescriptor(strFName, strFDoc, pParams);

                        // Add it to the global hashtable
                        luaFunctions.Add(strFName, pDesc);

                        // And tell the VM to register it.
                        lua.RegisterFunction(strFName, pTarget, mInfo);
                    }
                }
            }
        }
    }
    public class CruiserGameShutException : Exception
    {
        public CruiserGameShutException()
        {
        }
        public CruiserGameShutException(string message)
            : base(message)
        {
        }
        public CruiserGameShutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
