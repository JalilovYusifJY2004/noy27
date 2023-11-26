﻿using _16noyabr.Models;

namespace _16noyabr.Utilities.Extention
{
    public static class FileValidator
    {
        public static bool ValidateType(this IFormFile file,string type="image")
        {
            if (file.ContentType.Contains(type))
            {
                return true;
            }
            return false;
        }
        public static bool ValidateSize(this IFormFile file,int kb)
        {
            if (file.Length > kb * 1024*1024)
            {
                return false;
            }
            return true;
        }
        public  async static Task<string> CreateFile(this IFormFile file, string root,params string[] folders)
        {
            string filename = Guid.NewGuid().ToString() + file.FileName;
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path= Path.Combine(path, folders[i]);
            }
           path = Path.Combine(path,filename);

          using  (FileStream stream = new FileStream(path, FileMode.Create)) 
            {


                await file.CopyToAsync(stream);
            }
          return filename;




           
  
        }

        public static async void DeleteFile(this string filename,string root,params string[] folders)
        {
            string path = filename;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path,filename);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
