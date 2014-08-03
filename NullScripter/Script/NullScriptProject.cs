using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace NullScripter.Script
{
    class NullScriptProject
    {
        #region Declearing Variables
        public string path;
        public string ProjectName;
        #endregion

        public NullScriptProject (string filepath)
        {
            #region Read .nsp File
            StreamReader sr = new StreamReader(filepath);
            this.path = Path.GetDirectoryName(filepath);

            using (XmlReader xr = XmlReader.Create(new StringReader(sr.ReadToEnd())))
            { 
                xr.ReadToFollowing("Project");                
                xr.MoveToFirstAttribute();
                this.ProjectName = xr.Value;
            }
            #endregion
        }
    }

    class NullScriptProjectException : Exception
    {
        public ErrorType errortype;
        public enum ErrorType { Invalid_Project };
        public NullScriptProjectException(ErrorType ne)
        {
            this.errortype = ne;
        }
    }
}
