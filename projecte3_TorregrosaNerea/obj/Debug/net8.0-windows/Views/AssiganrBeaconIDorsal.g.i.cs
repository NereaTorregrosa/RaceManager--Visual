﻿#pragma checksum "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E25D6E09CE96933FAD59E5209082A60B378078D2"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

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
using projecte3_TorregrosaNerea.Views;


namespace projecte3_TorregrosaNerea.Views {
    
    
    /// <summary>
    /// AssiganrBeaconIDorsal
    /// </summary>
    public partial class AssiganrBeaconIDorsal : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbDorsal;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cboBeaconCodes;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCancel;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGuardar;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/projecte3_TorregrosaNerea;component/views/assiganrbeaconidorsal.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.txbDorsal = ((System.Windows.Controls.TextBox)(target));
            
            #line 32 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
            this.txbDorsal.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txbDorsal_TextChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cboBeaconCodes = ((System.Windows.Controls.ComboBox)(target));
            
            #line 33 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
            this.cboBeaconCodes.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cboBeaconCodes_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnCancel = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
            this.btnCancel.Click += new System.Windows.RoutedEventHandler(this.btnCancel_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnGuardar = ((System.Windows.Controls.Button)(target));
            
            #line 36 "..\..\..\..\Views\AssiganrBeaconIDorsal.xaml"
            this.btnGuardar.Click += new System.Windows.RoutedEventHandler(this.btnGuardar_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

