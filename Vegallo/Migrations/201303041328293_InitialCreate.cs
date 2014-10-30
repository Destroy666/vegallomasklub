namespace Vegallo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vendegs",
                c => new
                    {
                        vendegID = c.Int(nullable: false, identity: true),
                        nev = c.String(nullable: false),
                        uzenet = c.String(nullable: false),
                        datum = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.vendegID);
            
            CreateTable(
                "dbo.programs",
                c => new
                    {
                        programID = c.Int(nullable: false, identity: true),
                        datum = c.DateTime(nullable: false),
                        zenakarok = c.String(nullable: false),
                        nyitas = c.Time(nullable: false),
                        kezdes = c.Time(nullable: false),
                        jegyar = c.Int(nullable: false),
                        elmarad = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.programID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.programs");
            DropTable("dbo.Vendegs");
        }
    }
}
