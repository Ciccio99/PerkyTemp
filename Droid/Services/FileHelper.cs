using PerkyTemp.Interfaces;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PerkyTemp.Droid.Services.FileHelper))]
namespace PerkyTemp.Droid.Services
{
    /// <summary>
    /// Android implementation of the IFileHelper interface.
    /// </summary>
    /// <see cref="IFileHelper"/>
    public class FileHelper : IFileHelper
    {
        /// <summary>
        /// Android implementation of GetLocalFilePath.
        /// </summary>
        /// <see cref="IFileHelper.GetLocalFilePath(string)"/>
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}