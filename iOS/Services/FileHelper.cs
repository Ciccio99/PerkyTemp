using PerkyTemp.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PerkyTemp.iOS.Services.FileHelper))]
namespace PerkyTemp.iOS.Services
{
    /// <summary>
    /// iOS implementation of IFileHelper.
    /// </summary>
    /// <see cref="IFileHelper"/>
    public class FileHelper : IFileHelper
    {
        /// <summary>
        /// Android implementation of GetFileLocalPath.
        /// </summary>
        /// <see cref="IFileHelper.GetLocalFilePath(string)"/>
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }
    }
}