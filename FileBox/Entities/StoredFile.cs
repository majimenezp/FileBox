using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBox.Entities
{
    public class StoredFile
    {
        public virtual long Id { get; set; }
        public virtual Guid UniqueId { get; set; }
        public virtual string UrlKey { get; set; }
        public virtual string OriginalFileName { get; set; }
        public virtual string CurrentFileName { get; set; }
        public virtual string Extension { get; set; }
        public virtual long FileSize { get; set; }
        public virtual DateTime LastAccess { get; set; }
        public virtual int HitCount { get; set; }
        public StoredFile()
        {
            this.HitCount = 0;
            this.LastAccess = DateTime.Now;
            this.FileSize = 0;
            this.Extension = string.Empty;
            this.CurrentFileName = string.Empty;
            this.OriginalFileName = string.Empty;
            this.UrlKey = string.Empty;
        }
        public virtual string GetUrl()
        {
            return "/file/" + this.UrlKey;
        }
    }

    public class MappingStoredFile:ClassMap<StoredFile>
    {
        public MappingStoredFile()
        {
            Table("StoredFiles");
            Id(x=>x.Id)
                .GeneratedBy
                .HiLo("NHibernateHiLoIdentity", "NextHiValue", "1",
                x => x.AddParam("where", string.Format("Entity = '{0}'", "[StoredFiles]")));
            Map(x => x.OriginalFileName).Length(1000).Not.Nullable();
            Map(x => x.CurrentFileName).Length(1000).Not.Nullable();
            Map(x => x.Extension).Length(30).Not.Nullable();
            Map(x => x.UniqueId).Not.Nullable();
            Map(x => x.UrlKey).Length(100).Not.Nullable();
            Map(x => x.FileSize).Not.Nullable().Default("0");
            Map(x => x.LastAccess).Not.Nullable();
            Map(x => x.HitCount).Not.Nullable().Default("0");
        }
    }
}