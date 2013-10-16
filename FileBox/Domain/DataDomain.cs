using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using FileBox.Entities;
using NHibernate.Linq;
namespace FileBox.Domain
{
    public sealed class DataDomain
    {
        private static readonly Lazy<DataDomain> instance = new Lazy<DataDomain>(() => new DataDomain());
        ISessionFactory currentSession;
        private DataDomain()
        {

            this.currentSession = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(C => C.FromConnectionStringWithKey("DbConnection")))
            .Mappings(M => M.FluentMappings.AddFromAssemblyOf<DataDomain>())
            .ExposeConfiguration(cfg =>
            {
                CustomIdentityHiLoGeneratorConvention.CreateHighLowScript(cfg);
                BuildSchema(cfg);
            })
            .BuildSessionFactory();
        }

        public static DataDomain Instance { get { return instance.Value; } }

        private void BuildSchema(Configuration config)
        {
            new SchemaUpdate(config).Execute(false, true);
            //new SchemaExport(config).Create(true, true);
        }

        internal void CreateFile(StoredFile file)
        {
            using (var session = currentSession.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.Save(file);
                    file.UrlKey=UrlGenerator.CreateShortUrlFromId(file.Id);
                    var query1=session.CreateQuery("update StoredFile set UrlKey=:url where Id=:id")
                        .SetParameter("url", file.UrlKey)
                        .SetParameter("id", file.Id);
                    query1.ExecuteUpdate();
                    trans.Commit();
                }
            }
        }

        internal StoredFile GetFileInfo(string Urlkey)
        {
            StoredFile result = null;
            using (var session = currentSession.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    result=session.Query<StoredFile>().FirstOrDefault(x => x.UrlKey == Urlkey);
                    trans.Commit();
                }
            }
            return result;
        }

        internal void ReportHit(int Id)
        {
            using (var session = currentSession.OpenSession())
            {
                using (var trans = session.BeginTransaction())
                {
                    session.CreateQuery("update StoredFile set LastAccess=:date,set HitCount=HitCount+1 where Id=:id")
                        .SetParameter("date", DateTime.Now)
                        .SetParameter("id", Id)
                        .ExecuteUpdate();
                    trans.Commit();
                }
            }
        }
    }
}