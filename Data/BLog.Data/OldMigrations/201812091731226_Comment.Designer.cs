// <auto-generated />
namespace Blog.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class Comment : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Comment));
        
        string IMigrationMetadata.Id
        {
            get { return "201812091731226_Comment"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}