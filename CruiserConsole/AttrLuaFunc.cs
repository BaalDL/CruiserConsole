﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CruiserConsole
{
    // http://www.gamedev.net/page/resources/_/technical/game-programming/using-lua-with-c-r2275
    class AttrLuaFunc : Attribute
    {
        private String FunctionName;
        private String FunctionDoc;
        private String[] FunctionParameters = null;

        public AttrLuaFunc(String strFuncName, String strFuncDoc, params String[] strParamDocs)
        {
            FunctionName = strFuncName;
            FunctionDoc = strFuncDoc;
            FunctionParameters = strParamDocs;
        }

        public AttrLuaFunc(String strFuncName, String strFuncDoc)
        {
            FunctionName = strFuncName;
            FunctionDoc = strFuncDoc;
        }

        public String getFuncName()
        {
            return FunctionName;
        }

        public String getFuncDoc()
        {
            return FunctionDoc;
        }

        public String[] getFuncParams()
        {
            return FunctionParameters;
        }
    }
}
