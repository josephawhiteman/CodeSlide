using System;
using System.Collections.Generic;
using System.Text;
using CodeGenerator.BL.DBReader;
using CodeGenerator.BL.Modeler;

namespace CodeGenerator.BL.Generator
{
    public enum CodeLanguage
    {
        CSharp,
        PHP,
        Java,
        Python,
        CPlusPlus,
        ObjectiveC,
        TSQL,


    }
    public abstract class LanguageGenerator
    {

        private string _TargetDataAdapterPath;
        public string TargetDataAdapterPath
        {
            get
            {
                return _TargetDataAdapterPath;
            }
            set
            {
                _TargetDataAdapterPath = value;
            }
        }

        public string _TargetObjectPath;
        public string TargetObjectPath
        {
            get
            {
                return _TargetObjectPath;
            }
            set
            {
                _TargetObjectPath = value;
            }
        }
        private bool _UtilizesDataSet;
        public bool UtilizesDataSet
        {
            get
            {
                return _UtilizesDataSet;
            }
            set
            {
                _UtilizesDataSet = value;
            }
        }


        public static LanguageGenerator CodeGenerator(CodeLanguage lang)
        {
            LanguageGenerator gen = null;
            switch (lang)
            {
                case CodeLanguage.CSharp:

                    gen = new CSharpGenerator();
                    break;
                case CodeLanguage.PHP:

                    gen = new PHPGenerator();
                    break;
            }
            return gen;
        }

        public virtual int SupportsNullablePrimitive
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        public virtual string CodeFileExtension
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public abstract BaseFieldType GetCodeType(BaseDBFieldTypeMapper dbType);
    }
}
