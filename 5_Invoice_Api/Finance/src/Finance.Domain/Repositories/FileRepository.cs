using System;
using System.Collections.Generic;
using System.Text;

namespace Finance.Domain.Repositories
{
    using Domain;

    public sealed class FileRepository : IRepository<File>
    {
        private readonly Dictionary<string, File> files = new Dictionary<string, File> { {"1", new File(Encoding.UTF8.GetBytes("test"),
            new DateTimeOffset(2016, 12, 10, 0, 0, 0, TimeSpan.Zero),  "text/plain")}};

        public File Get(string id)
        {
            if (!files.ContainsKey(id))
            {
                return null;
            }
            return files[id];
        }

        public void Delete(string id)
        {
            files.Remove(id);
        }

        public Version Update(File instance)
        {
            throw new System.NotImplementedException();
        }

        public File Create(File instance)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(string id)
        {
            return files.ContainsKey(id);
        }

        public string GetCurrentVersion(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<File> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}