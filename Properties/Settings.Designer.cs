﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3620
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeGenerator.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "8.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ConnectionString")]
        public string ConnectionString {
            get {
                return ((string)(this["ConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ALLYSON\\sqlexpress;Initial Catalog=MenuMaster;Integrated Security=Tru" +
            "e")]
        public string MenuMasterConnectionString {
            get {
                return ((string)(this["MenuMasterConnectionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.ConnectionString)]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=ALLYSON\\sqlexpress;Initial Catalog=MenuMaster;Integrated Security=Tru" +
            "e")]
        public string MenuMasterConnectionString1 {
            get {
                return ((string)(this["MenuMasterConnectionString1"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>Data Source=ALLYSON\sqlexpress;Initial Catalog=testcs;Integrated Security=True</string>
  <string>Data Source=enkonvic-dev2\sql2008;Initial Catalog=Amtrak_Next_gen;user id=sa;password=SAFu11@cce$$</string>
  <string>Data Source=ALLYSON\sqlexpress;Initial Catalog=menumaster;Integrated Security=True</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection DBConnections {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["DBConnections"]));
            }
        }
    }
}
