﻿#pragma checksum "..\..\..\..\Views\LiveResults.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "B12F22E6A0EE68BE8642F2FF6FEF30E49D7BEA8A"
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
    /// LiveResults
    /// </summary>
    public partial class LiveResults : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 22 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbFiltreNom;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txbFiltreDorsal;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFiltrar;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnNetejarFiltre;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgResults;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdDetallRegistre;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txbNomParticipant;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txbDorsalParticipant;
        
        #line default
        #line hidden
        
        
        #line 83 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lsvCheckpoints;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\Views\LiveResults.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnTornarAConsultarCurses;
        
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
            System.Uri resourceLocater = new System.Uri("/projecte3_TorregrosaNerea;component/views/liveresults.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\LiveResults.xaml"
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
            this.txbFiltreNom = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.txbFiltreDorsal = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.btnFiltrar = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\Views\LiveResults.xaml"
            this.btnFiltrar.Click += new System.Windows.RoutedEventHandler(this.btnFiltrar_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnNetejarFiltre = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\..\..\Views\LiveResults.xaml"
            this.btnNetejarFiltre.Click += new System.Windows.RoutedEventHandler(this.btnNetejarFiltre_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dgResults = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.grdDetallRegistre = ((System.Windows.Controls.Grid)(target));
            return;
            case 9:
            this.txbNomParticipant = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 10:
            this.txbDorsalParticipant = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.lsvCheckpoints = ((System.Windows.Controls.ListView)(target));
            return;
            case 12:
            this.btnTornarAConsultarCurses = ((System.Windows.Controls.Button)(target));
            
            #line 94 "..\..\..\..\Views\LiveResults.xaml"
            this.btnTornarAConsultarCurses.Click += new System.Windows.RoutedEventHandler(this.btnTornarAConsultarCurses_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 42 "..\..\..\..\Views\LiveResults.xaml"
            ((System.Windows.Controls.StackPanel)(target)).Loaded += new System.Windows.RoutedEventHandler(this.StackPanel_Loaded);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 54 "..\..\..\..\Views\LiveResults.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnViewDetail_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

