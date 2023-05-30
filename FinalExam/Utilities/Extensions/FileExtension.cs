namespace FinalExam.Utilities.Extensions
{
    public static class FileExtension
    {
        public static bool CheckFileType(this IFormFile file, string type)
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool CheckFileSize(this IFormFile file, int kb)
        {
            if (file.Length <= kb * 1024)
            {
                return true;
            }
            return false;
        }
        public static async Task<string> CreateFileAsync(this IFormFile file, string root, string folder)
        {
            string foldername = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(root, folder, foldername);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return foldername;
        }
        public static void DeleteFile(this string foldername,string root,string folder) 
        {
            string path=Path.Combine(root, folder,foldername);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
