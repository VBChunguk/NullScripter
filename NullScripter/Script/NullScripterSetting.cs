using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml;

namespace NullScripter.Script
{
    public class NullScripterSetting
    {
        #region Declarement
        public Font font;
        public const string SettingVersion = "alpha.1.03";
        #endregion

        public NullScripterSetting()
        {
            ReadSetting();
        }
        public void ReadSetting()
        {
            
            #region Exception Check
            if (!File.Exists("Setting.set"))
                CreateSetting();
            #endregion

            #region Read .set File
            Debugger.WriteLine("Opening Setting File");
            using (XmlReader xr = XmlReader.Create("Setting.set"))
            {
                xr.ReadToFollowing("NullScripter");
                xr.MoveToFirstAttribute();
                Debugger.WriteLine("Setting Version " + xr.Value);
                if (xr.Value != SettingVersion)
                {
                    Debugger.WriteLine("Setting File is Out of date");
                    xr.Close();
                    CreateSetting();
                    ReadSetting();
                    return;
                }

                xr.ReadToFollowing("Font");
                xr.MoveToFirstAttribute();
                string fontname = xr.Value;
                xr.MoveToNextAttribute();
                string fontsize = xr.Value;
                this.font = new Font(fontname, float.Parse(fontsize));

                Debugger.WriteLine("Font : " + font.Name + ", " + font.Size.ToString());
            }
            Debugger.CarriageReturn();
            #endregion
        }

        public static void CreateSetting()
        {
            #region Create Default .set File
            Font font = new Font("Courier New", 10);
            CreateSetting(font);
            #endregion
        }
        public static void CreateSetting(Font font)
        {
            #region Create .set File
            XmlWriterSettings xmlsetting = new XmlWriterSettings();
            xmlsetting.Indent = true;
            xmlsetting.IndentChars = "\t";
            xmlsetting.CloseOutput = true;
            using (XmlWriter xw = XmlWriter.Create(File.Create("Setting.set"), xmlsetting))
            {
                xw.WriteStartElement("NullScripter");
                xw.WriteAttributeString("Version", SettingVersion.ToString());

                xw.WriteStartElement("Font");
                xw.WriteAttributeString("Name", font.Name);
                xw.WriteAttributeString("Size", font.Size.ToString());
                
                xw.Flush();
                xw.Close();
            }
            #endregion
        }

        #region Change Setting
        public void ChangeSetting(Font font)
        {
            CreateSetting(font);

            this.font = font;
        }
        #endregion
    }
}
