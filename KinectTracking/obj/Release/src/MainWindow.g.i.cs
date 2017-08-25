﻿#pragma checksum "..\..\..\src\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "80D4E74E4BA17C77DAC715B23533F2FF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using KinectTracking;
using Microsoft.Expression.Controls;
using Microsoft.Expression.Media;
using Microsoft.Expression.Shapes;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


namespace KinectTracking {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 43 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image camera;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas canvas;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas topProjectionCanvas;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas sideProjectionCanvas;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas Buttoncanvas;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button record_button;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button stop_record_button;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlockFrameNr;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textBlock;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label label;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label markerCount;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label recognised_gesture;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\src\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Ellipse recordBlinker;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/KinectSample1;component/src/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\src\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 18 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Open_Log);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 20 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Open_Log_Folder);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 21 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.openCoordLog);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 22 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenOrientationLog);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 24 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Close_app);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 27 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ToggleGestureRecognition);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 28 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ToggleColorCamera);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 29 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Replay_Button_DrawBuffer);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 32 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.extract_marked_gestures_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 33 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.calc_bezier_button_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 35 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.calc_distances_button_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 36 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.calc_distances_from_buffer_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 38 "..\..\..\src\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Set_marker_count_click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.camera = ((System.Windows.Controls.Image)(target));
            return;
            case 15:
            this.canvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 16:
            this.topProjectionCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 17:
            this.sideProjectionCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 18:
            this.Buttoncanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 19:
            this.record_button = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\src\MainWindow.xaml"
            this.record_button.Click += new System.Windows.RoutedEventHandler(this.record_button_Click);
            
            #line default
            #line hidden
            return;
            case 20:
            this.stop_record_button = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\..\src\MainWindow.xaml"
            this.stop_record_button.Click += new System.Windows.RoutedEventHandler(this.stop_record_button_Click);
            
            #line default
            #line hidden
            return;
            case 21:
            this.textBlockFrameNr = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 22:
            this.textBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 23:
            this.label = ((System.Windows.Controls.Label)(target));
            return;
            case 24:
            this.markerCount = ((System.Windows.Controls.Label)(target));
            return;
            case 25:
            this.recognised_gesture = ((System.Windows.Controls.Label)(target));
            return;
            case 26:
            this.recordBlinker = ((System.Windows.Shapes.Ellipse)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

