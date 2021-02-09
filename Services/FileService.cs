using System.IO;

namespace Catalogue.Services
{
    /// <summary>
    /// Responsible for managing all file and directory operations
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Determines if the directory exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsDirectoryExists(string path);

        /// <summary>
        /// Determines if the file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        bool IsFileExists(string path);

        /// <summary>
        /// Will produce single path based on all paths specified as string array.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        string CombinePaths(string[] paths);

        /// <summary>
        /// Will create new directory.
        /// </summary>
        /// <param name="path"></param>
        void CreateDirectory(string path);

        /// <summary>
        /// Will read data from file if file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        TextReader ReadText(string path);
    }

    /// <summary>
    /// Responsible for managing all file and directory operations
    /// </summary>
    public class FileService: IFileService
    {
        /// <summary>
        /// Determines if the directory exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Determines if the file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsFileExists(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Will produce single path based on all paths specified as string array.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public string CombinePaths(string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// Will create new directory.
        /// </summary>
        /// <param name="path"></param>
        public void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Will read data from file if file exists.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public TextReader ReadText(string path)
        {
            return File.Exists(path) ? File.OpenText(path) : null;
        }
    }
}
