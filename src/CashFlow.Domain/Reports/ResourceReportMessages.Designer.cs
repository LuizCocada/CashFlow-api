﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CashFlow.Domain.Reports {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceReportMessages {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceReportMessages() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("CashFlow.Domain.Reports.ResourceReportMessages", typeof(ResourceReportMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string TITLE {
            get {
                return ResourceManager.GetString("TITLE", resourceCulture);
            }
        }
        
        public static string AMOUNT {
            get {
                return ResourceManager.GetString("AMOUNT", resourceCulture);
            }
        }
        
        public static string DESCRIPTION {
            get {
                return ResourceManager.GetString("DESCRIPTION", resourceCulture);
            }
        }
        
        public static string PAYMENT_TYPE {
            get {
                return ResourceManager.GetString("PAYMENT_TYPE", resourceCulture);
            }
        }
        
        public static string DATE {
            get {
                return ResourceManager.GetString("DATE", resourceCulture);
            }
        }
        
        public static string EXPENSES_FOR {
            get {
                return ResourceManager.GetString("EXPENSES_FOR", resourceCulture);
            }
        }
        
        public static string TOTAL_SPENT_IN {
            get {
                return ResourceManager.GetString("TOTAL_SPENT_IN", resourceCulture);
            }
        }
    }
}
