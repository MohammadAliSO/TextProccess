using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asynchronous_TextProcessing.Models
{
    /// <summary>
    /// Model of all Configs
    /// </summary>
    public class ConfigModels
    {
        /// <summary>
        /// declare of Connection strings
        /// </summary>
        public ConnectionStringsModel? ConnectionStrings { get; set; }
    }
    /// <summary>
    /// ConnectionStringsModel 
    /// </summary>
    public class ConnectionStringsModel
    {
        /// <summary>
        /// declare of DefaultConnection
        /// </summary>
        public string? DefaultConnection { get; set; }
    }
}
