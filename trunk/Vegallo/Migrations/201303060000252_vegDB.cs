namespace Vegallo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vegDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.programs", "plakat", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.programs", "plakat");
        }
    }
}
