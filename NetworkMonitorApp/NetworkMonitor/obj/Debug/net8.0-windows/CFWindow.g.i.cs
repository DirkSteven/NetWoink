﻿#pragma checksum "..\..\..\CFWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C17D5611F51E13904D0C0A690D991C1EA0CA13A2"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using NetworkMonitor;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace NetworkMonitor {
    
    
    /// <summary>
    /// CFWindow
    /// </summary>
    public partial class CFWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 72 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label ipLbl;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label macLbl;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label devtypeLbl;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label runtimeLbl;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label rateLbl;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label annualCFLbl;
        
        #line default
        #line hidden
        
        
        #line 105 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label dailyCFLbl;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label totalCFLbl;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\CFWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CFBackBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/NetworkMonitor;component/cfwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\CFWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.1.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ipLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.macLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.devtypeLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.runtimeLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.rateLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.annualCFLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 7:
            this.dailyCFLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.totalCFLbl = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.CFBackBtn = ((System.Windows.Controls.Button)(target));
            
            #line 111 "..\..\..\CFWindow.xaml"
            this.CFBackBtn.Click += new System.Windows.RoutedEventHandler(this.CFBackBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

