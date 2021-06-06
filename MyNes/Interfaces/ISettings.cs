// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MyNes.Core
{
    public abstract class ISettings
    {
        public ISettings(string filePath)
        {
            this.filePath = filePath;
        }
        protected string filePath;
        protected FieldInfo[] Fields;

        public virtual void LoadSettings()
        {
            Fields = this.GetType().GetFields();
            string[] readLines;
            if (File.Exists(filePath))
            {
                readLines = File.ReadAllLines(filePath);

                for (int i = 0; i < readLines.Length; i++)
                {
                    string[] codes = readLines[i].Split('=');
                    if (codes != null)
                    {
                        if (codes.Length == 2)
                        {
                            //Console.WriteLine("Setting: " + codes[0] + "=" + codes[1]);
                            SetField(codes[0], codes[1]);
                        }
                    }
                }
            }
        }
        public virtual void SaveSettings()
        {
            Fields = GetType().GetFields();
            List<string> lines = new List<string>();
            foreach (FieldInfo inf in Fields)
            {
                if (inf.IsPublic)
                    lines.Add(inf.Name + "=" + GetFieldValue(inf));
            }

            File.WriteAllLines(filePath, lines.ToArray());
        }
        protected virtual void SetField(string fieldName, string val)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i].Name == fieldName)
                {
                    if (Fields[i].FieldType == typeof(String))
                    {
                        Fields[i].SetValue(this, val);
                    }
                    else if (Fields[i].FieldType == typeof(Boolean))
                    {
                        Fields[i].SetValue(this, val == "1");
                    }
                    else if (Fields[i].FieldType == typeof(Int32))
                    {
                        int num = 0;
                        if (int.TryParse(val, out num))
                            Fields[i].SetValue(this, num);
                    }
                    else if (Fields[i].FieldType == typeof(Single))
                    {
                        float num = 0;
                        if (float.TryParse(val, out num))
                            Fields[i].SetValue(this, num);
                    }
                    else if (Fields[i].FieldType == typeof(System.String[]))
                    {
                        string[] codes = val.Split(new string[] { "*" }, StringSplitOptions.RemoveEmptyEntries);
                        Fields[i].SetValue(this, codes);
                    }
                    else
                    {
                        Tracer.WriteLine("Unknown setting type = " + Fields[i].FieldType);
                    }
                    break;
                }
            }
        }
        protected virtual string GetFieldValue(string fieldName)
        {
            for (int i = 0; i < Fields.Length; i++)
            {
                if (Fields[i].Name == fieldName)
                {
                    return GetFieldValue(Fields[i]);
                }
            }
            return "";
        }
        protected virtual string GetFieldValue(FieldInfo field)
        {
            object val = field.GetValue(this);
            if (field.FieldType == typeof(String))
            {
                return val.ToString();
            }
            else if (field.FieldType == typeof(Boolean))
            {
                return (bool)val ? "1" : "0";
            }
            else if (field.FieldType == typeof(Int32))
            {
                return val.ToString();
            }
            else if (field.FieldType == typeof(Single))
            {
                return val.ToString();
            }
            else if (field.FieldType == typeof(System.String[]))
            {
                string val_stt = "";
                string[] its = (string[])val;
                if (its != null)
                {
                    foreach (string st in its)
                    {
                        val_stt += st + "*";
                    }
                }
                if (val_stt.Length > 0)
                    return val_stt.Substring(0, val_stt.Length - 1);
                else
                    return "";
            }
            else
            {
                return "";
            }
        }
    }
}
