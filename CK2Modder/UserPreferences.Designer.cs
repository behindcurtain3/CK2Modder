﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CK2Modder {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class UserPreferences : global::System.Configuration.ApplicationSettingsBase {
        
        private static UserPreferences defaultInstance = ((UserPreferences)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new UserPreferences())));
        
        public static UserPreferences Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\\\Program Files\\\\Steam\\\\steamapps\\\\common\\\\crusader kings ii")]
        public string WorkingLocation {
            get {
                return ((string)(this["WorkingLocation"]));
            }
            set {
                this["WorkingLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastMod {
            get {
                return ((string)(this["LastMod"]));
            }
            set {
                this["LastMod"] = value;
            }
        }
    }
}
