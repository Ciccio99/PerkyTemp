using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerkyTemp.Interfaces
{
    /// <summary>
    /// Interface for cross-platform (iOS and Android) file operations. This
    /// should have a concrete implementation for each platform. The correct
    /// implementation will be chosen via Xamarin's dependency injection.
    /// </summary>
    public interface IFileHelper
    {
        /// <summary>
        /// Get an app-local file path for a specific filename.
        /// </summary>
        string GetLocalFilePath(string filename);
    }
}
