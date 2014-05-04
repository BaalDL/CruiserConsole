using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CruiserConsole
{
    public class LuaFuncDescriptor
    {
        private String FunctionName;
        private String FunctionDoc;
        private ArrayList FunctionParameters;
        private ArrayList FunctionParamDocs;
        private String FunctionDocString;

        public LuaFuncDescriptor(String strFuncName, String strFuncDoc, ArrayList strParams,
                                 ArrayList strParamDocs)
        {
            FunctionName = strFuncName;
            FunctionDoc = strFuncDoc;
            FunctionParameters = strParams;
            FunctionParamDocs = strParamDocs;

            String strFuncHeader = strFuncName + "(%params%) - " + strFuncDoc;
            String strFuncBody = "\n\n";
            String strFuncParams = "";

            Boolean bFirst = true;

            for (int i = 0; i < strParams.Count; i++)
            {
                if (!bFirst)
                    strFuncParams += ", ";

                strFuncParams += strParams[i];
                strFuncBody += "\t" + strParams[i] + "\t\t" + strParamDocs[i] + "\n";

                bFirst = false;
            }

            strFuncBody = strFuncBody.Substring(0, strFuncBody.Length - 1);
            if (bFirst)
                strFuncBody = strFuncBody.Substring(0, strFuncBody.Length - 1);

            FunctionDocString = strFuncHeader.Replace("%params%", strFuncParams) + strFuncBody;
        }
        public LuaFuncDescriptor(String strFuncName, String strFuncDoc, Hashtable pParams)
        {
            FunctionName = strFuncName;
            FunctionDoc = strFuncDoc;
            FunctionParameters = new ArrayList(pParams.Keys);
            FunctionParamDocs = new ArrayList(pParams.Values);
        }

        public String getFuncName()
        {
            return FunctionName;
        }

        public String getFuncDoc()
        {
            return FunctionDoc;
        }

        public ArrayList getFuncParams()
        {
            return FunctionParameters;
        }

        public ArrayList getFuncParamDocs()
        {
            return FunctionParamDocs;
        }

        public String getFuncHeader()
        {
            if (FunctionDocString.IndexOf("\n") == -1)
                return FunctionDocString;

            return FunctionDocString.Substring(0, FunctionDocString.IndexOf("\n"));
        }

        public String getFuncFullDoc()
        {
            return FunctionDocString;
        }
    }
}
