﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cl.uv.leikelen.Properties {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("cl.uv.leikelen.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Not Tracked.
        /// </summary>
        public static string Body_NotTracked {
            get {
                return ResourceManager.GetString("Body_NotTracked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a KStudio Event File.
        /// </summary>
        public static string EventFileDescription {
            get {
                return ResourceManager.GetString("EventFileDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a (*.xef, *.xrf)|*.xef;*.xrf.
        /// </summary>
        public static string EventFileFilter {
            get {
                return ResourceManager.GetString("EventFileFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Failed to write screenshot to {0}.
        /// </summary>
        public static string FailedScreenshotStatusTextFormat {
            get {
                return ResourceManager.GetString("FailedScreenshotStatusTextFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a 3.
        /// </summary>
        public static string maxIntervalGroupsInViewPerPerson {
            get {
                return ResourceManager.GetString("maxIntervalGroupsInViewPerPerson", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a 2000.
        /// </summary>
        public static string MinPostureIntervalDuration {
            get {
                return ResourceManager.GetString("MinPostureIntervalDuration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ninguna.
        /// </summary>
        public static string NonePostureName {
            get {
                return ResourceManager.GetString("NonePostureName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No ready Kinect found!.
        /// </summary>
        public static string NoSensorStatusText {
            get {
                return ResourceManager.GetString("NoSensorStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a 1000.
        /// </summary>
        public static string PostureDurationDetectionThreshold {
            get {
                return ResourceManager.GetString("PostureDurationDetectionThreshold", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a app_sqlite.db.
        /// </summary>
        public static string PostureTypeDbPath {
            get {
                return ResourceManager.GetString("PostureTypeDbPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Running.
        /// </summary>
        public static string RunningStatusText {
            get {
                return ResourceManager.GetString("RunningStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Kinect not available!.
        /// </summary>
        public static string SensorNotAvailableStatusText {
            get {
                return ResourceManager.GetString("SensorNotAvailableStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a .xef.
        /// </summary>
        public static string XefExtension {
            get {
                return ResourceManager.GetString("XefExtension", resourceCulture);
            }
        }
    }
}
