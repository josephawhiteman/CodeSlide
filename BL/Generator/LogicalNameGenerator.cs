using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.BL.Generator
{
    class LogicalNameGenerator
    {
        private string _LogicalEntityNameRegex;
        public string LogicalEntityNameRegex
        {
            get
            {
                return _LogicalEntityNameRegex;
            }
            set
            {
                _LogicalEntityNameRegex = value;
            }
        }
        string[] nums = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
        public void GenerateLogicalName(Entity ent)
        {
            string text = ent.DBName;
            string pat = @"(?<1>[a-zA-Z0-9]+)_(?<2>[a-zA-Z0-9]+)_(?<3>\w+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(text);
            if (m.Success)
            {
                int tryNum = 0;
                string grp = m.Groups[1].Value;
                if (char.IsNumber(grp, 0))
                {
                    int x = Convert.ToInt32(char.GetNumericValue(grp, 0));
                    grp = nums[x][0] + grp;
                }
                if (grp != "") ent.LogicalPackage = grp;
                grp = m.Groups[2].Value;
                if (char.IsNumber(grp, 0))
                {
                    int x = Convert.ToInt32(char.GetNumericValue(grp, 0));
                    grp = nums[x][0] + grp;
                }
                if (grp != "") ent.LogicalModule = grp;
                grp = m.Groups[3].Value;
                if (char.IsNumber(grp, 0))
                {
                    int x = Convert.ToInt32(char.GetNumericValue(grp, 0));
                    grp = nums[x][0] + grp;
                }
                if (grp != "") ent.LogicalName = grp;

                m = m.NextMatch();
            }
        }
    }
}
